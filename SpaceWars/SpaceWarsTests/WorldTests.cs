using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceWars;
using System.Collections.Generic;

namespace SpaceWarsTests
{
    [TestClass]
    public class WorldTests
    {
        [TestMethod]
        public void SanityCheck()
        {
            // make a new world 
            World w = new World();

            // make a star
            w.MakeNewStar("0", "0", ".01");

            // make lots of ships and add them to the world 
            for (int i = 0; i < 50; i++)
            {
                // make a new ship
                w.MakeNewShip("ship", i);
            }

            int starCounter = 0;
            foreach (Star s in w.GetStars())
            {
                starCounter++;
            }

            Assert.AreEqual(1, starCounter);

            int shipCounter = 0;
            foreach (Ship s in w.GetAllShips())
            {
                shipCounter++;
            }

            Assert.AreEqual(50, shipCounter);
        }

        [TestMethod]
        public void TestWorld1()
        {
            // make a new world 
            World w = new World();

            
        }
    }
}
