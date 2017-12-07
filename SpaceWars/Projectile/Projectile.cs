// Anastasia Gonzalez and Aaron Bellis UID: u0985898 & u0981638
// Code implemented as part of PS7 : SpaceWars client CS3500 Fall Semester
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpaceWars
{
    /// <summary>
    /// Represents a Projectile object in a SpaceWars game. Provides information
    /// about location direction to game.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        [JsonProperty(PropertyName = "proj")]
        private int ID; // unique ID for the projectile

        [JsonProperty]
        private Vector2D loc; // current location of the projectile

        [JsonProperty]
        private Vector2D dir; // current direction that the projectile is headed in

        [JsonProperty]
        private bool alive; // true if the projectile is still in the world

        [JsonProperty]
        private int owner; // ID for the ship that the projectile belongs to

        private int width = 25; // image width in pixels

        private int height = 25; // image height in pixels

        /// <summary>
        /// Default construtor, needed to deserialize a ship from Json
        /// initializes a Ship object with no parameters set.
        /// </summary>
        public Projectile()
        {
        }

        /// <summary>
        /// Creates a new ship with the given ship owner and id and located at
        /// the specified location and orientation with a constant velocity
        /// </summary>
        public Projectile(int owner, int id, Vector2D location, Vector2D direction)
        {
            this.owner = owner;
            ID = id;
            loc = location;
            dir = direction;
            alive = true;
        }

        /// <summary>
        /// Get the projectile's ID
        /// </summary>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// Get the location of the projectile
        /// </summary>
        public Vector2D GetLocation()
        {
            return loc;
        }

        /// <summary>
        /// Sets the location of the projectile
        /// </summary>
        public void SetLocation(Vector2D location)
        {
            loc = location;
        }

        /// <summary>
        /// Get the projectile's direction 
        /// </summary>
        public Vector2D GetDirection()
        {
            return dir;
        }

        /// <summary>
        /// Sets the direction of the projectile
        /// </summary>
        public void SetDirection(Vector2D direction)
        {
            dir = direction;
        }

        /// <summary>
        /// if the projectile is alive then return true otherwise returns false
        /// </summary>
        public bool IsAlive()
        {
            return alive;
        }

        /// <summary>
        /// Sets the boolean alive to the passed in boolean
        /// </summary>
        public void Alive(bool alive)
        {
            this.alive = alive;
        }

        /// <summary>
        /// Gets the owner of the projectile
        /// </summary>
        public int GetOwner()
        {
            return owner;
        }

        /// <summary>
        /// Gets the width of the image in pixels
        /// </summary>
        public int GetWidth()
        {
            return width;
        }

        /// <summary>
        /// Gets the height of the image in pixels
        /// </summary>
        public int GetHeight()
        {
            return height;
        }
    }
}
