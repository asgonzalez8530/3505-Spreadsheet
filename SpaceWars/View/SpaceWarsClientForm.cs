using NetworkController;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Media;

namespace View
{
    /// <summary>
    /// The form for the Client of a SpaceWars game. 
    /// Handles the View and Controller.
    /// </summary>
    public partial class SpaceWarsClientForm : Form
    {
        /// <summary>
        /// The size of the World.
        /// </summary>
        int worldSize;

        /// <summary>
        /// The ID for this Client (player)
        /// </summary>
        int playerID;

        /// <summary>
        /// Boolean indicating if a particular command request has been input.
        /// </summary>
        bool isLeftOn, isRightOn, isThrustOn, isFireOn;

        /// <summary>
        /// The Socket we're connecting to the Server on.
        /// </summary>
        Socket theServer;

        /// <summary>
        /// This client's World. Populated by the server.
        /// </summary>
        World theWorld;

        /// <summary>
        /// The drawingPanel for this Client.
        /// </summary>
        DrawingPanel drawingPanel;

        /// <summary>
        /// The scoreboard for this Client.
        /// </summary>
        ScoreboardPanel scoreboardPanel;

        /// <summary>
        /// An array of laserSFX MediaPlayers.
        /// </summary>
        MediaPlayer[] laserFireSFX;

        /// <summary>
        /// An array of projImpact MediaPlayers.
        /// </summary>
        MediaPlayer[] projImpactSFX;

        /// <summary>
        /// An array of shipDestroyed MediaPlayers.
        /// </summary>
        MediaPlayer[] shipDestroyedSFX;

        /// <summary>
        /// A set of ships whose death has been observed. Used for playing SFX.
        /// </summary>
        HashSet<int> deadShipIDs;

        /// <summary>
        /// Construct a new SpaceWarsClientForm, using a default size for the World.
        /// </summary>
        public SpaceWarsClientForm()
        {
            InitializeComponent();

            // use a default world size when the form is first made
            // this will change when we receive the world from the server
            worldSize = 750;
            theWorld = new World(worldSize);

            // initialize command request booleans to false
            isLeftOn = false;
            isRightOn = false;
            isThrustOn = false;
            isFireOn = false;

            // Set up the windows Form
            ClientSize = new Size(worldSize, worldSize + menuStrip.Height);
            // Make sound effects arrays
            laserFireSFX = MakeLaserSFX();
            projImpactSFX = MakeImpactSFX();
            shipDestroyedSFX = MakeDestroyedSFX();

            // Make set for dead ships
            deadShipIDs = new HashSet<int>();

            frameTimer.Start();
        }

        /// <summary>
        /// When the frameTimer interval elapses redraw the world.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrameTimer_Tick(object sender, EventArgs e)
        {
            // Invalidate this form and all its children (true)
            // This will cause the form to redraw as soon as it can
            MethodInvoker m = new MethodInvoker(() => { this.Invalidate(true); });
            try
            {
                this.Invoke(m);
            }
            catch (Exception)
            {
                // this try catch protects from an exception being thrown when the program is closed.
            }
        }

        /// <summary>
        /// Try to connect when the user clicks the connect button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (serverTextBox.Text == "")
            {
                MessageBox.Show("You didn't enter a server address, try again.", "Warning!");
                return;
            }

            if (nameTextBox.Text == "")
            {
                MessageBox.Show("Please enter a name!", "Warning!");
                return;
            }

            // Disable serverTextBox, nameTextBox and connectButton
            connectButton.Enabled = false;
            serverTextBox.Enabled = false;
            nameTextBox.Enabled = false;

            try
            {
                // Connect to the server, specifying the first thing we want to do once a connection is made is FirstContact
                theServer = Networking.ConnectToServer(FirstContact, serverTextBox.Text);   //callMe delegate and IPaddress.
            }
            catch (Exception)
            {
                NetworkError();
            }

        }


