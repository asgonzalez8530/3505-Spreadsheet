using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceWarsView;
using Communication;
using System.Text.RegularExpressions;

namespace SpaceWarsControl
{
    /// <summary>
    /// The class Controller contains references to the SpaceWars model and view
    /// and controls the view based on the implementation of the model. 
    /// </summary>
    public class Controller
    {

        // the windows form controlled by this controller
        ISpaceWarsWindow window;

        /// <summary>
        /// Takes an object which implements the ISpaceWarsWindow interface, SpaceWarsWindow
        /// and creates a Controller associated with that instance. 
        /// </summary>
        public Controller(ISpaceWarsWindow SpaceWarsWindow)
        {
            // keep a reference to the window associated with this controller
            window = SpaceWarsWindow;

        }

        /// <summary>
        /// Takes a SocketState object, state, and initiates a game with the
        /// server associated with state by sending the name given by the user. 
        /// </summary>
        private void FirstContact(SocketState state)
        {
            // TODO: replace by player given name
            string name = "TestPlayer";

            // begin "handshake" by sending name
            Network.Send(state.GetSocket(), name);

            // Change the action that is take when a network event occurs. Now when data is received,
            // the Networking library will invoke ProcessMessage
            state.SetNetworkAction(ReceiveStartup);

            Network.GetData(state);
        }

        //TODO: Implement function as described 
        /// <summary>
        /// Takes a SocketState object, state, extracts the player ID and world
        /// size, updates the network action and waits for more data. 
        /// </summary>
        private void ReceiveStartup(SocketState state)
        {
            // It would do something like this:

            // get the player ID and world size out of state.sb
            string totalData = state.GetStringBuilder().ToString();

            // ID and world size are separated by "\n"
            int newlineCount = 0;
            int totalDataIndex = 0;
            while (newlineCount < 2 && totalDataIndex < totalData.Length)
            {
                if (totalData[totalDataIndex] == '\n')
                {
                    newlineCount++;
                }

                totalDataIndex++;
            }

            // TODO: 
            // get the first two messages which should be the id and size respectively

            // Update the action to take when network events happen
            state.SetNetworkAction(ProcessMessage);

            // Start waiting for data
            Network.GetData(state);

        }

        /// <summary>
        /// Takes an int, n and a string s. If s contains n new line terminated
        /// strings, returns a string array containing those strings, excluding
        /// the new line characters, else returns an empty string array
        /// </summary>
        private string[] GetNTokens(int n, string s)
        {
            // create an array of strings to return 
            string[] strings = new string[n];

            // we will need some sub strings, the first one will start at index 0
            int substringStart = 0;

            // tokens are delimited by "\n"
            int newlineCount = 0;
            // we are going to look linearly through s starting at index 0
            int sIndex = 0;


            while (newlineCount < n && sIndex < s.Length)
            {
                // if we have found a delimiter
                if (s[sIndex] == '\n')
                {
                    // get the length of the token
                    int subLength = sIndex - substringStart;
                    // put it in our array
                    strings[newlineCount] = s.Substring(substringStart, subLength);
                    // the next substring will start at the next index
                    substringStart = sIndex + 1;
                    // increase the count of found tokens
                    newlineCount++;
                }

                sIndex++;
            }

            // if we haven't found n tokens, return an empty array
            if (newlineCount < n)
            {
                strings = new string[0];
            }

            return strings;

        }

        // TODO: reimplement for SpaceWars
        /// <summary>
        /// This method acts as a NetworkAction delegate (see Networking.cs)
        /// When used as a NetworkAction, this method will be called whenever the Networking code
        /// has an event (ConnectedCallback or ReceiveCallback)
        /// </summary>
        private void ProcessMessage(SocketState state)
        {
            string totalData = state.GetStringBuilder().ToString();

            // Messages are separated by newline
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
                if (p[p.Length - 1] != '\n')
                    break;

                // TODO: this.Invoke is a reference in the windows.forms dll ie our view
                // we will need to implement a method for doing this in through our interface

                // Display the message
                // "messages" is the big message text box in the form.
                // We must use a MethodInvoker, because only the thread that created the GUI can modify it.
                // This method will be invoked by another thread from the Networking callbacks.
                // this.Invoke(new MethodInvoker(
                //  () => messages.AppendText(p)));

                // Then remove the processed message from the SocketState's growable buffer
                state.GetStringBuilder().Remove(0, p.Length);
            }

            // Now ask for more data. This will start an event loop.
            Network.GetData(state);
        }




    }// end of Controller class
}
