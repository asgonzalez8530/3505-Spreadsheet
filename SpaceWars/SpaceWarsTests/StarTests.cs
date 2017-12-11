using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceWars;

namespace SpaceWarsTests
{
    [TestClass]
    public class StarTests
    {
        [TestMethod]
        public void TestStarConstructor1()
        {
            // make a new star
            Star s = new Star(1, 199, 17, 19);

            // test the star properties are correct
            Assert.AreEqual(1, s.GetID());
            Assert.AreEqual(new Vector2D(199, 17), s.GetLocation());
            Assert.AreEqual(19, s.GetMass());
            Assert.AreEqual(100, s.GetWidth());
            Assert.AreEqual(70, s.GetHeight());
        }

        [TestMethod]
        public void TestStarConstructor2()
        {
            // make a new star
            Star s = new Star();

            // test default settings
            Assert.AreEqual(100, s.GetWidth());
            Assert.AreEqual(70, s.GetHeight());
        }
    }
}
