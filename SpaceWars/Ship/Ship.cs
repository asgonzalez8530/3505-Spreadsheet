// Anastasia Gonzalez and Aaron Bellis UID: u0985898 & u0981638
// Code implemented as part of PS7 and PS8: SpaceWars client/Server CS3500 Fall Semester
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpaceWars
{
    /// <summary>
    /// Represents a ship object in a SpaceWars game. Provides information about
    /// location, direction and game scoring.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Ship
    {
        [JsonProperty(PropertyName = "ship")]
        private int ID; // unique ID for the ship

        [JsonProperty]
        private Vector2D loc; // current location of the ship in the world

        [JsonProperty]
        private Vector2D dir; // dirction that the ship is headed in the world

        [JsonProperty]
        private bool thrust; // True if thrusts on the ship are on 

        [JsonProperty]
        private string name; // name of the user 

        [JsonProperty]
        private int hp; // health points for the ship

        [JsonProperty]
        private int score; // amount of ship's the current ship had shot down

        private int fireTimer; // counts time in frames since last fire up to FireLimit

        private int respawnTimer; // counts time in frames up to respawnLimit

        private int respawnLimit; // the amount of time in frames a ship must wait to respawn

        private int fireLimit; // the amount of time in frames a ship must wait before firing again

        private bool fireRequest;

        private Vector2D velocity = new Vector2D(0, 0); // current velocity of the ship

        private bool king = false; // true when extra game mode is on and the current ship is the king

        private int width = 65; // image width in pixels

        private int height = 100; // image height in pixel

        


        /// <summary>
        /// Default construtor, needed to deserialize a ship from Json
        /// initializes a Ship object with no parameters set.
        /// </summary>
        public Ship()
            : this("", 0, new Vector2D(0,0), new Vector2D(0,-1), 5, 300, 6)
        {
        }
        
        /// <summary>
        /// Creates a new ship with the given ship name and id and located at
        /// the specified location and orientation
        /// </summary>
        public Ship(String shipName, int id, Vector2D location, Vector2D orientation, int startingHitPoints, int respawnDelay, int fireDelay)
        {
            ID = id;
            name = shipName;

            loc = location;
            dir = orientation;
            dir.Normalize();

            thrust = false;
            hp = startingHitPoints;
            score = 0;

            respawnLimit = respawnDelay;
            respawnTimer = 0;

            fireTimer = fireDelay;
            fireLimit = fireDelay;
            fireRequest = false;

            // new information needed keep player stats in db
            ShotsFired = 0;
            ShotsHit = 0; 
        }

        public bool TurnRight { get; set; }
        public bool TurnLeft { get; set; }

        /// <summary>
        /// Allows FireProjectile to be set to true only if fireDelay has elapsed.
        /// </summary>
        public bool FireProjectile { get  => fireRequest; set { fireRequest = FireRequest(value);} }
        public bool Thrust { get => thrust; set => thrust = value; }

        // new information needed keep player stats in db
        
        /// <summary>
        /// The total number of shots fired by this ship
        /// during a game
        /// </summary>
        public int ShotsFired { get; set; }

        /// <summary>
        /// The total number of shots fired by this ship which have hit another
        /// ship
        /// </summary>
        public int ShotsHit { get; set; }

        /// <summary>
        /// Returns a double representing the shot accuracy of this ship
        /// </summary>
        public double GetAccuracy()
        {
            // if no shots have been fired, do not divide by zer0
            if (ShotsFired == 0)
            {
                return 0.0;
            }

            // do this nonsense so the double we save in database doesn't have extra numbers
            // (same method from spreadsheet project)
            double accuracy = Double.Parse(((double)ShotsHit / ShotsFired).ToString());
            return accuracy;
        }

        /// <summary>
        /// If the request passed in is true then it returns true only if fireDelay has elapsed.
        /// Otherwise if the request is false then it returns false.
        /// </summary>
        private bool FireRequest(bool request)
        {
            // if trying to set to false, set to false
            if (!request)
            {
                return request;
            }

            // if we can fire and we are trying to set to true, set to true
            if (CanFire() && request)
            {
                return request;
            }

            return false;
        }



        /// <summary>
        /// Get the ship's ID
        /// </summary>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// Get the ship's location
        /// </summary>
        public Vector2D GetLocation()
        {
            return loc;
        }

        /// <summary>
        /// Set the ship's location to be at the passed in x and y
        /// </summary>
        public void SetLocation(Vector2D location)
        {
            loc = location;
        }

        /// <summary>
        /// Set the ship's location to be at the passed in x and y
        /// </summary>
        public void SetLocation(double x, double y)
        {
            loc = new Vector2D(x, y);
        }

        /// <summary>
        /// Get the direction of the ship
        /// </summary>
        public Vector2D GetDirection()
        {
            return dir;
        }

        /// <summary>
        /// Sets the ship's direction as a normalized vector
        /// </summary>
        public void SetDirection(Vector2D direction)
        {
            direction.Normalize();
            dir = direction;
        }

        /// <summary>
        /// if the ship has thrust then return true otherwise return false
        /// </summary>
        public bool HasThrust()
        {
            return thrust;
        }

        /// <summary>
        /// Gets the name of the ship
        /// </summary>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Sets the ship's name to be the passed in name
        /// </summary>
        public void SetName(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Get the ship's hp 
        /// </summary>
        public int GetHP()
        {
            return hp;
        }

        /// <summary>
        /// Sets the ships HP to be the passed in score
        /// </summary>
        public void SetHP(int score)
        {
            hp = score;
        }

        /// <summary>
        /// Gets the ship's score 
        /// </summary>
        public int GetScore()
        {
            return score;
        }

        /// <summary>
        /// Sets the ship's score to be the passed in score
        /// </summary>
        public void SetScore(int score)
        {
            this.score = score;
        }

        /// <summary>
        /// Gets the images width in pixels
        /// </summary>
        public int GetWidth()
        {
            return width;
        }

        /// <summary>
        /// Gets the image height in pixels
        /// </summary>
        public int GetHeight()
        {
            return height;
        }

        /// <summary>
        /// Gets the ship's current velocity
        /// </summary>
        public Vector2D GetVelocity()
        {
            return velocity;
        }

        /// <summary>
        /// Sets the ship's velocity to be the one passed in
        /// </summary>
        public void SetVelocity(Vector2D vel)
        {
            velocity = vel;
        }

        /// <summary>
        /// Returns true if the current ship is a king, false otherwise
        /// </summary>
        public bool IsKing()
        {
            return king;
        }

        /// <summary>
        /// Sets the passed k to be the boolean value of king
        /// </summary>
        public void SetKing(bool k)
        {
            king = k;
        }
        
        /// <summary>
        /// Returns true if the ship's health points are above 0. Otherwise, returns false.
        /// </summary>
        public bool IsAlive()
        {
            return (hp > 0);
        }

        /// <summary>
        /// If fireTimer is less than fireLimit increments fireTimer by one
        /// else, does nothing.
        /// </summary>
        public void IncrementFireTimer()
        {
            if (fireTimer <= fireLimit)
            {
                fireTimer++;
            }
        }

        /// <summary>
        /// If respawnTimer is less than respawnLimit increments respawnTimer by one
        /// else, does nothing.
        /// </summary>
        public void IncrementRespawnTimer()
        {
            if (respawnTimer <= respawnLimit)
            {
                respawnTimer++;
            }
        }

        /// <summary>
        /// If the last time we fired was more than the fireLimit, returns true
        /// else returns false
        /// </summary>
        public bool CanFire()
        {
            return (fireTimer >= fireLimit) && IsAlive();
        }

        /// <summary>
        /// If we died more frames ago than the respawnLimit, returns true
        /// else returns false.
        /// </summary>
        public bool CanRespawn()
        {
            return respawnTimer > respawnLimit;
        }

        /// <summary>
        /// Resets the FireTimer to 0;
        /// </summary>
        public void ResetFireTimer()
        {
            fireTimer = 0;
        }

        /// <summary>
        /// Resets the respawnTimer to 0;
        /// </summary>
        public void ResetRespawnTimer()
        {
            respawnTimer = 0;
        }
    }
}
