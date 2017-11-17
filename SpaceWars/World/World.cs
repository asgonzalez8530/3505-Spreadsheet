using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWars
{
    public class World
    {
        private Dictionary<int, Ship> ships;
        private Dictionary<int, Star> stars;
        private Dictionary<int, Projectile> projectiles;

        public World()
        {
            ships = new Dictionary<int, Ship>();
            stars = new Dictionary<int, Star>();
            projectiles = new Dictionary<int, Projectile>();

        }

        public IEnumerable<Ship> GetShips()
        {
            return ships.Values;
        }

        public void AddShip(Ship s)
        {
            ships.Add(s.GetID(), s);
        }

        public IEnumerable<Star> GetStars()
        {
            return stars.Values;
        }

        public void AddStar(Star s)
        {
            stars.Add(s.GetID(), s);
        }

        public IEnumerable<Projectile> GetProjs()
        {
            return projectiles.Values;
        }

        public void AddStar(Projectile p)
        {
            projectiles.Add(p.GetID(), p);
        }
    }
}
