// Anastasia Gonzalez and Aaron Bellis UID: u0985898 & u0981638
// Code implemented as part of PS7 : SpaceWars client CS3500 Fall Semester
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Communication
{
    /// <summary>
    /// Network Action takes a SocketState object, and allows that state to perform an
    /// action within a network callback. 
    /// </summary>
    public delegate void NetworkAction(SocketState state);

    /// <summary>
    /// A class which represents the state of a socket connection as well as a network
    /// action delegate which can be invoked upon receiving data on the socket
    /// </summary>
    public class SocketState
    {
        // a delegate which can be invoked when data is received
        // enforced invariant: cannot be null
        private NetworkAction action;
        // the socket whos state is represented by this object
        private Socket theSocket;
        // the id given by client which identifies this connection
        private int ClientID;

        // This is the buffer where we will receive data from the socket
        private byte[] messageBuffer;

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        private StringBuilder sb;

        /// <summary>
        /// Creates a new socket state object with the given socket and id
        /// </summary>
        /// <param name="s"> the socket </param>
        /// <param name="id"> id sent from the client that describes the current socket </param>
        public SocketState(Socket s, int id)
        {
            theSocket = s;
            ClientID = id;
            messageBuffer = new byte[1024];
            sb = new StringBuilder();
            HasError = false;
            ErrorMessage = "";

            // method guarantees action is not null when created.
            SetNetworkAction(null);
        }

        /// <summary>
        /// True if the socket has encountered an error, else false.
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Contains the error message reported by the system if one was encountered.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets the byte array which is used as the sockets buffer
        /// </summary>
        public byte[] GetMessageBuffer()
        {
            return messageBuffer;
        }

        /// <summary>
        /// Returns the StringBuilder used as a dynamic buffer used by this socket.
        /// </summary>
        public StringBuilder GetStringBuilder()
        {
            return sb;
        }

        /// <summary>
        /// Takes a NetworkAction delegate, a, and sets it to this sockets action.
        /// if a is null, sets action to empty lambda.
        /// </summary>
        public void SetNetworkAction(NetworkAction a)
        {
            if (a != null)
            {
                action = a;
            }
            else
            {
                action = x => { };
            }
        }

        /// <summary>
        /// Takes in a SocketState, s, and calls the NetworkAction method stored
        /// by s.
        /// </summary>
        public void InvokeNetworkAction(SocketState s)
        {
            s.action(s);
        }

        /// <summary>
        /// Gets the socket associated with this socket state
        /// </summary>
        public Socket GetSocket()
        {
            return theSocket;
        }

        /// <summary>
        /// Sets the id sent from the client that describes the current socket
        /// </summary>
        public void SetID(int id)
        {
            ClientID = id;
        }

        /// <summary>
        /// Gets the id sent from the client that describes the current socket
        /// </summary>
        public int GetID()
        {
            return ClientID;
        }

    }

    /// <summary>
    /// The Connection State class holds data and methods nessasary for handling incoming network
    /// requests.
    /// </summary>
    public class ConnectionState
    {
        // the TCP listener for this connection request
        TcpListener listener;

        // a delegate which can be invoked upon receiving a connection request
        // enforced invarient: this action cannot be null
        NetworkAction action;

        /// <summary>
        /// Creates a new ConnectionState object with the user given TcpListener
        /// and NetworkAction delegate. 
        /// 
        /// If listen is null, throws ArgumentNullException.
        /// If action is null, sets networkAction to an empty lambda.
        /// </summary>
        public ConnectionState(TcpListener listen, NetworkAction networkAction)
        {
            if (listen == null)
            {
                string exceptionMessage = "TcpListener object passed to ConnectionState cannot be null";
                throw new ArgumentNullException(exceptionMessage);
            }

            listener = listen;
            SetNetworkAction(networkAction);
        }

        /// <summary>
        /// Takes a NetworkAction delegate, a, and sets it to this sockets action.
        /// if a is null, sets action to empty lambda.
        /// </summary>
        private void SetNetworkAction(NetworkAction a)
        {
            if (a != null)
            {
                action = a;
            }
            else
            {
                action = x => { };
            }
        }

        /// <summary>
        /// Returns the NetworkAction delegate stored by this connection state
        /// object.
        /// </summary>
        public NetworkAction GetNetworkAction()
        {
            return action;
        }

        /// <summary>
        /// Returns the TcpListener stored by this connection state
        /// object.
        /// </summary>
        public TcpListener GetTcpListener()
        {
            return listener;
        }

    }

    /// <summary>
    /// Class that intiates the passing of information with sockets between the server and client
    /// </summary>
    public static class Network
    {
        //the port that we will be using
        public const int DEFAULT_PORT = 11000;
        public const int HTTP_PORT = 80;


        /// <summary>
        /// Attempt to connect to the server via the provided hostname and default port 11000
        /// </summary>
        /// <param name="callbackFunction"> a function to be called when a connection is made </param>
        /// <param name="hostname"> name of the server to connect to </param>
        /// <returns></returns>
        public static Socket ConnectToServer(NetworkAction callbackFunction, string hostname)
        {
            return ConnectToServer(callbackFunction, hostname, DEFAULT_PORT);
        }

        /// <summary>
        /// Attempt to connect to the server via the provided hostname and port.  
        /// </summary>
        /// <param name="callbackFunction"> a function to be called when a connection is made </param>
        /// <param name="hostname"> name of the server to connect to </param>
        /// <returns></returns>
        public static Socket ConnectToServer(NetworkAction callbackFunction, string hostname, int port)
        {
            // Create a TCP/IP socket.
            MakeSocket(hostname, out Socket socket, out IPAddress ipAddress);

            // make a new state of the socket we just made
            SocketState state = new SocketState(socket, -1);

            // call the correct function needed for the client
            state.SetNetworkAction(callbackFunction);

            // make a new connection to the server
            state.GetSocket().BeginConnect(ipAddress, port, ConnectedCallback, state);

            // return the current socket with respect to the state
            return state.GetSocket();
        }

        /// <summary>
        /// Called by the OS when the socket connects to the server. 
        /// </summary>
        public static void ConnectedToServer(IAsyncResult stateAsArObject)
        {
            SocketState state = (SocketState)stateAsArObject;
            state.InvokeNetworkAction(state);
        }

        /// <summary>
        /// Takes in a SocketState object, state, and loads the buffer with data coming from the socket.
        /// </summary>
        public static void GetData(SocketState state)
        {
            try
            {
                state.GetSocket().BeginReceive(state.GetMessageBuffer(), 0, state.GetMessageBuffer().Length, SocketFlags.None, ReceiveCallback, state);
            }
            catch (SocketException e)
            {
                // put the state object in a state of error with the given message
                state.HasError = true;
                state.ErrorMessage = e.Message;
                // invoke client delegate so it can take whatever action it needs with an error
                state.InvokeNetworkAction(state);
            }
        }

        /// <summary>
        /// Called by the OS when new data arrives. If the connection is closed does nothing, else
        /// gets the SocketState and calls the callback function provided by the SocketState.
        /// </summary>
        public static void ReceiveCallback(IAsyncResult stateAsArObject)
        {
            SocketState state = (SocketState)stateAsArObject.AsyncState;

            int bytesRead = 0;
            try
            {
                bytesRead = state.GetSocket().EndReceive(stateAsArObject);
            }
            catch (SocketException e)
            {
                state.HasError = true;
                state.ErrorMessage = e.Message;
                // invoke client delegate so it can take whatever action it needs with an error
                state.InvokeNetworkAction(state);
            }
            catch (ObjectDisposedException e)
            {
                state.HasError = true;
                state.ErrorMessage = e.Message;
                // invoke client delegate so it can take whatever action it needs with an error
                state.InvokeNetworkAction(state);
            }

            // If the socket is still open
            if (bytesRead > 0)
            {
                string theMessage = Encoding.UTF8.GetString(state.GetMessageBuffer(), 0, bytesRead);
                // Append the received data to the growable buffer.
                // It may be an incomplete message, so we need to start building it up piece by piece
                state.GetStringBuilder().Append(theMessage);

                // Instead, just invoke the client's delegate, so it can take whatever action it desires.
                state.InvokeNetworkAction(state);
            }
        }

        /// <summary>
        /// Takes in a Socket socket, and a String, data and sends the data on that socket.
        /// </summary>
        public static void Send(Socket socket, String data)
        {            
            byte[] messageBytes = Encoding.UTF8.GetBytes(data);
            try
            {
                socket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, socket);
            }
            catch (SocketException)
            {
                socket.Close();
            }
            catch (ObjectDisposedException)
            { 
                // object is already disposed don't need to close anything
            }
            
        }

        /// <summary>
        /// A callback invoked when a send operation completes
        /// </summary>
        public static void SendCallback(IAsyncResult ar)
        {
            Socket s = (Socket)ar.AsyncState;
            // end the feedback loop for the current socket. 
            try
            {
                s.EndSend(ar);
            }
            catch (SocketException)
            {
                s.Close();
            }
            catch (ObjectDisposedException)
            {
                // object already disposed dont need to do anything
            }
        }



        /// <summary>
        /// Creates a Socket object for the given host string
        /// </summary>
        /// <param name="hostName">The host name or IP address</param>
        /// <param name="socket">The created Socket</param>
        /// <param name="ipAddress">The created IPAddress</param>
        public static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
        {
            //intialize the ipAddress and socket that are passed in 
            ipAddress = IPAddress.None;
            socket = null;

            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;

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
                        throw new ArgumentException("Invalid address");
                    }
                }
                catch (Exception)
                {
                    // see if host name is actually an ipaddress, i.e., 155.99.123.456
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostName);
                }

                // Create a TCP/IP socket.
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                // Disable Nagle's algorithm - can speed things up for tiny messages, such as for a game
                socket.NoDelay = true;
                
            }
            catch (Exception e)
            {
                throw new ArgumentException("Invalid address");
            }
        }

        /// <summary>
        /// This function is "called" by the operating system when the remote site acknowledges connect request
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectedCallback(IAsyncResult ar)
        {
            //pull SocketState object from the IAsyncResult object, ar
            SocketState state = (SocketState)ar.AsyncState;

            try
            {
                // Complete the connection.
                state.GetSocket().EndConnect(ar);
            }
            catch (Exception e)
            {
                state.HasError = true;
                state.ErrorMessage = e.Message;
            }

            state.InvokeNetworkAction(state);
        }

        #region Server Listening event loop methods

        /// <summary>
        /// Takes a NetworkAction delegate, action and begins an event loop 
        /// listening for new connections on the default port: 11000
        /// </summary>
        /// <param name="action">A delegate to be invoked when a new connection 
        /// comes in. </param>
        public static void ServerAwaitingClientLoop(NetworkAction action)
        {
            ServerAwaitingClientLoop(action, DEFAULT_PORT);
        }

        /// <summary>
        /// Takes a NetworkAction delegate, and a specified port and begins
        /// an event loop listening for new connections on that port.
        /// </summary>
        /// <param name="action">A delegate to be invoked when a new connection 
        /// comes in.</param>
        /// <param name="port">The port number to listen on</param>
        public static void ServerAwaitingClientLoop(NetworkAction action, int port)
        {
            // TODO: make sure another server is not open
            try
            {
                // create a new TcpListener and start it listening on the given port
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                // save the state of our connection allong with its NetworkAction
                ConnectionState state = new ConnectionState(listener, action);

                // begin accepting incoming connection attempts
                listener.BeginAcceptSocket(AcceptNewClient, state);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// This function is "called" by the operating system when a remote
        /// connection request comes in.
        /// 
        /// AcceptNewClient creates a new socket and saves it in a SocketState
        /// object then invokes the NetworkAction delegate stored by the 
        /// parameter ar and continues the event loop. 
        /// </summary>
        public static void AcceptNewClient(IAsyncResult ar)
        {
            // extract the ConnectionState from ar
            ConnectionState cs = (ConnectionState)ar.AsyncState;

            // extract the socket from the TCP listener 
            Socket socket = cs.GetTcpListener().EndAcceptSocket(ar);

            // save the socket along with a network action
            SocketState ss = new SocketState(socket, -1);
            ss.SetNetworkAction(cs.GetNetworkAction());

            // invoke the network action and pass it our new socket state
            ss.InvokeNetworkAction(ss);

            //continue the event loop
            cs.GetTcpListener().BeginAcceptSocket(AcceptNewClient, cs);
        }

        /// <summary>
        /// Sends data on the given socket then closes the socket when done.
        /// </summary>
        public static void SendAndClose(Socket socket, string data)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(data);

            socket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, socket);
        }

        /// <summary>
        /// A callback invoked when a SendAndClose operation completes
        /// </summary>
        public static void SendAndCloseCallback(IAsyncResult ar)
        {
            Socket s = (Socket)ar.AsyncState;
            // using notation to ensure socket gets disposed
            using (s)
            {
                // end the feedback loop for the current socket. 
                s.EndSend(ar);
                s.Close();
            }
        }

        #endregion
    }
}