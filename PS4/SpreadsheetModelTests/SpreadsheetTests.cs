// Written by Aaron Bellis u0981638 for CS 3500 2017 Fall Semester.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;
using SpreadsheetUtilities;

namespace SpreadsheetModelTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Creates an empty spreadsheet. If spreasheet is empty
        /// the number of all non empty cells should be 0.
        /// </summary>
        [TestMethod]
        public void ConstructorTestSpreadsheet()
        {
            // create empty spreadsheet. Should throw no exception
            Spreadsheet sheet = new Spreadsheet();

            // create a list from the IEnumerable to check size
            List<string> contents = new List<string>(sheet.GetNamesOfAllNonemptyCells());

            // check that there were no non-empty cells
            Assert.AreEqual(0, contents.Count);
        }

        /// <summary>
        /// Creates an empty spreadsheet and stores it as an AbstractSpreadsheet. If 
        /// spreasheet is empty the number of all non empty cells should be 0.
        /// </summary>
        [TestMethod]
        public void ConstructorTestAbstractSpreadsheet()
        {
            // create empty spreadsheet. Should throw no exception
            // per instructions this should work for another developer
            AbstractSpreadsheet sheet = new Spreadsheet();

            // create a list from the IEnumerable to check size
            List<string> contents = new List<string>(sheet.GetNamesOfAllNonemptyCells());

            // check that there were no non-empty cells
            Assert.AreEqual(0, contents.Count);
        }

        /// <summary>
        /// Creates an empty spreadsheet all valid cell names should return an empty string ""
        /// </summary>
        [TestMethod]
        public void GetCellContentsEmptySheet()
        {
            // create empty spreadsheet.
            Spreadsheet sheet = new Spreadsheet();

            string expectedContents = "";

            // check that all cells are returning an empty string
            string actualContents = (string)sheet.GetCellContents("x");
            Assert.AreEqual(expectedContents, actualContents);

            actualContents = (string)sheet.GetCellContents("_");
            Assert.AreEqual(expectedContents, actualContents);

            actualContents = (string)sheet.GetCellContents("x2");
            Assert.AreEqual(expectedContents, actualContents);

            actualContents = (string)sheet.GetCellContents("y_15");
            Assert.AreEqual(expectedContents, actualContents);

            actualContents = (string)sheet.GetCellContents("___");
            Assert.AreEqual(expectedContents, actualContents);

            actualContents = (string)sheet.GetCellContents("xXyYzZ_2_4_8_16_BleepBloopRobot");
            Assert.AreEqual(expectedContents, actualContents);
        }

        /// <summary>
        /// Check value non empty cells
        /// </summary>
        [TestMethod]
        public void GetCellContentsString()
        {
            Spreadsheet sheet = new Spreadsheet();

            // set contents of cells
            // couple strings
            string expectedContents = "Hello World";
            sheet.SetCellContents("x", "Hello World");
            string actualContents = (string)sheet.GetCellContents("x");
            Assert.AreEqual(expectedContents, actualContents);

            expectedContents = "Don't Bite Me";
            sheet.SetCellContents("_", "Don't Bite Me");
            actualContents = (string)sheet.GetCellContents("_");
            Assert.AreEqual(expectedContents, actualContents);
        }

        /// <summary>
        /// Check value non empty cells
        /// </summary>
        [TestMethod]
        public void GetCellContentsDouble()
        {
            Spreadsheet sheet = new Spreadsheet();

            // set contents of cells
            // couple doubles
            double expectedContents = 3.1415;
            sheet.SetCellContents("x2", 3.1415);
            double actualContents = (double)sheet.GetCellContents("x2");
            Assert.AreEqual(expectedContents, actualContents);

            expectedContents = 2;
            sheet.SetCellContents("y_15", 2);
            actualContents = (double)sheet.GetCellContents("y_15");
            Assert.AreEqual(expectedContents, actualContents);

        }

        /// <summary>
        /// Check value non empty cells
        /// </summary>
        [TestMethod]
        public void GetCellContentsFormula()
        {
            Spreadsheet sheet = new Spreadsheet();

            // set contents of cells
            // some formulas
            Formula expectedContents = new Formula("2.5 + 2.5");
            sheet.SetCellContents("___", new Formula("2.5 + 2.5"));
            Formula actualContents = (Formula)sheet.GetCellContents("___");
            Assert.AreEqual(expectedContents, actualContents);

            expectedContents = new Formula("4 - 2");
            sheet.SetCellContents("xXyYzZ_2_4_8_16_BleepBloopRobot", new Formula("4 - 2"));
            actualContents = (Formula)sheet.GetCellContents("xXyYzZ_2_4_8_16_BleepBloopRobot");
            Assert.AreEqual(expectedContents, actualContents);

            expectedContents = new Formula("4 - c16");
            sheet.SetCellContents("c1", new Formula("4 - c16"));
            actualContents = (Formula)sheet.GetCellContents("c1");
            Assert.AreEqual(expectedContents, actualContents);
        }

        /// <summary>
        /// GetCellContents(string name) should throw InvalidNameException if name is 
        /// null or invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsNull()
        {
            Spreadsheet sheet = new Spreadsheet();

            Formula actualContents = (Formula)sheet.GetCellContents(null);
        }

        /// <summary>
        /// GetCellContents(string name) should throw InvalidNameException if name is 
        /// null or invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsInvalidName()
        {
            Spreadsheet sheet = new Spreadsheet();

            Formula actualContents = (Formula)sheet.GetCellContents("2x");
        }

        /// <summary>
        /// GetCellContents(string name) should throw InvalidNameException if name is 
        /// null or invalid
        /// </summary>
        [TestMethod]
        public void GetCellContentsOverwriteCells()
        {
            Spreadsheet sheet = new Spreadsheet();
            
            // set some cell contents
            sheet.SetCellContents("x", new Formula("1/2 + 1/2"));
            sheet.SetCellContents("y", 3.5);
            sheet.SetCellContents("z", "String Content");

            // check cell values
            string actualContentsString = (string)sheet.GetCellContents("z");
            Formula actualContentsFormula = (Formula)sheet.GetCellContents("x");
            double actualContentsDouble = (double)sheet.GetCellContents("y");

            Assert.AreEqual("String Content", actualContentsString);
            Assert.IsTrue(new Formula("1/2 + 1/2") == actualContentsFormula);
            Assert.AreEqual(3.5, actualContentsDouble);


            // overwrite some cells
            sheet.SetCellContents("x", "Bleep Bloop Warble");
            sheet.SetCellContents("y", new Formula("2+3"));
            sheet.SetCellContents("z", 4.5);

            // check the overwritten values
            actualContentsString = (string)sheet.GetCellContents("x");
            actualContentsFormula = (Formula)sheet.GetCellContents("y");
            actualContentsDouble = (double)sheet.GetCellContents("z");

            // check that overwritten values have changed
            Assert.AreEqual("Bleep Bloop Warble", actualContentsString);
            Assert.IsTrue(new Formula("2+3") == actualContentsFormula);
            Assert.AreEqual(4.5, actualContentsDouble);
        }

        /// <summary>
        /// Put the GetCellContents() tests together to see if they work outside of isolation
        /// </summary>
        [TestMethod]
        public void GetCellContentsAllTogether()
        {
            Spreadsheet sheet = new Spreadsheet();

            // some strings
            string expectedContentsString = "Hello World";
            sheet.SetCellContents("x", "Hello World");
            string actualContentsString = (string)sheet.GetCellContents("x");
            Assert.AreEqual(expectedContentsString, actualContentsString);

            expectedContentsString = "Don't Bite Me";
            sheet.SetCellContents("_", "Don't Bite Me");
            actualContentsString = (string)sheet.GetCellContents("_");
            Assert.AreEqual(expectedContentsString, actualContentsString);

            // some doubles
            double expectedContentsDouble = 3.1415;
            sheet.SetCellContents("x2", 3.1415);
            double actualContentsDouble = (double)sheet.GetCellContents("x2");
            Assert.AreEqual(expectedContentsDouble, actualContentsDouble);

            expectedContentsDouble = 2;
            sheet.SetCellContents("y_15", 2);
            actualContentsDouble = (double)sheet.GetCellContents("y_15");
            Assert.AreEqual(expectedContentsDouble, actualContentsDouble);

            // some formulas
            Formula expectedContentsFormula = new Formula("2.5 + 2.5");
            sheet.SetCellContents("___", new Formula("2.5 + 2.5"));
            Formula actualContentsFormula = (Formula)sheet.GetCellContents("___");
            Assert.IsTrue(expectedContentsFormula == actualContentsFormula);

            expectedContentsFormula = new Formula("4 - 2");
            sheet.SetCellContents("xXyYzZ_2_4_8_16_BleepBloopRobot", new Formula("4 - 2"));
            actualContentsFormula = (Formula)sheet.GetCellContents("xXyYzZ_2_4_8_16_BleepBloopRobot");
            Assert.IsTrue(expectedContentsFormula == actualContentsFormula);

            expectedContentsFormula = new Formula("4 - c16");
            sheet.SetCellContents("c1", new Formula("4 - c16"));
            actualContentsFormula = (Formula)sheet.GetCellContents("c1");
            Assert.IsTrue(expectedContentsFormula == actualContentsFormula);

            // overwrite some cells
            sheet.SetCellContents("xXyYzZ_2_4_8_16_BleepBloopRobot", "Bleep Bloop Warble");
            sheet.SetCellContents("y_15", new Formula("2+3"));
            sheet.SetCellContents("x", 3.5);

            // check the overwritten values
            actualContentsString = (string)sheet.GetCellContents("xXyYzZ_2_4_8_16_BleepBloopRobot");
            actualContentsFormula = (Formula)sheet.GetCellContents("y_15");
            actualContentsDouble = (double)sheet.GetCellContents("x");

            // check that overwritten values have changed
            Assert.AreEqual("Bleep Bloop Warble", actualContentsString);
            Assert.IsTrue(new Formula("2+3") == actualContentsFormula);
            Assert.AreEqual(3.5, actualContentsDouble);

            // check that we can still get empty cells
            Assert.AreEqual("", (string)sheet.GetCellContents("_x23"));
            Assert.AreEqual("", (string)sheet.GetCellContents("_xmen"));
        }

        /// <summary>
        /// Creates an empty spreadsheet. If spreasheet is empty the number of all 
        /// non empty cells should be 0.
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsEmptySheet()
        {
            // create empty spreadsheet. Should throw no exception
            // per instructions this should work for another developer
            Spreadsheet sheet = new Spreadsheet();

            // create a list from the IEnumerable to check size
            List<string> contents = new List<string>(sheet.GetNamesOfAllNonemptyCells());

            // check that there were no non-empty cells
            Assert.AreEqual(0, contents.Count);
        }


        /// <summary>
        /// check the size of contents of all non-empty cells after adding a bunch
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsAddAFew()
        {
            // create empty spreadsheet. Should throw no exception
            // per instructions this should work for another developer
            Spreadsheet sheet = new Spreadsheet();

            // get a set to check against
            HashSet<string> nonEmpty = new HashSet<string>();

            int size = 100;
            for (int i = 0; i < size; i++)
            {
                string cellName = "a" + i;
                double contents = 0.5 + i;

                nonEmpty.Add(cellName);
                sheet.SetCellContents(cellName, contents);
            }
            
            // create a list from the IEnumerable to check size
            List<string> nonEmptyActual = new List<string>(sheet.GetNamesOfAllNonemptyCells());

            //check size
            Assert.AreEqual(size, nonEmptyActual.Count);

            // check contents
            foreach (string cell in nonEmptyActual)
            {
                Assert.IsTrue(nonEmpty.Contains(cell));
            }
        }

        /// <summary>
        /// check the size of contents of all non-empty cells after adding a bunch
        /// then replacing some
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsReplaceCells()
        {
            // create empty spreadsheet. Should throw no exception
            // per instructions this should work for another developer
            Spreadsheet sheet = new Spreadsheet();

            // get a set to check against
            HashSet<string> nonEmpty = new HashSet<string>();

            // add the cells
            int size = 100;
            for(int i = 0; i < size; i++)
            {
                string cellName = "a" + i;
                double contents = 0.5 + i;

                nonEmpty.Add(cellName);
                sheet.SetCellContents(cellName, contents);
            }

            // replace some cells
            for(int i = 0; i < size; i += 2)
            {
                string cellName = "a" + i;
                double contents = 0.3 + i;

                sheet.SetCellContents(cellName, contents);
            }

            // create a list from the IEnumerable to check size
            List<string> nonEmptyActual = new List<string>(sheet.GetNamesOfAllNonemptyCells());

            //check size
            Assert.AreEqual(size, nonEmptyActual.Count);

            // check contents
            foreach(string cell in nonEmptyActual)
            {
                Assert.IsTrue(nonEmpty.Contains(cell));
            }
        }

        /// <summary>
        /// check the size of contents of all non-empty cells after adding a bunch
        /// then removing some
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsRemoveCells()
        {
            // create empty spreadsheet. Should throw no exception
            // per instructions this should work for another developer
            Spreadsheet sheet = new Spreadsheet();

            // get a set to check against
            HashSet<string> nonEmpty = new HashSet<string>();

            // add the cells
            int size = 100;
            for(int i = 0; i < size; i++)
            {
                string cellName = "a" + i;
                double contents = 0.5 + i;

                nonEmpty.Add(cellName);
                sheet.SetCellContents(cellName, contents);
            }

            // remove some cells
            for(int i = 0; i < size; i += 2)
            {
                string cellName = "a" + i;
                string contents = "";

                nonEmpty.Remove(cellName);
                sheet.SetCellContents(cellName, contents);
            }

            // create a list from the IEnumerable to check size
            List<string> nonEmptyActual = new List<string>(sheet.GetNamesOfAllNonemptyCells());

            //check size
            Assert.AreEqual(nonEmpty.Count, nonEmptyActual.Count);

            // check contents
            foreach(string cell in nonEmptyActual)
            {
                Assert.IsTrue(nonEmpty.Contains(cell));
            }
        }

        /// <summary>
        /// check the size of contents of all non-empty cells after adding a bunch
        /// then removing some
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsRemoveAndReplaceCells()
        {
            
            Spreadsheet sheet = new Spreadsheet();

            // get a set to check against
            HashSet<string> nonEmpty = new HashSet<string>();

            // add the cells
            int size = 100;
            for(int i = 0; i < size; i++)
            {
                string cellName = "a" + i;
                double contents = 0.5 + i;

                nonEmpty.Add(cellName);
                sheet.SetCellContents(cellName, contents);
            }

            // remove some cells
            for(int i = 0; i < size; i += 2)
            {
                string cellName = "a" + i;
                string contents = "";

                nonEmpty.Remove(cellName);
                sheet.SetCellContents(cellName, contents);
            }

            // replace some cells
            for(int i = 0; i < size; i += 3)
            {
                string cellName = "a" + i;
                double contents = 0.3 + i;

                nonEmpty.Add(cellName);
                sheet.SetCellContents(cellName, contents);
            }

            // create a list from the IEnumerable to check size
            List<string> nonEmptyActual = new List<string>(sheet.GetNamesOfAllNonemptyCells());

            //check size
            Assert.AreEqual(nonEmpty.Count, nonEmptyActual.Count);

            // check contents
            foreach(string cell in nonEmptyActual)
            {
                Assert.IsTrue(nonEmpty.Contains(cell));
            }
        }
        
        /// <summary>
        /// Check the proper exception is thrown when null name is passed to method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsDoubleNullName()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetCellContents(null, 2.5);
        }

        /// <summary>
        /// Check the proper exception is thrown when invalid name is passed to method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsDoubleInvalidName()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetCellContents("3x", 2.5);
        }

        /// <summary>
        /// Check the proper contents are in the cell
        /// </summary>
        [TestMethod]
        public void SetCellContentsDoubleContents()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 2.5);

            Assert.AreEqual(2.5, (double)sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Check proper set is returned, documentation example
        /// if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned
        /// </summary>
        [TestMethod]
        public void SetCellContentsDoubleSimpleDependencies()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B1", new Formula("A1*2"));
            sheet.SetCellContents("C1", new Formula("B1+A1"));

            // get the set
            ISet<string> dependencySet = sheet.SetCellContents("A1", 2.5);

            // check its size
            Assert.AreEqual(3, dependencySet.Count);

            // check the list
            Assert.IsTrue(dependencySet.Contains("A1"));
            Assert.IsTrue(dependencySet.Contains("B1"));
            Assert.IsTrue(dependencySet.Contains("C1"));
        }



    }
}
