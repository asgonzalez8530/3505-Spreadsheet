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
        }

        public static void ConnectedToServer(IAsyncResult stateAsArObject)
        {
            //TODO: Write method comment
        }

        public static void GetData(SocketState state)
        {
            //TODO: Write method comment
        }

        public static void ReceiveCallback(IAsyncResult stateAsArObject)
        {
           //TODO: write method comment
        }

        public static void Send(Socket socket, String data)
        {
            //TODO: write method comment
        }

        public static void SendCallback(IAsyncResult ar)
        {
            //TODO: write method comment
        }
    }
}