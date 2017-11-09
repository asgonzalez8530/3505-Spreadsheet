using System;
using System.Net.Sockets;

namespace Communication
{
    public class SocketState
    {

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