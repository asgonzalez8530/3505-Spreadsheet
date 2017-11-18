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
        /// Get the projectile's ID
        /// </summary>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// Set the projectile's ID to be the id 'passed in 
        /// </summary>
        public void SetID(int id)
        {
            ID = id;
        }

        /// <summary>
        /// Get the location of the projectile
        /// </summary>
        public Vector2D GetLocation()
        {
            return loc;
        }

        /// <summary>
        /// Set the location of the projectile to be the passed in projectile
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
        /// Set the direction of the projectile to be the passed in direction
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
        /// Set the life status of the projectile to be the passed in status
        /// </summary>
        public void SetAlive(bool alive)
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
        /// Set the owner of the projectile to be the passed in owner
        /// </summary>
        public void SetOwner(int owner)
        {
            this.owner = owner;
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
