using System;
using System.Collections.Generic;

namespace SpaceWars
{
    /// <summary>
    /// The World containing collections of objects for a SpaceWars game.
    /// </summary>
    public class World
    {
        /// <summary>
        /// The size of this World.
        /// </summary>
        private int size;

        /// <summary>
        /// Dictionary of the Stars in this world, keyed off their unique ID.
        /// </summary>
        private Dictionary<int, Star> theStars;

        /// <summary>
        /// Dictionary of the Ships in this world, keyed off their unique ID.
        /// </summary>
        private Dictionary<int, Ship> theShips;

        /// <summary>
        /// Dictionary of the Projectiles in this world, keyed off their unique ID.
        /// </summary>
        private Dictionary<int, Projectile> theProjectiles;

        /// <summary>
        /// A set of new projectiles, used to play SFX
        /// </summary>
        public HashSet<int> newProjIDs;

        /// <summary>
        /// The object used to lock critical sections dealing with changes to theStars.
        /// </summary>
        public Star starLock;

        /// <summary>
        /// The object used to lock critical sections dealing with changes to theShips.
        /// </summary>
        public Ship shipLock;

        /// <summary>
        /// The object used to lock critical sections dealing with changes to theProjectiles.
        /// </summary>
        public Projectile projLock;

        /// <summary>
        /// Thrust strength, default is 0.08.
        /// </summary>
        private double engineStrength;

        /// <summary>
        /// Units to move per frame, default is 15.
        /// </summary>
        private int projSpeed;

        /// <summary>
        /// Stars' radius, default is 35.
        /// </summary>
        private int starRadius;

        /// <summary>
        /// Ships' radius, default is 20.
        /// </summary>
        private int shipRadius;

        /// <summary>
        /// Frames between respawning, default is 300.
        /// </summary>
        private int respawnRate;

        /// <summary>
        /// The amount a ship rotates because of a turn request.
        /// </summary>
        private int turningRate;

        /// <summary>
        /// The starting HP, default is 5.
        /// </summary>
        private int startingHP;

        /// <summary>
        /// The world's RNG. Used for making spawn points.
        /// </summary>
        private Random random;

        /// <summary>
        /// Extra game mode, keeps the ship safe for 100 frames after respawning.
        /// </summary>
        private bool safeRespawnGameMode;

        /// <summary>
        /// Construct the World, using a provided size.
        /// </summary>
        /// <param name="inputSize"></param>
        public World(int inputSize)
        {
            this.size = inputSize;
            theStars = new Dictionary<int, Star>();
            theShips = new Dictionary<int, Ship>();
            theProjectiles = new Dictionary<int, Projectile>();
            newProjIDs = new HashSet<int>();

            starLock = new Star();
            shipLock = new Ship();
            projLock = new Projectile();

            engineStrength = 0.08;
            projSpeed = 15;
            turningRate = 2;
            starRadius = 35;
            shipRadius = 20;
            respawnRate = 300;
            startingHP = 5;
            safeRespawnGameMode = false;

            random = new Random();
        }

        /// <summary>
        /// Constructs the World with the given parameters.
        /// </summary>
        public World(int inputSize, double engineStrength, int projSpeed, int turningRate, 
            int starRadius, int shipRadius, int respawnRate, int startingHP, bool safeRespawnGameMode = false) : this(inputSize)
        {
            this.engineStrength = engineStrength;
            this.projSpeed = projSpeed;
            this.turningRate = turningRate;
            this.starRadius = starRadius;
            this.shipRadius = shipRadius;
            this.respawnRate = respawnRate;
            this.startingHP = startingHP;
            this.safeRespawnGameMode = safeRespawnGameMode;
        }


        /// <summary>
        /// Returns the size of the World
        /// </summary>
        public int GetSize()
        {
            return size;
        }


        /// <summary>
        /// Returns the setting of the preferred starting HP.
        /// </summary>
        /// <returns></returns>
        public int GetStartingHP()
        {
            return startingHP;
        }


        /// <summary>
        /// Yield returns the Stars in this World.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Star> GetStars()
        {
            foreach (Star star in theStars.Values)
            {
                yield return star;
            }
        }


        /// <summary>
        /// Returns the Ships in this World, sorted by score.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Ship> GetShips()
        {
            List<Ship> sortedShipList = new List<Ship>();
            foreach (Ship ship in theShips.Values)
            {
                sortedShipList.Add(ship);
            }

            sortedShipList.Sort();
            return sortedShipList;
        }


        /// <summary>
        /// Yield returns the Projectiles in this World.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Projectile> GetProjectiles()
        {
            foreach (Projectile projectile in theProjectiles.Values)
            {
                yield return projectile;
            }
        }


