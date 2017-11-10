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

    public class SocketState
    {
        private NetworkAction action;
        private Socket theSocket;
        private int ID;

        // This is the buffer where we will receive data from the socket
        private byte[] messageBuffer;

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        private StringBuilder sb;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="s"> the socket </param>
        /// <param name="id"> id sent from the client that describes the current socket </param>
        public SocketState(Socket s, int id)
        {
            theSocket = s;
            ID = id;
            messageBuffer = new byte[1024];
            sb = new StringBuilder();

            // an empty action, does nothing, guarantees action is not null when created.
            action = x => { };
        }

        /// <summary>
        /// Gets the byte array which is used as the sockets buffer
        /// </summary>
        public byte[] GetMessageBuffer()
        {
            return messageBuffer;
        }

        /// <summary>
        /// Takes a byte array, message, and sets it to this sockets buffer
        /// </summary>
        public void SetMessageBuffer(byte[] message)
        {
            //TODO: may need to remove if this method is not used
            messageBuffer = message;
        }

        /// <summary>
        /// Returns the StringBuilder used as a dynamic buffer used by this socket.
        /// </summary>
        public StringBuilder GetStringBuilder()
        {
            return sb;
        }

        /// <summary>
        /// Takes a string, message, and creates a new StringBuilder with message and sets it 
        /// to the StringBuilder used as a dynamic buffer used by this socket.
        /// </summary>
        public void SetStringBuilder(string message)
        {
            sb = new StringBuilder(message);
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
        /// Takes in a SocketState, s, and calls the NetworkAction method stored by the
        /// delegate.
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
        /// Sets the socket passed in, s, to the current sockets state
        /// </summary>
        public void SetSocket(Socket s)
        {
            theSocket = s;
        }
    }

    public static class Network
    {
        //the port that we will be using
        public const int DEFAULT_PORT = 11000;


        /// <summary>
        /// Attempt to connect to the server via the provided hostname. It should 
        /// </summary>
        /// <param name="callbackFunction"> a function to be called when a connection is made </param>
        /// <param name="hostname"> name of the server to connect to </param>
        /// <returns></returns>
        public static Socket ConnectToServer(NetworkAction callbackFunction, string hostname)
        {
            System.Diagnostics.Debug.WriteLine("connecting  to " + hostname);

            // Create a TCP/IP socket.
            MakeSocket(hostname, out Socket socket, out IPAddress ipAddress);

            // make a new state of the socket we just made
            SocketState state = new SocketState(socket, -1);

            // call the correct function needed for the client
            state.SetNetworkAction(callbackFunction);

            // make a new connection to the server
            state.GetSocket().BeginConnect(ipAddress, DEFAULT_PORT, ConnectedCallback, state);

            // return the current socket with respect to the state
            return state.GetSocket();
        }


        public static void ConnectedToServer(IAsyncResult stateAsArObject)
        {
            //TODO: Write method comment
            throw new NotImplementedException();

        }

        public static void GetData(SocketState state)
        {
            //TODO: Write method comment
            throw new NotImplementedException();
        }

        public static void ReceiveCallback(IAsyncResult stateAsArObject)
        {
            //TODO: write method comment
        }

        public static void Send(Socket socket, String data)
        {
            //TODO: write method comment
            throw new NotImplementedException();
        }

        public static void SendCallback(IAsyncResult ar)
        {
            //TODO: write method comment
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a Socket object for the given host string
        /// </summary>
        /// <param name="hostName">The host name or IP address</param>
        /// <param name="socket">The created Socket</param>
        /// <param name="ipAddress">The created IPAddress</param>
        public static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
        {
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

                // Disable Nagle's algorithm - can speed things up for tiny messages, 
                // such as for a game
                socket.NoDelay = true;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
                throw new ArgumentException("Invalid address");
            }
        }

        // TODO: May go in one of the provided methods
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
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }
        }
    }
}