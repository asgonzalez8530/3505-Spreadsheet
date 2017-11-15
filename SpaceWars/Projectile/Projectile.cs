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

        public Vector2D GetDirection()
        {
            return dir;
        }

        public void SetDirection(Vector2D direction)
        {
            dir = direction;
        }

        public bool IsAlive()
        {
            return alive;
        }

        public void SetAlive(bool alive)
        {
            this.alive = alive;
        }

        public int GetOwner()
        {
            return owner;
        }

        public void SetOwner(int owner)
        {
            this.owner = owner;
        }
    }
}
