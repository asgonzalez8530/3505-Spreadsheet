// implementation by Aaron Bellis u0981638 for CS 3500 2017 Fall Semester.
// Documentation copied from interface written by Joe Zachary for CS 3500, September 2013

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.Xml;

namespace SS
{
    /// <summary>
    /// A Spreadsheet object inherits from and implements the AbstractSpreadsheet class which 
    /// represents the state of a simple spreadsheet.  A spreadsheet consists of an infinite 
    /// number of named cells.
    /// 
    ///  A string is a cell name if and only if it consists of one or more letters,
    /// followed by one or more digits AND it satisfies the predicate IsValid.
    /// For example, "A15", "a15", "XY032", and "BC7" are cell names so long as they
    /// satisfy IsValid.  On the other hand, "Z", "X_", and "hello" are not cell names,
    /// regardless of IsValid.
    /// 
    /// Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
    /// must be normalized with the Normalize method before it is used by or saved in 
    /// this spreadsheet.  For example, if Normalize is s => s.ToUpper(), then
    /// the Formula "x3+a5" should be converted to "X3+A5" before use.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  (This
    /// means that a spreadsheet contains an infinite number of cells.)  In addition to 
    /// a name, each cell has a contents and a value.  The distinction is important.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In a new spreadsheet, the contents of every cell is the empty string.
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
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        // a graph which keeps track of the contained in the formulas of each cell.
        private DependencyGraph dependencies;

