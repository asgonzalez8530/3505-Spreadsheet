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
        private int ID;

        [JsonProperty]
        private Vector2D loc;

        [JsonProperty]
        private Vector2D dir;

        [JsonProperty]
        private bool thrust;

        [JsonProperty]
        private string name;

        [JsonProperty]
        private int hp;

        [JsonProperty]
        private int score;

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

        public bool IsThrust()
        {
            return thrust;
        }

        public void SetThrust(bool thrust)
        {
            this.thrust = thrust;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public int GetHP()
        {
            return hp;
        }

        public void SetHP(int HP)
        {
            hp = HP;
        }

        public int GetScore()
        {
            return score;
        }

        public void SetScore(int score)
        {
            this.score = score;
        }
    }
}
