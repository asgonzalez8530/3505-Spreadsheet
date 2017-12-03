// Anastasia Gonzalez and Aaron Bellis UID: u0985898 & u0981638
// Code implemented as part of PS7 : SpaceWars client CS3500 Fall Semester
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWars
{
    public class World
    {
        private Dictionary<int, Ship> aliveShips; // keeps track of all the ships on the screen
        private Dictionary<int, Ship> allShips; // keeps track of all the ships in the world (game)
        private Dictionary<int, Star> stars; // keeps track of all the stars on the screen
        private Dictionary<int, Projectile> projectiles; // keeps track of all the projectiles

        // Default data for the game
        private int startingHitPoints = 5;
        private int projectileSpeed = 15;
        private double engineStrength = .08;
        private int turningRate = 2;
        private int shipSize = 20;
        private int starSize = 35;
        private int universeSize = 750;
        private int MSPerFrame = 16;
        private int projectileFiringDelay = 6;
        private int respawnDelay = 300; 

        public World()
        {
            aliveShips = new Dictionary<int, Ship>();
            allShips = new Dictionary<int, Ship>();
            stars = new Dictionary<int, Star>();
            projectiles = new Dictionary<int, Projectile>();
        }

        /// <summary>
        /// Gets the world size
        /// </summary>
        public int GetSize()
        {
            return universeSize;
        }

        /// <summary>
        /// Sets the worlds size
        /// </summary>
        /// <param name="size"> the world's size </param>
        public void SetUniverseSize(string size)
        {
            // make sure that size is not null and can be parsed to an int 
            if (size == null || !int.TryParse(size, out int s))
            {
                throw new ArgumentException("Server Error: Invalid UniverseSize in the XML file was found");
            }
            else
            {
                universeSize = s;
            }
            
            
        }

        /// <summary>
        /// Gets a list of all the ships that are alive
        /// </summary>
        public IEnumerable<Ship> GetAliveShips()
        {
            return aliveShips.Values;
        }

        /// <summary>
        /// Gets a list of all the ships in the we have seen
        /// </summary>
        public IEnumerable<Ship> GetAllShips()
        {
            return allShips.Values;
        }

        /// <summary>
        /// Gets a list of all the stars in the world
        /// </summary>
        public IEnumerable<Star> GetStars()
        {
            return stars.Values;
        }

        /// <summary>
        /// Gets a list of all the projectiles in the world
        /// </summary>
        public IEnumerable<Projectile> GetProjs()
        {
            return projectiles.Values;
        }

        /// <summary>
        /// Sets the starting hit points for the game
        /// </summary>
        public void SetStartingHitPoint(int points)
        {
            startingHitPoints = points;
        }

        /// <summary>
        /// Sets how fast the projectiles travel 
        /// </summary>
        public void SetProjectileSpeed(int speed)
        {
            projectileSpeed = speed;
        }

        /// <summary>
        /// Sets the acceleration a ship engine applies
        /// </summary>
        /// <param name="speed"></param>
        public void SetEngineStrength(int strength)
        {
            engineStrength = strength;
        }

        /// <summary>
        /// Sets the degrees that a chip can rotate per frame
        /// </summary>
        public void SetTurningRate(int rotate)
        {
            turningRate = rotate;
        }

        /// <summary>
        /// Sets the area that a ship occupies
        /// </summary>
        public void SetShipSize(int size)
        {
            shipSize = size;
        }

        /// <summary>
        /// Sets the area that a star occupies
        /// </summary>
        public void SetStarSize(int size)
        {
            starSize = size;
        }

        /// <summary>
        /// Sets how often the server attempts to update the world
        /// </summary>
        public void SetMSPerFrame(string frames)
        {
            // make sure that frames is not null and can be parsed to an int 
            if (frames == null || !int.TryParse(frames, out int f))
            {
                throw new ArgumentException("Server Error: Invalid MSPerFrame in the XML file was found");
            }
            else
            {
                MSPerFrame = f;
            }
            
            
        }

        /// <summary>
        /// Set how many frames a ship must wait between firing projectiles
        /// </summary>
        public void SetProjectileFiringDelay(string firingDelay)
        {
            // make sure that firingDelay is not null and can be parsed to an int 
            if (firingDelay == null || !int.TryParse(firingDelay, out int f))
            {
                throw new ArgumentException("Server Error: Invalid FramesPerShot in the XML file was found");
            }
            else
            {
                projectileFiringDelay = f;
            }
            
        }

        /// <summary>
        /// Sets the amount of frames before a ship respawns
        /// </summary>
        public void SetRespawnDelay(string delay)
        {
            // make sure that delay is not null and can be parsed to an int 
            if (delay == null || !int.TryParse(delay, out int d))
            {
                throw new ArgumentException("Server Error: Invalid RespawnRate in the XML file was found");
            }
            else
            {
                respawnDelay = d;
            }
        }

        /// <summary>
        /// When a ship is added to the game we update the info or add it
        /// to the world 
        /// </summary>
        private void AddAllShips(Ship s)
        {
            // if the ship is in the world, update its reference
            if (allShips.ContainsKey(s.GetID()))
            {
                allShips[s.GetID()] = s;
            }
            // if the ship is not in the world then add it
            else
            {
                allShips.Add(s.GetID(), s);
            }
        }
        
        /// <summary>
        /// If Ship s does not exist in ships adds it. If the id already exists
        /// in ships, updates reference in ships to s.
        /// </summary>
        public void AddShip(Ship s)
        {
            // if the ship is null then do nothing
            if (s == null)
            {
                return;
            }
            
            // if the ship is dead then remove it from the world
            if (!s.IsAlive())
            {
                aliveShips.Remove(s.GetID());
            }
            // if the ship is in the world then replace the old ship with the passed in ship
            else if (aliveShips.ContainsKey(s.GetID()))
            {
                aliveShips[s.GetID()] = s;
            }
            // if the ship is not in the world then add it
            else
            {
                aliveShips.Add(s.GetID(), s);
            }

            // we have taken care of the alive ships, now lets keep track of all ships
            AddAllShips(s);
        }

        /// <summary>
        /// If Star s does not exist in stars adds it. If the id already exists
        /// in Star, updates reference in stars to s.
        /// </summary>
        public void AddStar(Star s)
        {
            // if the star is null then do nothing
            if (s == null)
            {
                return;
            }

            // if the star is in the world then replace the old star with the passed in star
            else if (stars.ContainsKey(s.GetID()))
            {
                stars[s.GetID()] = s;
            }
            // if the star is not in the world then add it to the world
            else
            {
                stars.Add(s.GetID(), s);
            }
        }

        /// <summary>
        /// If Projectile p does not exist in projectiles adds it. If the id already exists
        /// in projectiles, updates reference in projectiles to p.
        /// </summary>
        public void AddProjectile(Projectile p)
        {
            // if the projectile is null then do nothing
            if (p == null)
            {
                return;
            }

            // if the projectile is dead then remove it fromt he world
            if (!p.IsAlive())
            {
                projectiles.Remove(p.GetID());
            }
            // if the projectile is in the world then replace the old projectile with the 
            // passed in projectile
            else if (projectiles.ContainsKey(p.GetID()))
            {
                projectiles[p.GetID()] = p;
            }
            // if the projectile is not in the world then add it to the world
            else
            {
                projectiles.Add(p.GetID(), p);
            }
        }

        /// <summary>
        /// Makes a new reference to a star in the world on the servers side at the
        /// passed in x and y coordinates and the given mass
        /// </summary>
        public void MakeNewStar(string x, string y, string mass)
        {
            // make sure that all the properties of a star are vaild
            if (x == null || y == null || mass == null)
            {
                throw new ArgumentException("Server Error: Invalid star in the XML file was found");
            }

            // parse the x, y, and mass
            int.TryParse(x, out int X);
            int.TryParse(y, out int Y);
            int.TryParse(mass, out int Mass);

            // make a new unique ID for the star
            int id = stars.Count;

            // make a new star and add it to the world dictionary
            Star s = new Star(id, X, Y, Mass);
            AddStar(s);
        }

        /// <summary>
        /// Computes the acceleration of the passed in ship
        /// </summary>
        public void Motion(Ship ship)
        {
            Vector2D acceleration = new Vector2D();
            //compute the acceleration caused by the star
            foreach (Star star in stars.Values)
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

        /// <summary>
        /// Checks to see if the passed in ship is on the edge of the world
        /// </summary>
        public void Wraparound(Ship s)
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

        public void Collision()
        {

        }
    }
}
