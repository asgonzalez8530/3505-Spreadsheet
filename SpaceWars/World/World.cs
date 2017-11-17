﻿using System;
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

        /// <summary>
        /// If Ship s does not exist in ships adds it. If the id already exists
        /// in ships, updates reference in ships to s.
        /// </summary>
        public void AddShip(Ship s)
        {
            if (s == null)
            {
                return;
            }

            if (ships.ContainsKey(s.GetID()))
            {
                ships[s.GetID()] = s;
            }
            else
            {
                ships.Add(s.GetID(), s);
            }
        }

        public IEnumerable<Star> GetStars()
        {
            return stars.Values;
        }

        /// <summary>
        /// If Star s does not exist in stars adds it. If the id already exists
        /// in Star, updates reference in stars to s.
        /// </summary>
        public void AddStar(Star s)
        {
            if (s == null)
            {
                return;
            }

            if (stars.ContainsKey(s.GetID()))
            {
                stars[s.GetID()] = s;
            }
            else
            {
                stars.Add(s.GetID(), s);
            }
        }

        public IEnumerable<Projectile> GetProjs()
        {
            return projectiles.Values;
        }

        /// <summary>
        /// If Projectile p does not exist in projectiles adds it. If the id already exists
        /// in projectiles, updates reference in projectiles to p.
        /// </summary>
        public void AddProjectile(Projectile p)
        {
            if (p == null)
            {
                return;
            }

            if (projectiles.ContainsKey(p.GetID()))
            {
                projectiles[p.GetID()] = p;
            }
            else
            {
                projectiles.Add(p.GetID(), p);
            }
        }
    }
}
