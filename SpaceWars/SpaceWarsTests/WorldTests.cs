using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceWars;
using System.Collections.Generic;

namespace SpaceWarsTests
{
    [TestClass]
    public class WorldTests
    {

        //************************ Valid Test ************************// 

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
        public void TestWorldSize1()
        {
            // make a new world 
            World w = new World();

            // test default size
            Assert.AreEqual(750, w.GetSize());

            // change the universe size
            w.SetUniverseSize("199");

            // test that the default was changed
            Assert.AreEqual(199, w.GetSize());
        }

        [TestMethod]
        public void TestWorldSize2()
        {
            // make a new world 
            World w = new World();

            // change the universe size
            w.SetUniverseSize(199);

            // test that the default was changed
            Assert.AreEqual(199, w.GetSize());
        }

        [TestMethod]
        public void TestStartingHitPoints1()
        {
            // make a new world 
            World w = new World();

            // test default size
            PrivateObject worldAccessor = new PrivateObject(w);
            Assert.AreEqual(5, worldAccessor.GetField("startingHitPoints"));

            // change the universe size
            w.SetStartingHitPoint("199");

            // test that the default was changed
            Assert.AreEqual(199, worldAccessor.GetField("startingHitPoints"));
        }

        [TestMethod]
        public void TestProjectileSpeed1()
        {
            // make a new world 
            World w = new World();

            // test default size
            PrivateObject worldAccessor = new PrivateObject(w);
            Assert.AreEqual(15, worldAccessor.GetField("projectileSpeed"));

            // change the universe size
            w.SetProjectileSpeed("199");

            // test that the default was changed
            Assert.AreEqual(199, worldAccessor.GetField("projectileSpeed"));
        }

        [TestMethod]
        public void TestEngineStrength1()
        {
            // make a new world 
            World w = new World();

            // test default size
            PrivateObject worldAccessor = new PrivateObject(w);
            Assert.AreEqual(.08, worldAccessor.GetField("engineStrength"));

            // change the universe size
            w.SetEngineStrength("199");

            // test that the default was changed
            Assert.AreEqual((double) 199, worldAccessor.GetField("engineStrength"));
        }

        //************************ Invalid Test ************************// 


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWorldSize3()
        {
            // make a new world 
            World w = new World();

            // try to set the size with null
            w.SetUniverseSize("null");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWorldSize4()
        {
            // make a new world 
            World w = new World();

            // try to set the size with an invalid number
            w.SetUniverseSize("a199");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWorldSize5()
        {
            // make a new world 
            World w = new World();

            // try to set the size with an invalid number
            w.SetUniverseSize("Princess Aurora");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStartingHitPoints2()
        {
            // make a new world 
            World w = new World();

            // try to set the hit points with null
            w.SetStartingHitPoint("null");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStartingHitPoints3()
        {
            // make a new world 
            World w = new World();

            // try to set the hit points with null
            w.SetStartingHitPoint("a199");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStartingHitPoints4()
        {
            // make a new world 
            World w = new World();

            // try to set the hit points with null
            w.SetStartingHitPoint("Princess Jasmine");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestProjectileSpeed2()
        {
            // make a new world 
            World w = new World();

            // try to set the projectile speed with null
            w.SetProjectileSpeed("null");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestProjectileSpeed3()
        {
            // make a new world 
            World w = new World();

            // try to set the projectile speed with null
            w.SetProjectileSpeed("a199");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestProjectileSpeed4()
        {
            // make a new world 
            World w = new World();

            // try to set the projectile speed with null
            w.SetProjectileSpeed("Princess Anna");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEngineStrength2()
        {
            // make a new world 
            World w = new World();

            // try to set the projectile speed with null
            w.SetEngineStrength("null");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEngineStrength3()
        {
            // make a new world 
            World w = new World();

            // try to set the projectile speed with null
            w.SetEngineStrength("a199");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEngineStrength4()
        {
            // make a new world 
            World w = new World();

            // try to set the projectile speed with null
            w.SetEngineStrength("Snow White");
        }
    }
}
