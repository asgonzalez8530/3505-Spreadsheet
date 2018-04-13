using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SpreadsheetUtilities;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;




namespace SpreadsheetModelTests
{
    /// <summary>
    /// Tests the functionality of a Spreadsheet.
    /// </summary>
    /// 
    /// <author>Rebekah Peterson, u0871657</author>
    [TestClass]
    public class BekahTests
    {
        /// <summary>
        /// Normalizes variables to uppercase
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public string Uppercase(string var)
        {
            return var.ToUpper();
        }

        /// <summary>
        /// Normalizes variables to lowercase
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public string Lowercase(string var)
        {
            return var.ToLower();
        }

        /// <summary>
        /// A simple validity check, these variable must only have two characters.
        /// </summary>
        public bool TestValidity(string str)
        {
            return str.Length == 2;
        }
        // ********************************** SetContentsOfCell ******************************************* //
        [TestMethod]
        public void TestDouble()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a4", "3.2");
            double expected = 3.2;
            Assert.AreEqual(expected, sheet.GetCellContents("a4"));
        }

        [TestMethod]
        public void TestFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("g5", "=(42*h4)");
            Formula expected = new Formula("(42*h4)");
            Assert.AreEqual(expected, sheet.GetCellContents("g5"));
        }

        [TestMethod]
        //[ExpectedException(typeof(FormulaFormatException))] changed for 3505
        public void TestFormulaFormatException()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("g5", "=((42*h4)");
            Object val = sheet.GetCellValue("g5");
            Assert.IsTrue(val is FormatError);
        }

        [TestMethod]
        public void TestString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("k15", "never");
            string expected = "never";
            Assert.AreEqual(expected, sheet.GetCellContents("k15"));
        }

        [TestMethod]
        public void TestStringNormalizeVariable()
        {
            Spreadsheet sheet = new Spreadsheet(s => true, Uppercase, "default");
            sheet.SetContentsOfCell("K15", "never");
            string expected = "never";
            Assert.AreEqual(expected, sheet.GetCellContents("k15"));
        }

        [TestMethod]
        public void TestFormulaWithVariablesThatNeedNormalize()
        {
            Spreadsheet sheet = new Spreadsheet(s => true, Lowercase, "default");
            sheet.SetContentsOfCell("g5", "=(42*H4)");
            Formula expected = new Formula("(42*h4)");
            Assert.AreEqual(expected, sheet.GetCellContents("g5"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullContentTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            string n = null;
            sheet.SetContentsOfCell("H14", n);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullVariableTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            string n = null;
            sheet.SetContentsOfCell(n, "never");
        }

        [TestMethod]
        public void DoublesSetCells()
        {
            Spreadsheet sheet = new Spreadsheet();
            HashSet<string> set1 = (sheet.SetContentsOfCell("a2", "3.211")) as HashSet<string>;
            HashSet<string> set2 = (sheet.SetContentsOfCell("b4", "17.0")) as HashSet<string>;
            HashSet<string> set3 = (sheet.SetContentsOfCell("b1", "-533333.1")) as HashSet<string>;
            Assert.AreEqual(1, set1.Count);
            Assert.AreEqual(1, set2.Count);
            Assert.AreEqual(1, set3.Count);
            Assert.IsTrue(set1.Contains("a2"));
            Assert.IsTrue(set2.Contains("b4"));
            Assert.IsTrue(set3.Contains("b1"));
        }

        [TestMethod]
        public void DoubleChangeCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            HashSet<string> set1 = (sheet.SetContentsOfCell("a2", "3.211")) as HashSet<string>;
            HashSet<string> set2 = (sheet.SetContentsOfCell("a2", "17.0")) as HashSet<string>;
            Assert.AreEqual(1, set1.Count);
            Assert.AreEqual(1, set2.Count);
            Assert.IsTrue(set1.Contains("a2"));
            Assert.IsTrue(set2.Contains("a2"));
            object expected = 17.0;
            Assert.AreEqual(expected, sheet.GetCellContents("a2"));
        }

        [TestMethod]
        public void DoubleToStringChangeCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            HashSet<string> set1 = (sheet.SetContentsOfCell("a2", "lol")) as HashSet<string>;
            HashSet<string> set2 = (sheet.SetContentsOfCell("a2", "17.0")) as HashSet<string>;
            Assert.AreEqual(1, set1.Count);
            Assert.AreEqual(1, set2.Count);
            Assert.IsTrue(set1.Contains("a2"));
            Assert.IsTrue(set2.Contains("a2"));
            object expected = 17.0;
            Assert.AreEqual(expected, sheet.GetCellContents("a2"));
        }

        [TestMethod]
        public void DoubleToFormulaChangeCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2.0");
            sheet.SetContentsOfCell("a1", "=4+2");

            List<string> arr = new List<string>();
            foreach (string str in sheet.GetNamesOfAllNonemptyCells())
            {
                arr.Add(str);
            }
            Assert.AreEqual(1, arr.Count);
            Assert.IsTrue(arr.Contains("a1"));

            Assert.AreEqual(new Formula("4+2"), sheet.GetCellContents("a1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void DoubleNullVariableTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            string n = null;
            sheet.SetContentsOfCell(n, "14.2");
        }

        [TestMethod]
        public void GetCellContentsOfUnsetVariable()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("", sheet.GetCellContents("a5"));
        }



        // ********************************** Other Tests ******************************************* //
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameGetCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("510");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullNameGetCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            string n = null;
            sheet.GetCellContents(n);
        }

        [TestMethod]
        public void IndirectDependentsTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("c1", "=b1");
            sheet.SetContentsOfCell("e1", "=b1+7");
            sheet.SetContentsOfCell("f1", "never forget");
            sheet.SetContentsOfCell("d1", "=a1*f1");
            sheet.SetContentsOfCell("b1", "=a1");
            HashSet<string> cSetOfDependents = (sheet.SetContentsOfCell("a1", "44.2")) as HashSet<string>;
            Assert.AreEqual(5, cSetOfDependents.Count);
            Assert.IsTrue(cSetOfDependents.Contains("a1"));
            Assert.IsTrue(cSetOfDependents.Contains("b1"));
            Assert.IsTrue(cSetOfDependents.Contains("c1"));
            Assert.IsTrue(cSetOfDependents.Contains("e1"));
            Assert.IsTrue(cSetOfDependents.Contains("d1"));
        }

        [TestMethod]
        public void ChangeCellContentsToEmptyStringsConstructorTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            HashSet<string> set1 = (sheet.SetContentsOfCell("b3", "lol")) as HashSet<string>;
            HashSet<string> set2 = (sheet.SetContentsOfCell("b3", "")) as HashSet<string>;
            Assert.AreEqual(1, set1.Count);
            Assert.IsTrue(set1.Contains("b3"));

            object expected = "";
            Assert.AreEqual(expected, sheet.GetCellContents("b3"));

            int count = 0;
            foreach (string str in sheet.GetNamesOfAllNonemptyCells())
            {
                count++;
            }
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void AddDuplicates()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "fo");
            HashSet<string> seta = (sheet.SetContentsOfCell("a1", "fo")) as HashSet<string>;
            sheet.SetContentsOfCell("c1", "=3/b1");
            HashSet<string> setc = (sheet.SetContentsOfCell("c1", "=3/b1")) as HashSet<string>;
            sheet.SetContentsOfCell("b1", "3.0");
            HashSet<string> setb = (sheet.SetContentsOfCell("b1", "3.0")) as HashSet<string>;

            // check stuff
            Assert.AreEqual("fo", sheet.GetCellContents("a1"));
            Assert.AreEqual(3.0, sheet.GetCellContents("b1"));
            Assert.AreEqual(new Formula("3/b1"), sheet.GetCellContents("c1"));

            Assert.AreEqual(1, seta.Count);
            Assert.IsTrue(seta.Contains("a1"));

            Assert.AreEqual(2, setb.Count);
            Assert.IsTrue(setb.Contains("b1"));
            Assert.IsTrue(setb.Contains("c1"));

            Assert.AreEqual(1, setc.Count);
            Assert.IsTrue(setc.Contains("c1"));

            List<string> arr = new List<string>();
            foreach (string str in sheet.GetNamesOfAllNonemptyCells())
            {
                arr.Add(str);
            }
            Assert.AreEqual(3, arr.Count);
            Assert.IsTrue(arr.Contains("a1"));
            Assert.IsTrue(arr.Contains("b1"));
            Assert.IsTrue(arr.Contains("c1"));
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(CircularException))]
        public void DependsOnItselfTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=a1");
        }

        [TestMethod]
        public void TwoSpreadsheets()
        {
            // from a different test
            Spreadsheet sheet = new Spreadsheet();
            HashSet<string> set1 = (sheet.SetContentsOfCell("a2", "hello")) as HashSet<string>;
            HashSet<string> set2 = (sheet.SetContentsOfCell("b4", "nada")) as HashSet<string>;
            HashSet<string> set3 = (sheet.SetContentsOfCell("b1", "55")) as HashSet<string>;
            Assert.AreEqual(1, set1.Count);
            Assert.AreEqual(1, set2.Count);
            Assert.AreEqual(1, set3.Count);
            Assert.IsTrue(set1.Contains("a2"));
            Assert.IsTrue(set2.Contains("b4"));
            Assert.IsTrue(set3.Contains("b1"));

            //from another test

            Spreadsheet sheet2 = new Spreadsheet();
            sheet2.SetContentsOfCell("a1", "=b1 + d1");
            sheet2.SetContentsOfCell("b1", "2.0");
            sheet2.SetContentsOfCell("c1", "=a1");
            try
            {
                sheet2.SetContentsOfCell("d1", "=c1");
            }
            catch (CircularException)
            {
                int count = 0;
                bool hasA = false;
                bool hasB = false;
                bool hasC = false;
                bool hasD = false;
                foreach (string str in sheet2.GetNamesOfAllNonemptyCells())
                {
                    count++;
                    if (str == "a1")
                    {
                        hasA = true;
                    }
                    if (str == "b1")
                    {
                        hasB = true;
                    }
                    if (str == "c1")
                    {
                        hasC = true;
                    }
                    if (str == "d1")
                    {
                        hasD = true;
                    }
                }
                Assert.IsFalse(hasD);
                Assert.AreEqual(3, count);
                Assert.IsTrue(hasA);
                Assert.IsTrue(hasB);
                Assert.IsTrue(hasC);
            }

        }

        [TestMethod]
        public void GetNamesOfEmptyCellsTest()
        {
            int count = 0;
            Spreadsheet sheet = new Spreadsheet();
            foreach (string str in sheet.GetNamesOfAllNonemptyCells())
            {
                count++;
            }
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void GetNamesOfOneCellTest()
        {
            int count = 0;
            string[] arr = new string[1];
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a7", "3.2");
            foreach (string str in sheet.GetNamesOfAllNonemptyCells())
            {
                arr[count] = str;
                count++;
            }
            Assert.AreEqual(1, count);
            Assert.AreEqual("a7", arr[0]);
        }

        [TestMethod]
        public void StringsSetCells()
        {
            Spreadsheet sheet = new Spreadsheet();
            HashSet<string> set1 = (sheet.SetContentsOfCell("a2", "hello")) as HashSet<string>;
            HashSet<string> set2 = (sheet.SetContentsOfCell("b4", "nada")) as HashSet<string>;
            HashSet<string> set3 = (sheet.SetContentsOfCell("b1", "55")) as HashSet<string>;
            Assert.AreEqual(1, set1.Count);
            Assert.AreEqual(1, set2.Count);
            Assert.AreEqual(1, set3.Count);
            Assert.IsTrue(set1.Contains("a2"));
            Assert.IsTrue(set2.Contains("b4"));
            Assert.IsTrue(set3.Contains("b1"));
        }

        [TestMethod]
        public void StringChangeCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            HashSet<string> set1 = (sheet.SetContentsOfCell("a2", "lol")) as HashSet<string>;
            HashSet<string> set2 = (sheet.SetContentsOfCell("a2", "hello")) as HashSet<string>;
            Assert.AreEqual(1, set1.Count);
            Assert.AreEqual(1, set2.Count);
            Assert.IsTrue(set1.Contains("a2"));
            Assert.IsTrue(set2.Contains("a2"));
            object expected = "hello";
            Assert.AreEqual(expected, sheet.GetCellContents("a2"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void StringNullVariable()
        {
            Spreadsheet sheet = new Spreadsheet();
            string n = null;
            sheet.SetContentsOfCell(n, "aa");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void StringInvalidVariableTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("??", "lol");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StringNullContentsTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            string n = null;
            sheet.SetContentsOfCell("a4", n);
        }

        [TestMethod]
        public void FormulaToDoubleChangeCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("d3", "=h4+42");
            sheet.SetContentsOfCell("d3", "3.2");
            HashSet<string> set = (sheet.SetContentsOfCell("h4", "owe")) as HashSet<string>;
            Assert.AreEqual(1, set.Count);
            Assert.IsTrue(set.Contains("h4"));
        }

        [TestMethod]
        public void FormulaToStringChangeCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("d3", "=h4+42");
            sheet.SetContentsOfCell("d3", "lala");
            HashSet<string> set = (sheet.SetContentsOfCell("h4", "owe")) as HashSet<string>;
            Assert.AreEqual(1, set.Count);
            Assert.IsTrue(set.Contains("h4"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void FormulaInvalidNameSetCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("f#", "=f1+g1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void FormulaNullNameSetCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            string n = null;
            sheet.SetContentsOfCell(n, "=f1+g1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullFormulaSetCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            string n = null;
            sheet.SetContentsOfCell("A2", n);
        }

        [TestMethod]
        public void FormulaSetCellsTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=b1");
            HashSet<string> bDependentsSet = (sheet.SetContentsOfCell("b1", "2.0")) as HashSet<string>;
            Assert.AreEqual(2, bDependentsSet.Count);
            Assert.IsTrue(bDependentsSet.Contains("a1"));
        }

        [TestMethod]
        [Ignore]
        [ExpectedException(typeof(CircularException))]
        public void CircularExceptionFormulaTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=b1 + d1");
            sheet.SetContentsOfCell("b1", "2.0");
            sheet.SetContentsOfCell("c1", "=a1");
            sheet.SetContentsOfCell("d1", "=c1");
        }

        [TestMethod]
        public void CircularExceptionFormulaStateTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "b1 + d1");
            sheet.SetContentsOfCell("b1", "2.0");
            sheet.SetContentsOfCell("c1", "a1");
            try
            {
                sheet.SetContentsOfCell("d1", "=c1");
            }
            catch (CircularException)
            {
                int count = 0;
                bool hasA = false;
                bool hasB = false;
                bool hasC = false;
                foreach (string str in sheet.GetNamesOfAllNonemptyCells())
                {
                    count++;
                    if (str == "a1")
                    {
                        hasA = true;
                    }
                    if (str == "b1")
                    {
                        hasB = true;
                    }
                    if (str == "c1")
                    {
                        hasC = true;
                    }
                }
                Assert.AreEqual(3, count);
                Assert.IsTrue(hasA);
                Assert.IsTrue(hasB);
                Assert.IsTrue(hasC);
            }
        }

        [TestMethod]
        public void CircularExceptionFormulaStateReplaceGoodCellTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=b1 + d1");
            sheet.SetContentsOfCell("b1", "2.0");
            sheet.SetContentsOfCell("c1", "=a1");
            sheet.SetContentsOfCell("d1", "15.2");
            try
            {
                sheet.SetContentsOfCell("d1", "=c1");
            }
            catch (CircularException)
            {
                int count = 0;
                bool hasA = false;
                bool hasB = false;
                bool hasC = false;
                bool hasD = false;
                foreach (string str in sheet.GetNamesOfAllNonemptyCells())
                {
                    count++;
                    if (str == "a1")
                    {
                        hasA = true;
                    }
                    if (str == "b1")
                    {
                        hasB = true;
                    }
                    if (str == "c1")
                    {
                        hasC = true;
                    }
                    if (str == "d1")
                    {
                        hasD = true;
                    }
                }
                Assert.AreEqual(4, count);
                Assert.IsTrue(hasA);
                Assert.IsTrue(hasB);
                Assert.IsTrue(hasC);
                Assert.IsTrue(hasD);
                Assert.AreEqual(15.2, sheet.GetCellContents("d1"));
            }
        }

        [TestMethod]
        public void CircularExceptionFormulaStateReplaceGoodDependencyDramaCellTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=b1 / 2");
            sheet.SetContentsOfCell("b1", "=c1");
            sheet.SetContentsOfCell("c1", "=d1");
            try
            {
                sheet.SetContentsOfCell("c1", "=c1 + d1");
            }
            catch (CircularException)
            {
                int count = 0;
                bool hasA = false;
                bool hasB = false;
                bool hasC = false;
                foreach (string str in sheet.GetNamesOfAllNonemptyCells())
                {
                    count++;
                    if (str == "a1")
                    {
                        hasA = true;
                    }
                    if (str == "b1")
                    {
                        hasB = true;
                    }
                    if (str == "c1")
                    {
                        hasC = true;
                    }
                }
                Assert.AreEqual(3, count);
                Assert.IsTrue(hasA);
                Assert.IsTrue(hasB);
                Assert.IsTrue(hasC);
                Assert.AreEqual(new Formula("d1"), sheet.GetCellContents("c1"));
            }
        }

        [TestMethod]
        public void GetCellContentsEmpty()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("", sheet.GetCellContents("f5"));
        }

        // ********************************** GetCellValue Tests ******************************************* //
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullVariableGetValue()
        {
            Spreadsheet sheet = new Spreadsheet();
            string n = null;
            sheet.GetCellValue(n);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidVariableGetValue()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue("ddd13a");
        }

        [TestMethod]
        public void GetCellValueEmpty()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("", sheet.GetCellValue("a5"));
        }

        [TestMethod]
        public void BadLookupGetValue()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("e15", "=e4*2");
            Assert.IsTrue(sheet.GetCellValue("e15").GetType().Equals(typeof(FormulaError)));
        }

        [TestMethod]
        public void GetCellValueString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("t6", "apple");
            Assert.AreEqual("apple", sheet.GetCellValue("t6"));
        }

        [TestMethod]
        public void GetCellValueDouble()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("t6", "32.4");
            Assert.AreEqual(32.4, sheet.GetCellValue("t6"));
        }

        [TestMethod]
        public void GetCellValueFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("t6", "=3*12");
            Assert.AreEqual(36.0, sheet.GetCellValue("t6"));
        }

        /// <summary>
        /// Testing dependent Formula
        /// </summary>
        [TestMethod]
        public void GetCellValueFormula2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("t6", "=3*12");
            sheet.SetContentsOfCell("a4", "=t6/2");
            Assert.AreEqual(18.0, sheet.GetCellValue("a4"));
        }

        [TestMethod]
        public void SaveTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=(3+2)");
            sheet.SetContentsOfCell("a2", "3.2");
            sheet.SetContentsOfCell("a3", "income");
            sheet.Save("savetest.xml");

            Spreadsheet sheet2 = new Spreadsheet("savetest.xml", s => true, s => s, "default");
            Assert.AreEqual(5.0, sheet2.GetCellValue("a1"));
            Assert.AreEqual(3.2, sheet2.GetCellValue("a2"));
            Assert.AreEqual("income", sheet2.GetCellValue("a3"));
            Assert.AreEqual(new Formula("(3+2)"), sheet2.GetCellContents("a1"));
        }

        //[TestMethod]
        //public void CreateSpreadsheetFromFileTest()
        //{
        //    Spreadsheet sheet = new Spreadsheet("getsavedversiontest.xml", s => true, s => s, "default");

        //    List<string> actual = new List<string>();
        //    foreach (string var in sheet.GetNamesOfAllNonemptyCells())
        //    {
        //        actual.Add(var);
        //    }
        //    Assert.AreEqual(3, actual.Count);

        //    Assert.AreEqual("floop", sheet.GetCellValue("a5"));
        //    Assert.AreEqual(32.4, sheet.GetCellValue("a6"));
        //    Assert.AreEqual(504.0, sheet.GetCellValue("a7"));
        //    Assert.AreEqual(new Formula("42*12"), sheet.GetCellContents("a7"));
        //}

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void IsValidTest()
        {
            Spreadsheet sheet = new Spreadsheet(TestValidity, Uppercase, "default");
            sheet.SetContentsOfCell("aa3", "whatever");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ReadFileException()
        {
            Spreadsheet sheet = new Spreadsheet("notreal.xml", s => true, s => s, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void WriteFileException()
        {
            Spreadsheet sheet = new Spreadsheet();
            string notReal = null;
            sheet.Save(notReal);
        }

        [TestMethod]
        public void ChangedTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.IsFalse(sheet.Changed);
            sheet.SetContentsOfCell("W3", "happiness");
            Assert.IsTrue(sheet.Changed);
            sheet.SetContentsOfCell("e5", "=(((3-e4)))");
            Assert.IsTrue(sheet.Changed);
            sheet.SetContentsOfCell("e4", "1.0");
            Assert.IsTrue(sheet.Changed);
            sheet.Save("change.xml");
            Assert.IsFalse(sheet.Changed);
            sheet.SetContentsOfCell("w2", "word");
            Assert.IsTrue(sheet.Changed);
            sheet.Save("change.xml");
            Assert.IsFalse(sheet.Changed);
            List<string> actual = new List<string>();
            foreach (string str in sheet.GetNamesOfAllNonemptyCells())
            {
                actual.Add(str);
            }
            Assert.IsFalse(sheet.Changed);
            Assert.AreEqual(4, actual.Count);
            sheet.SetContentsOfCell("h4", "string");
            sheet = new Spreadsheet();
            Assert.IsFalse(sheet.Changed);
        }
    }
}
