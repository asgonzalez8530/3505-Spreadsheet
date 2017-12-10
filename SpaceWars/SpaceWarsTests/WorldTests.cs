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

            // change the starting hit points
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

            // change the projectile speed
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

            // change the engine strength
            w.SetEngineStrength("199");

            // test that the default was changed
            Assert.AreEqual((double)199, worldAccessor.GetField("engineStrength"));
        }

        [TestMethod]
        public void TestTurningRate1()
        {
            // make a new world 
            World w = new World();

            // test default size
            PrivateObject worldAccessor = new PrivateObject(w);
            Assert.AreEqual(2, worldAccessor.GetField("turningRate"));

            // change the turning rate
            w.SetTurningRate("199");

            // test that the default was changed
            Assert.AreEqual(199, worldAccessor.GetField("turningRate"));
        }

        [TestMethod]
        public void TestShipSize1()
        {
            // make a new world 
            World w = new World();

            // test default size
            PrivateObject worldAccessor = new PrivateObject(w);
            Assert.AreEqual(20, worldAccessor.GetField("shipSize"));

            // change the ship size
            w.SetShipSize("199");

            // test that the default was changed
            Assert.AreEqual(199, worldAccessor.GetField("shipSize"));
        }

        [TestMethod]
        public void TestStarSize1()
        {
            // make a new world 
            World w = new World();

            // test default size
            PrivateObject worldAccessor = new PrivateObject(w);
            Assert.AreEqual(35, worldAccessor.GetField("starSize"));

            // change the star size
            w.SetStarSize("199");

            // test that the default was changed
            Assert.AreEqual(199, worldAccessor.GetField("starSize"));
        }

        [TestMethod]
        public void TestMSPerFrame1()
        {
            // make a new world 
            World w = new World();

            // test default size
            Assert.AreEqual(16, w.GetMSPerFrame());

            // change the MS per frame
            w.SetMSPerFrame("199");

            // test that the default was changed
            Assert.AreEqual(199, w.GetMSPerFrame());
        }

        [TestMethod]
        public void TestFiringDelay1()
        {
            // make a new world 
            World w = new World();

            // test default size
            PrivateObject worldAccessor = new PrivateObject(w);
            Assert.AreEqual(6, worldAccessor.GetField("projectileFiringDelay"));

            // change the firing delay
            w.SetProjectileFiringDelay("199");

            // test that the default was changed
            Assert.AreEqual(199, worldAccessor.GetField("projectileFiringDelay"));
        }

        [TestMethod]
        public void TestRespawnDelay1()
        {
            // make a new world 
            World w = new World();

            // test default size
            PrivateObject worldAccessor = new PrivateObject(w);
            Assert.AreEqual(300, worldAccessor.GetField("respawnDelay"));

            // change the respawn delay
            w.SetRespawnDelay("199");

            // test that the default was changed
            Assert.AreEqual(199, worldAccessor.GetField("respawnDelay"));
        }

        [TestMethod]
        public void TestKingOfTheHill1()
        {
            // make a new world 
            World w = new World();

            // test default size
            PrivateObject worldAccessor = new PrivateObject(w);
            Assert.AreEqual(false, worldAccessor.GetField("kingIsOn"));

            // change the king setting
            w.SetKingOfTheHill("true");

            // test that the default was changed
            Assert.AreEqual(true, worldAccessor.GetField("kingIsOn"));

            // change the king setting
            w.SetKingOfTheHill("True");

            // test that the default was changed
            Assert.AreEqual(true, worldAccessor.GetField("kingIsOn"));

            // change the king setting
            w.SetKingOfTheHill("Princess Ariel");

            // test that the default was changed
            Assert.AreEqual(false, worldAccessor.GetField("kingIsOn"));
        }

        [TestMethod]
        public void TestMakeShipWithKingOn()
        {
            // make a new world 
            World w = new World();

            // turn on king
            w.SetKingOfTheHill("true");

            // make a ship as king 
            w.MakeNewShip("ship1", 1);

            // check that the ship was made king
            foreach (Ship s in w.GetAllShips())
            {
                Assert.IsTrue(s.GetName() == "King ship1");
                s.SetHP(0);
            }

            // make lots of ships
            for (int i = 1; i < 50; i++)
            {
                // make a new ship
                w.MakeNewShip("", i);
            }

            // make sure that no other ship is king
            int shipCounter = 1;
            foreach (Ship s in w.GetAliveShips())
            {
                Assert.IsFalse(s.GetName() == "King ");
                shipCounter++;
            }

            Assert.AreEqual(50, shipCounter);
        }

        [TestMethod]
        public void TestAddShip()
        {
            // make a new world 
            World w = new World();

            // make a ship as king 
            Ship ship = new Ship("ship1", 1, new Vector2D(0, 0), new Vector2D(0, 0), 0, 0, 0);
            w.AddShip(ship);

            int counter = 0;
            // check that the ship was made king
            foreach (Ship s in w.GetAliveShips())
            {
                counter++;
            }

            Assert.AreEqual(0, counter);
            
            // make lots of ships
            for (int i = 1; i < 50; i++)
            {
                // make a new ship
                w.MakeNewShip("ship", i);
            }

            int shipCounter = 1;
            foreach (Ship s in w.GetAliveShips())
            {
                shipCounter++;
            }

            Assert.AreEqual(50, shipCounter);
        }

        [TestMethod]
        public void TestAddShip()
        {
            // make a new world 
            World w = new World();

            // make a ship as king 
            Ship ship = new Ship("ship1", 1, new Vector2D(0, 0), new Vector2D(0, 0), 0, 0, 0);
            w.AddShip(ship);

            int counter = 0;
            // check that the ship was made king
            foreach (Ship s in w.GetAliveShips())
            {
                counter++;
            }

            Assert.AreEqual(0, counter);

            // make lots of ships
            for (int i = 1; i < 50; i++)
            {
                // make a new ship
                w.MakeNewShip("ship", i);
            }

            int shipCounter = 1;
            foreach (Ship s in w.GetAliveShips())
            {
                shipCounter++;
            }

            Assert.AreEqual(50, shipCounter);
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

            // try to set the engine strength with null
            w.SetEngineStrength("null");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEngineStrength3()
        {
            // make a new world 
            World w = new World();

            // try to set the engine strength with null
            w.SetEngineStrength("a199");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEngineStrength4()
        {
            // make a new world 
            World w = new World();

            // try to set the engine strength with null
            w.SetEngineStrength("Snow White");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTurningRate2()
        {
            // make a new world 
            World w = new World();

            // try to set the turning rate with null
            w.SetTurningRate("null");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTurningRate3()
        {
            // make a new world 
            World w = new World();

            // try to set the turning rate with null
            w.SetTurningRate("a199");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTurningRate4()
        {
            // make a new world 
            World w = new World();

            // try to set the turning rate with null
            w.SetTurningRate("Cinderella");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestShipSize2()
        {
            // make a new world 
            World w = new World();

            // try to set the ship size with null
            w.SetShipSize("null");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestShipSize3()
        {
            // make a new world 
            World w = new World();

            // try to set the ship size with null
            w.SetShipSize("a199");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestShipSize4()
        {
            // make a new world 
            World w = new World();

            // try to set the ship size with null
            w.SetShipSize("Princess Anna");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStarSize2()
        {
            // make a new world 
            World w = new World();

            // try to set the star size with null
            w.SetStarSize("null");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStarSize3()
        {
            // make a new world 
            World w = new World();

            // try to set the star size with null
            w.SetStarSize("a199");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStarSize4()
        {
            // make a new world 
            World w = new World();

            // try to set the star size with null
            w.SetStarSize("Princess Elsa");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMSPerFrame2()
        {
            // make a new world 
            World w = new World();

            // try to set the MS per frame with null
            w.SetMSPerFrame("null");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMSPerFrame3()
        {
            // make a new world 
            World w = new World();

            // try to set the MS per frame with null
            w.SetMSPerFrame("a199");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMSPerFrame4()
        {
            // make a new world 
            World w = new World();

            // try to set the MS per frame with null
            w.SetMSPerFrame("Meredith");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFiringDelay2()
        {
            // make a new world 
            World w = new World();

            // try to set the firing delay with null
            w.SetProjectileFiringDelay("null");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFiringDelay3()
        {
            // make a new world 
            World w = new World();

            // try to set the firing delay with null
            w.SetProjectileFiringDelay("a199");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFiringDelay4()
        {
            // make a new world 
            World w = new World();

            // try to set the firing delay with null
            w.SetProjectileFiringDelay("Princess Tiana");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestReSpawnDelay2()
        {
            // make a new world 
            World w = new World();

            // try to set the firing delay with null
            w.SetRespawnDelay("null");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRespawnDelay3()
        {
            // make a new world 
            World w = new World();

            // try to set the firing delay with null
            w.SetRespawnDelay("a199");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRespawnDelay4()
        {
            // make a new world 
            World w = new World();

            // try to set the firing delay with null
            w.SetRespawnDelay("Princess Rapunzel");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMakeANewStarNull1()
        {
            // make a new world 
            World w = new World();

            // make a star
            w.MakeNewStar(null, "0", ".01");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMakeANewStarNull2()
        {
            // make a new world 
            World w = new World();

            // make a star
            w.MakeNewStar("0", null, ".01");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMakeANewStarNull3()
        {
            // make a new world 
            World w = new World();

            // make a star
            w.MakeNewStar("0", "0", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMakeANewStarNull4()
        {
            // make a new world 
            World w = new World();

            // make a star
            w.MakeNewStar(null, null, null);
        }
    }
}
