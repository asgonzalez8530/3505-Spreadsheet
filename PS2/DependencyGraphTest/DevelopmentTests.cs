// tests in indicated sections written by Aaron Bellis u0981638 for CS3500

using SpreadsheetUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PS2GradingTests
{
    /// <summary>
    ///  This is a test class for DependencyGraphTest
    /// 
    ///  These tests should help guide you on your implementation.  Warning: you can not "test" yourself
    ///  into correctness.  Tests only show incorrectness.  That being said, a large test suite will go a long
    ///  way toward ensuring correctness.
    /// 
    ///  You are strongly encouraged to write additional tests as you think about the required
    ///  functionality of yoru library.
    /// 
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        // ************************** TESTS ON EMPTY DGs ************************* //

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void ZeroSize()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void HasNoDependees()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependees("a"));
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void HasNoDependents()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependents("a"));
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyDependees()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.GetDependees("a").GetEnumerator().MoveNext());
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyDependents()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.GetDependents("a").GetEnumerator().MoveNext());
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyIndexer()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t["a"]);
        }

        /// <summary>
        ///Removing from an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void RemoveFromEmpty()
        {
            DependencyGraph t = new DependencyGraph();
            t.RemoveDependency("a", "b");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Adding to an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void AddToEmpty()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
        }

        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void ReplaceEmptyDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependents("a", new HashSet<string>());
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void ReplaceEmptyDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependees("a", new HashSet<string>());
            Assert.AreEqual(0, t.Size);
        }


        /**************************** SIMPLE NON-EMPTY TESTS ****************************/

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void NonEmptySize()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            Assert.AreEqual(2, t.Size);
        }

        /// <summary>
        ///Slight variant
        ///</summary>
        [TestMethod()]
        public void AddDuplicate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "b");
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        ///Nonempty graph should contain something
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest3()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            Assert.IsFalse(t.HasDependees("a"));
            Assert.IsTrue(t.HasDependees("b"));
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsTrue(t.HasDependees("c"));
        }

        /// <summary>
        ///Nonempty graph should contain something
        ///</summary>
        [TestMethod()]
        public void ComplexGraphCount()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            HashSet<String> aDents = new HashSet<String>(t.GetDependents("a"));
            HashSet<String> bDents = new HashSet<String>(t.GetDependents("b"));
            HashSet<String> cDents = new HashSet<String>(t.GetDependents("c"));
            HashSet<String> dDents = new HashSet<String>(t.GetDependents("d"));
            HashSet<String> eDents = new HashSet<String>(t.GetDependents("e"));
            HashSet<String> aDees = new HashSet<String>(t.GetDependees("a"));
            HashSet<String> bDees = new HashSet<String>(t.GetDependees("b"));
            HashSet<String> cDees = new HashSet<String>(t.GetDependees("c"));
            HashSet<String> dDees = new HashSet<String>(t.GetDependees("d"));
            HashSet<String> eDees = new HashSet<String>(t.GetDependees("e"));
            Assert.IsTrue(aDents.Count == 2 && aDents.Contains("b") && aDents.Contains("c"));
            Assert.IsTrue(bDents.Count == 0);
            Assert.IsTrue(cDents.Count == 0);
            Assert.IsTrue(dDents.Count == 1 && dDents.Contains("c"));
            Assert.IsTrue(eDents.Count == 0);
            Assert.IsTrue(aDees.Count == 0);
            Assert.IsTrue(bDees.Count == 1 && bDees.Contains("a"));
            Assert.IsTrue(cDees.Count == 2 && cDees.Contains("a") && cDees.Contains("d"));
            Assert.IsTrue(dDees.Count == 0);
            Assert.IsTrue(dDees.Count == 0);
        }

        /// <summary>
        ///Nonempty graph should contain something
        ///</summary>
        [TestMethod()]
        public void ComplexGraphIndexer()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            Assert.AreEqual(0, t["a"]);
            Assert.AreEqual(1, t["b"]);
            Assert.AreEqual(2, t["c"]);
            Assert.AreEqual(0, t["d"]);
            Assert.AreEqual(0, t["e"]);
        }

        /// <summary>
        ///Removing from a DG 
        ///</summary>
        [TestMethod()]
        public void Remove()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.RemoveDependency("a", "b");
            Assert.AreEqual(2, t.Size);
        }

        /// <summary>
        ///Replace on a DG
        ///</summary>
        [TestMethod()]
        public void ReplaceDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.ReplaceDependents("a", new HashSet<string>() { "x", "y", "z" });
            HashSet<String> aPends = new HashSet<string>(t.GetDependents("a"));
            Assert.IsTrue(aPends.SetEquals(new HashSet<string>() { "x", "y", "z" }));
        }

        /// <summary>
        ///Replace on a DG
        ///</summary>
        [TestMethod()]
        public void ReplaceDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.ReplaceDependees("c", new HashSet<string>() { "x", "y", "z" });
            HashSet<String> cDees = new HashSet<string>(t.GetDependees("c"));
            Assert.IsTrue(cDees.SetEquals(new HashSet<string>() { "x", "y", "z" }));
        }

        // ************************** STRESS TESTS ******************************** //
        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest1()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for(int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for(int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for(int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }



        // ********************************** ANOTHER STESS TEST ******************** //
        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void StressTest8()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for(int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for(int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependents
            for(int i = 0; i < SIZE; i += 4)
            {
                HashSet<string> newDents = new HashSet<String>();
                for(int j = 0; j < SIZE; j += 7)
                {
                    newDents.Add(letters[j]);
                }
                t.ReplaceDependents(letters[i], newDents);

                foreach(string s in dents[i])
                {
                    dees[s[0] - 'a'].Remove(letters[i]);
                }

                foreach(string s in newDents)
                {
                    dees[s[0] - 'a'].Add(letters[i]);
                }

                dents[i] = newDents;
            }

            // Make sure everything is right
            for(int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        // ********************************** A THIRD STESS TEST ******************** //
        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void StressTest15()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for(int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for(int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependees
            for(int i = 0; i < SIZE; i += 4)
            {
                HashSet<string> newDees = new HashSet<String>();
                for(int j = 0; j < SIZE; j += 7)
                {
                    newDees.Add(letters[j]);
                }
                t.ReplaceDependees(letters[i], newDees);

                foreach(string s in dees[i])
                {
                    dents[s[0] - 'a'].Remove(letters[i]);
                }

                foreach(string s in newDees)
                {
                    dents[s[0] - 'a'].Add(letters[i]);
                }

                dees[i] = newDees;
            }

            // Make sure everything is right
            for(int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }

        }

        // ************************** Additional Tests By Aaron Bellis ************************* //

        /// <summary>
        /// check the size of graph after adding several dependencies
        ///</summary>
        [TestMethod()]
        public void GraphSizeAdd()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");
            t.AddDependency("c", "d");

            // size should be the 3
            Assert.AreEqual(3, t.Size);
        }

        /// <summary>
        /// Check the size of the graph after adding several dependencies including duplicates
        ///</summary>
        [TestMethod()]
        public void GraphSizeAddDuplicate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");
            t.AddDependency("c", "d");

            // add duplicates
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");

            // size should be the same
            Assert.AreEqual(3, t.Size);
        }

        /// <summary>
        /// Check the size of the graph after adding several dependencies then removing a few
        ///</summary>
        [TestMethod()]
        public void GraphSizeAddRemove()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");
            t.AddDependency("c", "d");
            t.AddDependency("d", "e");
            t.AddDependency("e", "f");
            t.AddDependency("f", "g");


            // remove a couple dependencies
            t.RemoveDependency("a", "b");
            t.RemoveDependency("b", "c");

            // size should be the same
            Assert.AreEqual(4, t.Size);
        }

        /// <summary>
        /// Check the size of the graph after adding several dependencies including duplicates
        /// then removing a few
        ///</summary>
        [TestMethod()]
        public void GraphSizeAddRemoveDuplicates()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");
            t.AddDependency("c", "d");
            t.AddDependency("d", "e");
            t.AddDependency("e", "f");
            t.AddDependency("f", "g");

            // add some duplicates
            t.AddDependency("c", "d");
            t.AddDependency("d", "e");

            // remove a couple dependencies
            t.RemoveDependency("a", "b");
            t.RemoveDependency("b", "c");

            // size should be the same
            Assert.AreEqual(4, t.Size);
        }

        /// <summary>
        /// indexer for Dependency graph checks the size of the indexed dependees
        /// check size of several sets of dependees
        ///</summary>
        [TestMethod()]
        public void GraphIndexer()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.AddDependency("b", "a");
            t.AddDependency("b", "c");
            t.AddDependency("e", "a");

            // add some duplicates
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");

            // remove a couple dependencies
            t.RemoveDependency("a", "b");
            t.RemoveDependency("b", "c");

            // dependees of a = {"b", "e"}
            // dependees of b = {}
            // dependees of c = {"a"}
            // dependees of d = {"a"}
            // dependees of e = {}

            // check each member in indexer
            Assert.AreEqual(2, t["a"]);
            Assert.AreEqual(0, t["b"]);
            Assert.AreEqual(1, t["c"]);
            Assert.AreEqual(1, t["d"]);
            Assert.AreEqual(0, t["e"]);
        }

        /// <summary>
        /// check HasDependents for a small graph
        ///</summary>
        [TestMethod()]
        public void HasDependentsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.AddDependency("b", "a");
            t.AddDependency("b", "c");
            t.AddDependency("e", "a");

            // add some duplicates
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");

            // remove a couple dependencies
            t.RemoveDependency("a", "b");
            t.RemoveDependency("b", "c");

            // dependents of a = {"c", "d"}
            // dependents of b = {"a"}
            // dependents of c = {}
            // dependents of d = {}
            // dependents of e = {"a"}

            // dependees of a = {"b", "e"}
            // dependees of b = {}
            // dependees of c = {"a"}
            // dependees of d = {"a"}
            // dependees of e = {}

            // check dependents of each graph member
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsTrue(t.HasDependents("b"));
            Assert.IsFalse(t.HasDependents("c"));
            Assert.IsFalse(t.HasDependents("d"));
            Assert.IsTrue(t.HasDependents("e"));
        }

        /// <summary>
        /// check HasDependees for a small graph
        ///</summary>
        [TestMethod()]
        public void HasDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.AddDependency("b", "a");
            t.AddDependency("b", "c");
            t.AddDependency("e", "a");

            // add some duplicates
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");

            // remove a couple dependencies
            t.RemoveDependency("a", "b");
            t.RemoveDependency("b", "c");

            // dependents of a = {"c", "d"}
            // dependents of b = {"a"}
            // dependents of c = {}
            // dependents of d = {}
            // dependents of e = {"a"}

            // dependees of a = {"b", "e"}
            // dependees of b = {}
            // dependees of c = {"a"}
            // dependees of d = {"a"}
            // dependees of e = {}

            // check dependees of each graph member
            Assert.IsTrue(t.HasDependees("a"));
            Assert.IsFalse(t.HasDependees("b"));
            Assert.IsTrue(t.HasDependees("c"));
            Assert.IsTrue(t.HasDependees("d"));
            Assert.IsFalse(t.HasDependees("e"));
        }

        /// <summary>
        /// check GetDependents for a small graph
        ///</summary>
        [TestMethod()]
        public void GetDependentsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.AddDependency("b", "a");
            t.AddDependency("b", "c");
            t.AddDependency("e", "a");

            // add some duplicates
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");

            // remove a couple dependencies
            t.RemoveDependency("a", "b");
            t.RemoveDependency("b", "c");

            // dependents of a = {"c", "d"}
            // dependents of b = {"a"}
            // dependents of c = {}
            // dependents of d = {}
            // dependents of e = {"a"}

            List<string> aDependents = new List<string> { "c", "d" };
            List<string> bDependents = new List<string> { "a" };
            List<string> cDependents = new List<string> { };
            List<string> dDependents = new List<string> { };
            List<string> eDependents = new List<string> { "a" };


            // dependees of a = {"b", "e"}
            // dependees of b = {}
            // dependees of c = {"a"}
            // dependees of d = {"a"}
            // dependees of e = {}


            // check dependees of each graph member
            Assert.IsTrue(StringListEquals(aDependents, new List<string>(t.GetDependents("a"))));
            Assert.IsTrue(StringListEquals(bDependents, new List<string>(t.GetDependents("b"))));
            Assert.IsTrue(StringListEquals(cDependents, new List<string>(t.GetDependents("c"))));
            Assert.IsTrue(StringListEquals(dDependents, new List<string>(t.GetDependents("d"))));
            Assert.IsTrue(StringListEquals(eDependents, new List<string>(t.GetDependents("e"))));
        }

        /// <summary>
        /// check GetDependents for a small graph
        ///</summary>
        [TestMethod()]
        public void GetDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.AddDependency("b", "a");
            t.AddDependency("b", "c");
            t.AddDependency("e", "a");

            // add some duplicates
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");

            // remove a couple dependencies
            t.RemoveDependency("a", "b");
            t.RemoveDependency("b", "c");

            // dependents of a = {"c", "d"}
            // dependents of b = {"a"}
            // dependents of c = {}
            // dependents of d = {}
            // dependents of e = {"a"}

            // dependees of a = {"b", "e"}
            // dependees of b = {}
            // dependees of c = {"a"}
            // dependees of d = {"a"}
            // dependees of e = {}

            List<string> aDependees = new List<string> { "b", "e" };
            List<string> bDependees = new List<string> { };
            List<string> cDependees = new List<string> { "a" };
            List<string> dDependees = new List<string> { "a" };
            List<string> eDependees = new List<string> { };


            // check dependees of each graph member
            Assert.IsTrue(StringListEquals(aDependees, new List<string>(t.GetDependees("a"))));
            Assert.IsTrue(StringListEquals(bDependees, new List<string>(t.GetDependees("b"))));
            Assert.IsTrue(StringListEquals(cDependees, new List<string>(t.GetDependees("c"))));
            Assert.IsTrue(StringListEquals(dDependees, new List<string>(t.GetDependees("d"))));
            Assert.IsTrue(StringListEquals(eDependees, new List<string>(t.GetDependees("e"))));
        }

        

        /// <summary>
        /// try removing from empty graph
        ///</summary>
        [TestMethod()]
        public void RemoveEmptyGraph()
        {
            DependencyGraph t = new DependencyGraph();
            // removing from an empty graph shouldn't cause any error
            t.RemoveDependency("a", "b");

            // state should be the same
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        /// try removing from empty graph
        ///</summary>
        [TestMethod()]
        public void RemoveTooMany()
        {
            DependencyGraph t = new DependencyGraph();
            // add then remove dependencies
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.RemoveDependency("a", "b");
            t.RemoveDependency("a", "c");

            // removing from an empty graph shouldn't cause any error
            t.RemoveDependency("a", "b");
            t.RemoveDependency("a", "c");
            t.RemoveDependency("b", "r");

            // check graph state
            Assert.AreEqual(0, t.Size);
            Assert.IsFalse(t.HasDependents("a"));
            Assert.IsFalse(t.HasDependents("b"));
            Assert.IsFalse(t.HasDependees("b"));
            Assert.IsFalse(t.HasDependees("c"));
            Assert.IsFalse(t.HasDependees("r"));
        }

        /// <summary>
        /// Make sure removing dependency not in graph changes nothing
        ///</summary>
        [TestMethod()]
        public void RemoveDependencyDoesntExistSize()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            // remove a dependency that doesn't exist. 
            t.RemoveDependency("a", "d");
            // check state
            // there should be two dependencies in graph
            Assert.AreEqual(2, t.Size);
        }

        /// <summary>
        /// Make sure removing dependency not in graph changes nothing
        ///</summary>
        [TestMethod()]
        public void RemoveDependencyDoesntExistDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            // remove a dependency that doesn't exist. 
            t.RemoveDependency("a", "d");
            // check state

            // a should have two dependents
            List<string> dependents = new List<string>(t.GetDependents("a"));
            Assert.AreEqual(2, dependents.Count);


        }

        /// <summary>
        /// Make sure removing dependency not in graph changes nothing
        ///</summary>
        [TestMethod()]
        public void RemoveDependencyDoesntExistDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            // remove a dependency that doesn't exist. 
            t.RemoveDependency("a", "d");
            // check state

            // "b" and "c" should have one dependee called "a"
            List<string> dependees = new List<string>(t.GetDependees("b"));
            Assert.AreEqual(1, dependees.Count);
            Assert.AreEqual("a", dependees[0]);
            dependees = new List<string>(t.GetDependees("c"));
            Assert.AreEqual(1, dependees.Count);
            Assert.AreEqual("a", dependees[0]);

        }

        //****************************methods to facilitate testing *************************************// 

        /// <summary>
        /// This determines if two List<string> objects are equal. They are equal if they are the same size 
        /// and contain the same members. The order does not matter.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool StringListEquals(List<string> a, List<string> b)
        {
            // check the size
            if(a.Count != b.Count)
            {
                return false;
            }

            // check the elements of a against b
            // since they are the same size should check everything in b
            foreach(string s in a)
            {
                if(!b.Contains(s))
                {
                    return false;
                }
            }

            return true;

        }
    }
}
