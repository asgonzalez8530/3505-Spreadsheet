using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpaceWars
{
    /// <summary>
    /// Represents a Star in a SpaceWars game. Provides information about
    /// location and mass to game. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Star
    {
        [JsonProperty(PropertyName = "star")]
        private int ID;

        [JsonProperty]
        private Vector2D loc;

        [JsonProperty]
        private double mass;

        private int width = 132;
        private int height = 103;

        /// <summary>
        /// Gets the Star's ID
        /// </summary>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// Sets the stars ID to the passed in id
        /// </summary>
        public void SetID(int id)
        {
            ID = id;
        }

        /// <summary>
        /// Gets the ship's location
        /// </summary>
        public Vector2D GetLocation()
        {
            return loc;
        }

        /// <summary>
        /// Sets the passed in location to the star's location
        /// </summary>
        public void SetLocation(Vector2D location)
        {
            loc = location;
        }

        /// <summary>
        /// Get star's mass 
        /// </summary>
        public double GetMass()
        {
            return mass;
        }

        /// <summary>
        /// Sets the star's mass to the passed in mass
        /// </summary>
        public void SetMass(double mass)
        {
            this.mass = mass;
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
