// Anastasia Gonzalez and Aaron Bellis UID: u0985898 & u0981638
// Code implemented as part of PS7 and PS8: SpaceWars client/Server CS3500 Fall Semester
using Communication;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;

namespace SpaceWarsServer
{
    class Server
    {
        private World world; // reference to the world
        private int IDCounter; // the unique id's set to clients
        private HashSet<SocketState> clients; // a set of all clients to update
        private Stopwatch watch; // a timer to update our world;
        private HashSet<int> clientsToCleanUp;

        private HashSet<PlayerStats> stats;

        /// <summary>
        /// Reports if at least one player has joined the game
        /// </summary>
        private bool FirstPlayerHasJoined { get; set; }

        // used to calculate duration of the game
        private DateTime startTime;

        // All frame counter code has been commented out in case it would be needed late
        //private int frameCounter; // a counter to calculate frame rate

        public Server()
        {
            world = new World();
            IDCounter = 0;
            clients = new HashSet<SocketState>();
            clientsToCleanUp = new HashSet<int>();
            watch = new Stopwatch();
            watch.Start();

            // file path for the game settings
            string filePath = @"../../../Resources/ServerSettings/";
            string fileName = @"settings.xml";

            // get the game settings and pass them to the world
            ReadSettingsXML(filePath + fileName);

            // stats needed to keep database updated with high scores
            stats = new HashSet<PlayerStats>();
            FirstPlayerHasJoined = false;

            startTime = DateTime.UtcNow;

            //frameCounter = 0;
            //System.Timers.Timer frameRateTimer = new System.Timers.Timer();
            //frameRateTimer.Interval = 5000;
            //frameRateTimer.Elapsed += (x,y) => PrintFrameRate();
            //frameRateTimer.Start();

        }

        /// <summary>
        /// Starts the server and begins waiting for the first client
        /// </summary>
        public void StartServer()
        {
            Network.ServerAwaitingClientLoop(HandleNewClient);
        }

        /// <summary>
        /// NetworkAction delegate which handles new client requests
        /// </summary>
        private void HandleNewClient(SocketState state)
        {
            // change the callback for the socketstate to a method which 
            // receives the player's name
            state.SetNetworkAction(ReceivePlayerName);

            // ask for more data
            Network.GetData(state);
        }

        /// <summary>
        /// NetworkAction delegate which implements the server's part of the
        /// initial handshake. 
        /// 
        /// Creates a new Ship with the given name and a new unique ID and sets
        /// the SocketState's ID. 
        /// 
        /// Changes the state's callback method to one that handles command 
        /// requests from the client then asks the clients for data.
        /// </summary>
        private void ReceivePlayerName(SocketState state)
        {
            // enumerate the complete received messages.
            IEnumerable<string> messages = GetTokens(state.GetStringBuilder());
            // find the name in the complete messages
            string playerName = "";
            if (messages.Any())
            {
                playerName = messages.First();
                state.GetStringBuilder().Clear();
            }
            // if there wasn't a complete message continue the event loop
            else
            {
                Network.GetData(state);
                return;
            }

            // the id and world are being changed by callback methods (different threads)
            int playerID = 0;
            lock (world)
            {
                playerID = IDCounter++;
                // make a ship with a the name and give it a unique id
                // add ship to world
                world.MakeNewShip(playerName.TrimEnd(), playerID);
                // set the socket state ID
                state.SetID(playerID);
            }

            if (state.HasError)
            {
                HandleNetworkError(state);
                return;
            }
            

            // change the callback to handle incoming commands from the client
            state.SetNetworkAction(HandleClientCommands);

            // send the startup info to the client (ID and world size)
            string startupInfo = playerID + "\n" + world.GetSize() + "\n";
            Network.Send(state.GetSocket(), startupInfo);

            // set FirstPlayerHasJoined to true if this is the first player and
            // get the start time
            if (!FirstPlayerHasJoined)
            {
                FirstPlayerHasJoined = true;
                // set the start time to now (current value will be server start time)
                startTime = DateTime.UtcNow;
            }

            // handshake is done print line that someone has connected
            Console.Out.WriteLine("A new Client has contacted the Server.");

            // add the client's socket to a set of all clients
            lock (clients)
            {
                clients.Add(state);
            }

            // ask the client for data (continue loop)
            Network.GetData(state);
        }

        /// <summary>
        /// Network action for processing client direction commands. 
        /// </summary>
        private void HandleClientCommands(SocketState state)
        {
            if (state.HasError)
            {
                HandleNetworkError(state);
                return;
            }

            // enumerate the complete received messages.
            IEnumerable<string> messages = GetTokens(state.GetStringBuilder());
            lock (world)
            {
                // process all of the commands now, 
                //may have recieved more than one before refreshing screen
                foreach (string command in messages)
                {
                    world.UpdateShipCommands(command, state.GetID());
                    state.GetStringBuilder().Remove(0, command.Length);
                }
            }

            // ask client for data (continue loop)
            Network.GetData(state);
        }

