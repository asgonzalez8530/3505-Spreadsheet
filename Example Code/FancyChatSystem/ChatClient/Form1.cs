using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkController;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;

namespace ChatClient
{
  public partial class Form1 : Form
  {

    private SocketState theServer;

    public Form1()
    {
      InitializeComponent();
      messageToSendBox.KeyDown += new KeyEventHandler(MessageEnterHandler);
    }

    /// <summary>
    /// Connect button event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void connectButton_Click(object sender, EventArgs e)
    {
      // TODO: This needs better error handling. 
      // If the server is invalid, it gives a message, but doesn't allow us to try to reconnect.
      if (serverAddress.Text == "")
      {
        MessageBox.Show("Please enter a server address");
        return;
      }


      // Disable the controls and try to connect
      connectButton.Enabled = false;
      serverAddress.Enabled = false;

      
      // TODO: If the SocketState stored a delegate that determines what to do after 
      //       the connection is completed, we could pass that delegate to a
      //       static ConnectToServer method, and it would know what to do after
      //       a connection is established. As is, we hard-code this decision 
      //       in our networking code (line 141).
      ConnectToServer(serverAddress.Text);
    }



    /// <summary>
    /// Start attempting to connect to the server
    /// </summary>
    /// <param name="host_name"> server to connect to </param>
    /// <returns></returns>
    public void ConnectToServer(string hostName)
    {
      System.Diagnostics.Debug.WriteLine("connecting  to " + hostName);

      // Connect to a remote device.
      try
      {

        // Establish the remote endpoint for the socket.
        IPHostEntry ipHostInfo;
        IPAddress ipAddress = IPAddress.None;

        // Determine if the server address is a URL or an IP
        try
        {
          ipHostInfo = Dns.GetHostEntry(hostName);
          bool foundIPV4 = false;
          foreach (IPAddress addr in ipHostInfo.AddressList)
            if (addr.AddressFamily != AddressFamily.InterNetworkV6)
            {
              foundIPV4 = true;
              ipAddress = addr;
              break;
            }
          // Didn't find any IPV4 addresses
          if (!foundIPV4)
          {
            System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
            return;
          }
        }
        catch (Exception e1)
        {
          // see if host name is actually an ipaddress, i.e., 155.99.123.456
          System.Diagnostics.Debug.WriteLine("using IP");
          ipAddress = IPAddress.Parse(hostName);
        }

      // Create a TCP/IP socket.
      Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

      socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

      theServer = new SocketState(socket, -1);
      
      theServer.theSocket.BeginConnect(ipAddress, 2112 /* Networking.DEFAULT_PORT */, ConnectedCallback, theServer);

      }
      catch (Exception e)
      {
        System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
        return;
      }
    }

    /// <summary>
    /// This function is "called" by the operating system when the remote site acknowledges connect request
    /// </summary>
    /// <param name="ar"></param>
    private void ConnectedCallback(IAsyncResult ar)
    {
      SocketState ss = (SocketState)ar.AsyncState;
      
      try
      {
        // Complete the connection.
        ss.theSocket.EndConnect(ar);
      }
      catch (Exception e)
      {
        System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
        return;
      }

      // TODO: If we had a "EventProcessor" delagate stored in the state, we could call that,
      //       instead of hard-coding a method to call.
      AwaitDataFromServer(ss);

    }

    /// <summary>
    /// This starts an event loop to continuously listen for messages from the server.
    /// </summary>
    /// <param name="ss">The state representing the server connection</param>
    private void AwaitDataFromServer(SocketState ss)
    {

      // Start listening for a message
      // When a message arrives, handle it on a new thread with ReceiveCallback
      ss.theSocket.BeginReceive(ss.messageBuffer, 0, ss.messageBuffer.Length, SocketFlags.None, ReceiveCallback, ss);

      // TODO: If we had an "EventProcessor" delegate in the SocketState, 
      //       We could update it here so that it handles received messages, 
      //       instead of handling the initial server connection.
      //       The ReceiveCallback would then know what to do when a message arrives,
      //       instead of hard-coding it (line 178).

    }

    private void ReceiveCallback(IAsyncResult ar)
    {
      SocketState ss = (SocketState)ar.AsyncState;

      int bytesRead = ss.theSocket.EndReceive(ar);

      // If the socket is still open
      if (bytesRead > 0)
      {
        string theMessage = Encoding.UTF8.GetString(ss.messageBuffer, 0, bytesRead);
        // Append the received data to the growable buffer.
        // It may be an incomplete message, so we need to start building it up piece by piece
        ss.sb.Append(theMessage);

        ProcessMessage(ss);
      }

      // Continue the "event loop" that was started on line 154.
      // Start listening for more parts of a message, or more new messages
      ss.theSocket.BeginReceive(ss.messageBuffer, 0, ss.messageBuffer.Length, SocketFlags.None, ReceiveCallback, ss);


    }

    private void ProcessMessage(SocketState ss)
    {
      string totalData = ss.sb.ToString();
      string[] parts = Regex.Split(totalData, @"(?<=[\n])");

      // Loop until we have processed all messages.
      // We may have received more than one.

      foreach (string p in parts)
      {
        // Ignore empty strings added by the regex splitter
        if (p.Length == 0)
          continue;
        // The regex splitter will include the last string even if it doesn't end with a '\n',
        // So we need to ignore it if this happens. 
        if (p[p.Length-1] != '\n')
          break;

        // Display the message
        // "messages" is the big message text box in the form.
        // We must use a MethodInvoker, because only the thread that created the GUI can modify it.
        System.Diagnostics.Debug.WriteLine("appending \"" + p + "\"");
                this.Invoke(new MethodInvoker(
                  () => messages.AppendText(p)));

        // Then remove it from the SocketState's growable buffer
        ss.sb.Remove(0, p.Length);
      }
    }
      

    /// <summary>
    /// This is the event handler when the enter key is pressed in the messageToSend box
    /// </summary>
    /// <param name="sender">The Form control that fired the event</param>
    /// <param name="e"></param>
    private void MessageEnterHandler(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        string message = messageToSendBox.Text;
        byte[] messageBytes = Encoding.UTF8.GetBytes(message + "\n");
        messageToSendBox.Text = "";
        theServer.theSocket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, theServer);
      }
    }

    /// <summary>
    /// A callback invoked when a send operation completes
    /// </summary>
    /// <param name="ar"></param>
    private void SendCallback(IAsyncResult ar)
    {
      SocketState ss = (SocketState)ar.AsyncState;
      // Nothing much to do here, just conclude the send operation so the socket is happy.
      ss.theSocket.EndSend(ar);
    }

  }
}
