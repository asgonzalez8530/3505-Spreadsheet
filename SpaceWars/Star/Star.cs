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

        public int GetID()
        {
            return ID;
        }

        public void SetID(int id)
        {
            ID = id;
        }

        public Vector2D GetLocation()
        {
            return loc;
        }

        public void SetLocation(Vector2D location)
        {
            loc = location;
        }

        public double GetMass()
        {
            return mass;
        }

        public void SetMass(double mass)
        {
            this.mass = mass;
        }
    }
}
