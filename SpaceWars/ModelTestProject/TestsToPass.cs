using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceWars;
using System.Collections.Generic;

namespace ModelTestProject
{
    [TestClass]
    public class TestsToPass
    {
        [TestMethod]
        public void ProjDiesWhenLeavingWorldNorth()
        {
            // make world and projectile
            int worldSize = 400;
            World newWorld = new World(worldSize);
            Projectile newProj = new Projectile(0, 0, new Vector2D(0, 1), new Vector2D(0, 0));
            newWorld.UpdateProjectile(newProj, 0);
            Assert.IsTrue(newProj.IsAlive());

            // relocate projectile offscreen
            newProj.SetLocation(new Vector2D(0, -400));
            newWorld.Update(100);
            Assert.IsFalse(newProj.IsAlive());
        }

        [TestMethod]
        public void ProjDiesWhenLeavingWorldSouth()
        {
            int worldSize = 400;
            World newWorld = new World(worldSize);
            Projectile newProj = new Projectile(0, 0, new Vector2D(0, 1), new Vector2D(0, 0));
            newWorld.UpdateProjectile(newProj, 0);
            Assert.IsTrue(newProj.IsAlive());

            // relocate projectile offscreen
            newProj.SetLocation(new Vector2D(0, (worldSize) / 2));
            newWorld.Update(100);
            Assert.IsFalse(newProj.IsAlive());
        }

        [TestMethod]
        public void ProjDiesWhenLeavingWorldEast()
        {
            int worldSize = 400;
            World newWorld = new World(worldSize);
            Projectile newProj = new Projectile(0, 0, new Vector2D(0, 1), new Vector2D(0, 0));
            newWorld.UpdateProjectile(newProj, 0);
            Assert.IsTrue(newProj.IsAlive());

            // relocate projectile offscreen
            newProj.SetLocation(new Vector2D(-400, 0));
            newWorld.Update(100);
            Assert.IsFalse(newProj.IsAlive());
        }

        [TestMethod]
        public void ProjDiesWhenLeavingWorldWest()
        {
            int worldSize = 400;
            World newWorld = new World(worldSize);
            Projectile newProj = new Projectile(0, 0, new Vector2D(0, 1), new Vector2D(0, 0));
            newWorld.UpdateProjectile(newProj, 0);
            Assert.IsTrue(newProj.IsAlive());

            // relocate projectile offscreen
            newProj.SetLocation(new Vector2D(400, 0));
            newWorld.Update(100);
            Assert.IsFalse(newProj.IsAlive());
        }

        [TestMethod]
        public void ProjDiesWhenHittingStar()
        {
            // make world, star, and projectile
            int worldSize = 400;
            int starRadius = 35;
            int projSpeed = 15;
            Vector2D starLocation = new Vector2D(-100, 0);

            World newWorld = new World(worldSize, 0.08, projSpeed, 2, starRadius, 20, 300, 5);
            Star theStar = new Star(0, 0.01, starLocation);
            theStar.GetID();
            Projectile newProj = new Projectile(0, 0, new Vector2D(0, 1), new Vector2D(0, 0));
            newWorld.UpdateProjectile(newProj, 0);
            newWorld.UpdateStar(theStar, 0);
            Assert.IsTrue(newProj.IsAlive());

            // relocate projectile
            newProj.SetLocation(starLocation + new Vector2D(starRadius - projSpeed + 1, 0));
            newWorld.Update(100);
            Assert.IsFalse(newProj.IsAlive());
        }

        [TestMethod]
        public void ProjCollideWithStar()
        {
            World theWorld = new World(750);
            Star aStar = new Star(0, 0.02, new Vector2D(0, 0));
            Projectile proj = new Projectile(0, 0, new Vector2D(0, 1), new Vector2D(0, 0));
            Projectile farProj = new Projectile(0, 0, new Vector2D(0, 1), new Vector2D(0, 50));

            PrivateObject obj = new PrivateObject(theWorld);
            Assert.AreEqual(obj.Invoke("CollidedWith", new object[] { proj, aStar}), true);
            Assert.AreEqual(obj.Invoke("CollidedWith", new object[] { farProj, aStar}), false);
        }

