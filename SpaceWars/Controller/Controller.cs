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

            //window.enterConnectEvent += GetConnected();
            

        }

        private Action GetConnected()
        {
            throw new NotImplementedException();
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


        /// <summary>
        /// Takes a SocketState object, state, extracts the player ID and world
        /// size, updates the network action and waits for more data. 
        /// </summary>
        private void ReceiveStartup(SocketState state)
        {

            // get the player ID and world size out of state.sb
            IEnumerable<string> tokens = GetTokens(state.GetStringBuilder());
            List<string> IDAndWorldSize = new List<string>();
            foreach (string token in tokens)
            {
                IDAndWorldSize.Add(token);

                // we only want two tokens
                if (IDAndWorldSize.Count >= 2)
                {
                    break;
                }
            }

            // see if we actually got out two tokens, should be world size and 
            // player id, if we did, process them and change network action
            if (IDAndWorldSize.Count == 2)
            {
                // remove tokens from StringBuilder should be size of tokens
                int sizeOfTokensAndNewLines = IDAndWorldSize[0].Length + IDAndWorldSize[1].Length;
                state.GetStringBuilder().Remove(0, sizeOfTokensAndNewLines);

                // Update the action to take when network events happen
                state.SetNetworkAction(ProcessMessage);

                // parse the id and worldsize and set them in our client
                GetWorldSizeAndID(IDAndWorldSize, out int ID, out int worldSize);
                SetPlayerID(ID);
                SetWorldSize(worldSize);
                
            }

            // Start waiting for data
            Network.GetData(state);

        }

        /// <summary>
        /// Helper method for ReceiveStartup
        /// Takes in a List<string> object of size 2, parses the strings as int
        /// and sets them to the out parameters id and worldsize respectively. If 
        /// IDAndWorldSize is smaller than size 2, behavior is undefined
        /// </summary>
        private void GetWorldSizeAndID(List<string> IDAndWorldSize, out int id, out int worldSize)
        {
            // id is sent by server first
            int.TryParse(IDAndWorldSize[0], out id);
            // world size is sent by server second
            int.TryParse(IDAndWorldSize[1], out worldSize);
        }

        //TODO: implement when we have gui
        private void SetPlayerID(int iD)
        {
            throw new NotImplementedException();
        }

        //TODO: implement when we have gui
        private void SetWorldSize(int worldSize)
        {
            throw new NotImplementedException();
        }

        

        /// <summary>
        /// Takes n StringBuilder sb and returns an IEnumerable<string> that
        /// enumerates the '\n' delimited strings in sb.
        /// </summary>
        private IEnumerable<string> GetTokens(StringBuilder sb)
        {
            // split sb into tokens
            string[] parts = Regex.Split(sb.ToString(), @"(?<=[\n])");

            // iterator for keeping track of number of tokens found
            foreach(string part in parts)
            {

                // if it is an empty string ignore it
                if (part.Length < 1)
                {
                    continue;
                }

                // regex will give a the last token no matter if it ends with \n
                if (part[part.Length - 1] != '\n')
                {
                    break;
                }

                // return the token
                yield return part.Substring(0, part.Length);
            }
        }

        // TODO: reimplement for SpaceWars
        /// <summary>
        /// This method acts as a NetworkAction delegate (see Networking.cs)
        /// When used as a NetworkAction, this method will be called whenever the Networking code
        /// has an event (ConnectedCallback or ReceiveCallback)
        /// </summary>
        private void ProcessMessage(SocketState state)
        {
            IEnumerable<string> messages = GetTokens(state.GetStringBuilder());

            // Loop until we have processed all messages.
            // We may have received more than one.

            foreach (string message in messages)
            { 

                // TODO: this.Invoke is a reference in the windows.forms dll ie our view
                // we will need to implement a method for doing this in through our interface

                // Display the message
                // "messages" is the big message text box in the form.
                // We must use a MethodInvoker, because only the thread that created the GUI can modify it.
                // This method will be invoked by another thread from the Networking callbacks.
                // this.Invoke(new MethodInvoker(
                //  () => messages.AppendText(p)));

                // Then remove the processed message from the SocketState's growable buffer
                state.GetStringBuilder().Remove(0, message.Length);
            }

            // Now ask for more data. This will start an event loop.
            Network.GetData(state);
        }




    }// end of Controller class
}
