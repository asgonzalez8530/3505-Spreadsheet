using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkController
{
    /// <summary>
    /// The delegate declaration for actions the client needs to take in response to
    /// being "notified" that data was sent from the server.
    /// </summary>
    /// <param name="state"></param>
    public delegate void NetworkAction(SocketState state);


    /// <summary>
    /// This class holds all the necessary state to represent a socket connection
    /// Note that all of its fields are public because we are using it like a "struct"
    /// It is a simple collection of fields
    /// </summary>
    public class SocketState
    {
        /// <summary>
        /// The Socket of this SocketState.
        /// </summary>
        public Socket theSocket;

        public int ID;

        /// <summary>
        /// This is the buffer where we will receive data from the socket.
        /// </summary>
        public byte[] messageBuffer = new byte[1024];

        /// <summary>
        /// This is a larger (growable) buffer, in case a single receive does not contain the full message.
        /// </summary>
        public StringBuilder sBuilder = new StringBuilder();

        /// <summary>
        /// This is how the networking library will "notify" users when a connection is made, or when data is received.
        /// </summary>
        public NetworkAction callMe;

        /// <summary>
        /// Keeps track of errors that have happened on our socket.
        /// </summary>
        public bool hasError;

        /// <summary>
        /// Construct a SocketState using the provided Socket and ID.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="id"></param>
        public SocketState(Socket s, int id)
        {
            theSocket = s;
            ID = id;
            hasError = false;
        }
    }


    /// <summary>
    /// Holds a TCPListener and the current NetworkAction delegate for the connection. Used by the server.
    /// </summary>
    public class ConnectionState
    {
        // call me
        public NetworkAction callMe;
        // TCPlistener
        public TcpListener listener;
    }

    /// <summary>
    /// A generic Networking class that handles connections between client and server.
    /// </summary>
    public class Networking
    {
        /// <summary>
        /// The default port for establishing a Socket.
        /// </summary>
        public const int DEFAULT_PORT = 2112;


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


        /// <summary>
        /// Start attempting to connect to the server
        /// </summary>
        /// <param name="hostName"> server to connect to </param>
        /// <returns></returns>
        public static Socket ConnectToServer(NetworkAction callMe, string hostName)
        {
            // Create a TCP/IP socket.
            Socket socket;
            IPAddress ipAddress;

            Networking.MakeSocket(hostName, out socket, out ipAddress);

            SocketState state = new SocketState(socket, -1);

            state.callMe = callMe;

            state.theSocket.BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedCallback, state);

            return state.theSocket;
        }


        /// <summary>
        /// This function is "called" by the operating system when the remote site acknowledges connect request
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectedCallback(IAsyncResult ar)
        {
            SocketState state = (SocketState)ar.AsyncState;

            try
            {
                // Complete the connection.
                state.theSocket.EndConnect(ar);
            }
            catch (Exception e)
            {
                state.hasError = true;
                throw e; // TODO take this out?
            }

            // Don't start an event loop to receive data from the server. The client might not want to do that.
            // state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
            // Instead, just invoke the client's delegate so it can take whatever action it desires.
            state.callMe(state);
        }


        /// <summary>
        /// This function is "called" by the operating system when data is available on the socket
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            SocketState state = (SocketState)ar.AsyncState;

            try
            {
                int bytesRead = state.theSocket.EndReceive(ar);

                // If the socket is still open
                if (bytesRead > 0)
                {
                    string theMessage = Encoding.UTF8.GetString(state.messageBuffer, 0, bytesRead);
                
                    // Append the received data to the growable buffer.
                    // It may be an incomplete message, so we need to start building it up piece by piece
                    state.sBuilder.Append(theMessage);

                    // We can't process the message directly, because different users of this library might have different
                    // processing needs.
                    // ProcessMessage(state);

                    // Instead, just invoke the client's delegate, so it can take whatever action it desires.
                    state.callMe(state);
                }
            }
            catch (Exception)
            {
                state.hasError = true;
                state.callMe(state);
            }
        }


        /// <summary>
        /// GetData is just a wrapper for BeginReceive.
        /// This is the public entry point for asking for data.
        /// Necessary so that we can separate networking concerns from client concerns.
        /// </summary>
        /// <param name="state"></param>
        public static void GetData(SocketState state)
        {
            try
            {
                state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
            }
            catch (Exception)
            {
                state.hasError = true;
            }
        }


        /// <summary>
        /// This function assists the Send function. It should extract the Socket out of the IAsyncResult, 
        /// and then call socket.EndSend. You may, when first prototyping your program, put a WriteLine in here to see when data goes out
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            Socket s = (Socket)ar.AsyncState;

            try
            {
                s.EndSend(ar);
            }
            catch (Exception)
            {
                // connection error/ network
            }
        }


        /// <summary>
        /// This function (along with its helper 'SendCallback') will allow a program to send data over a socket.
        /// This function needs to convert the data into bytes and then send them using socket.BeginSend.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static void Send(Socket socket, String data)
        {
            try
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(data);
                socket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, socket);
            }
            catch (Exception)
            {
                // connection error/ network
            }
        }


        /// <summary>
        /// This is the heart of the server code. It starts with a TcpListener for new connections 
        /// and passes the listener, along with the callMe delegate, to BeginAcceptSocket as the state parameter.
        /// </summary>
        public static void ServerAwaitingClientLoop(NetworkAction action)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, DEFAULT_PORT);
            listener.Start();

            // setting up the connection state
            ConnectionState state = new ConnectionState();
            state.listener = listener;
            state.callMe = action;

            listener.BeginAcceptSocket(AcceptNewClient, state);
        }


        /// <summary>
        /// This is the callback that BeginAcceptSocket to use. 
        /// This code will be invoked by the OS when a connection request comes in.
        /// </summary>
        public static void AcceptNewClient(IAsyncResult ar)
        {
            ConnectionState conState = (ConnectionState)ar.AsyncState;

            // set up new client
            Socket socket = conState.listener.EndAcceptSocket(ar);
            SocketState socketState = new SocketState(socket, -1);
            socketState.callMe = conState.callMe;
            socketState.callMe(socketState);

            // start accepting other clients
            conState.listener.BeginAcceptSocket(AcceptNewClient, conState);
        }
    }
}

