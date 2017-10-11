﻿// Written by Aaron Bellis u0981638 for CS 3500 2017 Fall Semester.

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
            string actualContents = (string)sheet.GetCellContents("x3");
            Assert.AreEqual(expectedContents, actualContents);


            actualContents = (string)sheet.GetCellContents("Skljdi1");
            Assert.AreEqual(expectedContents, actualContents);


            actualContents = (string)sheet.GetCellContents("x2");
            Assert.AreEqual(expectedContents, actualContents);


            actualContents = (string)sheet.GetCellContents("ys67");
            Assert.AreEqual(expectedContents, actualContents);


            actualContents = (string)sheet.GetCellContents("n42");
            Assert.AreEqual(expectedContents, actualContents);


            actualContents = (string)sheet.GetCellContents("xXyYzZBleepBloopRobot24816");
            Assert.AreEqual(expectedContents, actualContents);

        }

        /// <summary>
        /// Creates an empty spreadsheet all valid cell names should return an empty string ""
        /// </summary>
        [TestMethod]
        public void GetCellValueEmptySheet()
        {
            // create empty spreadsheet.
            Spreadsheet sheet = new Spreadsheet();

            string expectedValue = "";

            // check that all cells are returning an empty string

            string actualValue = (string)sheet.GetCellContents("x3");
            Assert.AreEqual(expectedValue, actualValue);


            actualValue = (string)sheet.GetCellContents("Skljdi1");
            Assert.AreEqual(expectedValue, actualValue);


            actualValue = (string)sheet.GetCellContents("x2");
            Assert.AreEqual(expectedValue, actualValue);


            actualValue = (string)sheet.GetCellContents("ys67");
            Assert.AreEqual(expectedValue, actualValue);


            actualValue = (string)sheet.GetCellContents("n42");
            Assert.AreEqual(expectedValue, actualValue);


            actualValue = (string)sheet.GetCellContents("x3");
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Check value non empty cells
        /// </summary>
        [TestMethod]
        public void GetCellContentsString()
        {
            Spreadsheet sheet = new Spreadsheet();

            // set the contents of x2
            string expectedContents = "Hello World";
            sheet.SetContentsOfCell("x2", "Hello World");

            // get the contents of x2
            string actualContents = (string)sheet.GetCellContents("x2");
            Assert.AreEqual(expectedContents, actualContents);


            // set the contents of blink183
            expectedContents = "One Blink More";
            sheet.SetContentsOfCell("blink183", "One Blink More");

            // get the contents of blink183
            actualContents = (string)sheet.GetCellContents("blink183");
            Assert.AreEqual(expectedContents, actualContents);

        }

        /// <summary>
        /// Check value non empty cells
        /// </summary>
        [TestMethod]
        public void GetCellValueString()
        {
            Spreadsheet sheet = new Spreadsheet();

            // set the contents of x2
            string expectedContents = "Hello World";
            sheet.SetContentsOfCell("x2", "Hello World");

            // get the value of x2
            string actualValue = (string)sheet.GetCellValue("x2");
            Assert.AreEqual(expectedContents, actualValue);

            // set the contents of blink183
            expectedContents = "One Blink More";
            sheet.SetContentsOfCell("blink183", "One Blink More");

            //Get the value of Blink183
            actualValue = (string)sheet.GetCellValue("blink183");
            Assert.AreEqual(expectedContents, actualValue);
        }

        /// <summary>
        /// Check value non empty cells
        /// </summary>
        [TestMethod]
        public void GetCellContentsDouble()
        {
            Spreadsheet sheet = new Spreadsheet();


            // set contents
            double expectedContents = 3.1415;
            sheet.SetContentsOfCell("x2", "3.1415");
            // get contents
            double actualContents = (double)sheet.GetCellContents("x2");
            Assert.AreEqual(expectedContents, actualContents);

            // set contents
            expectedContents = 2;
            sheet.SetContentsOfCell("y15", "2");
            // get contents
            actualContents = (double)sheet.GetCellContents("y15");
            Assert.AreEqual(expectedContents, actualContents);

        }

        /// <summary>
        /// Check value non empty cells
        /// </summary>
        [TestMethod]
        public void GetCellValueDouble()
        {
            Spreadsheet sheet = new Spreadsheet();

            // couple doubles
            // set contents
            double expectedValue = 3.1415;
            sheet.SetContentsOfCell("x2", "3.1415");
            // get value
            double actualValue = (double)sheet.GetCellValue("x2");
            Assert.AreEqual(expectedValue, actualValue);

            // set contents
            expectedValue = 2;
            sheet.SetContentsOfCell("y15", "2");
            // get value
            actualValue = (double)sheet.GetCellValue("y15");
            Assert.AreEqual(expectedValue, actualValue);

        }

        /// <summary>
        /// Check value non empty cells
        /// </summary>
        [TestMethod]
        public void GetCellContentsFormula()
        {
            Spreadsheet sheet = new Spreadsheet();

            // some formulas
            // set contents of cell
            Formula expectedContents = new Formula("2.5 + 2.5");
            sheet.SetContentsOfCell("x24", "=2.5 + 2.5");
            // get cell contents
            Formula actualContents = (Formula)sheet.GetCellContents("x24");
            Assert.AreEqual(expectedContents, actualContents);
            

            // set contents of cell
            expectedContents = new Formula("4 - 2");
            sheet.SetContentsOfCell("xXyYzZ24816", "=4 - 2");
            // get cell contents
            actualContents = (Formula)sheet.GetCellContents("xXyYzZ24816");
            Assert.AreEqual(expectedContents, actualContents);
           

            // set contents of cell
            expectedContents = new Formula("4 - c16");
            sheet.SetContentsOfCell("c1", "=4 - c16");
            // get cell contents
            actualContents = (Formula)sheet.GetCellContents("c1");
            Assert.AreEqual(expectedContents, actualContents);
            

            // set contents of cell
            expectedContents = new Formula("2");
            sheet.SetContentsOfCell("c16", "=2");
            // get cell contents
            actualContents = (Formula)sheet.GetCellContents("c16");
            Assert.AreEqual(expectedContents, actualContents);
            

            
        }

        /// <summary>
        /// Check value non empty cells
        /// </summary>
        [TestMethod]
        public void GetCellValueFormula()
        {
            Spreadsheet sheet = new Spreadsheet();

            // some formulas
            // set contents of cell
            Formula expectedContents = new Formula("2.5 + 2.5");
            sheet.SetContentsOfCell("x24", "=2.5 + 2.5");
           
            // get cell value
            double expectedValue = 5;
            double actualValue = (double)sheet.GetCellValue("x24");
            Assert.AreEqual(expectedValue, actualValue);

            // set contents of cell
            expectedContents = new Formula("4 - 2");
            sheet.SetContentsOfCell("xXyYzZ24816", "=4 - 2");
            
            // get cell value
            expectedValue = 2;
            actualValue = (double)sheet.GetCellValue("xXyYzZ24816");
            Assert.AreEqual(expectedValue, actualValue);

            // set contents of cell
            expectedContents = new Formula("4 - c16");
            sheet.SetContentsOfCell("c1", "=4 - c16");
            
            // get cell value
            object actualValueObject = sheet.GetCellValue("c1");
            Assert.IsTrue(actualValueObject.GetType() == typeof(FormulaError));

            // set contents of cell
            expectedContents = new Formula("2");
            sheet.SetContentsOfCell("c16", "=2");
            
            // get cell value
            expectedValue = 2;
            actualValue = (double)sheet.GetCellValue("c16");
            Assert.AreEqual(expectedValue, actualValue);

            // value of c1 should have been updated

            // get cell value
            expectedValue = 2;
            actualValue = (double)sheet.GetCellValue("c1");
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}



//        /// <summary>
//        /// GetCellContents(string name) should throw InvalidNameException if name is 
//        /// null or invalid
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void GetCellContentsNull()
//        {
//            Spreadsheet sheet = new Spreadsheet();

//            Formula actualContents = (Formula)sheet.GetCellContents(null);
//        }

//        /// <summary>
//        /// GetCellContents(string name) should throw InvalidNameException if name is 
//        /// null or invalid
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void GetCellContentsInvalidName()
//        {
//            Spreadsheet sheet = new Spreadsheet();

//            Formula actualContents = (Formula)sheet.GetCellContents("2x");
//        }

//        /// <summary>
//        /// GetCellContents(string name) should throw InvalidNameException if name is 
//        /// null or invalid
//        /// </summary>
//        [TestMethod]
//        public void GetCellContentsOverwriteCells()
//        {
//            Spreadsheet sheet = new Spreadsheet();

//            // set some cell contents
//            sheet.SetCellContents("x", new Formula("1/2 + 1/2"));
//            sheet.SetCellContents("y", 3.5);
//            sheet.SetCellContents("z", "String Content");

//            // check cell values
//            string actualContentsString = (string)sheet.GetCellContents("z");
//            Formula actualContentsFormula = (Formula)sheet.GetCellContents("x");
//            double actualContentsDouble = (double)sheet.GetCellContents("y");

//            Assert.AreEqual("String Content", actualContentsString);
//            Assert.IsTrue(new Formula("1/2 + 1/2") == actualContentsFormula);
//            Assert.AreEqual(3.5, actualContentsDouble);


//            // overwrite some cells
//            sheet.SetCellContents("x", "Bleep Bloop Warble");
//            sheet.SetCellContents("y", new Formula("2+3"));
//            sheet.SetCellContents("z", 4.5);

//            // check the overwritten values
//            actualContentsString = (string)sheet.GetCellContents("x");
//            actualContentsFormula = (Formula)sheet.GetCellContents("y");
//            actualContentsDouble = (double)sheet.GetCellContents("z");

//            // check that overwritten values have changed
//            Assert.AreEqual("Bleep Bloop Warble", actualContentsString);
//            Assert.IsTrue(new Formula("2+3") == actualContentsFormula);
//            Assert.AreEqual(4.5, actualContentsDouble);
//        }

//        /// <summary>
//        /// Put the GetCellContents() tests together to see if they work outside of isolation
//        /// </summary>
//        [TestMethod]
//        public void GetCellContentsAllTogether()
//        {
//            Spreadsheet sheet = new Spreadsheet();

//            // some strings
//            string expectedContentsString = "Hello World";
//            sheet.SetCellContents("x", "Hello World");
//            string actualContentsString = (string)sheet.GetCellContents("x");
//            Assert.AreEqual(expectedContentsString, actualContentsString);

//            expectedContentsString = "Don't Bite Me";
//            sheet.SetCellContents("_", "Don't Bite Me");
//            actualContentsString = (string)sheet.GetCellContents("_");
//            Assert.AreEqual(expectedContentsString, actualContentsString);

//            // some doubles
//            double expectedContentsDouble = 3.1415;
//            sheet.SetCellContents("x2", 3.1415);
//            double actualContentsDouble = (double)sheet.GetCellContents("x2");
//            Assert.AreEqual(expectedContentsDouble, actualContentsDouble);

//            expectedContentsDouble = 2;
//            sheet.SetCellContents("y_15", 2);
//            actualContentsDouble = (double)sheet.GetCellContents("y_15");
//            Assert.AreEqual(expectedContentsDouble, actualContentsDouble);

//            // some formulas
//            Formula expectedContentsFormula = new Formula("2.5 + 2.5");
//            sheet.SetCellContents("___", new Formula("2.5 + 2.5"));
//            Formula actualContentsFormula = (Formula)sheet.GetCellContents("___");
//            Assert.IsTrue(expectedContentsFormula == actualContentsFormula);

//            expectedContentsFormula = new Formula("4 - 2");
//            sheet.SetCellContents("xXyYzZ_2_4_8_16_BleepBloopRobot", new Formula("4 - 2"));
//            actualContentsFormula = (Formula)sheet.GetCellContents("xXyYzZ_2_4_8_16_BleepBloopRobot");
//            Assert.IsTrue(expectedContentsFormula == actualContentsFormula);

//            expectedContentsFormula = new Formula("4 - c16");
//            sheet.SetCellContents("c1", new Formula("4 - c16"));
//            actualContentsFormula = (Formula)sheet.GetCellContents("c1");
//            Assert.IsTrue(expectedContentsFormula == actualContentsFormula);

//            // overwrite some cells
//            sheet.SetCellContents("xXyYzZ_2_4_8_16_BleepBloopRobot", "Bleep Bloop Warble");
//            sheet.SetCellContents("y_15", new Formula("2+3"));
//            sheet.SetCellContents("x", 3.5);

//            // check the overwritten values
//            actualContentsString = (string)sheet.GetCellContents("xXyYzZ_2_4_8_16_BleepBloopRobot");
//            actualContentsFormula = (Formula)sheet.GetCellContents("y_15");
//            actualContentsDouble = (double)sheet.GetCellContents("x");

//            // check that overwritten values have changed
//            Assert.AreEqual("Bleep Bloop Warble", actualContentsString);
//            Assert.IsTrue(new Formula("2+3") == actualContentsFormula);
//            Assert.AreEqual(3.5, actualContentsDouble);

//            // check that we can still get empty cells
//            Assert.AreEqual("", (string)sheet.GetCellContents("_x23"));
//            Assert.AreEqual("", (string)sheet.GetCellContents("_xmen"));
//        }

//        /// <summary>
//        /// Creates an empty spreadsheet. If spreasheet is empty the number of all 
//        /// non empty cells should be 0.
//        /// </summary>
//        [TestMethod]
//        public void GetNamesOfAllNonemptyCellsEmptySheet()
//        {
//            // create empty spreadsheet. Should throw no exception
//            // per instructions this should work for another developer
//            Spreadsheet sheet = new Spreadsheet();

//            // create a list from the IEnumerable to check size
//            List<string> contents = new List<string>(sheet.GetNamesOfAllNonemptyCells());

//            // check that there were no non-empty cells
//            Assert.AreEqual(0, contents.Count);
//        }


//        /// <summary>
//        /// check the size of contents of all non-empty cells after adding a bunch
//        /// </summary>
//        [TestMethod]
//        public void GetNamesOfAllNonemptyCellsAddAFew()
//        {
//            // create empty spreadsheet. Should throw no exception
//            // per instructions this should work for another developer
//            Spreadsheet sheet = new Spreadsheet();

//            // get a set to check against
//            HashSet<string> nonEmpty = new HashSet<string>();

//            int size = 100;
//            for(int i = 0; i < size; i++)
//            {
//                string cellName = "a" + i;
//                double contents = 0.5 + i;

//                nonEmpty.Add(cellName);
//                sheet.SetCellContents(cellName, contents);
//            }

//            // create a list from the IEnumerable to check size
//            List<string> nonEmptyActual = new List<string>(sheet.GetNamesOfAllNonemptyCells());

//            //check size
//            Assert.AreEqual(size, nonEmptyActual.Count);

//            // check contents
//            foreach(string cell in nonEmptyActual)
//            {
//                Assert.IsTrue(nonEmpty.Contains(cell));
//            }
//        }

//        /// <summary>
//        /// check the size of contents of all non-empty cells after adding a bunch
//        /// then replacing some
//        /// </summary>
//        [TestMethod]
//        public void GetNamesOfAllNonemptyCellsReplaceCells()
//        {
//            // create empty spreadsheet. Should throw no exception
//            // per instructions this should work for another developer
//            Spreadsheet sheet = new Spreadsheet();

//            // get a set to check against
//            HashSet<string> nonEmpty = new HashSet<string>();

//            // add the cells
//            int size = 100;
//            for(int i = 0; i < size; i++)
//            {
//                string cellName = "a" + i;
//                double contents = 0.5 + i;

//                nonEmpty.Add(cellName);
//                sheet.SetCellContents(cellName, contents);
//            }

//            // replace some cells
//            for(int i = 0; i < size; i += 2)
//            {
//                string cellName = "a" + i;
//                double contents = 0.3 + i;

//                sheet.SetCellContents(cellName, contents);
//            }

//            // create a list from the IEnumerable to check size
//            List<string> nonEmptyActual = new List<string>(sheet.GetNamesOfAllNonemptyCells());

//            //check size
//            Assert.AreEqual(size, nonEmptyActual.Count);

//            // check contents
//            foreach(string cell in nonEmptyActual)
//            {
//                Assert.IsTrue(nonEmpty.Contains(cell));
//            }
//        }

//        /// <summary>
//        /// check the size of contents of all non-empty cells after adding a bunch
//        /// then removing some
//        /// </summary>
//        [TestMethod]
//        public void GetNamesOfAllNonemptyCellsRemoveCells()
//        {
//            // create empty spreadsheet. Should throw no exception
//            // per instructions this should work for another developer
//            Spreadsheet sheet = new Spreadsheet();

//            // get a set to check against
//            HashSet<string> nonEmpty = new HashSet<string>();

//            // add the cells
//            int size = 100;
//            for(int i = 0; i < size; i++)
//            {
//                string cellName = "a" + i;
//                double contents = 0.5 + i;

//                nonEmpty.Add(cellName);
//                sheet.SetCellContents(cellName, contents);
//            }

//            // remove some cells
//            for(int i = 0; i < size; i += 2)
//            {
//                string cellName = "a" + i;
//                string contents = "";

//                nonEmpty.Remove(cellName);
//                sheet.SetCellContents(cellName, contents);
//            }

//            // create a list from the IEnumerable to check size
//            List<string> nonEmptyActual = new List<string>(sheet.GetNamesOfAllNonemptyCells());

//            //check size
//            Assert.AreEqual(nonEmpty.Count, nonEmptyActual.Count);

//            // check contents
//            foreach(string cell in nonEmptyActual)
//            {
//                Assert.IsTrue(nonEmpty.Contains(cell));
//            }
//        }

//        /// <summary>
//        /// check the size of contents of all non-empty cells after adding a bunch
//        /// then removing some
//        /// </summary>
//        [TestMethod]
//        public void GetNamesOfAllNonemptyCellsRemoveAndReplaceCells()
//        {

//            Spreadsheet sheet = new Spreadsheet();

//            // get a set to check against
//            HashSet<string> nonEmpty = new HashSet<string>();

//            // add the cells
//            int size = 100;
//            for(int i = 0; i < size; i++)
//            {
//                string cellName = "a" + i;
//                double contents = 0.5 + i;

//                nonEmpty.Add(cellName);
//                sheet.SetCellContents(cellName, contents);
//            }

//            // remove some cells
//            for(int i = 0; i < size; i += 2)
//            {
//                string cellName = "a" + i;
//                string contents = "";

//                nonEmpty.Remove(cellName);
//                sheet.SetCellContents(cellName, contents);
//            }

//            // replace some cells
//            for(int i = 0; i < size; i += 3)
//            {
//                string cellName = "a" + i;
//                double contents = 0.3 + i;

//                nonEmpty.Add(cellName);
//                sheet.SetCellContents(cellName, contents);
//            }

//            // create a list from the IEnumerable to check size
//            List<string> nonEmptyActual = new List<string>(sheet.GetNamesOfAllNonemptyCells());

//            //check size
//            Assert.AreEqual(nonEmpty.Count, nonEmptyActual.Count);

//            // check contents
//            foreach(string cell in nonEmptyActual)
//            {
//                Assert.IsTrue(nonEmpty.Contains(cell));
//            }
//        }

//        /// <summary>
//        /// Check the proper exception is thrown when null name is passed to method
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void SetCellContentsDoubleNullName()
//        {
//            Spreadsheet sheet = new Spreadsheet();

//            sheet.SetCellContents(null, 2.5);
//        }

//        /// <summary>
//        /// Check the proper exception is thrown when invalid name is passed to method
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void SetCellContentsDoubleInvalidName()
//        {
//            Spreadsheet sheet = new Spreadsheet();

//            sheet.SetCellContents("3x", 2.5);
//        }

//        /// <summary>
//        /// Check the proper contents are in the cell
//        /// </summary>
//        [TestMethod]
//        public void SetCellContentsDoubleContents()
//        {
//            Spreadsheet sheet = new Spreadsheet();
//            sheet.SetCellContents("A1", 2.5);

//            Assert.AreEqual(2.5, (double)sheet.GetCellContents("A1"));
//        }

//        /// <summary>
//        /// Check proper set is returned, documentation example
//        /// if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
//        /// set {A1, B1, C1} is returned
//        /// </summary>
//        [TestMethod]
//        public void SetCellContentsDoubleSimpleDependencies()
//        {
//            Spreadsheet sheet = new Spreadsheet();
//            sheet.SetCellContents("B1", new Formula("A1*2"));
//            sheet.SetCellContents("C1", new Formula("B1+A1"));

//            // get the set
//            ISet<string> dependencySet = sheet.SetCellContents("A1", 2.5);

//            // check its size
//            Assert.AreEqual(3, dependencySet.Count);

//            // check the list
//            Assert.IsTrue(dependencySet.Contains("A1"));
//            Assert.IsTrue(dependencySet.Contains("B1"));
//            Assert.IsTrue(dependencySet.Contains("C1"));

//            sheet.SetCellContents("C1", 3);

//            // check the state of spreadsheet after changing a cell value
//            List<string> nonEmpty = new List<string>(sheet.GetNamesOfAllNonemptyCells());
//            Assert.IsTrue(3 == nonEmpty.Count);
//            Assert.IsTrue(nonEmpty.Contains("A1"));
//            Assert.IsTrue(nonEmpty.Contains("B1"));
//            Assert.IsTrue(nonEmpty.Contains("C1"));

//            // make sure dependencies were changed correctly
//            dependencySet = sheet.SetCellContents("A1", 3.5);

//            Assert.AreEqual(2, dependencySet.Count);

//            Assert.IsTrue(dependencySet.Contains("A1"));
//            Assert.IsTrue(dependencySet.Contains("B1"));

//        }

//        /// <summary>
//        /// Check the proper exception is thrown when null name is passed to method
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void SetCellContentsTextNullName()
//        {
//            Spreadsheet sheet = new Spreadsheet();

//            sheet.SetCellContents(null, "Hello World");
//        }

//        /// <summary>
//        /// Check the proper exception is thrown when invalid name is passed to method
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void SetCellContentsTextInvalidName()
//        {
//            Spreadsheet sheet = new Spreadsheet();

//            sheet.SetCellContents("3x", "Hey!");
//        }

//        /// <summary>
//        /// Check the proper contents are in the cell
//        /// </summary>
//        [TestMethod]
//        public void SetCellContentsTextContents()
//        {
//            Spreadsheet sheet = new Spreadsheet();
//            sheet.SetCellContents("A1", "Hello World");

//            Assert.AreEqual("Hello World", (string)sheet.GetCellContents("A1"));
//        }

//        /// <summary>
//        /// Check the behavior of SetCellContents if setting the contents as 
//        /// text. Should still return a set of dependencies. Contents of the cell should be 
//        /// the set text.
//        /// </summary>
//        [TestMethod]
//        public void SetCellContentsTextDependencies()
//        {
//            Spreadsheet sheet = new Spreadsheet();
//            sheet.SetCellContents("B1", new Formula("A1*2"));
//            sheet.SetCellContents("C1", new Formula("B1+A1"));

//            // get the set
//            ISet<string> dependencySet = sheet.SetCellContents("A1", "3");

//            // check its size
//            Assert.AreEqual(3, dependencySet.Count);

//            // check the list
//            Assert.IsTrue(dependencySet.Contains("A1"));
//            Assert.IsTrue(dependencySet.Contains("B1"));
//            Assert.IsTrue(dependencySet.Contains("C1"));

//            // check contents fo the cell
//            Assert.IsTrue("3" == (string)sheet.GetCellContents("A1"));

//            // if we empty a cell, we should see that correctly reflected in a set of non-emtpy cells
//            sheet.SetCellContents("C1", "");

//            // check the state of spreadsheet after emptying a cell
//            List<string> nonEmpty = new List<string>(sheet.GetNamesOfAllNonemptyCells());
//            Assert.IsTrue(2 == nonEmpty.Count);
//            Assert.IsTrue(nonEmpty.Contains("A1"));
//            Assert.IsTrue(nonEmpty.Contains("B1"));

//            // make sure dependencies were changed correctly
//            dependencySet = sheet.SetCellContents("A1", "2");

//            Assert.AreEqual(2, dependencySet.Count);

//            Assert.IsTrue(dependencySet.Contains("A1"));
//            Assert.IsTrue(dependencySet.Contains("B1"));

//        }

//        /// <summary>
//        /// Check the proper exception is thrown when null formula is passed to method
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void SetCellContentsNullFormula()
//        {
//            Spreadsheet sheet = new Spreadsheet();
//            Formula formula = null;
//            sheet.SetCellContents("A1", formula);
//        }

//        /// <summary>
//        /// Check the proper exception is thrown when null name is passed to method
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void SetCellContentsFormulaNullName()
//        {
//            Spreadsheet sheet = new Spreadsheet();

//            sheet.SetCellContents(null, new Formula("A1*2"));
//        }

//        /// <summary>
//        /// Check the proper exception is thrown when invalid name is passed to method
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void SetCellContentsFormulaInvalidName()
//        {
//            Spreadsheet sheet = new Spreadsheet();

//            sheet.SetCellContents("3x", new Formula("A1*2"));
//        }

//        /// <summary>
//        /// Check the proper contents are in the cell
//        /// </summary>
//        [TestMethod]
//        public void SetCellContentsFormulaContents()
//        {
//            Spreadsheet sheet = new Spreadsheet();
//            sheet.SetCellContents("A1", new Formula("B1*2"));
//            Formula f = new Formula("B1*2");

//            Assert.IsTrue(f == (Formula)sheet.GetCellContents("A1"));
//        }

//        /// <summary>
//        /// Check the behavior of SetCellContents if setting the contents as 
//        /// text. Should still return a set of dependencies. Contents of the cell should be 
//        /// the set text.
//        /// </summary>
//        [TestMethod]
//        public void SetCellContentsFormulaDependencies()
//        {
//            Spreadsheet sheet = new Spreadsheet();
//            sheet.SetCellContents("B1", new Formula("A1*2"));
//            sheet.SetCellContents("C1", new Formula("B1+A1"));

//            // get the set
//            ISet<string> dependencySet = sheet.SetCellContents("A1", new Formula("2 + 2"));

//            // check its size
//            Assert.AreEqual(3, dependencySet.Count);

//            // check the list
//            Assert.IsTrue(dependencySet.Contains("A1"));
//            Assert.IsTrue(dependencySet.Contains("B1"));
//            Assert.IsTrue(dependencySet.Contains("C1"));

//            // check contents fo the cell
//            Formula f = new Formula("2+2");
//            Assert.IsTrue(f == (Formula)sheet.GetCellContents("A1"));

//            // if we change a cell, we should see that correctly reflected in a set of non-emtpy cells
//            sheet.SetCellContents("C1", new Formula("2+4"));

//            // check the state of spreadsheet after emptying a cell
//            List<string> nonEmpty = new List<string>(sheet.GetNamesOfAllNonemptyCells());
//            Assert.IsTrue(3 == nonEmpty.Count);
//            Assert.IsTrue(nonEmpty.Contains("A1"));
//            Assert.IsTrue(nonEmpty.Contains("B1"));
//            Assert.IsTrue(nonEmpty.Contains("C1"));

//            // make sure dependencies were changed correctly
//            dependencySet = sheet.SetCellContents("A1", "2 + 1");

//            Assert.AreEqual(2, dependencySet.Count);

//            Assert.IsTrue(dependencySet.Contains("A1"));
//            Assert.IsTrue(dependencySet.Contains("B1"));

//        }

//        /// <summary>
//        /// Check the proper exception is thrown when circular
//        /// dependency is created
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(CircularException))]
//        public void SetCellContentsCircularDependency()
//        {
//            Spreadsheet sheet = new Spreadsheet();
//            sheet.SetCellContents("B1", new Formula("A1*2"));
//            sheet.SetCellContents("C1", new Formula("B1+A1"));

