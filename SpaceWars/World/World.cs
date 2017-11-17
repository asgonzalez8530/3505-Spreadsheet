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
            if (s != null)
            {
                ships.Add(s.GetID(), s);
            }
        }

        public IEnumerable<Star> GetStars()
        {
            return stars.Values;
        }

        public void AddStar(Star s)
        {
            if (s != null)
            {
                stars.Add(s.GetID(), s);
            }
        }

        public IEnumerable<Projectile> GetProjs()
        {
            return projectiles.Values;
        }

        public void AddProjectile(Projectile p)
        {
            if (p != null)
            {
                projectiles.Add(p.GetID(), p);
            }
        }
    }
}