        /// <summary>
        /// Updates the chosen Star.
        /// </summary>
        /// <param name="star"></param>
        /// <param name="ID"></param>
        public void UpdateStar(Star star, int ID)
        {
            if (theStars.ContainsKey(ID))
            {
                theStars[ID] = star;
            }
            else
            {
                theStars.Add(ID, star);
            }
        }


        /// <summary>
        /// Updates the chosen Ship.
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="ID"></param>
        public void UpdateShip(Ship ship, int ID)
        {
            if (theShips.ContainsKey(ID))
            {
                theShips[ID] = ship;
            }
            else
            {
                theShips.Add(ID, ship);
            }
        }


        /// <summary>
        /// Updates the chosen Projectile. If it is dead, also removes it from theProjectiles.
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="ID"></param>
        public void UpdateProjectile(Projectile proj, int ID)
        {
            if (theProjectiles.ContainsKey(ID))
            {
                theProjectiles[ID] = proj;

                // remove from newProjIDs if this proj is in there.
                if (newProjIDs.Contains(ID))
                {
                    newProjIDs.Remove(ID);
                }
            }
            else
            {
                theProjectiles.Add(ID, proj);

                // add to newProjIDs
                newProjIDs.Add(ID);
            }

            // remove if the projectile sent is dead
            if (!proj.IsAlive())
            {
                theProjectiles.Remove(ID);
            }
        }


        /// <summary>
        /// Returns a specific ship.
        /// </summary>
        /// <param name="shipID"></param>
        public Ship GetShip(int shipID)
        {
            return theShips[shipID];
        }

        /// <summary>
        /// Remove the specified ship from theShips.
        /// </summary>
        /// <param name="shipID"></param>
        public void RemoveShip(int shipID)
        {
            theShips[shipID].Connected = false;
            theShips[shipID].SetHP(0);
        }


        /// <summary>
        /// Fires a projectile by creating a projectile with the given ID for the given ship.
        /// </summary>
        public void FireProjectile(int shipID, int projID)
        {
            Projectile newProj = new Projectile(shipID, projID, theShips[shipID].GetOrientation(), theShips[shipID].GetLocation());
            UpdateProjectile(newProj, projID);
        }


        /// <summary>
        /// Rotates the ship right for the given shipID.
        /// </summary>
        public void RotateShipRight(int shipID)
        {
            Ship ship = theShips[shipID];
            Vector2D newDir = ship.GetOrientation();
            newDir.Rotate(turningRate);
            ship.SetOrientation(newDir);
        }


        /// <summary>
        /// Rotates a ship left for the given shipID.
        /// </summary>
        public void RotateShipLeft(int shipID)
        {
            Ship ship = theShips[shipID];
            Vector2D newDir = ship.GetOrientation();
            newDir.Rotate(-turningRate);
            ship.SetOrientation(newDir);
        }


        /// <summary>
        /// The world updates all the positions and statuses of its ships and projectiles.
        /// </summary>
        public void Update(int frameCount)
        {
            foreach (Ship ship in theShips.Values)
            {
                // if the ship is dead and frameCount > ship.TimeOfDeath + respawnRate\
                if (ship.GetHP() == 0 && ship.Connected && (frameCount > ship.TimeOfDeath + respawnRate || frameCount < ship.TimeOfDeath))
                {
                    // MakeSpawnPoint, set location, set health, reset ship.Velocity
                    ship.SetLocation(MakeSpawnPoint());
                    ship.SetHP(startingHP);
                    ship.TimeLastSpawned = frameCount;
                    ship.Velocity = new Vector2D(0, 0);
                }
                Vector2D acc = CalculateInstantAcceleration(ship);
                ship.Velocity += acc;
                Vector2D newLoc = ship.GetLocation() + ship.Velocity;
                WrapLocation(ref newLoc);
                ship.SetLocation(newLoc);
            }

            // remove dead projectiles
            Dictionary<int, Projectile> projCopy = new Dictionary<int, Projectile>(theProjectiles);
            foreach(KeyValuePair<int, Projectile> kvp in projCopy)
            {
                if(!kvp.Value.IsAlive())
                {
                    theProjectiles.Remove(kvp.Key);
                }
            }
            // update remaining projectiles
            foreach(Projectile proj in theProjectiles.Values)
            {
                proj.SetLocation(proj.GetLocation() + (proj.GetOrientation() * projSpeed));

                Vector2D tempLocation = proj.GetLocation();

                if (WrapLocation(ref tempLocation))
                {
                    proj.KillProj();
                }
            }

            CheckCollisions(frameCount);
        }