        // a dictionary which is keyed by cell names and contains all non-empty cells.
        // if a cell becomes empty, will be removed from the dictionary
        private Dictionary<string, Cell> cells;

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }

        /// <summary>
        /// Creates a new spreadhseet. In a new spreadsheet, the contents of every cell is
        /// the empty string. This constructor imposes no extra validity conditions, 
        /// normalizes every cell name to itself, and has version "default".
        /// </summary>
        public Spreadsheet()
            : this(x => true, x => x, "default")
        {

        }

        /// <summary>
        /// Creates a new spreadhseet. In a new spreadsheet, the contents of every cell is
        /// the empty string. The provided isValid delegate is used to impose additional 
        /// restrictions to the validity of cell names. The normalize delegate allows cell 
        /// names to be stored in a standardized format prior to use. Version provided to
        /// allow user to define versioning schema.
        /// </summary>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            dependencies = new DependencyGraph();
            cells = new Dictionary<string, Cell>();
            Changed = false;
        }

        /// <summary>
        /// Reads the saved spreadsheet from the file stored at the provided filePath and 
        /// uses it to construct a new spreadsheet The new spreadsheet will use the provided 
        /// validity delegate, normalization delegate and version. 
        /// 
        /// If the version of the saved spreadsheet does not match thte version parameter 
        /// proveded to the constructor, throws a SpreadsheetReadWriteException.
        ///
        /// If any other problems such as, invalid names, circular dependencies, problems
        /// opening reading or closing the file occurs, thtrows a SpreadsheetReadWriteException
        /// </summary>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : this(isValid, normalize, version)
        {
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                // first check that version is correct
                                if (reader["version"] != version)
                                {
                                    string msg = "Spreadsheet version mismatch";
                                    throw new SpreadsheetReadWriteException(msg);
                                }

                                break;

                            case "cell":
                                // read cell's contents and add it to this spreadsheet
                                ReadCellFromXML(reader);
                                break;
                        }
                    }
                }

                // we're done with the reader, time to close it
                reader.Close();
            }

        }

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                string version = "";
                using (XmlReader reader = XmlReader.Create(filename))
                {

                    if (reader.ReadToFollowing("spreadsheet"))
                    {
                        version = reader["version"];
                    }
                    else
                    {
                        throw new SpreadsheetReadWriteException("Unable to read Spreadsheet Version from file");
                    }

                    reader.Close();
                }

                return version;
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Error reading file: " + filename);
            }

        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.WriteEndDocumentOnClose = true;
            settings.Indent = true;
            settings.IndentChars = "   ";
            settings.NewLineChars = "\n";

            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);
                    writer.WriteWhitespace("\n");

                    foreach (string cell in cells.Keys)
                    {

                        writer.WriteStartElement(cell);
                        writer.WriteElementString("name", cell);
                        writer.WriteElementString("contents", GetCellContentsString(cell));
                        writer.WriteEndElement();
                    }

                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement();
                    writer.Close();
                }
            }
            catch (Exception)
            {
                string msg = "Error writing spreadsheet to file";
                throw new SpreadsheetReadWriteException(msg);
            }

            Changed = false;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            name = SafelyNormalize(name);
            CellNameValidator(name);

            if (!cells.ContainsKey(name))
            {
                return "";
            }
            else
            {
                return cells[name].GetCellValue();
            }
        }



        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            name = SafelyNormalize(name);
            // make sure cell name is valid
            CellNameValidator(name);

            if (cells.ContainsKey(name))
            {
                return cells[name].GetCellContents();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }

            name = SafelyNormalize(name);
            CellNameValidator(name);

            // see if content parses as double
            double d;
            if (double.TryParse(content, out d))
            {

                return SetCellContents(name, d);
            }

            if (content.BeginsWithChar('='))
            {
                // set the formula string to the substring following '=', if there is nothing
                // set to empty string
                string formulaString = content.Length > 1 ? content.Substring(1) : "";
                Formula formula = new Formula(formulaString, Normalize, Validator);
                return SetCellContents(name, formula);
            }

            return SetCellContents(name, content);
        }





        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            // cast to list because documentation states keys refer back to the 
            // individual objects in Dictionary
            return cells.Keys.ToList();
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
        protected override ISet<string> SetCellContents(string name, double number)
        {

            // validate the cell name
            CellNameValidator(name);

            // set the cell contents
            Cell cell = new Cell(number);
            AddCellToDictionary(name, cell);

            Changed = true;

            // we need to update the value of cells whos value depends on this cell
            IEnumerable<string> recalcCells = GetCellsToRecalculate(name);
            RecalculateCells(recalcCells);

            // get all cells whose value depends on name
            return new HashSet<string>(recalcCells);
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
        protected override ISet<string> SetCellContents(string name, string text)
        {
            // validate the cell name
            CellNameValidator(name);

            Changed = true;

            // special case, if text is empty string "", need to update all dependencies 
            // and remove cell from list
            if (text == "")
            {
                EmptyCell(name);
                return new HashSet<string>(GetCellsToRecalculate(name));
            }

            Cell cell = new Cell(text);
            AddCellToDictionary(name, cell);

            // we need to update the value of cells whos value depends on this cell
            IEnumerable<string> recalcCells = GetCellsToRecalculate(name);
            RecalculateCells(recalcCells);

            // get all cells whose value depends on name
            return new HashSet<string>(recalcCells);
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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            // make sure formula isn't null
            if (formula == null)
            {
                throw new ArgumentNullException();
            }

            // validate the cell name
            CellNameValidator(name);

            // update dependencies and check circular exception
            IEnumerable<string> dependencies = CheckCircularGetDependency(name, formula);

            Changed = true;

            // set the cell contents
            if (cells.ContainsKey(name))
            {
                cells[name] = new Cell(formula, LookupCellValue);
            }
            else
            {
                cells.Add(name, new Cell(formula, LookupCellValue));
            }

            // we need to update the value of cells whos value depends on this cell
            IEnumerable<string> recalcCells = GetCellsToRecalculate(name);
            RecalculateCells(recalcCells);

            // get all cells whose value depends on name
            return new HashSet<string>(recalcCells);
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
            if (name == null)
            {
                throw new ArgumentNullException();
            }

            name = SafelyNormalize(name);
            CellNameValidator(name);

            return dependencies.GetDependees(name);

        }

        //------------------------------Private Methods------------------------------------//

        /// <summary>
        /// Takes in a list of cells, in the order which they must be calculated, and 
        /// recalculates value of each one. 
        /// </summary>
        private void RecalculateCells(IEnumerable<string> recalcCells)
        {
            foreach (string cell in recalcCells)
            {
                if (cells.ContainsKey(cell))
                {
                    cells[cell].RecalculateCellValue(LookupCellValue);
                }
            }
        }


        /// <summary>
        /// Gets a cell's contents and returns a string version of its contents. 
        /// If its contents is a double d, returns d.ToString().
        /// If its contents is a string s, returns s.
        /// If its contents is a Formula f, returns "=" prepended to f.ToString()
        /// </summary>
        private string GetCellContentsString(string name)
        {
            Cell c = cells[name];
            switch (c.Type)
            {
                case Cell.CellType.doubleType:
                    double d = (double)cells[name].GetCellContents();
                    return d.ToString();
                case Cell.CellType.stringType:
                    return (string)cells[name].GetCellContents();
                default:
                    Formula f = (Formula)cells[name].GetCellContents();
                    return "=" + f.ToString();
            }
        }

        /// <summary>
        /// Takes an xml reader currently at an opening element with the name "cell"
        /// reads its child tags and adds the cells contents to this spreadsheet.
        /// </summary>
        private void ReadCellFromXML(XmlReader reader)
        {

            string name;
            string content;

            // get first child of cell element should be name
            if (reader.ReadToDescendant("name"))
            {
                name = reader.ReadElementContentAsString();
            }
            else
            {
                string msg = "error reading cell name from saved spreadsheet";
                throw new SpreadsheetReadWriteException(msg);
            }

            // get second child of cell element should be contents
            if (reader.ReadToNextSibling("contents"))
            {
                content = reader.ReadElementContentAsString();
            }
            else
            {
                string msg = "error reading cell contents from saved spreadsheet";
                throw new SpreadsheetReadWriteException(msg);
            }

            // Try to set the contents of the cell. If something goes wrong
            // throw the proper exception
            try
            {
                SetContentsOfCell(name, content);
            }
            catch (Exception e)
            {
                string msg = "Error reading cell from saved spreadsheet: ";
                msg += e.Message;
                throw new SpreadsheetReadWriteException(msg);
            }
        }

        /// <summary>
        /// If name is null, returns null. Else returns Normalize(name)
        /// </summary>
        private string SafelyNormalize(string name)
        {
            if (name == null)
            {
                return null;
            }
            else
            {
                return Normalize(name);
            }
        }

        /// <summary>
        /// A function which returns true if name is a valid string name IsValid is true
        /// else, returns false. Used to pass to Formulas to validate their variable names.
        /// </summary>
        private bool Validator(string name)
        {
            string pattern = @"^[a-zA-Z]+\d+$";
            if (name != null || Regex.IsMatch(name, pattern) || IsValid(name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// if name is null or invalid, throws an InvalidNameException, used to validate
        /// cell names passed to spreadsheet
        /// </summary>
        private void CellNameValidator(string name)
        {
            if (!Validator(name))
            {
                throw new InvalidNameException();
            }

        }

        /// <summary>
        /// Updates dependencies then checks a cell for circular dependency. If There is 
        /// a circular dependency ensures spreadsheet is not changed and old state of
        /// dependencies is restored then throws CircularDependency. Else returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// </summary>
        private IEnumerable<string> CheckCircularGetDependency(string name, Formula formula)
        {
            // preserve state just in case
            IEnumerable<string> oldDependencies = dependencies.GetDependents(name);

            // update the dependency graph
            dependencies.ReplaceDependents(name, formula.GetVariables());

            // this should leave the unhandled exception that GetCellsToRecalculate throws
            // yet still return the state of the dependency graph
            try
            {
                return GetCellsToRecalculate(name);
            }
            catch (Exception)
            {
                dependencies.ReplaceDependents(name, oldDependencies);
                throw;
            }
        }

        /// <summary>
        /// Removes named cell's dependents from graph, then removes it from cells per
        /// invariant
        /// </summary>
        private void EmptyCell(string name)
        {
            dependencies.ReplaceDependents(name, new List<string>());

            if (cells.ContainsKey(name))
            {
                cells.Remove(name);
            }

        }

        /// <summary>
        /// Takes a cell name and cell and adds it to the Dictionary. 
        /// If the cell is replacing a cell which had a formula in it, 
        /// removes cells dependents.
        /// </summary>
        private void AddCellToDictionary(string name, Cell cell)
        {
            if (cells.ContainsKey(name))
            {
                if (cells[name].Type == Cell.CellType.formulaType)
                {
                    dependencies.ReplaceDependents(name, new List<string>());
                }
                cells[name] = cell;
            }
            else
            {
                cells.Add(name, cell);
            }
        }

        /// <summary>
        /// A lookup function for evaluating functions contained in this spreadsheet. 
        /// If the value of variable can mapped to a double, returns that double
        /// else throws an argument exception. 
        /// </summary>
        private double LookupCellValue(string variable)
        {
            try
            {
                object value = GetCellValue(variable);
                if (value.GetType() == typeof(double))
                {
                    return (double)value;
                }
                else
                {
                    throw new ArgumentException("Could not lookup the value of variable " + variable);
                }
            }
            catch
            {
                throw new ArgumentException("Could not lookup the value of variable " + variable);
            }
        }

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
        /// Cells are contents are immutable. In order to change the contents of a cell, a new 
        /// cell must be created.
        /// </summary>
        private class Cell
        {
            // an enum which defines the cell type. Used to speed up type casting of returned contents and values of
            // the cell. Should only have to be set once. 
            public enum CellType { stringType, doubleType, formulaType }

            // the cell's contents as opposed to its value
            private object cellContents;

            private object cellValue;

            // the type of this Cell, Attribute is access only after cell created
            public CellType Type { get; private set; }

            /// <summary>
            /// Constructor for the Cell class. Will set its contents and type attribute. 
            /// </summary>
            public Cell(string contents)
            {
                Type = CellType.stringType;

                cellContents = contents;
                cellValue = contents;
            }

            /// <summary>
            /// Constructor for the Cell class. Will set its contents and type attribute. 
            /// </summary>
            public Cell(double contents)
            {
                Type = CellType.doubleType;

                cellContents = contents;
                cellValue = contents;
            }

            /// <summary>
            /// Constructor for the Cell class. Will set its contents and type attribute. 
            /// </summary>
            public Cell(Formula contents, Func<string, double> lookup)
            {
                Type = CellType.formulaType;

                cellContents = contents;
                cellValue = contents.Evaluate(lookup);
            }

            /// <summary>
            /// Gets the contents of this cell. The contents can be a string, double or a formula.
            /// </summary>
            public object GetCellContents()
            {
                // may not have to do casting on this return will check in tests and create a branch that does not cast
                switch (Type)
                {
                    case CellType.doubleType:
                        return (double)cellContents;
                    case CellType.stringType:
                        return (string)cellContents;
                    default:
                        return (Formula)cellContents;
                }
            }

            public object GetCellValue()
            {
                switch (Type)
                {
                    case CellType.doubleType:
                        return (double)cellValue;
                    case CellType.stringType:
                        return (string)cellValue;
                    default:
                        // could possibly be an error, must check 
                        if (cellValue.GetType() == typeof(FormulaError))
                        {
                            return (FormulaError)cellValue;
                        }
                        else
                        {
                            return (double)cellValue;
                        }
                }
            }

            /// <summary>
            /// Recalculates the cell's value and returns it.
            /// </summary>
            public object RecalculateCellValue(Func<string, double> lookup)
            {
                if (Type == CellType.formulaType)
                {
                    Formula f = (Formula)cellContents;
                    cellValue = f.Evaluate(lookup);
                }

                return GetCellValue();
            }


        }

    }

    //------------------------------Extension Methods----------------------------------//

    internal static class ExtensionMethods
    {
        /// <summary>
        /// Takes a char c, and determins if it is the first character in this string.
        /// </summary>
        public static bool BeginsWithChar(this string s, char c)
        {
            // make sure this string instance is not null and is not empty
            if (s != null && s.Length > 0)
            {
                return (s[0] == c);
            }
            else
            {
                return false;
            }
        }
    }
}