        /// <summary>
        /// When a key is down, determine which key it is and set its respective command request boolean to true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpaceWarsClientForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                isLeftOn = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                isRightOn = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                isThrustOn = true;
            }
            if (e.KeyCode == Keys.Space)
            { 
                // stop the system from making its warning sound
                e.Handled = true;
                e.SuppressKeyPress = true;
                isFireOn = true;
            }
        }


        /// <summary>
        /// Send our current command requests to the server.
        /// </summary>
        private void SendCommandRequests()
        {
            string commands = "";
            if (isLeftOn)
            {
                commands += "L";
            }
            if (isRightOn)
            {
                commands += "R";
            }
            if (isThrustOn) 
            {
                commands += "T";
            }
            if (isFireOn)
            {
                commands += "F";
            }

            if (commands != "")
            {
                Networking.Send(theServer, "(" + commands + ")" + "\n");
            }
        }


        /// <summary>
        /// When a key is up, determine which key it is and set its respective command request boolean to false.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpaceWarsClientForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 37) // left
            {
                isLeftOn = false;
            }
            if (e.KeyValue == 39) // right
            {
                isRightOn = false;
            }
            if (e.KeyValue == 38) // up
            {
                isThrustOn = false;
            }
            if (e.KeyValue == 32) // space
            {
                isFireOn = false;
            }
        }


        /// <summary>
        /// Display message and re-enable button and text boxes.
        /// </summary>
        private void NetworkError()
        {
            MessageBox.Show("There was a network connection error. Please try again!", "Error");

            try
            {
                MethodInvoker enableControls = new MethodInvoker(() =>
                    {
                        connectButton.Enabled = true;
                        serverTextBox.Enabled = true;
                        nameTextBox.Enabled = true;
                    });

                Invoke(enableControls);
            }
            catch (Exception)
            {
                // prevents from exception being thrown when client is closed
            }
        }


        /// <summary>
        /// The first step of the 3-way handshake. Change the callMe delegate to the client's 
        /// next step of the handshake.
        /// </summary>
        /// <param name="state"></param>
        private void FirstContact(SocketState state)
        {
            if (theServer.Connected && !state.hasError)
            {
                state.callMe = ReceiveStartup;
                Networking.Send(state.theSocket, nameTextBox.Text + "\n");
                Networking.GetData(state);
            }
            else
            {
                NetworkError();
            }
        }

        /// <summary>
        /// The client's last part of the 3-way handshake. Change the callMe delegate to its final
        /// incarnation, to continue receiving and processing the World from the Server. If something went wrong 
        /// in the NetworkingLibrary, the SocketState will have an error, and the user will have
        /// another chance to connect.
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveStartup(SocketState state)
        {
            if (state.hasError)
            {
                NetworkError();
            }

            if (state.sBuilder == null)
            {
                return;
            }
            // get the player ID and world size out of state.sBuilder
            string info = state.sBuilder.ToString();
            string[] infoArray = info.Split('\n');

            if (infoArray.Length < 2)
            {
                return;
            }
            if(!(Int32.TryParse(infoArray[0], out playerID) && Int32.TryParse(infoArray[1], out worldSize)))
            { 
                return;
            }

            // recreate the World
            theWorld = new World(worldSize);

            // create the drawingPanel, based on the new World.
            drawingPanel = new DrawingPanel(theWorld);
            drawingPanel.Location = new Point(0, menuStrip.Height);
            drawingPanel.Size = new Size(worldSize, worldSize);

            // create the scoreboardPanel, based on the new World.
            scoreboardPanel = new ScoreboardPanel(theWorld);
            scoreboardPanel.Location = new Point(worldSize, menuStrip.Height);

            // Ask the form to Invoke Methods to add the drawingPanel and resize the form
            MethodInvoker addDrawingPanel = new MethodInvoker(() => this.Controls.Add(drawingPanel));
            MethodInvoker addScorePanel = new MethodInvoker(() => this.Controls.Add(scoreboardPanel));
            MethodInvoker resizeClientForm = new MethodInvoker(() => ClientSize = new Size(worldSize + scoreboardPanel.Width, worldSize + menuStrip.Height));
            try
            {
                this.Invoke(resizeClientForm);
                this.Invoke(addDrawingPanel);
                this.Invoke(addScorePanel);
            }
            catch (Exception)
            {
            }

            state.sBuilder.Clear();

            // Update the action to take when network events happen
            state.callMe = ReceiveWorld;

            // Start waiting for data
            Networking.GetData(state);
        }


        /// <summary>
        /// Shows an explanatory message box when the user clicks on the controls menu item.
        /// </summary>
        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("UP: thrust \nLEFT/RIGHT: turn \nSPACE: fire", "Controls");
        }


        /// <summary>
        /// Shows an explanatory message box when the user clicks on the controls menu item.
        /// </summary>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Authors: Jac and Bekah \nHP surrounds each player. \nsounds included! \nscoreboard is sorted", "About");
        }

        /// <summary>
        /// Invoked when the Server sends the Client data about the state of the World. Processes
        /// this data and updates the World.
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveWorld(SocketState state)
        {
            // if we're connected and have no errors, begin receiving the world
            if (theServer.Connected && !state.hasError)
            {
                SendCommandRequests();
                //check out ProcessMessage for solid starting point
                string totalData = state.sBuilder.ToString();

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

                    UpdateWorld(p);

                    state.sBuilder.Remove(0, p.Length);
                }

                // now ask for more data. this will start an event loop.
                Networking.GetData(state);
            }
            else
            {
                NetworkError();
            }
        }

        /// <summary>
        /// Takes information about an object in the World from the server and updates that object.
        /// </summary>
        /// <param name="message"></param>
        private void UpdateWorld(string message)
        {
            JObject obj = JObject.Parse(message);
            JToken ship = obj["ship"];
            JToken star = obj["star"];
            JToken proj = obj["proj"];

            // figure out the type of object we have and tell the world to update
            if (ship != null)
            {
                Ship rebuilt = JsonConvert.DeserializeObject<Ship>(message);
                lock (theWorld.shipLock)
                {
                    int shipID = rebuilt.GetID();
                    theWorld.UpdateShip(rebuilt, shipID);

                    // if the ship just died, play the shipDestroyed SFX and add the ship
                    // to the list of ships that we already know are dead
                    if(rebuilt.GetHP() == 0 && !deadShipIDs.Contains(shipID))
                    {
                        PlaySFX(shipID, shipDestroyedSFX);
                        deadShipIDs.Add(shipID);
                    }
                    else if (rebuilt.GetHP() != 0 && deadShipIDs.Contains(shipID))
                    {
                        deadShipIDs.Remove(shipID);
                    }
                }
            }
            else if (star != null)
            {
                Star rebuilt = JsonConvert.DeserializeObject<Star>(message);
                lock (theWorld.starLock)
                {
                    theWorld.UpdateStar(rebuilt, (int)star); 
                }
            }
            // if the object is a projectile, updateProjectiles and play appropriate SFX
            else if (proj != null)
            {
                Projectile rebuilt = JsonConvert.DeserializeObject<Projectile>(message);
                lock (theWorld.projLock)
                {
                    theWorld.UpdateProjectile(rebuilt, (int)proj);

                    // if proj is a new projectile, play laserFire sound
                    if (theWorld.newProjIDs.Contains((int)proj))
                    {
                        PlaySFX((int)proj, laserFireSFX);
                    }
                    // if rebuilt is dead but not off the screen in any direction, 
                    // play the impact SFX
                    if (!rebuilt.IsAlive() && DirectHit(rebuilt))
                    {
                        PlaySFX((int)proj, projImpactSFX);
                    }
                }
            }
        }

        /// <summary>
        /// Helper method for playing SFX.
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="SFX"></param>
        private void PlaySFX(int objID, MediaPlayer[] SFX)
        {
            int SFXid = objID % SFX.Length;
            MethodInvoker stopSFX = new MethodInvoker(() => SFX[SFXid].Stop());
            MethodInvoker playSFX = new MethodInvoker(() => SFX[SFXid].Play());

            // if the sound is playing already stop it (ideally, we'd check if it was playing...)
            try
            {
                BeginInvoke(stopSFX);
                BeginInvoke(playSFX);
            }
            catch(Exception)
            {
                // catches exceptions when an attempt is made to access a disposed object
            }
        }

        /// <summary>
        /// Helper method for determining if a projectile is within the bounds of the world
        /// </summary>
        /// <param name="rebuilt"></param>
        /// <returns></returns>
        private bool DirectHit(Projectile rebuilt)
        {
            return ((rebuilt.GetLocation().GetX() < worldSize / 2) &&
                        (rebuilt.GetLocation().GetX() > -(worldSize / 2)) &&
                        (rebuilt.GetLocation().GetY() < worldSize / 2) &&
                        (rebuilt.GetLocation().GetY() > -(worldSize / 2)));
        }

        /// <summary>
        /// Make array for storing and loading laser fire SFX MediaPlayers
        /// </summary>
        /// <returns></returns>
        private MediaPlayer[] MakeLaserSFX()
        {
            // make the array
            MediaPlayer[] meArray = new MediaPlayer[4];

            // make the MediaPlayers
            MediaPlayer laserHi = new MediaPlayer();
            MediaPlayer laserLo = new MediaPlayer();
            MediaPlayer laserLongHi = new MediaPlayer();
            MediaPlayer laserLongLo = new MediaPlayer();

            // Load the .wav files
            laserHi.Open(new Uri(Path.GetFullPath("..\\..\\..\\Resources\\SFX\\Laser_hi.wav")));
            laserLo.Open(new Uri(Path.GetFullPath("..\\..\\..\\Resources\\SFX\\Laser_lo.wav")));
            laserLongHi.Open(new Uri(Path.GetFullPath("..\\..\\..\\Resources\\SFX\\Laser_long_hi.wav")));
            laserLongLo.Open(new Uri(Path.GetFullPath("..\\..\\..\\Resources\\SFX\\Laser_long_lo.wav")));

            // store the loaded MediaPlayers
            meArray[0] = laserHi;
            meArray[1] = laserLo;
            meArray[2] = laserLongHi;
            meArray[3] = laserLongLo;

            return meArray;
        }

        /// <summary>
        /// Make array for storing and loading laser impact SFX MediaPlayers
        /// </summary>
        /// <returns></returns>
        private MediaPlayer[] MakeImpactSFX()
        {
            // make the array
            MediaPlayer[] meArray = new MediaPlayer[2];

            // make the MediaPlayers
            MediaPlayer impact1 = new MediaPlayer();
            MediaPlayer impact2 = new MediaPlayer();

            // load the .wav files
            impact1.Open(new Uri(Path.GetFullPath("..\\..\\..\\Resources\\SFX\\LaserImpact.wav")));
            impact2.Open(new Uri(Path.GetFullPath("..\\..\\..\\Resources\\SFX\\LaserImpact.wav")));

            // store the loaded MediaPlayers
            meArray[0] = impact1;
            meArray[1] = impact2;

            return meArray;
        }

        /// <summary>
        /// Make array for storing and loading ship destroyed SFX MediaPlayers
        /// </summary>
        /// <returns></returns>
        private MediaPlayer[] MakeDestroyedSFX()
        {
            // make the array
            MediaPlayer[] meArray = new MediaPlayer[2];

            // make the MediaPlayers
            MediaPlayer destroyed1 = new MediaPlayer();
            MediaPlayer destroyed2 = new MediaPlayer();

            // load the .wav files
            destroyed1.Open(new Uri(Path.GetFullPath("..\\..\\..\\Resources\\SFX\\Ballistar_AttackImpact.wav")));
            destroyed2.Open(new Uri(Path.GetFullPath("..\\..\\..\\Resources\\SFX\\Ballistar_AttackImpact.wav")));

            // store the loaded MediaPlayers
            meArray[0] = destroyed1;
            meArray[1] = destroyed2;

            return meArray;
        }

    }
}
