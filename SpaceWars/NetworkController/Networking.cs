using System;
using System.Net.Sockets;
using System.Text;

namespace Communication
{
    public class SocketState
    {
        /// <summary>
        /// Network Action takes a SocketState object, and allows that state to perform an
        /// action within a network callback. 
        /// </summary>
        public delegate void NetworkAction(SocketState state);

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
    }

        public static class Network
        {
            public static Socket ConnectToServer(Delegate callbackFunction, string hostname)
            {
                //TODO: Fix red lines (get delegate signature)
                throw new NotImplementedException();
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


        }
    }