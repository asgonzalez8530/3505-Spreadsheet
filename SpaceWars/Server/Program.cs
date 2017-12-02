// Anastasia Gonzalez and Aaron Bellis UID: u0985898 & u0981638
// Code implemented as part of PS8 : SpaceWars Server CS3500 Fall Semester
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWarsServer
{
    class Server
    {
        //TODO: Default data for the game
        private int startingHitPoints = 5;
        private int projectileSpeed = 15;
        private double engineStrength = .08;
        private int turningRate = 2;
        private int shipSize = 20;
        private int starSize = 35;
        private int universeSize = 750;
        private int timePerFrame = 16;
        private int projectileFiringDelay = 6;
        private int respawnDelay = 300;

        private Dictionary<int, Ship> allShips;
        private Dictionary<int, Star> allStars;

        public Server()
        {
            allShips = new Dictionary<int, Ship>();
            allStars = new Dictionary<int, Star>();
        }

        private void Motion(Ship ship)
        {
            Vector2D acceleration = new Vector2D(); 
            //compute the acceleration caused by the star
            foreach (Star star in allStars.Values)
            {
                Vector2D g = star.GetLocation() - ship.GetLocation();
                g.Normalize();
                acceleration = acceleration + g * star.GetMass();
            }

            //compute the acceleration 
            Vector2D t = new Vector2D(ship.GetDirection());
            t = t * engineStrength;

            acceleration = acceleration + t;
        }

        private void Wraparound(Ship s)
        {
            int borderCoordinate = universeSize / 2;

            if (Math.Abs(s.GetLocation().GetX()) == borderCoordinate)
            {
                //TODO: times x by -1
            }
            if (Math.Abs(s.GetLocation().GetY()) == borderCoordinate)
            {
                //TODO: times y by -1
            }
        }

        static void Main(string[] args)
        {
            //TODO: set up the server 
            Console.WriteLine("Server is up. Awaiting first client.");
        }
    }
}
