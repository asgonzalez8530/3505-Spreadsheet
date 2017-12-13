using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;

namespace SpaceWarsServer
{
    /// <summary>
    /// A very simple web server
    /// </summary>
    public static class WebServer
    {
        // The server must follow the HTTP protocol when replying to a client (web browser).
        // That involves sending an HTTP header before any web content.

        // This is the header to use when the server recognizes the browser's request
        private const string httpOkHeader =
          "HTTP/1.1 200 OK\r\n" +
          "Connection: close\r\n" +
          "Content-Type: text/html; charset=UTF-8\r\n" +
          "\r\n";

        // This is the header to use when the server does not recognize the browser's request
        private const string httpBadHeader =
          "HTTP/1.1 404 Not Found\r\n" +
          "Connection: close\r\n" +
          "Content-Type: text/html; charset=UTF-8\r\n" +
          "\r\n";


        public static void Main(string[] args)
        {
            // Start an event loop that listens for socket connections on port 80
            // This requires a slight modification to the networking library to take the port argument
            Network.ServerAwaitingClientLoop(HandleHttpConnection, Network.HTTP_PORT);

            Console.Read();
        }

        /// <summary>
        /// This is the delegate for when a new socket is accepted
        /// The networking library will invoke this method when a browser connects
        /// </summary>
        /// <param name="state"></param>
        public static void HandleHttpConnection(SocketState state)
        {
            // Before receiving data from the browser, we need to change what we do when network activity occurs.
            state.SetNetworkAction(ServeHttpRequest);

            Network.GetData(state);
        }


        /// <summary>
        /// This method parses the HTTP request sent by the broswer, and serves the appropriate web page.
        /// </summary>
        /// <param name="state">The socket state representing the connection with the client</param>
        private static void ServeHttpRequest(SocketState state)
        {
            string request = state.GetStringBuilder().ToString();

            // Print it for debugging/examining
            Console.WriteLine("received http request: " + request);


            // If the browser requested the home page (e.g., localhost/)
            if (request.Contains("GET / HTTP/1.1"))
                Network.SendAndClose(state.GetSocket(), httpOkHeader + "<h2>here is a web page!</h2>");
            // Otherwise, our very simple web server doesn't recognize any other URL
            else
                Network.SendAndClose(state.GetSocket(), httpBadHeader + "<h2>page not found</h2>");
        }

        // NOTE: The above SendAndClose calls are an addition to the Networking library.
        //       The only difference from the basic Send method is that this method uses a callback
        //       that closes the socket after ending the send.

    }
}
