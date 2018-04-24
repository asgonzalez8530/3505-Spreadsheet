using NetworkController;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;

namespace SpaceWars
{
    class Server
    {
        /// <summary>
        /// The last unique ID assigned to a player and socket. Starts at -1.
        /// </summary>
        private int lastUniqueID;

        /// <summary>
        /// The last unique ID assigned to a projectile.
        /// </summary>
        private int lastProjID;

        /// <summary>
        /// The backing World object for the server.
        /// </summary>
        private World myWorld;

        /// <summary>
        /// A list of unique that this server is connected to.
        /// </summary>
        private Dictionary<int, SocketState> clients;

        /// <summary>
        /// A stopwatch used for sending updates to clients.
        /// </summary>
        private Stopwatch watch;

        /// <summary>
        /// Contains the current command requests for each ship.
        /// </summary>
        private Dictionary<int, string> shipCommands;

        /// <summary>
        /// Keeps track of the frame time.
        /// </summary>
        private int frameCounter;

        /// <summary>
        /// The number of frames between when a client can fire a projectile.
        /// </summary>
        private int projFiringDelay;

        /// <summary>
        /// ms between when frame updates
        /// </summary>
        private int timePerFrame;

        /// <summary>
        /// Extra game mode, keeps the ship safe for 100 frames after respawning.
        /// </summary>
        private bool safeRespawnMode;


        static void Main(string[] args)
        {
            Server theServer = new Server("..\\..\\..\\Resources\\ServerSettings.xml");

            Networking.ServerAwaitingClientLoop(theServer.HandleNewClient);

            while (true) 
            {
                theServer.Update();
            }
        }


        /// <summary>
        /// Creates a Server object to be used in SpaceWars from the given XML file.
        /// </summary>
        private Server(string xmlFile)
        {
            // get setting from XML file
            List<Star> starList = new List<Star>();
            Dictionary<string, double> settingsDictionary = GetServerSettings(xmlFile, out starList);

            projFiringDelay = 6;
            timePerFrame = 16;


            if (settingsDictionary != null)
            {
                // construct the world from the settings
                myWorld = new World((int)settingsDictionary["worldSize"],
                            settingsDictionary["engineStrength"], (int)settingsDictionary["projSpeed"],
                            (int)settingsDictionary["turningRate"], (int)settingsDictionary["starSize"],
                            (int)settingsDictionary["shipSize"], (int)settingsDictionary["respawnRate"],
                            (int)settingsDictionary["startingHP"], safeRespawnMode);

                foreach(Star star in starList)
                {
                    myWorld.UpdateStar(star, star.GetID());
                }

                lastUniqueID = -1;
                lastProjID = -1;
                clients = new Dictionary<int, SocketState>();

                shipCommands = new Dictionary<int, string>();
                frameCounter = 0;
                watch = new Stopwatch();
                watch.Start(); 
            }
            else
            {
                Console.WriteLine("Failed to read settings file.");
                Console.Read();
            }
        }

