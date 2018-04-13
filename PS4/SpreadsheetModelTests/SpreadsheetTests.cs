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
            sheet.SetContentsOfCell("x1", "=1/2 + 1/2");
            sheet.SetContentsOfCell("y1", "3.5");
            sheet.SetContentsOfCell("z1", "String Content");

            // check cell Contents
            string actualContentsString = (string)sheet.GetCellContents("z1");
            Formula actualContentsFormula = (Formula)sheet.GetCellContents("x1");
            double actualContentsDouble = (double)sheet.GetCellContents("y1");

            Assert.AreEqual("String Content", actualContentsString);
            Assert.IsTrue(new Formula("1/2 + 1/2") == actualContentsFormula);
            Assert.AreEqual(3.5, actualContentsDouble);


            // overwrite some cells
            sheet.SetContentsOfCell("x1", "Bleep Bloop Warble");
            sheet.SetContentsOfCell("y1", "=2+3");
            sheet.SetContentsOfCell("z1", "4.5");

            // check the overwritten contents
            actualContentsString = (string)sheet.GetCellContents("x1");
            actualContentsFormula = (Formula)sheet.GetCellContents("y1");
            actualContentsDouble = (double)sheet.GetCellContents("z1");

            // check that overwritten values have changed
            Assert.AreEqual("Bleep Bloop Warble", actualContentsString);
            Assert.IsTrue(new Formula("2+3") == actualContentsFormula);
            Assert.AreEqual(4.5, actualContentsDouble);
        }

        /// <summary>
        /// GetCellContents(string name) should throw InvalidNameException if name is 
        /// null or invalid
        /// </summary>
        [TestMethod]
        public void GetCellValueOverwriteCells()
        {
            Spreadsheet sheet = new Spreadsheet();

            // set some cell contents
            sheet.SetContentsOfCell("x1", "=1/2 + 1/2");
            sheet.SetContentsOfCell("y1", "3.5");
            sheet.SetContentsOfCell("z1", "String Content");

            // check cell values
            string actualValueString = (string)sheet.GetCellValue("z1");
            double actualValueFormula = (double)sheet.GetCellValue("x1");
            double actualValueDouble = (double)sheet.GetCellValue("y1");

            Assert.AreEqual("String Content", actualValueString);
            Assert.IsTrue(1.0 == actualValueFormula);
            Assert.AreEqual(3.5, actualValueDouble);


            // overwrite some cells
            sheet.SetContentsOfCell("x1", "Bleep Bloop Warble");
            sheet.SetContentsOfCell("y1", "=2+3");
            sheet.SetContentsOfCell("z1", "4.5");

            // check cell values
            actualValueString = (string)sheet.GetCellValue("x1");
            actualValueFormula = (double)sheet.GetCellValue("y1");
            actualValueDouble = (double)sheet.GetCellValue("z1");

            Assert.AreEqual("Bleep Bloop Warble", actualValueString);
            Assert.IsTrue(5.0 == actualValueFormula);
            Assert.AreEqual(4.5, actualValueDouble);
        }

        /// <summary>
        /// Put the GetCellContents() tests together to see if they work outside of isolation
        /// </summary>
        [TestMethod]
        public void GetCellContentsAllTogether()
        {
            Spreadsheet sheet = new Spreadsheet();

            // some strings
            //set content
            string expectedContentsString = "Hello World";
            sheet.SetContentsOfCell("x1", "Hello World");
            // get and check content
            string actualContentsString = (string)sheet.GetCellContents("x1");
            Assert.AreEqual(expectedContentsString, actualContentsString);


            //set content
            expectedContentsString = "Don't Bite Me";
            sheet.SetContentsOfCell("znb45", "Don't Bite Me");
            //get and check content
            actualContentsString = (string)sheet.GetCellContents("znb45");
            Assert.AreEqual(expectedContentsString, actualContentsString);


            // some doubles
            // set content
            double expectedContentsDouble = 3.1415;
            sheet.SetContentsOfCell("x2", "3.1415");
            // get and check content
            double actualContentsDouble = (double)sheet.GetCellContents("x2");
            Assert.AreEqual(expectedContentsDouble, actualContentsDouble);


            // set contents
            expectedContentsDouble = 2;
            sheet.SetContentsOfCell("y15", "2");
            // get and check content
            actualContentsDouble = (double)sheet.GetCellContents("y15");
            Assert.AreEqual(expectedContentsDouble, actualContentsDouble);


            // some formulas
            // set content
            Formula expectedContentsFormula = new Formula("2.5 + 2.5");
            sheet.SetContentsOfCell("beautiful10", "=2.5 + 2.5");
            // get and check content
            Formula actualContentsFormula = (Formula)sheet.GetCellContents("beautiful10");
            Assert.IsTrue(expectedContentsFormula == actualContentsFormula);


            // set content
            expectedContentsFormula = new Formula("4 - 2");
            sheet.SetContentsOfCell("xXyYzZ24816", "=4 - 2");
            // get and check content
            actualContentsFormula = (Formula)sheet.GetCellContents("xXyYzZ24816");
            Assert.IsTrue(expectedContentsFormula == actualContentsFormula);


            // set content
            expectedContentsFormula = new Formula("4 - c16");
            sheet.SetContentsOfCell("c1", "=4 - c16");
            // check content
            actualContentsFormula = (Formula)sheet.GetCellContents("c1");
            Assert.IsTrue(expectedContentsFormula == actualContentsFormula);



            // overwrite some cells
            sheet.SetContentsOfCell("xXyYzZ24816", "Bleep Bloop Warble");
            sheet.SetContentsOfCell("y15", "=2+3");
            sheet.SetContentsOfCell("x1", "3.5");

            // check the overwritten contents
            actualContentsString = (string)sheet.GetCellContents("xXyYzZ24816");
            actualContentsFormula = (Formula)sheet.GetCellContents("y15");
            actualContentsDouble = (double)sheet.GetCellContents("x1");

            // check that overwritten contents have changed
            Assert.AreEqual("Bleep Bloop Warble", actualContentsString);
            Assert.IsTrue(new Formula("2+3") == actualContentsFormula);
            Assert.AreEqual(3.5, actualContentsDouble);

            // check that we can still get empty cells
            Assert.AreEqual("", (string)sheet.GetCellContents("x23"));
            Assert.AreEqual("", (string)sheet.GetCellContents("xmen1"));
        }

        /// <summary>
        /// Put the GetCellContents() tests together to see if they work outside of isolation
        /// </summary>
        [TestMethod]
        public void GetCellValueAllTogether()
        {
            Spreadsheet sheet = new Spreadsheet();

            // some strings
            //set content
            string expectedContentsString = "Hello World";
            sheet.SetContentsOfCell("x1", "Hello World");

            // get and check value
            string actualValueString = (string)sheet.GetCellValue("x1");
            Assert.AreEqual(expectedContentsString, actualValueString);

            //set content
            expectedContentsString = "Don't Bite Me";
            sheet.SetContentsOfCell("znb45", "Don't Bite Me");

            // get and check value
            actualValueString = (string)sheet.GetCellValue("znb45");
            Assert.AreEqual(expectedContentsString, actualValueString);

            // some doubles
            // set content
            double expectedContentsDouble = 3.1415;
            sheet.SetContentsOfCell("x2", "3.1415");

            // get and check value
            double actualValueDouble = (double)sheet.GetCellValue("x2");
            Assert.AreEqual(expectedContentsDouble, actualValueDouble);

            // set contents
            expectedContentsDouble = 2;
            sheet.SetContentsOfCell("y15", "2");

            // get and check value
            actualValueDouble = (double)sheet.GetCellValue("y15");
            Assert.AreEqual(expectedContentsDouble, actualValueDouble);

            // some formulas
            // set content
            Formula expectedContentsFormula = new Formula("2.5 + 2.5");
            sheet.SetContentsOfCell("beautiful10", "=2.5 + 2.5");

            // get and check value
            expectedContentsDouble = 5.0;
            actualValueDouble = (double)sheet.GetCellValue("beautiful10");
            Assert.AreEqual(expectedContentsDouble, actualValueDouble);

            // set content
            expectedContentsFormula = new Formula("4 - 2");
            sheet.SetContentsOfCell("xXyYzZ24816", "=4 - 2");

            // get and check value
            expectedContentsDouble = 2.0;
            actualValueDouble = (double)sheet.GetCellValue("xXyYzZ24816");
            Assert.AreEqual(expectedContentsDouble, actualValueDouble);

            // set content
            expectedContentsFormula = new Formula("4 - c16");
            sheet.SetContentsOfCell("c1", "=4 - c16");

            // check value
            object actualValueObject = sheet.GetCellValue("c1");
            Assert.IsTrue(actualValueObject.GetType() == typeof(FormulaError));


            // overwrite some cells
            sheet.SetContentsOfCell("xXyYzZ24816", "Bleep Bloop Warble");
            sheet.SetContentsOfCell("y15", "=2+3");
            sheet.SetContentsOfCell("x1", "3.5");



            // check that overwritten values have changed
            // check the overwritten contents
            actualValueString = (string)sheet.GetCellValue("xXyYzZ24816");
            double actualValueFormula = (double)sheet.GetCellValue("y15");
            actualValueDouble = (double)sheet.GetCellValue("x1");

            Assert.AreEqual("Bleep Bloop Warble", actualValueString);
            Assert.IsTrue(5.0 == actualValueFormula);
            Assert.AreEqual(3.5, actualValueDouble);

            // udate cell value, cell c1 depends on the value of this cell
            sheet.SetContentsOfCell("c16", "2");
            actualValueDouble = (double)sheet.GetCellValue("c1");
            Assert.AreEqual(2.0, actualValueDouble);

            // change cell value again
            sheet.SetContentsOfCell("c16", "3");
            actualValueDouble = (double)sheet.GetCellValue("c1");
            Assert.AreEqual(1.0, actualValueDouble);

            // check that we can still get empty cells
            Assert.AreEqual("", (string)sheet.GetCellValue("x23"));
            Assert.AreEqual("", (string)sheet.GetCellValue("xmen1"));
        }

        /// <summary>
        /// Put the GetCellContents() tests together to see if they work outside of isolation
        /// </summary>
        [TestMethod]
        public void GetCellValueAndCellContentsAllTogether()
        {
            Spreadsheet sheet = new Spreadsheet();

            // some strings
            //set content
            string expectedContentsString = "Hello World";
            sheet.SetContentsOfCell("x1", "Hello World");
            // get and check content
            string actualContentsString = (string)sheet.GetCellContents("x1");
            Assert.AreEqual(expectedContentsString, actualContentsString);
            // get and check value
            string actualValueString = (string)sheet.GetCellValue("x1");
            Assert.AreEqual(expectedContentsString, actualValueString);

            //set content
            expectedContentsString = "Don't Bite Me";
            sheet.SetContentsOfCell("znb45", "Don't Bite Me");
            //get and check content
            actualContentsString = (string)sheet.GetCellContents("znb45");
            Assert.AreEqual(expectedContentsString, actualContentsString);
            // get and check value
            actualValueString = (string)sheet.GetCellValue("znb45");
            Assert.AreEqual(expectedContentsString, actualValueString);

            // some doubles
            // set content
            double expectedContentsDouble = 3.1415;
            sheet.SetContentsOfCell("x2", "3.1415");
            // get and check content
            double actualContentsDouble = (double)sheet.GetCellContents("x2");
            Assert.AreEqual(expectedContentsDouble, actualContentsDouble);
            // get and check value
            double actualValueDouble = (double)sheet.GetCellValue("x2");
            Assert.AreEqual(expectedContentsDouble, actualValueDouble);

            // set contents
            expectedContentsDouble = 2;
            sheet.SetContentsOfCell("y15", "2");
            // get and check content
            actualContentsDouble = (double)sheet.GetCellContents("y15");
            Assert.AreEqual(expectedContentsDouble, actualContentsDouble);
            // get and check value
            actualValueDouble = (double)sheet.GetCellValue("y15");
            Assert.AreEqual(expectedContentsDouble, actualValueDouble);

            // some formulas
            // set content
            Formula expectedContentsFormula = new Formula("2.5 + 2.5");
            sheet.SetContentsOfCell("beautiful10", "=2.5 + 2.5");
            // get and check content
            Formula actualContentsFormula = (Formula)sheet.GetCellContents("beautiful10");
            Assert.IsTrue(expectedContentsFormula == actualContentsFormula);
            // get and check value
            expectedContentsDouble = 5.0;
            actualValueDouble = (double)sheet.GetCellValue("beautiful10");
            Assert.AreEqual(expectedContentsDouble, actualValueDouble);

            // set content
            expectedContentsFormula = new Formula("4 - 2");
            sheet.SetContentsOfCell("xXyYzZ24816", "=4 - 2");
            // get and check content
            actualContentsFormula = (Formula)sheet.GetCellContents("xXyYzZ24816");
            Assert.IsTrue(expectedContentsFormula == actualContentsFormula);
            // get and check value
            expectedContentsDouble = 2.0;
            actualValueDouble = (double)sheet.GetCellValue("xXyYzZ24816");
            Assert.AreEqual(expectedContentsDouble, actualValueDouble);

            // set content
            expectedContentsFormula = new Formula("4 - c16");
            sheet.SetContentsOfCell("c1", "=4 - c16");
            // check content
            actualContentsFormula = (Formula)sheet.GetCellContents("c1");
            Assert.IsTrue(expectedContentsFormula == actualContentsFormula);
            // check value
            object actualValueObject = sheet.GetCellValue("c1");
            Assert.IsTrue(actualValueObject.GetType() == typeof(FormulaError));


            // overwrite some cells
            sheet.SetContentsOfCell("xXyYzZ24816", "Bleep Bloop Warble");
            sheet.SetContentsOfCell("y15", "=2+3");
            sheet.SetContentsOfCell("x1", "3.5");

            // check the overwritten contents
            actualContentsString = (string)sheet.GetCellContents("xXyYzZ24816");
            actualContentsFormula = (Formula)sheet.GetCellContents("y15");
            actualContentsDouble = (double)sheet.GetCellContents("x1");

            // check that overwritten contents have changed
            Assert.AreEqual("Bleep Bloop Warble", actualContentsString);
            Assert.IsTrue(new Formula("2+3") == actualContentsFormula);
            Assert.AreEqual(3.5, actualContentsDouble);

            // check that overwritten values have changed
            // check the overwritten contents
            actualValueString = (string)sheet.GetCellValue("xXyYzZ24816");
            double actualValueFormula = (double)sheet.GetCellValue("y15");
            actualValueDouble = (double)sheet.GetCellValue("x1");

            Assert.AreEqual("Bleep Bloop Warble", actualValueString);
            Assert.IsTrue(5.0 == actualValueFormula);
            Assert.AreEqual(3.5, actualValueDouble);

            // udate cell content, cell c1 depends on the value of this cell
            sheet.SetContentsOfCell("c16", "2");
            actualValueDouble = (double)sheet.GetCellValue("c1");
            Assert.AreEqual(2.0, actualValueDouble);

            // change cell content again
            sheet.SetContentsOfCell("c16", "3");
            actualValueDouble = (double)sheet.GetCellValue("c1");
            Assert.AreEqual(1.0, actualValueDouble);

            // check that we can still get empty cells
            Assert.AreEqual("", (string)sheet.GetCellContents("x23"));
            Assert.AreEqual("", (string)sheet.GetCellContents("xmen1"));

            // check that we can still get empty cells
            Assert.AreEqual("", (string)sheet.GetCellValue("x23"));
            Assert.AreEqual("", (string)sheet.GetCellValue("xmen1"));
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
                string contents = (0.5 + i).ToString();

                nonEmpty.Add(cellName);
                sheet.SetContentsOfCell(cellName, contents);
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
            for (int i = 0; i < size; i++)
            {
                string cellName = "a" + i;
                string contents = (0.5 + i).ToString();

                nonEmpty.Add(cellName);
                sheet.SetContentsOfCell(cellName, contents);
            }

            // replace some cells
            for (int i = 0; i < size; i += 2)
            {
                string cellName = "a" + i;
                string contents = (0.3 + i).ToString();

                sheet.SetContentsOfCell(cellName, contents);
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
            for (int i = 0; i < size; i++)
            {
                string cellName = "a" + i;
                string contents = (0.5 + i).ToString();

                nonEmpty.Add(cellName);
                sheet.SetContentsOfCell(cellName, contents);
            }

            // remove some cells
            for (int i = 0; i < size; i += 2)
            {
                string cellName = "a" + i;
                string contents = "";

                nonEmpty.Remove(cellName);
                sheet.SetContentsOfCell(cellName, contents);
            }

            // create a list from the IEnumerable to check size
            List<string> nonEmptyActual = new List<string>(sheet.GetNamesOfAllNonemptyCells());

            //check size
            Assert.AreEqual(nonEmpty.Count, nonEmptyActual.Count);

            // check contents
            foreach (string cell in nonEmptyActual)
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
            for (int i = 0; i < size; i++)
            {
                string cellName = "a" + i;
                string contents = (0.5 + i).ToString();

                nonEmpty.Add(cellName);
                sheet.SetContentsOfCell(cellName, contents);
            }

            // remove some cells
            for (int i = 0; i < size; i += 2)
            {
                string cellName = "a" + i;
                string contents = "";

                nonEmpty.Remove(cellName);
                sheet.SetContentsOfCell(cellName, contents);
            }

            // replace some cells
            for (int i = 0; i < size; i += 3)
            {
                string cellName = "a" + i;
                string contents = (0.3 + i).ToString();

                nonEmpty.Add(cellName);
                sheet.SetContentsOfCell(cellName, contents);
            }

            // create a list from the IEnumerable to check size
            List<string> nonEmptyActual = new List<string>(sheet.GetNamesOfAllNonemptyCells());

            //check size
            Assert.AreEqual(nonEmpty.Count, nonEmptyActual.Count);

            // check contents
            foreach (string cell in nonEmptyActual)
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

            sheet.SetContentsOfCell(null, "2.5");
        }

        /// <summary>
        /// Check the proper exception is thrown when invalid name is passed to method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsDoubleInvalidName()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("_3x", "2.5");
        }

        /// <summary>
        /// Check the proper contents are in the cell
        /// </summary>
        [TestMethod]
        public void SetCellContentsDoubleContents()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "2.5");

            Assert.AreEqual(2.5, (double)sheet.GetCellContents("A1"));


        }

        /// <summary>
        /// Check the proper value is in the cell
        /// </summary>
        [TestMethod]
        public void SetCellContentsDoubleValue()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "2.5");

            Assert.AreEqual(2.5, (double)sheet.GetCellValue("A1"));
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
            sheet.SetContentsOfCell("B1", "=A1*2");
            sheet.SetContentsOfCell("C1", "=B1+A1");

            // get the set
            ISet<string> dependencySet = sheet.SetContentsOfCell("A1", "2.5");

            // check its size
            Assert.AreEqual(3, dependencySet.Count);

            // check the list
            Assert.IsTrue(dependencySet.Contains("A1"));
            Assert.IsTrue(dependencySet.Contains("B1"));
            Assert.IsTrue(dependencySet.Contains("C1"));

            sheet.SetContentsOfCell("C1", "3");

            // check the state of spreadsheet after changing a cell value
            List<string> nonEmpty = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(3 == nonEmpty.Count);
            Assert.IsTrue(nonEmpty.Contains("A1"));
            Assert.IsTrue(nonEmpty.Contains("B1"));
            Assert.IsTrue(nonEmpty.Contains("C1"));

            // make sure dependencies were changed correctly
            dependencySet = sheet.SetContentsOfCell("A1", "3.5");

            Assert.AreEqual(2, dependencySet.Count);

            Assert.IsTrue(dependencySet.Contains("A1"));
            Assert.IsTrue(dependencySet.Contains("B1"));

        }

        /// <summary>
        /// Check the proper exception is thrown when null name is passed to method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsTextNullName()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell(null, "Hello World");
        }

        /// <summary>
        /// Check the proper exception is thrown when invalid name is passed to method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsTextInvalidName()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("3x", "Hey!");
        }

        /// <summary>
        /// Check the proper contents are in the cell
        /// </summary>
        [TestMethod]
        public void SetCellContentsTextContents()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "Hello World");

            Assert.AreEqual("Hello World", (string)sheet.GetCellContents("A1"));

        }

        /// <summary>
        /// Check the proper contents are in the cell
        /// </summary>
        [TestMethod]
        public void SetCellContentsTextValue()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "Hello World");


            Assert.AreEqual("Hello World", (string)sheet.GetCellValue("A1"));
        }

        /// <summary>
        /// Check the behavior of SetCellContents if setting the contents as 
        /// text. Should still return a set of dependencies. Contents of the cell should be 
        /// the set text.
        /// </summary>
        [TestMethod]
        public void SetCellContentsTextDependencies()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1*2");
            sheet.SetContentsOfCell("C1", "=B1+A1");

            // get the set
            ISet<string> dependencySet = sheet.SetContentsOfCell("A1", "3");

            // check its size
            Assert.AreEqual(3, dependencySet.Count);

            // check the list
            Assert.IsTrue(dependencySet.Contains("A1"));
            Assert.IsTrue(dependencySet.Contains("B1"));
            Assert.IsTrue(dependencySet.Contains("C1"));

            // check contents of the cell
            Assert.IsTrue(3.0 == (double)sheet.GetCellContents("A1"));
            // check value of the cell
            Assert.IsTrue(3.0 == (double)sheet.GetCellValue("A1"));


            // if we empty a cell, we should see that correctly reflected in a set of non-emtpy cells
            sheet.SetContentsOfCell("C1", "");

            // check the state of spreadsheet after emptying a cell
            List<string> nonEmpty = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(2 == nonEmpty.Count);
            Assert.IsTrue(nonEmpty.Contains("A1"));
            Assert.IsTrue(nonEmpty.Contains("B1"));

            // make sure dependencies were changed correctly
            dependencySet = sheet.SetContentsOfCell("A1", "2");

            Assert.AreEqual(2, dependencySet.Count);

            Assert.IsTrue(dependencySet.Contains("A1"));
            Assert.IsTrue(dependencySet.Contains("B1"));

        }

        /// <summary>
        /// Check the proper exception is thrown when null formula is passed to method
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(FormulaFormatException))] changed for 3505
        public void SetCellContentsNullFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            string formula = "=";
            sheet.SetContentsOfCell("A1", formula);
            Object val = sheet.GetCellValue("A1");
            Assert.IsTrue(val is FormatError);
        }

        /// <summary>
        /// Check the proper exception is thrown when null name is passed to method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsFormulaNullName()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell(null, "=A1*2");
        }

        /// <summary>
        /// Check the proper exception is thrown when invalid name is passed to method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsFormulaInvalidName()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("3x", "=A1*2");
        }

        /// <summary>
        /// Check the proper contents are in the cell
        /// </summary>
        [TestMethod]
        public void SetCellContentsFormulaContents()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1*2");
            Formula f = new Formula("B1*2");

            Assert.IsTrue(f == (Formula)sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Check the proper contents are in the cell
        /// </summary>
        [TestMethod]
        public void SetCellContentsFormulaValue()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1*2");
            Formula f = new Formula("B1*2");

            Assert.IsTrue(typeof(FormulaError) == sheet.GetCellValue("A1").GetType());
        }

        /// <summary>
        /// Check the behavior of SetCellContents if setting the contents as 
        /// text. Should still return a set of dependencies. Contents of the cell should be 
        /// the set text.
        /// </summary>
        [TestMethod]
        public void SetCellContentsFormulaDependencies()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1*2");
            sheet.SetContentsOfCell("C1", "=B1+A1");

            // get the set
            ISet<string> dependencySet = sheet.SetContentsOfCell("A1", "=2 + 2");

            // check its size
            Assert.AreEqual(3, dependencySet.Count);

            // check the list
            Assert.IsTrue(dependencySet.Contains("A1"));
            Assert.IsTrue(dependencySet.Contains("B1"));
            Assert.IsTrue(dependencySet.Contains("C1"));

            // check contents of the cell
            Formula f = new Formula("2+2");
            Assert.IsTrue(f == (Formula)sheet.GetCellContents("A1"));
            // check value of the cell
            Assert.IsTrue(4.0 == (double)sheet.GetCellValue("A1"));

            // if we change a cell, we should see that correctly reflected in a set of non-emtpy cells
            sheet.SetContentsOfCell("C1", "=2+4");

            // check the state of spreadsheet after emptying a cell
            List<string> nonEmpty = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(3 == nonEmpty.Count);
            Assert.IsTrue(nonEmpty.Contains("A1"));
            Assert.IsTrue(nonEmpty.Contains("B1"));
            Assert.IsTrue(nonEmpty.Contains("C1"));

            // make sure dependencies were changed correctly
            dependencySet = sheet.SetContentsOfCell("A1", "2 + 1");

            Assert.AreEqual(2, dependencySet.Count);

            Assert.IsTrue(dependencySet.Contains("A1"));
            Assert.IsTrue(dependencySet.Contains("B1"));

        }

        /// <summary>
        /// Check the proper exception is thrown when circular
        /// dependency is created
        /// </summary>
        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContentsCircularDependency()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1*2");
            sheet.SetContentsOfCell("C1", "=B1+A1");

            // create circular dependency
            sheet.SetContentsOfCell("A1", "=2 + C1");
        }

        /// <summary>
        /// Check the state of the Spreadsheet after dependency 
        /// </summary>
        [Ignore]
        [TestMethod]
        public void SetCellContentsCircularDependencyCheckSheetState()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1*2");
            sheet.SetContentsOfCell("C1", "=B1+A1");

            // create circular dependency
            try
            {
                sheet.SetContentsOfCell("A1", "=2 + C1");
                // should not reach this statement
                Assert.Fail();
            }
            catch (CircularException)
            {

            }

            // check state of spreadsheet spreadsheet should not have changed
            List<string> nonEmpty = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(2 == nonEmpty.Count);
            Assert.IsTrue(nonEmpty.Contains("B1"));
            Assert.IsTrue(nonEmpty.Contains("C1"));

            // lets make sure dependencies are returning correctly
            ISet<string> dependencySet = sheet.SetContentsOfCell("A1", "=2 + 2");
            Assert.AreEqual(3, dependencySet.Count);

            // check the list
            Assert.IsTrue(dependencySet.Contains("A1"));
            Assert.IsTrue(dependencySet.Contains("B1"));
            Assert.IsTrue(dependencySet.Contains("C1"));
        }

        /// <summary>
        /// Check the state of the Spreadsheet after dependency 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetContentsOfCellNullContent()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", null);
            
        }

        /// <summary>
        /// Check that Changed is false after new Spreadsheet is created
        /// </summary>
        [TestMethod]
        public void ChangedNewSpreadsheet()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.IsFalse(sheet.Changed);
            
        }

        /// <summary>
        /// Check that Changed is true after new Spreadsheet is changed
        /// </summary>
        [TestMethod]
        public void ChangedSpreadsheet()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("a1", "=2.0");
            Assert.IsTrue(sheet.Changed);

        }

        /// <summary>
        /// Check that Changed is false after new Spreadsheet is saved
        /// </summary>
        [TestMethod]
        public void ChangedSavedSpreadsheet()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("a1", "=2.0");
            sheet.SetContentsOfCell("a2", "2.0");
            sheet.SetContentsOfCell("a3", "Hello World");
            string saveLocation = "test1.xml";
            sheet.Save(saveLocation);
            Assert.IsFalse(sheet.Changed);

        }

        /// <summary>
        /// Check that Changed is true after new Spreadsheet is saved,
        /// then modified
        /// </summary>
        [TestMethod]
        public void ChangedSavedSpreadsheetChangeAfterSave()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("a1", "=2.0");
            string saveLocation = "test1.xml";
            sheet.Save(saveLocation);
            sheet.SetContentsOfCell("a1", "Hello World");
            Assert.IsTrue(sheet.Changed);

        }

        /// <summary>
        /// Check that Changed is true after new Spreadsheet is saved,
        /// then modified
        /// </summary>
        [TestMethod]
        public void GetSavedVersionTest()
        {
            string version = "Hello World";
            Spreadsheet sheet = new Spreadsheet(x => true, x => x.ToUpper(), version);

            sheet.SetContentsOfCell("a1", "=2.0");
            string saveLocation = "test1.xml";
            sheet.Save(saveLocation);
            string savedVersion = sheet.GetSavedVersion(saveLocation);

            Assert.AreEqual(version, savedVersion);

        }

        /// <summary>
        /// Check that Changed is true after new Spreadsheet is saved,
        /// then modified
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedVersionTestInvalidSaveLocation()
        {
            string version = "Hello World";
            Spreadsheet sheet = new Spreadsheet(x => true, x => x.ToUpper(), version);

            sheet.SetContentsOfCell("a1", "=2.0");
            string saveLocation = "test1.xml";
            sheet.Save(saveLocation);
            string savedVersion = sheet.GetSavedVersion("doesntexist.xml");

        }

        /// <summary>
        /// Check that Changed is true after new Spreadsheet is saved,
        /// then modified
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedVersionTestInvalidXml()
        {
            string version = "Hello World";
            Spreadsheet sheet = new Spreadsheet(x => true, x => x.ToUpper(), version);

            
            string savedVersion = sheet.GetSavedVersion("invalidVersion.xml");
            
        }

        /// <summary>
        /// Check that Changed is true after new Spreadsheet is saved,
        /// then modified
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedVersionTestInvalidXml2()
        {
            string version = "Hello World";
            Spreadsheet sheet = new Spreadsheet(x => true, x => x.ToUpper(), version);


            string savedVersion = sheet.GetSavedVersion("invalidVersion1.xml");

        }

        /// <summary>
        /// Check that Changed is true after new Spreadsheet is saved,
        /// then modified
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedSpreadsheet()
        {
            string version = "1.0";
            Spreadsheet sheet = new Spreadsheet(x => true, x => x.ToUpper(), version);

            sheet.SetContentsOfCell("a1", "Hello World");
            sheet.SetContentsOfCell("b1", "25");
            sheet.SetContentsOfCell("c1", "=b1 + 5");

            string saveLocation = "test1.xml";
            sheet.Save(saveLocation);

            sheet = new Spreadsheet(saveLocation, x => true, x => x.ToUpper(), "2.0");
        }

        /// <summary>
        /// Check that Changed is true after new Spreadsheet is saved,
        /// then modified
        /// </summary>
        [TestMethod]
        public void GetSavedSpreadsheetNoError()
        {
            // create a spreadsheet
            string version = "1.0";
            Spreadsheet sheet = new Spreadsheet(x => true, x => x.ToUpper(), version);

            sheet.SetContentsOfCell("a1", "Hello World");
            sheet.SetContentsOfCell("b1", "25");
            sheet.SetContentsOfCell("c1", "=b1 + 5");
            // save spreadsheet
            string saveLocation = "test1.xml";
            Assert.IsTrue(sheet.Changed);
            sheet.Save(saveLocation);

            // read spreasheet
            sheet = new Spreadsheet(saveLocation, x => true, x => x.ToUpper(), version);
            Assert.IsFalse(sheet.Changed);

            // check spreadsheet contents
            string a1Contents = (string)sheet.GetCellContents("a1");
            double b1Contents = (double)sheet.GetCellContents("b1");
            Formula c1Contents = (Formula)sheet.GetCellContents("c1");

            Assert.AreEqual("Hello World", a1Contents);
            Assert.AreEqual(25.0, b1Contents);
            Assert.IsTrue(c1Contents == new Formula("B1 + 5"));

            // check spreadsheet values
            string a1Value = (string)sheet.GetCellValue("a1");
            double b1Value = (double)sheet.GetCellValue("b1");
            double c1Value = (double)sheet.GetCellValue("c1");

            Assert.AreEqual("Hello World", a1Value);
            Assert.AreEqual(25.0, b1Value);
            Assert.AreEqual(30.0, c1Value);


        }
    }
}
