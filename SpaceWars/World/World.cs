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
        // TODO: need to move this into the proper classes??
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
        /// returns the nunber of milliseconds per frame.
        /// </summary>
        public int GetMSPerFrame()
        {
            return MSPerFrame;
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
        /// Takes in an int, size and sets the UniverseSize to that value;
        /// </summary>
        public void SetUniverseSize(int size)
        {
            universeSize = size;
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
        /// Takes in a string representing a command request and a playerId
        /// and updates the associated players ship with the commands.
        /// 
        /// A command request is a string containing one or more single-letter 
        /// commands enclosed in parentheses. 
        /// 
        /// The letters can be any combination of the below:
        /// 1. 'R' - turn right
        /// 2. 'L' - turn left
        /// 3. 'T' - engine thrust
        /// 4. 'F' - fire projectile
        /// 
        /// if any character other than these or parentheses are found in the 
        /// commands string, throws ArgumentException
        /// 
        /// if no player exists with playerID, throws ArgumentException
        /// </summary>
        public void UpdateShipCommands(string commands, int playerID)
        {
            if (!allShips.ContainsKey(playerID))
            {
                throw new ArgumentException("Ship Command Error: ship not in world");
            }
            else
            {
                Ship s = allShips[playerID];

                foreach (char command in commands)
                {
                    switch (command)
                    {
                        case 'R': // turn right
                            // can't turn right and left at the same time
                            if (!s.TurnLeft)
                            {
                                s.TurnRight = true;
                            }
                            break;
                        case 'L': // turn left
                            // can't turn right and left at the same time
                            if (!s.TurnRight)
                            {
                                s.TurnLeft = true;
                            }
                            break;
                        case 'T': // turn on thrust
                            s.Thrust = true;
                            break;
                        case 'F': // fire a projectile
                            s.FireProjectile = true;
                            break;
                        case '(': // valid but not used
                            break;
                        case ')': // valid but not used
                            break;
                        default:
                            throw new ArgumentException("Ship Command Error: Invalid command \'" + command + "\'");
                    }
                }
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
            double.TryParse(mass, out double Mass);

            // make a new unique ID for the star
            int id = stars.Count;

            // make a new star and add it to the world dictionary
            Star s = new Star(id, X, Y, Mass);
            AddStar(s);
        }

        /// <summary>
        /// Creates a new ship with the given name and id, then adds  it to the
        /// world
        /// </summary>
        public void MakeNewShip(String name, int id)
        {
            // TODO: Server needs to find suitable location to place ship when
            // made and when respawning.
            // may want to use the respawn method for this
            Vector2D location = new Vector2D(0, 0);
            Vector2D orientation = new Vector2D(0, 0);

            // make a ship
            Ship s = new Ship(name, id, location, orientation);

            // add the ship to the world
            AddShip(s);

        }

        /// <summary>
        /// Computes the acceleration of the passed in ship
        /// </summary>
        public void Motion(Ship ship)
        {
            // TODO: apply the commands with direction changes

            Vector2D acceleration = new Vector2D();
            //compute the acceleration caused by the star
            foreach (Star star in stars.Values)
            {
                Vector2D g = star.GetLocation() - ship.GetLocation();
                g.Normalize();
                acceleration = acceleration + g * star.GetMass();
            }

            if (ship.HasThrust())
            {
                //compute the acceleration due to thrust
                Vector2D t = new Vector2D(ship.GetDirection());
                t = t * engineStrength;
                acceleration = acceleration + t;
            }

            ship.SetVelocity(ship.GetVelocity() + acceleration);
            ship.SetLocation(ship.GetVelocity() + ship.GetLocation());

        }

        /// <summary>
        /// Checks to see if the passed in ship is on the edge of the world
        /// </summary>
        public void Wraparound(Ship s)
        {
            if (s == null)
            {
                return;
            }

            int borderCoordinate = universeSize / 2;

            if (Math.Abs(s.GetLocation().GetX()) >= borderCoordinate)
            {
                // times x by -1
                double x = s.GetLocation().GetX() * -1;
                double y = s.GetLocation().GetY();

                // set the ship's new location
                s.SetLocation(x, y);
            }
            if (Math.Abs(s.GetLocation().GetY()) >= borderCoordinate)
            {
                // times y by -1
                double y = s.GetLocation().GetY() * -1;
                double x = s.GetLocation().GetX();

                // set the ship's new location
                s.SetLocation(x, y);
            }
        }

        public void CollisionWithAStar(Ship ship, Star star)
        {
            if (ship == null || star == null)
            {
                throw new ArgumentException("CollisionWithAStar: one of the parameters is null");
            }

            if (ship.GetLocation().GetX() + shipSize < star.GetLocation().GetX())
            {
                return;
            }
            if (star.GetLocation().GetX() + starSize < ship.GetLocation().GetX())
            {
                return;
            }
            if (ship.GetLocation().GetY() + shipSize < star.GetLocation().GetY())
            {
                return;
            }
            if (star.GetLocation().GetY() + starSize < ship.GetLocation().GetY())
            {
                return;
            }

            // the passed in ship hit a star so we update the ship's health 
            HitAStar(ship);
        }

        /// <summary>
        /// Detects whether or not a ship is hit by a projectile
        /// </summary>
        public void CollisionWithAProjectile(Ship ship, Projectile proj)
        {
            if (ship == null || proj == null)
            {
                throw new ArgumentException("CollisionWithAStar: one of the parameters is null");
            }

            if (ship.GetLocation().GetX() + shipSize < proj.GetLocation().GetX())
            {
                return;
            }
            if (proj.GetLocation().GetX() < ship.GetLocation().GetX())
            {
                return;
            }
            if (ship.GetLocation().GetY() + shipSize < proj.GetLocation().GetY())
            {
                return;
            }
            if (proj.GetLocation().GetY() < ship.GetLocation().GetY())
            {
                return;
            }

            // the passed in ship was hit by a projectile so we update health and remove
            // the projectile from the world
            HitAProjectile(ship, proj);
        }

        /// <summary>
        /// The passed in ship has hit a star so the health points of the
        /// ship must be set to zero
        /// </summary>
        public void HitAStar(Ship ship)
        {
            if (ship == null)
            {
                return;
            }

            // kill the ship
            ship.SetHP(0);
        }

        /// <summary>
        /// When a ship is hit by a projectile then remove the a health point remove 
        /// the projectile form the world
        /// </summary>
        public void HitAProjectile(Ship ship, Projectile projectile)
        {
            if (ship == null)
            {
                return;
            }
            
            // subtract a health point
            ship.SetHP(ship.GetHP() - 1);

            // remove the projectile from the world
            projectiles.Remove(projectile.GetID());
        }

        /// <summary>
        /// When the projectile is off the screen then remove the projectile
        /// from the world
        /// </summary>
        public void ProjectileOffScreen(Projectile p)
        {
            if (p == null)
            {
                return;
            }

            int borderCoordinate = universeSize / 2;

            // check to see if its on the edge of the world
            if (Math.Abs(p.GetLocation().GetX()) >= borderCoordinate)
            {
                projectiles.Remove(p.GetID());
            }

            // check to see if its on the edge of the world
            else if (Math.Abs(p.GetLocation().GetY()) >= borderCoordinate)
            {
                projectiles.Remove(p.GetID());
            }
        }

        /// <summary>
        /// Creates a ship at a random location in the world in a random direction
        /// </summary>
        public void Respawn(Ship ship)
        {
            // make a randomizing object
            Random r = new Random();
            
            // TODO: Find a random location

            // make new random directions
            int x = r.Next(0, 2);
            int y = r.Next(0, 2);

            // make a new normalized direction vector
            Vector2D dir = new Vector2D(x, y);
            dir.Normalize();

            // sets a random direction for the ship
            ship.SetDirection(dir);
        }

    }
}