        /// <summary>
        /// Returns whether a given vector is within the world. If not, adjusts the vector to be in the world.
        /// </summary>
        /// <param name="location">The vector location (of a ship or projectile)</param>
        /// <returns>whether the vector was changed</returns>
        private bool WrapLocation(ref Vector2D location)
        {
            double xFromZero = Math.Abs(location.GetX());
            double newX = location.GetX();
            double yFromZero = Math.Abs(location.GetY());
            double newY = location.GetY();
            bool locIsOutOfThisWorld = false;

            double xOverhang = xFromZero - (size / 2);
            if (xOverhang > 0)
            {
                locIsOutOfThisWorld = true;
                newX = (size / 2) - xOverhang; // now on right side

                if (location.GetX() > 0) // started on right side
                {
                    newX *= -1; // now on left side
                }
            }

            double yOverhang = yFromZero - (size / 2);
            if (yOverhang > 0)
            {
                locIsOutOfThisWorld = true;
                newY = (size / 2) - yOverhang; // now on right side

                if (location.GetY() > 0) // started on right side
                {
                    newY *= -1; // now on left side
                }
            }

            if (locIsOutOfThisWorld)
            {
                location = new Vector2D(newX, newY);
            }

            return locIsOutOfThisWorld;

            
        }


        /// <summary>
        /// Calculates the instant acceleration from stars and engine thrusts.
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private Vector2D CalculateInstantAcceleration(Ship ship)
        {
            Vector2D totalAcc = new Vector2D(0, 0);
            // add gravity from each star
            foreach(Star star in theStars.Values)
            {
                totalAcc += CalculateForce(ship, star);
            }

            // if thrust add enginestrength
            if(ship.IsThrusting())
            {
                totalAcc += (ship.GetOrientation() * engineStrength);
            }

            // return accumulated vector
            return totalAcc;
        }


        /// <summary>
        /// Calculates gravity between a ship and a star.
        /// </summary>
        /// <returns>a Vector2D gravity</returns>
        private Vector2D CalculateForce(Ship ship, Star star)
        {
            // get direction
            Vector2D g = star.GetLocation() - ship.GetLocation();
            g.Normalize();

            // get magnitude
            g *= star.GetSize();

            return g;
        }


        /// <summary>
        /// Makes a random spawn point.
        /// </summary>
        /// <returns></returns>
        public Vector2D MakeSpawnPoint()
        {
            Vector2D spawn = new Vector2D(random.NextDouble() * size, random.NextDouble() * size);
            WrapLocation(ref spawn);
            spawn = CheckSafeness(spawn);
            return spawn;
        }

        private Vector2D CheckSafeness(Vector2D spawn)
        {
            Vector2D goodLoc = spawn;
            bool isSafe = true;
            foreach (Star star in theStars.Values)
            {
                Vector2D distanceVector = star.GetLocation() - spawn;
                // check if the ship is too close to the star
                if(distanceVector.Length() < ((3 * shipRadius) + starRadius))
                {
                    isSafe = false;
                }
            }

            if(!isSafe)
            {
                spawn = MakeSpawnPoint();
            }

            return spawn;
        }


        /// <summary>
        /// Checks for possible collisions and handles them.
        /// </summary>
        /// <param name="frameCount">current frame count when this update check was called</param>
        public void CheckCollisions(int frameCount)
        {
            //check stars
            foreach (Star star in theStars.Values)
            {
                foreach (Ship ship in theShips.Values)
                {
                    if (ship.GetHP() > 0 && CollidedWith(star, ship))
                    {
                        ship.SetHP(0);
                        ship.TimeOfDeath = frameCount;
                    }
                }
            }
            //check proj
            foreach (Projectile proj in theProjectiles.Values)
            {
                foreach(Ship ship in theShips.Values)
                {
                    bool friendlyFire = (ship.GetID() == proj.GetOwnerID());
                    if(!friendlyFire && proj.IsAlive() && ship.GetHP() > 0 && CollidedWith(proj, ship))
                    {
                        if (safeRespawnGameMode ? (frameCount > (ship.TimeLastSpawned + 100) || ship.TimeLastSpawned > frameCount) : true)
                        {
                            // decrease this ships hp
                            ship.SetHP(ship.GetHP() - 1);
                            if (ship.GetHP() == 0)
                            {
                                // if hp is 0, incremement owner score
                                ship.TimeOfDeath = frameCount;
                                theShips[proj.GetOwnerID()].IncrementScore();
                            } 
                        }
                        proj.KillProj();
                        break;
                    }
                }
                foreach(Star star in theStars.Values)
                {
                    if(proj.IsAlive() && CollidedWith(proj, star))
                    {
                        proj.KillProj();
                        break;
                    }
                }
            }
        }

        private bool CollidedWith(Star star, Ship ship)
        {
            Vector2D distance = star.GetLocation() - ship.GetLocation();
            return distance.Length() < (starRadius + shipRadius);
        }

        private bool CollidedWith(Projectile proj, Ship ship)
        {
            Vector2D distance = proj.GetLocation() - ship.GetLocation();
            return distance.Length() < shipRadius;
        }

        private bool CollidedWith(Projectile proj, Star star)
        {
            Vector2D distance = proj.GetLocation() - star.GetLocation();
            return distance.Length() < starRadius;
        }
    }
}
