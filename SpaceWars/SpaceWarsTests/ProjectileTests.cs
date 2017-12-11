// Anastasia Gonzalez and Aaron Bellis UID: u0985898 & u0981638
// Code implemented as part of PS7 and PS8: SpaceWars client/Server CS3500 Fall Semester
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceWars;

namespace SpaceWarsTests
{
    [TestClass]
    public class ProjectileTests
    {
        [TestMethod]
        public void TestEmptyConstructor()
        {
            // make a new projectile
            Projectile p = new Projectile();

            // test that the default settings are correct
            Assert.AreEqual(25, p.GetHeight());
            Assert.AreEqual(25, p.GetWidth());
        }

        [TestMethod]
        public void TestSetDirection()
        {
            // make a new projectile
            Projectile p = new Projectile(1, 3, new Vector2D(199, 17), new Vector2D(0, 0));

            // set a new direction
            p.SetDirection(new Vector2D(0, 4));

            // test that the default settings are correct
            Assert.AreEqual(new Vector2D(0, 1), p.GetDirection());
        }
    }
}