        [TestMethod]
        public void ShipCollideWithStar()
        {
            World theWorld = new World(750);
            Star aStar = new Star(0, 0.02, new Vector2D(0, 0));
            Ship ship = new Ship(0, "close");
            ship.SetLocation(new Vector2D(0, 20));
            Ship farShip = new Ship(1, "far");
            farShip.SetLocation(new Vector2D(100, 0));

            PrivateObject obj = new PrivateObject(theWorld);
            Assert.AreEqual(obj.Invoke("CollidedWith", new object[] { aStar, ship }), true);
            Assert.AreEqual(obj.Invoke("CollidedWith", new object[] { aStar, farShip }), false);
        }

        [TestMethod]
        public void ProjDiesWhenHittingShipAndShipLosesHP()
        {
            // make world
            int worldSize = 400;
            int shipRadius = 20;
            World newWorld = new World(worldSize, 0.08, 15, 2, 35, shipRadius, 300, 5);

            // make ship and projectile (ship is not projectile owner)
            Ship theShip = new Ship(3, "me");
            theShip.SetLocation(new Vector2D(shipRadius, 0));
            Projectile newProj = new Projectile(0, 0, new Vector2D(1, 0), new Vector2D(0, 0)); // starts at center, and will move right 15 units

            // add the projectile and ship to the world
            newWorld.UpdateProjectile(newProj, 0);
            newWorld.UpdateShip(theShip, 3);
            Assert.IsTrue(newProj.IsAlive());
            Assert.IsTrue(theShip.GetHP() == 5);

            newWorld.Update(100);
            Assert.IsFalse(newProj.IsAlive());
            Assert.IsTrue(theShip.GetHP() == 4);
        }

        [TestMethod]
        public void KillShipKillProjIncreaseScore()
        {
            // make world
            int worldSize = 400;
            int shipRadius = 20;
            World newWorld = new World(worldSize, 0.08, 15, 2, 35, shipRadius, 300, 5);

            // make them ship
            Ship themShip = new Ship(3, "them");
            themShip.SetHP(1);
            Vector2D themLocation = new Vector2D(100, 100);
            themShip.SetLocation(themLocation);
            newWorld.UpdateShip(themShip, 3);

            // make us ship
            Ship usShip = new Ship(0, "us");
            usShip.SetLocation(new Vector2D(100, 200));
            newWorld.UpdateShip(usShip, 0);

            // make projectile
            Projectile newProj = new Projectile(0, 0, new Vector2D(0, 1), new Vector2D(themLocation));
            newWorld.UpdateProjectile(newProj, 0);
            Assert.IsTrue(newProj.IsAlive());
            Assert.IsTrue(themShip.GetHP() == 1);
            Assert.IsTrue(usShip.GetScore() == 0);

            // relocate projectile
            newWorld.Update(100);
            Assert.IsFalse(newProj.IsAlive());
            Assert.IsTrue(themShip.GetHP() == 0);
            Assert.IsTrue(usShip.GetScore() == 1);
        }

        [TestMethod]
        public void NoSelfFriendlyFire()
        {
            // make world
            int worldSize = 400;
            int shipRadius = 20;
            World newWorld = new World(worldSize);

            // make us ship
            Ship usShip = new Ship(0, "us");
            usShip.SetHP(1);
            newWorld.UpdateShip(usShip, 0);

            // make projectile
            Projectile newProj = new Projectile(0, 0, new Vector2D(0, 1), new Vector2D(100, 100));
            newWorld.UpdateProjectile(newProj, 0);
            Assert.IsTrue(newProj.IsAlive());
            Assert.IsTrue(usShip.GetScore() == 0);
            Assert.IsTrue(usShip.GetHP() == 1);

            // relocate projectile
            newProj.SetLocation(new Vector2D(shipRadius, 0));
            newWorld.Update(100);
            Assert.IsTrue(newProj.IsAlive());
            Assert.IsTrue(usShip.GetScore() == 0);
            Assert.IsTrue(usShip.GetHP() == 1);
        }

        [TestMethod]
        public void ShipWraparoundNorth()
        {
            // make world
            int worldSize = 400;
            World newWorld = new World(worldSize);

            // make us ship
            Ship usShip = new Ship(0, "us");
            newWorld.UpdateShip(usShip, 0);

            // put ship off screen
            usShip.SetLocation(new Vector2D(0, ((-worldSize) / 2) - 1));

            // ship wraps around
            newWorld.Update(100);
            Assert.IsTrue(usShip.GetLocation().Equals(new Vector2D(0, (worldSize / 2) - 1)));
        }

