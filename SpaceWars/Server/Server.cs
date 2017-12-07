// Anastasia Gonzalez and Aaron Bellis UID: u0985898 & u0981638
// Code implemented as part of PS8 : SpaceWars Server CS3500 Fall Semester
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

        public Server()
        {
            world = new World();
            IDCounter = 0;
            clients = new HashSet<SocketState>();
            watch = new Stopwatch();
            watch.Start();

            // file path for the game settings
            string filePath = @"../../../Resources/ServerSettings/";
            string fileName = @"settings.xml";

            // get the game settings and pass them to the world
            ReadSettingsXML(filePath + fileName);
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
                playerName = messages.First().TrimEnd();
                state.GetStringBuilder().Clear();
            }
            // if there wasn't a complete message continue the event loop
            else
            {
                Network.GetData(state);
            }


            // the id and world are being changed by callback methods (different threads)
            int playerID = 0;
            lock (world)
            {
                playerID = IDCounter++;
                // make a ship with a the name and give it a unique id
                // add ship to world
                world.MakeNewShip(playerName, playerID);
            }

            // set the socket state ID
            state.SetID(playerID);

            // change the callback to handle incoming commands from the client
            state.SetNetworkAction(HandleClientCommands);

            // send the startup info to the client (ID and world size)
            string startupInfo = playerID + "\n" + world.GetSize() + "\n";
            Network.Send(state.GetSocket(), startupInfo);

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
            // enumerate the complete received messages.
            IEnumerable<string> messages = GetTokens(state.GetStringBuilder());

            // find the first complete message
            string commands = "";
            if (messages.Any())
            {
                commands = messages.First();
                state.GetStringBuilder().Remove(0, commands.Length);
            }
            // if there wasn't a complete message continue the event loop
            else
            {
                Network.GetData(state);
            }

            // give commands to ship
            commands.TrimEnd();
            lock (world)
            {
                world.UpdateShipCommands(commands, state.GetID());
            }

            // ask client for data (continue loop)
            Network.GetData(state);
        }

        /// <summary>
        /// Updates the world then sends its objects to each client
        /// </summary>
        private void UpdateWorld()
        {

            while (watch.ElapsedMilliseconds < world.GetMSPerFrame())
            { /* do nothing */ }
            watch.Restart();
            
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

                IEnumerable<Projectile> projectiles = world.GetProjs();
                foreach (Projectile p in projectiles)
                {
                    world.MotionForProjectiles(p);
                    sb.Append(JsonConvert.SerializeObject(p) + "\n");
                }

                IEnumerable<Ship> ships = world.GetAllShips();
                foreach (Ship s in ships)
                {
                    world.MotionForShips(s);
                    sb.Append(JsonConvert.SerializeObject(s) + "\n");
                }
            }

            string data = sb.ToString();
            // send each object to clients
            lock (clients)
            {
                foreach (SocketState client in clients)
                {
                    Network.Send(client.GetSocket(), data);
                }
            }
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
                // TODO: deal with any problems
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
}
