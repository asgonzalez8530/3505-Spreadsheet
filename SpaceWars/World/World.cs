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
        private int startingHitPoints;
        private int projectileSpeed;
        private double engineStrength;
        private int turningRate;
        private int shipSize;
        private int starSize;
        private int universeSize;
        private int MSPerFrame;
        private int projectileFiringDelay;
        private int respawnDelay;
        private bool kingIsOn;
        private int projectileID;

        public World()
        {
            aliveShips = new Dictionary<int, Ship>();
            allShips = new Dictionary<int, Ship>();
            stars = new Dictionary<int, Star>();
            projectiles = new Dictionary<int, Projectile>();
            startingHitPoints = 5;
            projectileSpeed = 15;
            engineStrength = .08;
            turningRate = 2;
            shipSize = 20;
            starSize = 35;
            universeSize = 750;
            MSPerFrame = 16;
            projectileFiringDelay = 6;
            respawnDelay = 300;
            kingIsOn = false;
            projectileID = 0;

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
        public void SetStartingHitPoint(string points)
        {
            // make sure that points is not null and can be parsed to an int
            if (points == null || !int.TryParse(points, out int p))
            {
                throw new ArgumentException("Server Error: Invalid FramesPerShot in the XML file was found");
            }
            else
            {
                startingHitPoints = p;
            }
        }

        /// <summary>
        /// Sets how fast the projectiles travel 
        /// </summary>
        public void SetProjectileSpeed(string speed)
        {
            // make sure that speed is not null and can be parsed to an int
            if (speed == null || !int.TryParse(speed, out int s))
            {
                throw new ArgumentException("Server Error: Invalid FramesPerShot in the XML file was found");
            }
            else
            {
                projectileSpeed = s;
            }
        }

        /// <summary>
        /// Sets the acceleration a ship engine applies
        /// </summary>
        /// <param name="speed"></param>
        public void SetEngineStrength(string strength)
        {
            // make sure that strength is not null and can be parsed to an int
            if (strength == null || !int.TryParse(strength, out int s))
            {
                throw new ArgumentException("Server Error: Invalid FramesPerShot in the XML file was found");
            }
            else
            {
                engineStrength = s;
            }
        }

        /// <summary>
        /// Sets the degrees that a chip can rotate per frame
        /// </summary>
        public void SetTurningRate(string rotate)
        {
            // make sure that rotate is not null and can be parsed to an int
            if (rotate == null || !int.TryParse(rotate, out int r))
            {
                throw new ArgumentException("Server Error: Invalid FramesPerShot in the XML file was found");
            }
            else
            {
                turningRate = r;
            }
        }

        /// <summary>
        /// Sets the area that a ship occupies
        /// </summary>
        public void SetShipSize(string size)
        {
            // make sure that size is not null and can be parsed to an int
            if (size == null || !int.TryParse(size, out int s))
            {
                throw new ArgumentException("Server Error: Invalid FramesPerShot in the XML file was found");
            }
            else
            {
                shipSize = s;
            }
        }

        /// <summary>
        /// Sets the area that a star occupies
        /// </summary>
        public void SetStarSize(string size)
        {
            // make sure that size is not null and can be parsed to an int
            if (size == null || !int.TryParse(size, out int s))
            {
                throw new ArgumentException("Server Error: Invalid FramesPerShot in the XML file was found");
            }
            else
            {
                starSize = s;
            }
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
                        //default:
                        //    throw new ArgumentException("Ship Command Error: Invalid command \'" + command + "\'");
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
        /// Sets the game mode to King Of The Hill
        /// </summary>
        public void SetKingOfTheHill(string king)
        {
            // make sure that king is not null
            if (king == null)
            {
                kingIsOn = false;
            }
            else if (king == "true" || king == "True")
            {
                kingIsOn = true;
            }
            else
            {
                kingIsOn = false;
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
            if (kingIsOn && allShips.Count == 0)
            {
                name = "King " + name;
            }

            // make a ship
            Ship s = new Ship(name, id, new Vector2D(0, 0), new Vector2D(0,-1), startingHitPoints, respawnDelay, projectileFiringDelay);

            // find a random location and a random direction
            Respawn(s);

            // add the ship to the world
            AddShip(s);
        }

        /// <summary>
        /// Computes the acceleration, velocity, and position of the passed in ship
        /// </summary>
        public void MotionForShips(Ship ship)
        {
            // handle left turn command
            if (ship.TurnLeft)
            {
                ship.GetDirection().Rotate(-turningRate);
                ship.TurnLeft = false;
            }
            // handle right turn command
            else if (ship.TurnRight)
            {
                ship.GetDirection().Rotate(turningRate);
                ship.TurnRight = false;
            }

            // spawn a projectile if projectile command has been given
            if (ship.FireProjectile)
            {
                Projectile p = new Projectile(ship.GetID(), projectileID++, ship.GetLocation(), ship.GetDirection());
                AddProjectile(p);
                ship.FireProjectile = false;
                ship.ResetFireTimer();
            }

            //get a zero vector
            Vector2D acceleration = new Vector2D(0.0,0.0);
            
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

            // recalculate velocity and location
            ship.SetVelocity(ship.GetVelocity() + acceleration);
            ship.SetLocation(ship.GetVelocity() + ship.GetLocation());
            // check to see if ship is off screen
            Wraparound(ship);

            // check for collisions with any star
            foreach(Star star in stars.Values)
            {
                CollisionWithAStar(ship, star);
            }

            // check for collisions with any projectiles
            foreach(Projectile proj in projectiles.Values)
            {
                CollisionWithAProjectile(ship, proj);
            }

            ship.IncrementFireTimer();

        }

        /// <summary>
        /// Computes the new location for the projectile
        /// </summary>
        public void MotionForProjectiles(Projectile projectile)
        {
            // find the velocity with respect to direction
            Vector2D velocity = projectile.GetDirection() * projectileSpeed;

            // reset the location of the projectile
            projectile.SetLocation(projectile.GetLocation() + velocity);
            if (!ProjectileOffScreen(projectile))
            {
                foreach (Star star in stars.Values)
                {
                    CollisionBetweenAStarAndProjectile(star, projectile);
                }
            }

        }

        /// <summary>
        /// Checks to see if the passed in ship is on the edge of the world
        /// </summary>
        private void Wraparound(Ship s)
        {
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

        /// <summary>
        /// Checks to see if the passed in ship touched a star
        /// </summary>
        private void CollisionWithAStar(Ship ship, Star star)
        {
            if (WithinARadius(ship.GetLocation(), star.GetLocation(), shipSize + starSize))
            {
                // the passed in ship hit a star so we update the ship's health 
                HitAStar(ship);
            }
        }

        /// <summary>
        /// Detects whether or not a ship is hit by a projectile
        /// </summary>
        private void CollisionWithAProjectile(Ship ship, Projectile proj)
        {
            if (ship.GetID() != proj.GetOwner())
            {
                if (WithinARadius(ship.GetLocation(), proj.GetLocation(), shipSize))
                {
                    // the passed in ship was hit by a projectile so we update health and remove
                    // the projectile from the world
                    HitAProjectile(ship, proj);
                }
            }
        }

        /// <summary>
        /// Detects whether or not a ship is hit by a projectile
        /// </summary>
        private void CollisionBetweenAStarAndProjectile(Star star, Projectile proj)
        {
            if (WithinARadius(star.GetLocation(), proj.GetLocation(), starSize))
            {
                // the passed in projectile hit a star so we remove it from the world
                proj.Alive(false);
            }
        }

        /// <summary>
        /// The passed in ship has hit a star so the health points of the
        /// ship must be set to zero
        /// </summary>
        private void HitAStar(Ship ship)
        {
            if (ship == null)
            {
                return;
            }

            // kill the ship
            ship.SetHP(0);

            // if king game mode is turned on 
            if (kingIsOn)
            {
                if (ship.IsKing())
                {
                    // make sure that the ship that died is no longer king
                    ship.SetKing(false);
                    ship.SetName(ship.GetName().Substring(5));
                    // reset health points to original
                    ship.SetHP(startingHitPoints);

                    // select a new king 
                    Ship newKingShip = RandomShip();
                    newKingShip.SetKing(true);
                    newKingShip.SetName("King " + newKingShip.GetName());
                    // hit points go up beacuse ship is now king
                    newKingShip.SetHP(startingHitPoints + 3);
                }
            }
        }

        /// <summary>
        /// Returns a random ship from the world
        /// </summary>
        private Ship RandomShip()
        {
            Random r = new Random();
            List<Ship> values = Enumerable.ToList(allShips.Values);
            int size = allShips.Count;
            
            return values[r.Next(size)];
        }

        /// <summary>
        /// When a ship is hit by a projectile then remove the a health point remove 
        /// the projectile form the world
        /// </summary>
        private void HitAProjectile(Ship ship, Projectile projectile)
        {
            if (kingIsOn)
            {
                // find the owner of the projectile
                int shipID = projectile.GetOwner();
                if (allShips.TryGetValue(shipID, out Ship projectileShip))
                {
                    // if the projectile is from the king or the ship is the king then...
                    if (ship.IsKing() || projectileShip.IsKing())
                    {
                        // subtract a health point from the ship
                        ship.SetHP(ship.GetHP() - 1);
                    }

                    if (ship.IsKing() && ship.GetHP() == 0)
                    {
                        // make sure that the ship that died is no longer king
                        ship.SetKing(false);
                        ship.SetName(ship.GetName().Substring(5));
                        // reset health points to original
                        ship.SetHP(startingHitPoints);

                        // select a new king 
                        Ship newKingShip = RandomShip();
                        newKingShip.SetKing(true);
                        newKingShip.SetName("King " + newKingShip.GetName());
                        // hit points go up beacuse ship is now king
                        newKingShip.SetHP(startingHitPoints + 3);
                    }
                }
                // set the projectile to dead
                projectile.Alive(false);
            }
            else
            {
                // subtract a health point
                ship.SetHP(ship.GetHP() - 1);

                // set the projectile to dead
                projectile.Alive(false);
            }
        }

        /// <summary>
        /// When the projectile is off the screen then remove the projectile
        /// from the world and return true. Else, if it was not removed return
        /// false;
        /// </summary>
        private bool ProjectileOffScreen(Projectile p)
        {

            //TODO: instead of removing projectile, mark projectile as dead
            int borderCoordinate = universeSize / 2;

            // check to see if its on the edge of the world
            if (Math.Abs(p.GetLocation().GetX()) >= borderCoordinate)
            {
                p.Alive(false);
                return true;
            }

            // check to see if its on the edge of the world
            else if (Math.Abs(p.GetLocation().GetY()) >= borderCoordinate)
            {
                p.Alive(false);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Creates a ship at a random location in the world in a random direction
        /// </summary>
        public void Respawn(Ship ship)
        {
            // find random empty location in the world
            FindRandomLocation(ship);

            // make a new normalized direction vector pointing up
            Vector2D dir = new Vector2D(0,-1);
            dir.Normalize();

            // make a randomizing object
            Random r = new Random();
            int randAngle = r.Next(0, 360);

            //rotate by random angle
            dir.Rotate(randAngle);

            // sets a random direction for the ship
            ship.SetDirection(dir);

            // reset ships health and velocity
            ship.SetHP(startingHitPoints);
            ship.SetVelocity(new Vector2D(0, 0));

            // reset ships death timer
            ship.ResetRespawnTimer();
        }

        // TODO: might need to be private
        /// <summary>
        /// Finds a random location for the ship that is not near a star
        /// </summary>
        private void FindRandomLocation(Ship ship)
        {
            // make a randomizing object
            Random r = new Random();
            
            // find a random location
            int x = r.Next(-(universeSize / 2), universeSize / 2);
            int y = r.Next(-(universeSize / 2), universeSize / 2);

            // make a new location vector
            Vector2D location = new Vector2D(x, y);

            // check to make sure that no stars are near
            foreach (Star star in stars.Values)
            {
                while (WithinARadius(location, star.GetLocation(), starSize + 50))
                {
                    // find a random location
                    x = r.Next(-(universeSize / 2), universeSize / 2);
                    y = r.Next(-(universeSize / 2), universeSize / 2);

                    // make a new location vector
                    location = new Vector2D(x, y);
                }
            }

            // sets the random location to the ship 
            ship.SetLocation(location);
        }

        /// <summary>
        /// Returns true if the two vectors are within the given radius of each other
        /// otherwise returns false
        /// </summary>
        private bool WithinARadius(Vector2D o1, Vector2D o2, int radius)
        {
            if (o1 == null || o2 == null)
            {
                throw new ArgumentException("LocationIsNearAStar: one of the parameters is null");
            }

            double xDifference = o1.GetX() - o2.GetX();
            double yDifference = o1.GetY() - o2.GetY();

            double distanceBetweenVectors = Math.Sqrt(xDifference * xDifference + yDifference * yDifference);

            if (distanceBetweenVectors < radius)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clean up the dead projectiles in the world and remove them so 
        /// that we are not sending uncessary information to the clients
        /// </summary>
        public void CleanUpProjectiles()
        {
            // get all dead projectiles
            HashSet<Projectile> deadProj = new HashSet<Projectile>();
            foreach (Projectile p in projectiles.Values)
            {
                if (!p.IsAlive())
                {
                    deadProj.Add(p);
                }
            }

            // remove dead projectiles from the world
            foreach (Projectile p in deadProj)
            {
                projectiles.Remove(p.GetID());
            }
        }

        /// <summary>
        /// Searches in ships for objects which are dead and need to be respawned.
        /// If a ship's respawn timer has elapsed, sets ship to alive and respawns it
        /// else, incriments its respawn timer.
        /// </summary>
        public void RespawnShips()
        {
            // get all the dead ships
            HashSet<Ship> deadShips = new HashSet<Ship>();
            foreach (Ship s in allShips.Values)
            {
                if (!s.IsAlive())
                {
                    deadShips.Add(s);
                }
            }

            // try to respawn each ship
            foreach (Ship s in deadShips)
            {
                if (s.CanRespawn())
                {
                    // respawn ship
                    Respawn(s);
                }
                else
                {
                    s.IncrementRespawnTimer();
                }
                    
            }
        }
    }
}
