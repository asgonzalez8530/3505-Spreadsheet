using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceWars;

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
            Star star = new Star(1, 0, 0, .01);
            w.AddStar(star);

            // make lots of ships and add them to the world 
            for (int i = 0; i < 50; i++)
            {
                // make a new star
                w.MakeNewShip("star", 1);
            }
            
            
        }
    }
}