        /// <summary>
        /// Updates the world then sends its objects to each client
        /// </summary>
        private void UpdateWorld()
        {
           
            // update and serialize each object in world
            StringBuilder sb = new StringBuilder();
            lock (world)
            {
                // update world objects then add each to sb.
                IEnumerable<Star> stars = world.GetStars();
                foreach (Star s in stars)
                {
                    sb.Append(JsonConvert.SerializeObject(s) + "\n");
                }

                IEnumerable<Ship> ships = world.GetAllShips();
                foreach (Ship s in ships)
                {
                    world.MotionForShips(s);
                    sb.Append(JsonConvert.SerializeObject(s) + "\n");
                    s.Thrust = false;
                }

                IEnumerable<Projectile> projectiles = world.GetProjs();
                foreach (Projectile p in projectiles)
                {
                    world.MotionForProjectiles(p);
                    sb.Append(JsonConvert.SerializeObject(p) + "\n");
                }

            }

            string data = sb.ToString();
            Task sendClients = SendDataToAllClientsAsync(data);
            Task cleanup = CleanupWorldAsync();

            while (watch.ElapsedMilliseconds < world.GetMSPerFrame())
            { /* do nothing */ }
            watch.Restart();
            
            //Interlocked.Increment(ref frameCounter);

        }

        /// <summary>
        /// Asyn method for cleaning up projectiles and ships including respawn
        /// </summary>
        private async Task CleanupWorldAsync()
        {
            lock (world)
            {
                world.CleanUpProjectiles();
                world.CleanupShips();
                world.RespawnShips();
            }
        }

        /// <summary>
        /// Async method takes a string of data and sends it to all clients in client list
        /// </summary>
        private async Task SendDataToAllClientsAsync(string data)
        {
            lock (clients)
            {
                foreach (SocketState client in clients)
                {
                    if (!client.HasError)
                    {
                        Network.Send(client.GetSocket(), data);
                    }
                }
            }

        }

        /// <summary>
        /// Takes a socket state object, sets its associated ship to dead then removes the ship from 
        /// the list of clients
        /// </summary>
        private void HandleNetworkError(SocketState state)
        {
            int clientID = state.GetID();
            
            // dispose all resources used by socket
            // state.GetSocket().Dispose();

            lock (clients)
            {
                clients.Remove(state);
            }

            // the ship that needs to be cleaned up
            // needed to get stats
            Ship deadShip;
            lock (world)
            {
                deadShip = world.AddShipToCleanup(clientID);
            }

            lock (stats)
            {
                // add stats to set of all stats when cleaning up clients
                AddShipToStats(deadShip);
            }

            Console.Out.WriteLine("Player " + clientID + " has left the game");
        }

        /// <summary>
        /// Convenient method that takes a Ship playerShip, and adds its information to stats.
        /// </summary>
        /// <param name="playerShip"></param>
        private void AddShipToStats(Ship playerShip)
        {
            // add stats to set of all stats
            stats.Add(new PlayerStats(playerShip.GetName(), playerShip.GetScore(), playerShip.GetAccuracy()));
        }

        /// <summary>
        /// Takes a StringBuilder sb and returns an IEnumerable<string> that
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

                // regex will give the last token no matter if it ends with \n
                if (part[part.Length - 1] != '\n')
                {
                    break;
                }

