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
        private int ID; // unique ID for the star

        [JsonProperty]
        private Vector2D loc; // current location of the star in the world

        [JsonProperty]
        private double mass; // star's mass

        private int width = 100; // image width in pixels

        private int height = 70; // image height in pixels

        /// <summary>
        /// Gets the Star's ID
        /// </summary>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// Gets the ship's location
        /// </summary>
        public Vector2D GetLocation()
        {
            return loc;
        }

        /// <summary>
        /// Get star's mass 
        /// </summary>
        public double GetMass()
        {
            return mass;
        }

        /// <summary>
        /// Gets the image width in pixels
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
    }
}
