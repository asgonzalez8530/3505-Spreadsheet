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
            w.SetKingOfTheHill("Princess Ariel");

            // test that the default was changed
            Assert.AreEqual(false, worldAccessor.GetField("kingIsOn"));
        }

        [TestMethod]
        public void TestKingOfTheHill2()
        {
            // make a new world 
            World w = new World();

            // test default size
            PrivateObject worldAccessor = new PrivateObject(w);
            Assert.AreEqual(false, worldAccessor.GetField("kingIsOn"));

            // change the king setting
            w.SetKingOfTheHill("True");

            // test that the default was changed
            Assert.AreEqual(true, worldAccessor.GetField("kingIsOn"));

            // change the king setting
            w.SetKingOfTheHill(null);

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

            Ship ship1 = null;
            w.AddShip(ship1);

            shipCounter = 1;
            foreach (Ship s in w.GetAliveShips())
            {
                shipCounter++;
            }

            Assert.AreEqual(50, shipCounter);
        }

        [TestMethod]
        public void TestMotionOfShip1()
        {
            // make a new world 
            World w = new World();

            // make a star
            w.MakeNewStar("0", "0", ".01");

            // make lots of ships and add them to the world 
            for (int i = 0; i < 50; i++)
            {
                // make a new ship
                w.MakeNewShip("ship" + i, i);
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
                w.MotionForShips(s);
                shipCounter++;
            }

            Assert.AreEqual(50, shipCounter);
        }

        [TestMethod]
        public void TestMotionOfProjectiles1()
        {
            // make a new world 
            World w = new World();

            // make a star
            w.MakeNewStar("0", "0", ".01");

            // make lots of ships and add them to the world 
            for (int i = 0; i < 50; i++)
            {
                // make a new ship
                w.MakeNewShip("ship" + i, i);
            }

            // make lots of ships and add them to the world 
            for (int i = 0; i < 50; i++)
            {
                // make a new ship
                w.AddProjectile(new Projectile(i, i, new Vector2D(0 + 1, 0 + 1), new Vector2D(0 + 1, 0 + 1)));
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
                w.MotionForShips(s);
                shipCounter++;
            }

            Assert.AreEqual(50, shipCounter);

            int projCounter = 0;
            foreach (Projectile p in w.GetProjs())
            {
                w.MotionForProjectiles(p);
                projCounter++;
            }

            Assert.AreEqual(50, projCounter);
        }

        //************************ Invalid Test ************************// 


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWorldSize3()
        {
            // make a new world 
            World w = new World();

            // try to set the size with null
            w.SetUniverseSize(null);
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
            w.SetStartingHitPoint(null);
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
            w.SetProjectileSpeed(null);
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
            w.SetEngineStrength(null);
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
            w.SetTurningRate(null);
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
            w.SetShipSize(null);
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
            w.SetStarSize(null);
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
            w.SetMSPerFrame(null);
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
            w.SetProjectileFiringDelay(null);
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
            w.SetRespawnDelay(null);
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUpDateShipCommands_NoShip()
        {
            // make a new world 
            World w = new World();

            string commands = "(TRLF)";
            int shipID = 1;

            w.UpdateShipCommands(commands, shipID);
        }

        [TestMethod]
        public void TestUpDateShipCommands_RightFirst()
        {
            // make a new world 
            World w = new World();

            string shipName = "foo";
            string commands = "(TRLF)";
            int shipID = 1;
            
            // make a ship
            w.MakeNewShip(shipName, shipID);

            // pass commands to ship
            w.UpdateShipCommands(commands, shipID);

            // get the ship back from the world
            HashSet<Ship> ships = new HashSet<Ship>(w.GetAllShips());
            Ship s = new Ship();
            foreach (Ship ship in ships)
            {
                if (ship.GetName() == shipName)
                {
                    s = ship;
                }
            }

            // check ship state for passed in commands
            Assert.IsTrue(s.Thrust);
            // only one direction should be turned on
            Assert.IsTrue(s.TurnLeft || s.TurnRight);
            Assert.IsFalse(s.TurnLeft && s.TurnRight);

            //Right should be on and left off
            Assert.IsTrue(s.TurnRight);
            Assert.IsFalse(s.TurnLeft);
        }

        [TestMethod]
        public void TestUpDateShipCommands_LeftFirst()
        {
            // make a new world 
            World w = new World();

            string shipName = "foo";
            string commands = "(TLRF)";
            int shipID = 1;

            // make a ship
            w.MakeNewShip(shipName, shipID);

            // pass commands to ship
            w.UpdateShipCommands(commands, shipID);

            // get the ship back from the world
            HashSet<Ship> ships = new HashSet<Ship>(w.GetAllShips());
            Ship s = new Ship();
            foreach (Ship ship in ships)
            {
                if (ship.GetName() == shipName)
                {
                    s = ship;
                }
            }

            // check ship state for passed in commands
            Assert.IsTrue(s.Thrust);
            // only one direction should be turned on
            Assert.IsTrue(s.TurnLeft || s.TurnRight);
            Assert.IsFalse(s.TurnLeft && s.TurnRight);

            //Right should be on and left off
            Assert.IsFalse(s.TurnRight);
            Assert.IsTrue(s.TurnLeft);
        }

        [TestMethod]
        public void TestAddStar_NullStar()
        {
            // make a new world 
            World w = new World();
            // add null star, nothing should be added
            w.AddStar(null);

            List<Star> stars = new List<Star>(w.GetStars());
            Assert.AreEqual(0, stars.Count);

        }

        [TestMethod]
        public void TestAddStar_UpdateReference()
        {
            // make a new world 
            World w = new World();
            
            int starID = 1;
            Star star1 = new Star(starID, 0, 0, 0.1);
            w.AddStar(star1);

            //replace star1 with star2, which has the same ID
            Star star2 = new Star(starID, 0, 0, 0.1);
            w.AddStar(star2);

            List<Star> stars = new List<Star>(w.GetStars());
            Assert.AreEqual(1, stars.Count);

            Star storedStar = stars[0];
            Assert.AreEqual(star2, storedStar);
            Assert.AreNotEqual(star1, storedStar);

        }

        [TestMethod]
        public void TestAddProjectile_NullProjectile()
        {
            // make a new world 
            World w = new World();
            // add null projectile, nothing should be added
            w.AddProjectile(null);

            List<Projectile> projectiles = new List<Projectile>(w.GetProjs());
            Assert.AreEqual(0, projectiles.Count);

        }

        [TestMethod]
        public void TestAddProjectile_DeadProjectile()
        {
            // make a new world 
            World w = new World();
           
            Projectile projectile1 = new Projectile();
            w.AddProjectile(projectile1);

            // kill projectile
            projectile1.Alive(false);
            w.AddProjectile(projectile1);

            List<Projectile> projectiles = new List<Projectile>(w.GetProjs());
            Assert.AreEqual(0, projectiles.Count);

        }

        [TestMethod]
        public void TestAddProjectile_ReplaceProjectile()
        {
            // make a new world 
            World w = new World();
           
            Projectile projectile1 = new Projectile();
            w.AddProjectile(projectile1);

            // kill projectile
            Projectile projectile2 = new Projectile();
            w.AddProjectile(projectile2);

            List<Projectile> projectiles = new List<Projectile>(w.GetProjs());
            Assert.AreEqual(1, projectiles.Count);

            Projectile storedProjectile = projectiles[0];
            Assert.AreEqual(projectile2, storedProjectile);
            Assert.AreNotEqual(projectile1, storedProjectile);

        }

        [TestMethod]
        public void TestMotionForProjectiles_DeadProjectile()
        {
            // make a new world 
            World w = new World();
            // add projectile
            Projectile projectile1 = new Projectile();
            w.AddProjectile(projectile1);

            // kill projectile
            projectile1.Alive(false);

            //move projectile should be skipped
            w.MotionForProjectiles(projectile1);

            List<Projectile> projectiles = new List<Projectile>(w.GetProjs());
            Vector2D expectedLocation= new Vector2D();
            double expectedProjectileLocationX = expectedLocation.GetX();
            double expectedProjectileLocationY = expectedLocation.GetY();

            Assert.IsTrue(projectile1.GetLocation().GetX() == expectedProjectileLocationX );
            Assert.IsTrue(projectile1.GetLocation().GetY() == expectedProjectileLocationY);

        }


        /*************** Test Private Methods **************/
        [TestMethod]
        public void TestWraparound_OutOfBounds()
        {
            // make a world and set its size
            World w = new World();
            w.SetUniverseSize(10);

            // a location outside the edge of the world
            Vector2D location = new Vector2D(6, 6);

            // a ship positioned at the location outside of world
            Ship myShip = new Ship("foo", 0, location, new Vector2D(0, 1), 5, 300, 7);

            // invoke method in provate object
            PrivateObject worldAccessor = new PrivateObject(w);
            worldAccessor.Invoke("Wraparound", new Ship[1] { myShip });

            // get x and y coordinates of ship
            Vector2D shipLocation = myShip.GetLocation();
            double shipX = shipLocation.GetX();
            double shipY = shipLocation.GetY();

            // get starting x and y coordinates
            double startX = location.GetX();
            double startY = location.GetY();

            // position should have changed
            Assert.IsFalse(shipX == startX);
            Assert.IsFalse(shipY == startY);
        }

        [TestMethod]
        public void TestWraparound_InBounds()
        {
            // make a world and set its size
            World w = new World();
            w.SetUniverseSize(10);

            // a location outside the edge of the world
            Vector2D location = new Vector2D(4, 4);

            // a ship positioned at the location inside of world
            Ship myShip = new Ship("foo", 0, location, new Vector2D(0, 1), 5, 300, 7);

            // invoke method in provate object
            PrivateObject worldAccessor = new PrivateObject(w);
            worldAccessor.Invoke("Wraparound", new Ship[1] { myShip });

            // get x and y coordinates of ship
            Vector2D shipLocation = myShip.GetLocation();
            double shipX = shipLocation.GetX();
            double shipY = shipLocation.GetY();

            // get starting x and y coordinates
            double startX = location.GetX();
            double startY = location.GetY();

            // position should have changed
            Assert.IsTrue(shipX == startX);
            Assert.IsTrue(shipY == startY);
        }

        [TestMethod]
        public void TestCollisionWithAStar_Hit()
        {
            // create world
            World w = new World();
            
            // define location to be center of world
            Vector2D location = new Vector2D(0, 0);

            // a ship and star with the same location
            Ship myShip = new Ship("King foo", 0, location, new Vector2D(0, 1), 5, 300, 7);
            Star myStar = new Star(0, 0, 0, 0.1);

            myShip.SetKing(true);
            w.AddShip(myShip);

            // set king of the hill for more consequences
            w.SetKingOfTheHill("true");

            

            // invoke private method
            PrivateObject worldAccessor = new PrivateObject(w);
            worldAccessor.Invoke("CollisionWithAStar", new Object[2] { myShip, myStar });

            // check that hit points have changed
            Assert.AreEqual(0, myShip.GetHP());
            
        }

        [TestMethod]
        public void TestCollisionWithAStar_Miss()
        {
            World w = new World();

            // create a location which should not cause a colision
            Ship myShip = new Ship("foo", 0, new Vector2D(0, 200), new Vector2D(0, 1), 5, 300, 7);
            Star myStar = new Star(0, 0, 0, 0.1);

            // turn on king of the hill for more consequences
            w.SetKingOfTheHill("true");

            // invoke private method
            PrivateObject worldAccessor = new PrivateObject(w);
            worldAccessor.Invoke("CollisionWithAStar", new Object[2] { myShip, myStar });

            // make sure ship missed star
            Assert.AreEqual(5, myShip.GetHP());

        }

        [TestMethod]
        public void TestCollisionProjectile_KingHit()
        {
            // create world
            World w = new World();

            // define location to be center of world
            Vector2D location = new Vector2D(0, 0);

            // a ship and star with the same location
            Ship KingShip = new Ship("King foo", 0, location, new Vector2D(0, 1), 1, 300, 7);
            Projectile myProjectile = new Projectile(1, 0, location, new Vector2D(0, 1));
            Ship otherShip = new Ship("foo", 1, location, new Vector2D(0, 1), 1, 300, 7);

            KingShip.SetKing(true);
            w.AddShip(KingShip);
            w.AddShip(otherShip);
            w.AddProjectile(myProjectile);

            // set king of the hill for more consequences
            w.SetKingOfTheHill("true");


            // invoke private method
            PrivateObject worldAccessor = new PrivateObject(w);
            worldAccessor.Invoke("CollisionWithAProjectile", new Object[2] { KingShip, myProjectile });

            // check that hit points have changed
            Assert.AreEqual(0, KingShip.GetHP());

        }

        [TestMethod]
        public void TestCollisionProjectile_Hit()
        {
            // create world
            World w = new World();

            // define location to be center of world
            Vector2D location = new Vector2D(0, 0);

            // a ship and star with the same location
            Ship KingShip = new Ship("King foo", 0, location, new Vector2D(0, 1), 1, 300, 7);
            Projectile myProjectile = new Projectile(1, 0, location, new Vector2D(0, 1));
            Ship otherShip = new Ship("foo", 1, location, new Vector2D(0, 1), 1, 300, 7);
            w.AddShip(KingShip);
            w.AddShip(otherShip);
            w.AddProjectile(myProjectile);


            // invoke private method
            PrivateObject worldAccessor = new PrivateObject(w);
            worldAccessor.Invoke("CollisionWithAProjectile", new Object[2] { KingShip, myProjectile });

            // check that hit points have changed
            Assert.AreEqual(0, KingShip.GetHP());

        }

        [TestMethod]
        public void TestProjectileOffScreen_InBounds()
        {
            // make a world and set its size
            World w = new World();
            w.SetUniverseSize(10);

            // a location outside the edge of the world
            Vector2D location = new Vector2D(4, 4);
            Vector2D direction = new Vector2D(0, 1);

            // a projectile positioned at the location inside of world
            Projectile myProjectile = new Projectile(0, 0, location, direction);

            // invoke method in provate object
            PrivateObject worldAccessor = new PrivateObject(w);
            worldAccessor.Invoke("ProjectileOffScreen", new Projectile[1] { myProjectile });

            Assert.IsTrue(myProjectile.IsAlive());
        }

        [TestMethod]
        public void TestProjectileOffScreen_OutOfBoundsX()
        {
            // make a world and set its size
            World w = new World();
            w.SetUniverseSize(10);

            // a location outside the edge of the world
            Vector2D location = new Vector2D(6, 0);
            Vector2D direction = new Vector2D(0, 1);

            // a ship projectile at the location outside of world
            Projectile myProjectile = new Projectile(0, 0, location, direction);

            // invoke method in provate object
            PrivateObject worldAccessor = new PrivateObject(w);
            worldAccessor.Invoke("ProjectileOffScreen", new Projectile[1] { myProjectile });

            Assert.IsFalse(myProjectile.IsAlive());
        }

        [TestMethod]
        public void TestProjectileOffScreen_OutOfBoundsY()
        {
            // make a world and set its size
            World w = new World();
            w.SetUniverseSize(10);

            // a location outside the edge of the world
            Vector2D location = new Vector2D(0, 6);
            Vector2D direction = new Vector2D(0, 1);

            // a ship projectile at the location outside of world
            Projectile myProjectile = new Projectile(0, 0, location, direction);

            // invoke method in provate object
            PrivateObject worldAccessor = new PrivateObject(w);
            worldAccessor.Invoke("ProjectileOffScreen", new Projectile[1] { myProjectile });

            Assert.IsFalse(myProjectile.IsAlive());
        }
    }
}
