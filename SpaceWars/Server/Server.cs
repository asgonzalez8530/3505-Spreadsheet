﻿// Anastasia Gonzalez and Aaron Bellis UID: u0985898 & u0981638
// Code implemented as part of PS8 : SpaceWars Server CS3500 Fall Semester
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SpaceWarsServer
{
    class Server
    {

        private World world;

        public Server()
        {
            world = new World();

            //TODO: file path for the game settings
            string filePath = "";

            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
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
                                    int.TryParse(reader.Value, out int size);
                                    world.SetUniverseSize(size);
                                    break;
                                case "MSPerFrame":
                                    reader.Read();
                                    int.TryParse(reader.Value, out int frames);
                                    world.SetMSPerFrame(frames);
                                    break;
                                case "FramesPerShot":
                                    reader.Read();
                                    int.TryParse(reader.Value, out int firingDelay);
                                    world.SetProjectileFiringDelay(firingDelay);
                                    break;
                                case "RespawnRate":
                                    reader.Read();
                                    int.TryParse(reader.Value, out int respawnDelay);
                                    world.SetRespawnDelay(respawnDelay);
                                    break;
                                //TODO: add all the extra features + the extra setting
                                case "Star":
                                    break;
                                case "x":
                                    reader.Read();
                                    int.TryParse(reader.Value, out int x);
                                    world.SetStarX(x);
                                    break;
                                case "y":
                                    reader.Read();
                                    int.TryParse(reader.Value, out int y);
                                    world.SetStarY(y);
                                    break;
                                case "mass":
                                    reader.Read();
                                    int.TryParse(reader.Value, out int mass);
                                    world.SetStarMass(mass);
                                    break;
                            }
                        }
                    }
                    // we're done with the reader, time to close it
                    reader.Close();
                }
                // we have successful opened a server... wait for first client
                Console.WriteLine("Server is up. Awaiting first client.");
            }
            catch (Exception)
            {
                //TODO: deal with any problems 
            }
        }
        
        public static void Main(string[] args)
        {
            Server s = new Server();
            //TODO: run an event-loop that listens for TCP socket connections from the clients
        }
    }
}