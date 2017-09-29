// Written by Aaron Bellis u0981638 for CS 3500 2017 Fall Semester.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        // a graph which keeps track of the contained in the formulas of each cell.
        private DependencyGraph dependencies;

        // a dictionary which is keyed by cell names and contains all non-empty cells.
        // if a cell becomes empty, will be removed from the dictionary
        private Dictionary<string, Cell> cells;

        public Spreadsheet()
        {
            
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }

        //------------------------------Helper Methods------------------------------------//

        //------------------------------Internal Classes----------------------------------//

        /// <summary>
        /// Class representation of a spreadsheet cell. Each cell has a contents and a value. 
        /// 
        /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
        /// contents is an empty string, we say that the cell is empty.
        /// 
        /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
        /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
        /// in the grid.)
        /// 
        /// If a cell's contents is a string, its value is that string.
        /// 
        /// If a cell's contents is a double, its value is that double.
        /// 
        /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
        /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
        /// of course, can depend on the values of variables.  The value of a variable is the 
        /// value of the spreadsheet cell it names (if that cell's value is a double) or 
        /// is undefined (otherwise).
        /// 
        /// Cells are immutable. In order to change the contents of a cell, a new cell must be created.
        /// </summary>
        private class Cell
        {
            // an enum which defines the cell type. Used to speed up type casting of returned contents and values of
            // the cell. Should only have to be set once. 
            public enum CellType { stringType, doubleType, formulaType }

            // the cell's contents as apposed to its value
            private object cellContents;

            // Cell values will be calculated on request

            // the type of this Cell, Attribute is access only after cell created
            public CellType Type { get; private set; }


            /// <summary>
            /// Constructor for the Cell class. Will set its contents and type attribute. 
            /// </summary>
            private Cell(string contents)
            {
                Type = CellType.stringType;

                cellContents = contents;
            }
            
            /// <summary>
            /// Constructor for the Cell class. Will set its contents and type attribute. 
            /// </summary>
            private Cell(double contents)
            {
                Type = CellType.doubleType;

                cellContents = contents;
            }
            
            /// <summary>
            /// Constructor for the Cell class. Will set its contents and type attribute. 
            /// </summary>
            private Cell(Formula contents)
            {
                Type = CellType.formulaType;

                cellContents = contents;
            }

            /// <summary>
            /// Gets the contents of this cell. The contents can be a string, double or a formula.
            /// </summary>
            public object GetCellContents()
            {
                // may not have to do casting on this return will check in tests and create a branch that does not cast
                switch(Type)
                {
                    case CellType.doubleType:
                        return (double)cellContents;
                    case CellType.stringType:
                        return (string)cellContents;
                    default:
                        return (Formula)cellContents;
                }
            }

            /// <summary>
            /// Gets the value of this cell. The value can be a string, double or a FormulaError.
            /// If the CellType is a doubleType returns a double.
            /// If the CellType is a stringType returns a string.
            /// If the CellType is a formulaType returns the value returned by the Formula's Evaluate function 
            /// and the provided lookup delegate. 
            /// </summary>
            public object GetCellValue(Func<string, double> lookup)
            {
                // may not have to do casting on this return will check in tests and create a branch that does not cast
                switch(Type)
                {
                    case CellType.doubleType:
                        return (double)cellContents;
                    case CellType.stringType:
                        return (string)cellContents;
                    default:
                        Formula formula = (Formula)cellContents;
                        return formula.Evaluate(lookup);
                }
            }

            /// <summary>
            /// Gets the value of this cell. The value can be a string, double or a FormulaError.
            /// If the CellType is a doubleType returns a double.
            /// If the CellType is a stringType returns a string.
            /// If the CellType is a formulaType returns the value returned by the Formula's Evaluate function 
            /// using a lookup delegate that assigns all varaibles the value 0.
            /// </summary>
            public object GetCellValue()
            {
                return GetCellValue(x => 0);
            }
        }

    }

    /// <summary>
    /// Extension Methods for Spreadsheet Class
    /// </summary>
    internal static class ExtensionMethods
    {
        
    }
}