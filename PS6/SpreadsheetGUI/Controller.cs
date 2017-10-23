using SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controller for the SpreadsheetGui Contains reference to view and model
    /// </summary>
    class Controller
    {
        private SS.Spreadsheet sheet;

        private ISpreadsheetWindow window;

        public Controller(ISpreadsheetWindow spreadsheetWindow)
        {
            // get the reference of the gui
            window = spreadsheetWindow;
            

            // create a new model
            string version = "PS6";
            sheet = new SS.Spreadsheet(CellValidator, CellNormalizer, version);

            // register methods with events
            SpreadsheetPanel panel = window.GetSpreadsheetPanel();
            panel.SelectionChanged += DisplayCurrentCellName;
            window.NewSheetAction += OpenNewSheet;

            // set defaults location
            panel.SetSelection(0,0);
            DisplayCurrentCellName(panel);
        }

        




        //******************************** Private Methods **************************//

        /// <summary>
        /// Default Cell Validator for the spreadsheet GUI. Takes in a cellName and 
        /// returns True if the cell name is within range column a-z and cell row 1-99 
        /// </summary>
        private bool CellValidator(string cellName)
        {
            string validCell = @"^[a-zA-Z][1-9]\d?$";
            return Regex.IsMatch(cellName, validCell);
        }

        /// <summary>
        /// Default normalizer for spreadsheet gui. Takes in a cell name and returns 
        /// cellname.ToUpper()
        /// 
        /// Method kept seperate to clean up constructor call. 
        /// </summary>
        private string CellNormalizer(string cellName)
        {
            return cellName.ToUpper();
        }

        /// <summary>
        /// Gets the currently selected cell's zero indexed row and column and sets
        /// the CurrentCell_Text to the normalized cell name. 
        /// </summary>
        /// <param name="ss"></param>
        private void DisplayCurrentCellName(SpreadsheetPanel ss)
        {
            int row, col;
            ss.GetSelection(out col, out row);
            window.CurrentCellText = ConvertCellName(row, col);

        }

        /// <summary>
        /// Takes in the zero indexed row and col and converts it to the string
        /// representation of a cell name
        /// </summary>
        private string ConvertCellName(int row, int col)
        {
            int rowName = row + 1;
            char colName = (char)col;
            colName += 'A';

            return "" + colName + "" + rowName;
        }

        /// <summary>
        /// creates a new sheet
        /// </summary>
        private void OpenNewSheet()
        {
            window.CreateNew();
        }
    }
}
