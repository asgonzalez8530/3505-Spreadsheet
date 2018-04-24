using Newtonsoft.Json;
using System;

namespace SpaceWars
{
    /// <summary>
    /// Ship in the SpaceWars game World. Fields of this class "Opt In" for JSONObject MemberSerialization.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Ship : IComparable
    {
        /// <summary>
        /// Ship's unique ID.
        /// </summary>
        [JsonProperty(PropertyName = "ship")]
        private int ID;

        /// <summary>
        /// Ship's current location information, as a 2D Vector.
        /// </summary>
        [JsonProperty]
        private Vector2D loc;

        /// <summary>
        /// Ship's current direction information, as a 2D Vector.
        /// </summary>
        [JsonProperty]
        private Vector2D dir;

        /// <summary>
        /// Ship's current "thrust" status.
        /// </summary>
        [JsonProperty]
        private bool thrust;

        /// <summary>
        /// Ship's user-provided name.
        /// </summary>
        [JsonProperty]
        private string name;

        /// <summary>
        /// Ship's current hit points.
        /// </summary>
        [JsonProperty]
        private int hp;

        /// <summary>
        /// Number of kills attributed to this Ship.
        /// </summary>
        [JsonProperty]
        private int score;

        /// <summary>
        /// Property for persistent velocity.
        /// </summary>
        public Vector2D Velocity { get; set; }

        /// <summary>
        /// Time stamp from the last time this Ship fired.
        /// </summary>
        public int TimeLastFired { get; set; }

        /// <summary>
        /// Time stamp for the time this Ship was killed.
        /// </summary>
        public int TimeOfDeath { get; set; }

        /// <summary>
        /// The time (frame) of the most recent spawn.
        /// </summary>
        public int TimeLastSpawned { get; set; }

        /// <summary>
        /// True if this ship's client is connected, false otherwise.
        /// </summary>
        public bool Connected { get; set; }

        public Ship() { }

        public Ship(int givenID, string givenName, int startingHP = 5)
        {
            ID = givenID;
            name = givenName;

            // make ship at 0,0 by default
            loc = new Vector2D(0, 0);

            // make ship face up by default
            dir = new Vector2D(0, -1);

            thrust = false;
            hp = startingHP;
            score = 0;
            Velocity = new Vector2D(0, 0);
            TimeLastFired = -7;
            TimeOfDeath = -5;
            TimeLastSpawned = -100;
            Connected = true;
        }


        /// <summary>
        /// Gets the location vector
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {
            return new Vector2D(loc);
        }


        /// <summary>
        /// Sets the location vector
        /// </summary>
        /// <param name="newLoc"></param>
        public void SetLocation(Vector2D newLoc)
        {
            loc = newLoc;
        }


        /// <summary>
        /// Gets the direction vector
        /// </summary>
        /// <returns></returns>
        public Vector2D GetOrientation()
        {
            return new Vector2D(dir);
        }


        /// <summary>
        /// Sets the direction vector
        /// </summary>
        /// <param name="newDir"></param>
        public void SetOrientation(Vector2D newDir)
        {
            dir = newDir;
        }


        /// <summary>
        /// Gets this Ship's ID (each client has its own)
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return ID;
        }


        /// <summary>
        /// Returns true if thrust is on, false otherwise
        /// </summary>
        /// <returns></returns>
        public bool IsThrusting()
        {
            return thrust;
        }


        /// <summary>
        /// Switch the thrust boolean to true.
        /// </summary>
        public void ThrustOn()
        {
            thrust = true;
        }


        /// <summary>
        /// Switch the thrust boolean to false.
        /// </summary>
        public void ThrustOff()
        {
            thrust = false;
        }


        /// <summary>
        /// Gets health points
        /// </summary>
        /// <returns></returns>
        public int GetHP()
        {
            return hp;
        }


        /// <summary>
        /// Sets health points
        /// </summary>
        /// <param name="newHP"></param>
        public void SetHP(int newHP)
        {
            hp = newHP;
        }


        /// <summary>
        /// Gets player name
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return name;
        }


        /// <summary>
        /// Gets player score
        /// </summary>
        /// <returns></returns>
        public int GetScore()
        {
            return score;
        }


        /// <summary>
        /// Increase score by 1.
        /// </summary>
        public void IncrementScore()
        {
            score++;
        }


        /// <summary>
        /// Comparison function for sorting Ships.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            //if obj is instance of ship
            //this.score compare to (ship)obj.GetScore()
            if(obj is Ship)
            {
                Ship daShip = obj as Ship;
                return -(score.CompareTo(daShip.score));
            }

            throw new ArgumentException("Compared object was not a Ship.");  
        }
    }
}