//            // create circular dependency
//            sheet.SetCellContents("A1", new Formula("2 + C1"));
//        }

//        /// <summary>
//        /// Check the state of the Spreadsheet after dependency 
//        /// </summary>
//        [TestMethod]
//        public void SetCellContentsCircularDependencyCheckSheetState()
//        {
//            Spreadsheet sheet = new Spreadsheet();
//            sheet.SetCellContents("B1", new Formula("A1*2"));
//            sheet.SetCellContents("C1", new Formula("B1+A1"));

//            // create circular dependency
//            try
//            {
//                sheet.SetCellContents("A1", new Formula("2 + C1"));
//                // should not reach this statement
//                Assert.Fail();
//            }
//            catch(CircularException)
//            {

//            }

//            // check state of spreadsheet spreadsheet should not have changed
//            List<string> nonEmpty = new List<string>(sheet.GetNamesOfAllNonemptyCells());
//            Assert.IsTrue(2 == nonEmpty.Count);
//            Assert.IsTrue(nonEmpty.Contains("B1"));
//            Assert.IsTrue(nonEmpty.Contains("C1"));

//            // lets make sure dependencies are returning correctly
//            ISet<string> dependencySet = sheet.SetCellContents("A1", new Formula("2 + 2"));
//            Assert.AreEqual(3, dependencySet.Count);

//            // check the list
//            Assert.IsTrue(dependencySet.Contains("A1"));
//            Assert.IsTrue(dependencySet.Contains("B1"));
//            Assert.IsTrue(dependencySet.Contains("C1"));
//        }
//    }
//}