        private Dictionary<string, double> GetServerSettings(string xmlFilePath, out List<Star> starList)
        {
            starList = new List<Star>();
            try
            {
                Dictionary<string, double> settings = new Dictionary<string, double>();
                int tempX = 0;
                int tempY = 0;
                double tempMass = 0.0;
                int tempStarID = -1;
                using (XmlReader reader = XmlReader.Create(xmlFilePath))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "UniverseSize":
                                    reader.Read();
                                    settings.Add("worldSize", Double.Parse(reader.Value));
                                    break;
                                case "MSPerFrame":
                                    reader.Read();
                                    timePerFrame = Int32.Parse(reader.Value);
                                    break;
                                case "FramesPerShot":
                                    reader.Read();
                                    projFiringDelay = Int32.Parse(reader.Value);
                                    break;
                                case "RespawnRate":
                                    reader.Read();
                                    settings.Add("respawnRate", Int32.Parse(reader.Value));
                                    break;
                                case "EngineStrength":
                                    reader.Read();
                                    settings.Add("engineStrength", Double.Parse(reader.Value));
                                    break;
                                case "ProjSpeed":
                                    reader.Read();
                                    settings.Add("projSpeed", Int32.Parse(reader.Value));
                                    break;
                                case "TurningRate":
                                    reader.Read();
                                    settings.Add("turningRate", Int32.Parse(reader.Value));
                                    break;
                                case "StarSize":
                                    reader.Read();
                                    settings.Add("starSize", Int32.Parse(reader.Value));
                                    break;
                                case "ShipSize":
                                    reader.Read();
                                    settings.Add("shipSize", Int32.Parse(reader.Value));
                                    break;
                                case "StartingHP":
                                    reader.Read();
                                    settings.Add("startingHP", Int32.Parse(reader.Value));
                                    break;
                                case "SafeRespawnMode":
                                    reader.Read();
                                    safeRespawnMode = Boolean.Parse(reader.Value);
                                    break;
                                case "Star":
                                    reader.Read();
                                    while (!reader.Name.Equals("Star") && reader.Read())
                                    {
                                        if (reader.IsStartElement())
                                        {
                                            switch (reader.Name)
                                            {
                                                case "x":
                                                    reader.Read();
                                                    tempX = Int32.Parse(reader.Value);
                                                    break;
                                                case "y":
                                                    reader.Read();
                                                    tempY = Int32.Parse(reader.Value);
                                                    break;
                                                case "mass":
                                                    reader.Read();
                                                    tempMass = Double.Parse(reader.Value);
                                                    break;
                                            }
                                        }
                                        //}
                                        //else if (reader.Name.Equals("Star") && reader.NodeType == XmlNodeType.EndElement)
                                        //{
                                        //}
                                    }
                                        starList.Add(new Star(++tempStarID, tempMass, new Vector2D(tempX, tempY)));
                                    break;
                            }
                        }
                    }
                }

                return settings;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// This is the delegate callback passed to the networking class to handle a new client connecting.
        /// </summary>
        /// <param name="state">the state the contains the socket for this new client</param>
        private void HandleNewClient(SocketState state)
        {
            state.callMe = ReceiveName;
            Networking.GetData(state);
        }


        /// <summary>
        /// This is a "callMe" delegate that implements the server's part of the initial handshake.
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveName(SocketState state)
        {
            if (Regex.IsMatch(state.sBuilder.ToString(), "\\Z\\n"))
            {
                string playerName = state.sBuilder.ToString(0, state.sBuilder.Length - 1);
                int newUniqueID = ++lastUniqueID;

                Ship newShip = new Ship(newUniqueID, playerName, myWorld.GetStartingHP());
                lock (myWorld)
                {
                    newShip.SetLocation(myWorld.MakeSpawnPoint());
                    myWorld.UpdateShip(newShip, newUniqueID);
                    shipCommands.Add(newUniqueID, "");
                }

                state.ID = newUniqueID;

                state.callMe = HandleCommandRequests;

                // Send the startup info to the client (ID and world size). 
                Networking.Send(state.theSocket, "" + newUniqueID + "\n" + myWorld.GetSize() + "\n");

                // Then add the new client's socket to a list of all clients. 
                lock (clients)
                {
                    clients.Add(newUniqueID, state);
                }

                state.sBuilder.Clear();

                // Ask the client for data to continue the loop
                Networking.GetData(state);

                Console.WriteLine("Added Client " + newUniqueID + ". YAY!"); 
            }
            else
            {
                Console.WriteLine("Failed to add Client. . . :(");
            }
        }


        /// <summary>
        /// Invoked when the Client sends the Server command requests. Process these requests
        /// and updates the world.
        /// </summary>
        /// <param name="state"></param>
        private void HandleCommandRequests(SocketState state)
        {
            if (state.theSocket.Connected && !state.hasError)
            {
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

                    ProcessCommands(p, state.ID);

                    state.sBuilder.Remove(0, p.Length);
                }

                // now ask for more data. this will start an event loop.
                Networking.GetData(state);
            }
            else
            {
                DisconnectClient(state);
            }
        }

        /// <summary>
        /// Helper method to gracefully handle disconnecting clients.
        /// </summary>
        /// <param name="state"></param>
        private void DisconnectClient(SocketState state)
        {
            lock (clients)
            {
                clients.Remove(state.ID);
            }
            lock (myWorld)
            {
                myWorld.RemoveShip(state.ID);
                shipCommands.Remove(state.ID);
            }
            Console.WriteLine("Disconnected Client " + state.ID + ". :(");
        }


        /// <summary>
        /// Process the commands, and store them to be applied later.
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="shipID"></param>
        private void ProcessCommands(string commands, int shipID)
        {
            string current = "";
            if (!Regex.IsMatch(commands, "[^RTFL)(\n]"))
            {
                if (commands.Contains("T"))
                {
                    current += "T";
                }
                if (commands.Contains("F"))
                {
                    current += "F";
                }
                if (commands.Contains("R"))
                {
                    current += "R";
                }
                if (commands.Contains("L"))
                {
                    current += "L";
                }

                lock (myWorld)
                {
                    if (shipCommands.ContainsKey(shipID))
                    {
                        shipCommands[shipID] = current;
                    }
                }
            }
            else
            {
                DisconnectClient(clients[shipID]);
            }
        }


        /// <summary>
        /// Applies commands to the World, increments frame counter, Updates the World, 
        /// Serializes the World, and sends to each of the clients. Happens once per frame.
        /// </summary>
        private void Update()
        {
            while (watch.ElapsedMilliseconds < timePerFrame) { /* do nothing */ }

            watch.Restart();
            ApplyCommands();
            frameCounter++;

            // make a string to send to all clients
            string messageTosend = "";

            // update the World and build string of it
            lock (myWorld)
            {
                myWorld.Update(frameCounter);
                foreach (Ship ship in myWorld.GetShips())
                {
                    messageTosend += JsonConvert.SerializeObject(ship).ToString() + "\n";
                }
                foreach (Star star in myWorld.GetStars())
                {
                    messageTosend += JsonConvert.SerializeObject(star).ToString() + "\n";
                }
                foreach (Projectile proj in myWorld.GetProjectiles())
                {
                    messageTosend += JsonConvert.SerializeObject(proj).ToString() + "\n";
                }
            }

            // send to each client
            lock (clients)
            {
                foreach (SocketState state in clients.Values)
                {
                    Networking.Send(state.theSocket, messageTosend.Substring(0, messageTosend.Length)); 
                }
            }

            if (frameCounter > (Int32.MaxValue / 2))
            {
                frameCounter = 0;
            }
        }

        
        /// <summary>
        /// Check the stored states, and applies the command requests appropriately to the World.
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="shipID"></param>
        private void ApplyCommands()
        {
            lock (myWorld)
            {
                // foreach ship's commands
                Dictionary<int, string> commandsCopy = new Dictionary<int, string>(shipCommands);
                foreach (KeyValuePair<int, string> kvp in commandsCopy)
                {
                    int shipID = kvp.Key;
                    Ship ship = myWorld.GetShip(shipID);
                    bool canFire = (frameCounter >= (ship.TimeLastFired + projFiringDelay) || ship.TimeLastFired > frameCounter);

                    // if it's a T, switch Thrust to true
                    if (kvp.Value.Contains("T"))
                    {
                        ship.ThrustOn();
                    }
                    else
                    {
                        ship.ThrustOff();
                    }

                    // if it's an F, make a new projectile
                    if (kvp.Value.Contains("F") && canFire && ship.GetHP() > 0)
                    {
                        myWorld.FireProjectile(shipID, ++lastProjID);
                        myWorld.GetShip(shipID).TimeLastFired = frameCounter;
                    }

                    // both R and L
                    if (kvp.Value.Contains("R") && kvp.Value.Contains("L"))
                    {
                        if (frameCounter % 2 == 0)
                        {
                            myWorld.RotateShipRight(shipID);
                        }
                        else
                        {
                            myWorld.RotateShipLeft(shipID);
                        }
                    }
                    else
                    {
                        // if it's an R
                        if (kvp.Value.Contains("R"))
                        {
                            myWorld.RotateShipRight(shipID);
                        }
                        // if it's an L
                        if (kvp.Value.Contains("L"))
                        {
                            myWorld.RotateShipLeft(shipID);
                        } 
                    }

                    // clear the applied commands
                    shipCommands[kvp.Key] = "";
                } 
            }
        }
    }
}