                // return the token
                yield return part.Substring(0, part.Length);
            }
        }

        /// <summary>
        /// Reads the settings file located at filePath and creates 
        /// </summary>
        private void ReadSettingsXML(string filePath)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {

                    string x = null;
                    string y = null;
                    string mass = null;

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "SpaceSettings":
                                    break;
                                case "UniverseSize":
                                    reader.Read();
                                    world.SetUniverseSize(reader.Value);
                                    break;
                                case "MSPerFrame":
                                    reader.Read();
                                    world.SetMSPerFrame(reader.Value);
                                    break;
                                case "FramesPerShot":
                                    reader.Read();
                                    world.SetProjectileFiringDelay(reader.Value);
                                    break;
                                case "RespawnRate":
                                    reader.Read();
                                    world.SetRespawnDelay(reader.Value);
                                    break;
                                case "StartingHitPoints":
                                    reader.Read();
                                    world.SetStartingHitPoint(reader.Value);
                                    break;
                                case "ProjectileSpeed":
                                    reader.Read();
                                    world.SetProjectileSpeed(reader.Value);
                                    break;
                                case "EngineStrength":
                                    reader.Read();
                                    world.SetEngineStrength(reader.Value);
                                    break;
                                case "TurningRate":
                                    reader.Read();
                                    world.SetTurningRate(reader.Value);
                                    break;
                                case "ShipSize":
                                    reader.Read();
                                    world.SetShipSize(reader.Value);
                                    break;
                                case "StarSize":
                                    reader.Read();
                                    world.SetStarSize(reader.Value);
                                    break;
                                case "KingOfTheHill":
                                    reader.Read();
                                    world.SetKingOfTheHill(reader.Value);
                                    break;
                                case "Star":
                                    StarXMLReader(filePath);
                                    break;
                            }
                        }
                    }
                    // we're done with the reader, time to close it
                    reader.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void StarXMLReader(string filePath)
        {
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                string x = null;
                string y = null;
                string mass = null;

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "x":
                                reader.Read();
                                x = reader.Value;
                                break;
                            case "y":
                                reader.Read();
                                y = reader.Value;
                                break;
                            case "mass":
                                reader.Read();
                                mass = reader.Value;

                                // make a new star
                                world.MakeNewStar(x, y, mass);

                                // reset the x, y, and mass to null
                                x = null;
                                y = null;
                                mass = null;
                                return;
                        }
                    }
                }
            }
        }

        //TODO: may need to be public
        /// <summary>
        /// Adds all ships represented in the world to set of all PlayerStats
        /// </summary>
        private void AddAllPlayersToStats ()
        {
            lock (world)
            {
                // get all the remaining ships in the world
                IEnumerable<Ship> remainingShips = world.GetAllShips();

                // add info to playerstats.
                foreach (Ship ship in remainingShips)
                {
                    AddShipToStats(ship);
                }
            }
        }

        //TODO: complete method, may need to be public
        /// <summary>
        /// If no player has joined the game, closes the server.
        /// Else gets the duration of the game and the stats of all players then 
        /// adds them to the database.
        /// </summary>
        private void CloseServer()
        {
            // if there was nobody in the game, close server
            if (!FirstPlayerHasJoined)
            {
                return;
            }

            // get the duration of the game
            TimeSpan duration = DateTime.UtcNow - startTime;
            
            // get all the stats
            AddAllPlayersToStats();

            //TODO: add the stats to database then return

        }

        /// <summary>
        /// Prints the frame rate to the console
        /// </summary>
        //private void PrintFrameRate()
        //{
        //    int rate = Interlocked.Exchange(ref frameCounter, 0);
        //    rate = rate / 5;
        //    Console.Out.WriteLine("Frames per second: " + rate);
        //}

        public static void Main(string[] args)
        {
            Server s = new Server();
            // start listening for client requests
            s.StartServer();

            // we have successful opened a server... wait for first client
            Console.WriteLine("Server is up. Awaiting first client.");

            while (true)
            {
                s.UpdateWorld();
            }
        }
    }

    /// <summary>
    /// PlayerStats class represents an individual Player's stats  in a SpaceWars
    /// game and contains all data needed to save a players stats in a database. 
    /// PlayerStats is an immutable class.
    /// </summary>
    public class PlayerStats
    {
        /// <summary>
        /// The name which identifies these states
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The score of this player
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// The accuracy of this player
        /// </summary>
        public double Accuracy { get; private set; }

        /// <summary>
        /// Creates a PlayerStats object with the given playerName, highScore and shotAccuracy
        /// as its data. If the playerName parameter begins with "King ", will create a PlayerStats
        /// object which does not contain this string.
        /// </summary>
        public PlayerStats(string playerName, int highScore, double shotAccuracy)
        {
            Name = GetPlayerName(playerName);
            Score = highScore;
            Accuracy = shotAccuracy;
        }

        /// <summary>
        /// Takes in a string representing the player name, checks if it is the 
        /// king. If it is the king returns the name minus "King ", else returns
        /// name.
        /// </summary>
        private string GetPlayerName(string name)
        {
            // check if name begins with "King " or any case variation of this 
            // string
            if (name.ToLower().StartsWith("king "))
            {
                // return substring excluding "King "
                return name.Substring(5);
            }

            // player was not the king, return name.
            return name;
        }

        /// <summary>
        /// Returns true if this objects, Name, Score and Accuracy fields are the same
        /// values as obj's Name, Score and Accuracy fields.
        /// </summary>
        public override bool Equals(object obj)
        {
            // cast obj as PlayerStats object
            PlayerStats stat = obj as PlayerStats;
            // check is not null and all parameters match
            return stat != null && stat.Name == Name && stat.Score == Score && stat.Accuracy == Accuracy;
        }

        /// <summary>
        /// Returns the hash code for this PlayerStat object. 
        /// 
        /// If one player stat is equal to another player stat if 
        /// their fields are the same, then the two hash codes 
        /// should be the same.
        /// </summary>
        public override int GetHashCode()
        {
            string statString = Name + Score.ToString() + Accuracy.ToString();
            return statString.GetHashCode();
        }

    }
}