        [TestMethod]
        public void ShipWraparoundSouth()
        {
            // make world
            int worldSize = 400;
            World newWorld = new World(worldSize);

            // make us ship
            Ship usShip = new Ship(0, "us");
            newWorld.UpdateShip(usShip, 0);

            // put ship off screen
            usShip.SetLocation(new Vector2D(0, ((worldSize) / 2) + 1));

            // ship wraps around
            newWorld.Update(100);
            Assert.IsTrue(usShip.GetLocation().Equals(new Vector2D(0, ((-worldSize) / 2) + 1)));
        }

        [TestMethod]
        public void ShipWraparoundEast()
        {
            // make world
            int worldSize = 400;
            World newWorld = new World(worldSize);

            // make us ship
            Ship usShip = new Ship(0, "us");
            newWorld.UpdateShip(usShip, 0);

            // put ship off screen
            usShip.SetLocation(new Vector2D((worldSize / 2) + 1, 0));

            // ship wraps around
            newWorld.Update(100);
            Assert.IsTrue(usShip.GetLocation().Equals(new Vector2D((-worldSize / 2) + 1, 0)));
        }

        [TestMethod]
        public void ShipWraparoundWest()
        {
            // make world
            int worldSize = 400;
            World newWorld = new World(worldSize);

            // make us ship
            Ship usShip = new Ship(0, "us");
            newWorld.UpdateShip(usShip, 0);

            // put ship off screen
            usShip.SetLocation(new Vector2D(((-worldSize) / 2) - 1, 0));

            // ship wraps around
            newWorld.Update(100);
            Assert.IsTrue(usShip.GetLocation().Equals(new Vector2D(((worldSize) / 2) - 1, 0)));
        }

        [TestMethod] 
        public void GetNameShip()
        {
            Ship ship = new Ship(0, "John");
            Assert.AreEqual("John", ship.GetName());
        }

        [TestMethod]
        public void CompareToShip2IsGreater()
        {
            Ship ship1 = new Ship(1, "one");
            ship1.IncrementScore();

            Ship ship2 = new Ship(2, "two");
            ship2.IncrementScore();
            ship2.IncrementScore();

            Assert.IsTrue(ship1.CompareTo(ship2) > 0);
        }

        [TestMethod]
        public void CompareToShip1IsGreater()
        {
            Ship ship1 = new Ship(1, "one");
            ship1.IncrementScore();
            ship1.IncrementScore();
            ship1.IncrementScore();

            Ship ship2 = new Ship(2, "two");
            ship2.IncrementScore();
            ship2.IncrementScore();

            Assert.IsTrue(ship1.CompareTo(ship2) < 0);
        }

