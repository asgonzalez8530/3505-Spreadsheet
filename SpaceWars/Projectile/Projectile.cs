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
        private int ID;

        [JsonProperty]
        private Vector2D loc;

        [JsonProperty]
        private Vector2D dir;

        [JsonProperty]
        private bool alive;

        [JsonProperty]
        private int owner;

        private int width = 50;
        private int height = 50;

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

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }
    }
}
