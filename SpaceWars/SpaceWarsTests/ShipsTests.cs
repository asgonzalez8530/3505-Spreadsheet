using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceWars;

namespace SpaceWarsTests
{
    [TestClass]
    public class ShipsTests
    {
        [TestMethod]
        public void TestEmptyConstructor()
        {
            // make a new ship
            Ship s = new Ship();

            // test that the defaults are correct
            Assert.AreEqual(65, s.GetWidth());
            Assert.AreEqual(100, s.GetHeight());
            Assert.AreEqual(0, s.GetScore());
            Assert.IsFalse(s.IsKing());
        }

        [TestMethod]
        public void TestShipProperties1()
        {
            // make a new ship
            Ship s = new Ship();

            // make some changes to the ship
            s.SetName("Nala");
            s.SetLocation(4, 5);
            s.SetDirection(new Vector2D(0, 4));

            // test that the changes are correct
            Assert.AreEqual("Nala", s.GetName());
            Assert.AreEqual(new Vector2D(4, 5), s.GetLocation());
            Assert.AreEqual(new Vector2D(0, 1), s.GetDirection());
            Assert.AreEqual(5, s.GetHP());
        }

        [TestMethod]
        public void TestKing()
        {
            // make a new ship
            Ship s = new Ship();

            // check that king is off
            Assert.IsFalse(s.IsKing());

            // make ship king
            s.SetKing(true);

            // check to see if ship is king
            Assert.IsTrue(s.IsKing());
        }

        [TestMethod()]
        public void TestFiring1()
        {
            // make a new ship
            Ship s = new Ship("Simba", 1, new Vector2D(0, 0), new Vector2D(0, 1), 5, 10, 6);

            // make sure the default is false
            Assert.IsFalse(s.FireProjectile);

            // change fire projectile to true
            s.FireProjectile = true;
            Assert.IsTrue(s.FireProjectile);

            // reset the timer
            s.ResetFireTimer();

            // try to fire again before ship can with the firing delay
            s.FireProjectile = true;
            Assert.IsFalse(s.FireProjectile);
        }

        [TestMethod()]
        public void TestFiring2()
        {
            // make a new ship
            Ship s = new Ship("Simba", 1, new Vector2D(0, 0), new Vector2D(0, 1), 5, 10, 6);

            // make sure the default is false
            Assert.IsFalse(s.FireProjectile);

            // change fire projectile to true
            s.FireProjectile = true;
            Assert.IsTrue(s.FireProjectile);

            // change it back to false
            s.FireProjectile = false;
            Assert.IsFalse(s.FireProjectile);
        }

        [TestMethod()]
        public void TestRespawn1()
        {
            // make a new ship
            Ship s = new Ship();
            
            // make sure the default timer is correct
            PrivateObject shipAccessor = new PrivateObject(s);
            Assert.AreEqual(0, shipAccessor.GetField("respawnTimer"));

            // increment timer
            s.IncrementRespawnTimer();
            Assert.AreEqual(1, shipAccessor.GetField("respawnTimer"));

            // check that ship can't respawn
            Assert.IsFalse(s.CanRespawn());

            // increment timer so that the ship can respawn
            for (int i = 0; i < 300; i++)
            {
                s.IncrementRespawnTimer();
            }

            // check that the ship can respawn
            Assert.IsTrue(s.CanRespawn());
        }

        [TestMethod()]
        public void TestRespawn2()
        {
            // make a new ship
            Ship s = new Ship("Simba", 1, new Vector2D(0, 0), new Vector2D(0, 1), 5, 10, 6);

            // make sure the default timer is correct
            PrivateObject shipAccessor = new PrivateObject(s);
            Assert.AreEqual(0, shipAccessor.GetField("respawnTimer"));

            // increment timer
            s.IncrementRespawnTimer();
            Assert.AreEqual(1, shipAccessor.GetField("respawnTimer"));

            // check that ship can't respawn
            Assert.IsFalse(s.CanRespawn());

            // increment timer so that the ship can respawn
            for (int i = 0; i < 10; i++)
            {
                s.IncrementRespawnTimer();
            }

            // check that the ship can respawn
            Assert.IsTrue(s.CanRespawn());
        }

        [TestMethod()]
        public void TestThrust()
        {
            // make a new ship
            Ship s = new Ship();

            // make sure thrust default is false
            Assert.IsFalse(s.Thrust);

            // set thrust to true
            s.Thrust = true;

            // check to see if the thrust was changed to true
            Assert.IsTrue(s.Thrust);
        }

        [TestMethod()]
        public void TestTurnRight()
        {
            // make a new ship
            Ship s = new Ship();

            // make sure default is false
            Assert.IsFalse(s.TurnRight);

            // set turning right to true
            s.TurnRight = true;

            // check to see if turning right was changed to true
            Assert.IsTrue(s.TurnRight);
        }

        [TestMethod()]
        public void TestTurnLeft()
        {
            // make a new ship
            Ship s = new Ship();

            // make sure default is false
            Assert.IsFalse(s.TurnLeft);

            // set turning left to true
            s.TurnLeft = true;

            // check to see if turning left was changed to true
            Assert.IsTrue(s.TurnLeft);
        }
    }
}
