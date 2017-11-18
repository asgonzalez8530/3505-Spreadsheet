﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceWarsView;
using Communication;
using System.Text.RegularExpressions;
using SpaceWars;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Net.Sockets;

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
        World theWorld;
        Controls theControls;

        /// <summary>
        /// Takes an object which implements the ISpaceWarsWindow interface, SpaceWarsWindow
        /// and creates a Controller associated with that instance. 
        /// </summary>
        public Controller(ISpaceWarsWindow SpaceWarsWindow)
        {
            theControls = new Controls();

            // keep a reference to the window associated with this controller
            window = SpaceWarsWindow;

            window.enterConnectEvent += GetConnected;
            window.ControlKeyDownEvent += ControlKeyDownHandler;
            window.ControlKeyUpEvent += ControlKeyUpHandler;

            theWorld = window.GetWorldPanelWorld();


        }

        private void ControlKeyDownHandler(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                theControls.Fire = true;
            }
            if (e.KeyCode == Keys.Left)
            {
                theControls.Left = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                theControls.Right = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                theControls.Thrust = true;
            }
        }

        private void ControlKeyUpHandler(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                theControls.Fire = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                theControls.Left = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                theControls.Right = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                theControls.Thrust = false;
            }
        }

        private void GetConnected()
        {
            string serverAddress = window.GetServer();

            // TODO: more error checking
            if (serverAddress == "")
            {
                window.DisplayMessageBox("Please enter a server address");
                return;
            }


            // Disable the controls and try to connect
            window.SetServerBoxInactive();
            window.SetUserBoxInactive();
            window.SetConnectButtonInactive();

            // Connect to the server, specifying the first thing we want to do once a connection is made is FirstContact
            Network.ConnectToServer(FirstContact, serverAddress);
        }

        /// <summary>
        /// Takes a SocketState object, state, and initiates a game with the
        /// server associated with state by sending the name given by the user. 
        /// </summary>
        private void FirstContact(SocketState state)
        {

            string name = window.GetUserName();

            // begin "handshake" by sending name
            Network.Send(state.GetSocket(), name);

            // Change the action that is take when a network event occurs. Now when data is received,
            // the Networking library will invoke ReceiveStartup
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
                //SetPlayerID(ID);
                SetWorldSize(worldSize);

                // now that we have a connection, we can start sending controls
                StartSendingControls(state.GetSocket());

            }

            // Start waiting for data
            Network.GetData(state);

        }

        /// <summary>
        /// Starts sending the active controls on the socket every time the frame
        /// is redrawn
        /// </summary>
        private void StartSendingControls(Socket socket)
        {

            window.GetFrameTimer().Elapsed += (x, y) => SendControls(socket);

        }

        /// <summary>
        /// Sends the active controls on the Socket, s.
        /// </summary>
        private void SendControls(Socket s)
        {
            if (theControls.HasActiveControls())
            {
                Network.Send(s, theControls.GetControls());
            }
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
            // set the size of the world
            theWorld.SetSize(worldSize);

            // updates the size of the worldPanel
            window.UpdateWorldSize(worldSize);

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
            foreach (string part in parts)
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
                JObject obj = JObject.Parse(message);
                JToken ship = obj["ship"];
                JToken proj = obj["proj"];
                JToken star = obj["star"];

                Ship theShip = null;
                Projectile theProj = null;
                Star theStar = null;

                if (ship != null)
                {
                    theShip = JsonConvert.DeserializeObject<Ship>(message);
                }
                if (proj != null)
                {
                    theProj = JsonConvert.DeserializeObject<Projectile>(message);
                }
                if (star != null)
                {
                    theStar = JsonConvert.DeserializeObject<Star>(message);
                }
                //}

                lock (theWorld)
                {
                    theWorld.AddStar(theStar);
                    theWorld.AddShip(theShip);
                    theWorld.AddProjectile(theProj);
                }

                // Then remove the processed message from the SocketState's growable buffer
                state.GetStringBuilder().Remove(0, message.Length);
            }

            // Now ask for more data. This will start an event loop.
            Network.GetData(state);
        }

        /// <summary>
        /// Simple class that contains the state of controls for a SpaceWars game
        /// </summary>
        private class Controls
        {
            public bool Right { private get; set; }
            public bool Left { private get; set; }
            public bool Thrust { private get; set; }
            public bool Fire { private get; set; }

            public Controls()
            {
                Right = false;
                Left = false;
                Thrust = false;
                Fire = false;
            }

            /// <summary>
            /// Returns an '\n' terminated string of all controls which are
            /// currently active enclosed in parentheses. For example, if all
            /// controlls are active, would return the string "(RLTF)\n"
            /// </summary>
            public string GetControls()
            {
                string controls = "(";

                if (Right)
                {
                    controls += "R";
                }

                if (Left)
                {
                    controls += "L";
                }

                if (Thrust)
                {
                    controls += "T";
                }

                if (Fire)
                {
                    controls += "F";
                }

                controls += ")\n";

                return controls;
            }

            /// <summary>
            /// Returns true if any controls are active, else returns false
            /// </summary>
            public bool HasActiveControls()
            {
                return (Right || Left || Thrust || Fire);
            }
        }




    }// end of Controller class


}