        [TestMethod]
        public void CompareToShipsAreEqual()
        {
            Ship ship1 = new Ship(1, "one");
            ship1.IncrementScore();
            ship1.IncrementScore();

            Ship ship2 = new Ship(2, "two");
            ship2.IncrementScore();
            ship2.IncrementScore();

            Assert.IsTrue(ship1.CompareTo(ship2) == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareToNotShip()
        {
            Ship ship1 = new Ship(1, "one");
            ship1.IncrementScore();
            ship1.IncrementScore();
            ship1.IncrementScore();

            String ship2 = "fake ship";

            ship1.CompareTo(ship2);

        }

        [TestMethod]
        public void ThrustingTest()
        {
            Ship ship = new Ship(0, "me");
            ship.ThrustOff();
            Assert.IsFalse(ship.IsThrusting());
            ship.ThrustOn();
            Assert.IsTrue(ship.IsThrusting());
        }

        [TestMethod]
        public void FireProjectile()
        {
            World myWorld = new World(750);

            Ship ship = new Ship(0, "me");
            ship.SetLocation(new Vector2D(0, 0));
            ship.SetOrientation(new Vector2D(1, 0));
            myWorld.UpdateShip(ship, 0);
            Assert.AreEqual(-7, ship.TimeLastFired);

            myWorld.FireProjectile(0, 0);

            List<Projectile> projs = new List<Projectile>(myWorld.GetProjectiles());
            Assert.IsTrue(projs.Count == 1);
            Assert.AreEqual(projs[0].GetOrientation(), ship.GetOrientation());
        }

        [TestMethod]
        public void RotateRight()
        {
            World myWorld = new World(750, 0.08, 15, 90, 35, 20, 300, 5);
            Vector2D expectedDir = new Vector2D(1, 0);
            expectedDir.Rotate(90);

            Ship ship = new Ship(0, "me");
            ship.SetLocation(new Vector2D(0, 0));
            ship.SetOrientation(new Vector2D(1, 0));
            myWorld.UpdateShip(ship, 0);

            myWorld.RotateShipRight(0);

            Assert.AreEqual(expectedDir, ship.GetOrientation());
        }

        [TestMethod]
        public void RotateLeft()
        {
            World myWorld = new World(750, 0.08, 15, 90, 35, 20, 300, 5);
            Vector2D expectedDir = new Vector2D(1, 0);
            expectedDir.Rotate(-90);

            Ship ship = new Ship(0, "me");
            ship.SetLocation(new Vector2D(0, 0));
            ship.SetOrientation(new Vector2D(1, 0));
            myWorld.UpdateShip(ship, 0);

            myWorld.RotateShipLeft(0);

            Assert.AreEqual(expectedDir, ship.GetOrientation());
        }

        [TestMethod]
        public void RespawnDeadShip()
        {
            World world = new World(750, 0.08, 15, 2, 35, 20, 300, 5);

            Ship ship = new Ship(0, "me");
            world.UpdateShip(ship, 0);
            ship.SetHP(0);
            ship.TimeOfDeath = 195;

            world.Update(200);
            Assert.AreEqual(0, ship.GetHP());

            world.Update(500);
            Assert.AreEqual(5, ship.GetHP());
        }

        [TestMethod]
        public void KillProj()
        {
            World world = new World(750);
            Projectile proj = new Projectile(0, 0, new Vector2D(1, 0), new Vector2D(0, 0));
            world.UpdateProjectile(proj, 0);

            proj.KillProj();
            
            List<Projectile> orig = new List<Projectile>(world.GetProjectiles());
            Assert.AreEqual(1, orig.Count);

            world.Update(100);
            List<Projectile> projs = new List<Projectile>(world.GetProjectiles());
            Assert.AreEqual(0, projs.Count);
        }

        [TestMethod]
        public void GetSize()
        {
            World world = new World(750);
            Assert.AreEqual(750, world.GetSize());
        }

        [TestMethod]
        public void SortedShips()
        {
            World world = new World(750);
            Ship ship1 = new Ship(1, "bro");
            Ship ship2 = new Ship(2, "bro");
            Ship ship3 = new Ship(3, "bro");

            world.UpdateShip(ship1, 1);
            world.UpdateShip(ship2, 2);
            world.UpdateShip(ship3, 3);

            ship1.IncrementScore();
            ship2.IncrementScore();
            ship2.IncrementScore();
            ship3.IncrementScore();
            ship3.IncrementScore();
            ship3.IncrementScore();

            List<Ship> sorted = new List<Ship>(world.GetShips());

            Assert.AreEqual(ship3, sorted[0]);
            Assert.AreEqual(ship2, sorted[1]);
            Assert.AreEqual(ship1, sorted[2]);
        }

        [TestMethod]
        public void StarGetSizeChangeSize()
        {
            World theWorld = new World(750);
            Star aStar = new Star(0, 0.02, new Vector2D(0, 0));
            theWorld.UpdateStar(aStar, 0);

            Assert.AreEqual(0.02, aStar.GetSize());
            aStar.SetSize(0.2);
            Assert.AreEqual(0.2, aStar.GetSize());
        }

        [TestMethod]
        public void StarGetLocChangeLoc()
        {
            World theWorld = new World(750);
            Star aStar = new Star(0, 0.02, new Vector2D(0, 0));
            theWorld.UpdateStar(aStar, 0);

            Assert.IsTrue(new Vector2D(0, 0).Equals(aStar.GetLocation()));
            aStar.SetLocation(new Vector2D(300, -20));
            Assert.IsTrue(new Vector2D(300, -20).Equals(aStar.GetLocation()));

        }

        [TestMethod]
        public void NullNotEqualToVector()
        {
            Vector2D meVector = new Vector2D(7, 7);
            Assert.IsFalse(meVector.Equals(null));
        }

        [TestMethod]
        public void NonVectorNotEqualToVector()
        {
            Vector2D meVector = new Vector2D(7, 7);
            Assert.IsFalse(meVector.Equals(new Ship()));
        }

        [TestMethod]
        public void SameVectorSameHashcode()
        {
            Vector2D aVector = new Vector2D(7, 7);
            Vector2D bVector = new Vector2D(7, 7);
            Assert.IsTrue(aVector.GetHashCode().Equals(bVector.GetHashCode()));
        }

        [TestMethod]
        public void TestPositiveClamp()
        {
            Vector2D aVector = new Vector2D(7, 7);
            aVector.Clamp();
            Assert.IsTrue(aVector.Equals(new Vector2D(1, 1)));
        }

        [TestMethod]
        public void TestNegativeClamp()
        {
            Vector2D aVector = new Vector2D(-7, -7);
            aVector.Clamp();
            Assert.IsTrue(aVector.Equals(new Vector2D(-1, -1)));
        }

        [TestMethod]
        public void ToAngleZeroX()
        {
            Vector2D meVector = new Vector2D(0, 1);
            meVector.Normalize();
            Assert.AreEqual(180, meVector.ToAngle());
        }

        [TestMethod]
        public void ToAngleNegativeX()
        {
            Vector2D meVector = new Vector2D(-1, 0);
            meVector.Normalize();
            Assert.AreEqual(-90, meVector.ToAngle());
        }

        [TestMethod]
        public void UpdateAndGetWorldStars()
        {
            World theWorld = new World(750);
            Star aStar = new Star(0, 0.02, new Vector2D(0, 0));
            theWorld.UpdateStar(aStar, 0);
            theWorld.UpdateStar(aStar, 0);

            HashSet<Star> starSet = new HashSet<Star>(theWorld.GetStars());
            Assert.AreEqual(1, starSet.Count);
        }

        [TestMethod]
        public void UpdateAndGetWorldShips()
        {
            World theWorld = new World(750);
            Ship meShip = new Ship();
            theWorld.UpdateShip(meShip, 0);
            theWorld.UpdateShip(meShip, 0);

            Ship otherShip = theWorld.GetShip(0);
            Assert.AreEqual(meShip.GetID(), otherShip.GetID());
        }

        [TestMethod]
        public void UpdateAndKillProj()
        {
            World theWorld = new World(750);
            Projectile aProj = new Projectile(0, 0, new Vector2D(1, 1), new Vector2D(0, 0));
            theWorld.UpdateProjectile(aProj, 0);
            theWorld.UpdateProjectile(aProj, 0);
            HashSet<Projectile> projSet = new HashSet<Projectile>(theWorld.GetProjectiles());
            Assert.AreEqual(1, projSet.Count);

            aProj.KillProj();
            theWorld.UpdateProjectile(aProj, 0);
            projSet = new HashSet<Projectile>(theWorld.GetProjectiles());
            Assert.AreEqual(0, projSet.Count);
        }

        [TestMethod]
        public void CheckGravity()
        {
            World theWorld = new World(750);
            Ship meShip = new Ship(0, "Us");
            theWorld.UpdateShip(meShip, 0);
            Star aStar = new Star(0, 0.05, new Vector2D(80, 80));
            theWorld.UpdateStar(aStar, 0);
            Assert.IsTrue(meShip.GetLocation().Equals(new Vector2D(0, 0)));

            // after updating, the ship should be in a different location, due
            // to the star's gravity
            theWorld.Update(1);
            Assert.IsFalse(meShip.GetLocation().Equals(new Vector2D(0, 0)));

            // Switch the ships thrust on
            meShip.ThrustOn();
            theWorld.Update(2);
        }

        [TestMethod]
        public void CheckThrust()
        {
            World theWorld = new World(750);
            Ship meShip = new Ship(0, "Us");
            theWorld.UpdateShip(meShip, 0);
            Assert.IsTrue(meShip.GetLocation().Equals(new Vector2D(0, 0)));

            // after updating, the ship should be in a different location, due
            // to the ship's thrust
            meShip.ThrustOn();
            theWorld.Update(1);
            Assert.IsFalse(meShip.GetLocation().Equals(new Vector2D(0, 0)));
        }

        [TestMethod]
        public void StarKillShip()
        {
            World theWorld = new World(750);
            Ship meShip = new Ship(0, "Us");
            theWorld.UpdateShip(meShip, 0);
            Star aStar = new Star(0, 0.05, new Vector2D(80, 80));
            theWorld.UpdateStar(aStar, 0);
            theWorld.CheckCollisions(1);
            Assert.AreEqual(-5, meShip.TimeOfDeath);

            meShip.SetLocation(new Vector2D(80, 40));
            theWorld.CheckCollisions(2);
            Assert.AreEqual(2, meShip.TimeOfDeath);
        }

        [TestMethod]
        public void NoCollisionIfProfAlreadyDead()
        {
            World theWorld = new World(750);

            Projectile meProj = new Projectile(0, 0, new Vector2D(0, 1), new Vector2D(80, 80));
            theWorld.UpdateProjectile(meProj, 0);
            meProj.KillProj();

            Star aStar = new Star(0, 0.05, new Vector2D(80, 80));
            theWorld.UpdateStar(aStar, 0);

            theWorld.CheckCollisions(1);
        }

        [TestMethod]
        public void NoCollisionIfShipAlreadyDead()
        {
            World theWorld = new World(750);
            Ship meShip = new Ship(0, "Us");
            meShip.SetHP(0);
            theWorld.UpdateShip(meShip, 0);
            Star aStar = new Star(0, 0.05, new Vector2D(80, 80));
            theWorld.UpdateStar(aStar, 0);
            theWorld.CheckCollisions(1);
            Assert.AreEqual(-5, meShip.TimeOfDeath);

            meShip.SetLocation(new Vector2D(80, 40));
            theWorld.CheckCollisions(2);
            Assert.AreEqual(-5, meShip.TimeOfDeath);
        }

        [TestMethod]
        public void GetStartingShipHP()
        {
            World theWorld = new World(750);
            Ship meShip = new Ship(0, "Us");
            theWorld.UpdateShip(meShip, 0);
            Assert.AreEqual(meShip.GetHP(), theWorld.GetStartingHP());
        }

        [TestMethod]
        public void RemoveShipFromWorld()
        {
            World theWorld = new World(750);
            Ship meShip = new Ship(0, "Us");
            theWorld.UpdateShip(meShip, 0);
            HashSet<Ship> worldShips = new HashSet<Ship>(theWorld.GetShips());
            Assert.AreEqual(1, worldShips.Count);

            theWorld.RemoveShip(0);
            Assert.AreEqual(0, meShip.GetHP());
            Assert.IsFalse(meShip.Connected);
        }

        [TestMethod]
        public void CheckShipLocation()
        {
            // check safeness with star
            World theWorld = new World(750);
            PrivateObject obj = new PrivateObject(theWorld);
            Assert.AreEqual(obj.Invoke("CheckSafeness", new object[] { new Vector2D(0, 0) }), new Vector2D(0, 0));

            Star theStar = new Star(0, 0.01, new Vector2D(0, 0));
            theWorld.UpdateStar(theStar, 0);
            Assert.AreNotEqual(obj.Invoke("CheckSafeness", new object[] { new Vector2D(0, 0) }), new Vector2D(0, 0));
        }

        [TestMethod]
        public void SafeGameModeOnShipIsSafe()
        {
            World theWorld = new World(750, 0.08, 15, 2, 35, 20, 300, 5, true);
            Ship theShip = new Ship(0, "Us");
            theWorld.UpdateShip(theShip, 0);
            theShip.TimeLastSpawned = 10;

            Projectile proj = new Projectile(2, 0, new Vector2D(0, 1), new Vector2D(0, 0));
            theWorld.UpdateProjectile(proj, 0);

            theWorld.Update(20);
            Assert.AreEqual(5, theShip.GetHP());
        }

        [TestMethod]
        public void SafeGameModeOnShipIsNotSafe()
        {
            World theWorld = new World(750, 0.08, 15, 2, 35, 20, 300, 5, true);
            Ship theShip = new Ship(0, "Us");
            theWorld.UpdateShip(theShip, 0);

            Projectile proj = new Projectile(2, 0, new Vector2D(0, 1), new Vector2D(0, 0));
            theWorld.UpdateProjectile(proj, 0);

            theWorld.Update(20);
            Assert.AreEqual(4, theShip.GetHP());
        }

    }
}
