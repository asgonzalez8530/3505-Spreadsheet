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

        public override object GetCellContents(string name)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

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
            // the cell
            public enum CellType { stringType, doubleType, formulaType }

            // these store the contents of this cell. Only one will have a value, the other two will be null, or in
            // the case of a primitive type, will be set to 0;
            private string stringContents;
            private double doubleContents;
            private Formula formulaContents;

            // Cell values will be calculated on request

            // the type of this Cell, Attribute is access only after cell created
            public CellType Type { get; private set; }

            /// <summary>
            /// Constructor for the Cell class. Used to initialize a cell if the cell contents are to be a string.
            /// </summary>
            public Cell(string contents)
            {
                // explicitly set the unused member variables;
                doubleContents = 0.0;
                formulaContents = null;

                // set the contents of the cell
                stringContents = contents;

                // set the cell type
                Type = CellType.stringType;

            }

            /// <summary>
            /// Constructor for the Cell class. Used to initialize a cell if the cell contents are to be a double.
            /// </summary>
            public Cell(double contents)
            {
                // explicitly set the unused member variables;
                stringContents = null;
                formulaContents = null;

                // set the contents of the cell
                doubleContents = contents;

                // set the cell type
                Type = CellType.doubleType;

            }

            /// <summary>
            /// Constructor for the Cell class. Used to initialize a cell if the cell contents are to be a formula.
            /// </summary>
            public Cell(Formula contents)
            {
                // explicitly set the unused member variables;
                stringContents = null;
                doubleContents = 0.0;

                // set the contents of the cell
                formulaContents = contents;

                // set the cell type
                Type = CellType.formulaType;
            }

            /// <summary>
            /// Gets the contents of this cell. The contents can be a string, double or a formula.
            /// </summary>
            public object GetCellContents()
            {
                switch(Type)
                {
                    case CellType.doubleType:
                        return doubleContents;
                    case CellType.stringType:
                        return stringContents;
                    default:
                        return formulaContents;
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
                switch(Type)
                {
                    case CellType.doubleType:
                        return doubleContents;
                    case CellType.stringType:
                        return stringContents;
                    default:
                        return formulaContents.Evaluate(lookup);
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