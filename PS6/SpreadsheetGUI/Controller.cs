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

        protected Controller(ISpreadsheetWindow spreadsheetWindow)
        {
            // get the reference of the gui
            window = spreadsheetWindow;

            // create a new model
            string version = "PS6";
            sheet = new SS.Spreadsheet(CellValidator, CellNormalizer, version);
        }



        //******************************** Private Methods *********************//

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

    }
}
