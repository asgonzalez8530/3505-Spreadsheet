using SS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            
            window.WindowText = "untitled.sprd";

            // create a new model
            string version = "ps6";
            sheet = new SS.Spreadsheet(CellValidator, CellNormalizer, version);

            // register methods with events
            SpreadsheetPanel panel = window.GetSpreadsheetPanel();
            panel.SelectionChanged += DisplayCurrentCellName;
            panel.SelectionChanged += SetCellValueBox;
            panel.SelectionChanged += SetCellContentsBox;
            
            window.NewSheetAction += OpenNewSheet;
            window.EnterContentsAction += SetCellContentsFromContentsBox;
            window.SetDefaultAcceptButton();

            window.SaveFileAction += Save;
            window.OpenFileAction += Open;

            // set defaults location
            panel.SetSelection(0, 0);
            UpdateCurrentCellBoxes();


        }

        /// <summary>
        /// Creates a new controller which references a Spreadsheet model which is created from the file
        /// represented by FileLocation.
        /// </summary>
        public Controller(ISpreadsheetWindow spreadsheetWindow, string FileLocation)
            : this(spreadsheetWindow)
        {
            try
            {
                sheet = new SS.Spreadsheet(FileLocation, CellValidator, CellNormalizer, "ps6");
            }
            catch (Exception e)
            {
                window.ShowErrorMessageBox(e.Message);
                window.CloseWindow();
            }

            SetSpreadsheetPanelValues(new HashSet<string>(sheet.GetNamesOfAllNonemptyCells()));
            UpdateCurrentCellBoxes();
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
            window.CurrentCellText = ConvertRowColToCellName(row, col);

        }

        /// <summary>
        /// Takes in the zero indexed row and col and converts it to the string
        /// representation of a cell name
        /// </summary>
        private string ConvertRowColToCellName(int row, int col)
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

        /// <summary>
        /// Takes in a valid cell name and replaces the out values with the equivalent zero indexed
        /// row and column values
        /// </summary>
        private void ConvertCellNameToRowCol(string cellName, out int row, out int col)
        {
            col = cellName[0] - 'A';

            int.TryParse(cellName.Substring(1), out row);
            row = row - 1;
            
        }

        /// <summary>
        /// Replaces the current value text box to the lookedup value of the cell
        /// </summary>
        private void SetCellValueBox(SpreadsheetPanel panel)
        {
            //located the current cell in the grid and convert to a variable
            panel.GetSelection(out int col, out int row);
            string cellName = ConvertRowColToCellName(row, col);

            //set the "value" object to the value of the variable
            object value = sheet.GetCellValue(cellName);

            //if value is a string or double then convert the object to a string
            if (value is string || value is double)
            {
                window.ValueBoxText = value.ToString();
            }
            //else text box value will be set to FormulaError
            else
            {
                window.ValueBoxText = "FormulaError";
            }
        }

        /// <summary>
        /// Gets the contents of a given cell from the model and places it in the current cell contents box
        /// for updating
        /// </summary>
        private void SetCellContentsBox(SpreadsheetPanel panel)
        {
            //locate the current cell in the grid and convert to a variable
            panel.GetSelection(out int col, out int row);
            string cellName = ConvertRowColToCellName(row, col);

            //set the contents text to the current contents of the cell
            object contents = sheet.GetCellContents(cellName);
            
            if (contents is string || contents is double)
            {
                window.ContentsBoxText = contents.ToString();
            }
            else
            {
                window.ContentsBoxText = "=" + contents.ToString();
            }
        }

        /// <summary>
        /// gets the current text in the current cell contents box, sets it to the model's cell contents and
        /// updates the views cell value. If an error occurs, creates a message box with the a message that 
        /// an error occured.
        /// </summary>
        private void SetCellContentsFromContentsBox()
        {

            // get the location of the currently selected cell
            window.GetCellSelection(out int row, out int col);

            // convert row and column to a cell name
            string cellName = ConvertRowColToCellName(row, col);

            try
            {

                ISet<string> cellsToUpdate = sheet.SetContentsOfCell(cellName, window.ContentsBoxText);
                SetSpreadsheetPanelValues(cellsToUpdate);
                UpdateCurrentCellBoxes();
                
            }
            catch (CircularException)
            {
                window.ShowErrorMessageBox("Circular dependency detected");
            }
            catch (Exception e)
            {
                window.ShowErrorMessageBox(e.Message);
            }

            window.SetFocusToContentBox();
        }

        /// <summary>
        /// Updates text boxes at the top of a spreadsheet window
        /// </summary>
        private void UpdateCurrentCellBoxes()
        {
            SpreadsheetPanel panel = window.GetSpreadsheetPanel();
            DisplayCurrentCellName(panel);
            SetCellValueBox(panel);
            SetCellContentsBox(panel);
        }

        /// <summary>
        /// takes a set of cell names, looks up their values then sets the SpreadsheetPanel 
        /// text for those cell to that value;
        /// </summary>
        private void SetSpreadsheetPanelValues(ISet<string> cellsToUpdate)
        {
            foreach (string cell in cellsToUpdate)
            {
                SetSpreadsheetPanelValue(cell);
            }
        }

        /// <summary>
        /// takes in a cell name, looks up its value and sets the value corresponding cell in the view
        /// </summary>
        private void SetSpreadsheetPanelValue(string cell)
        {
            object value = sheet.GetCellValue(cell);
            ConvertCellNameToRowCol(cell, out int row, out int col);

            //if value is a string or double then convert the object to a string
            if (value is string || value is double)
            {
                window.SetCellText(row, col, value.ToString());
            }
            //else text box value will be set to FormulaError
            else
            {
                window.SetCellText(row, col, "FormulaError");
            }

        }

        private void Save()
        {
            try
            {
                //open file explorer
                SaveFileDialog saveFile = new SaveFileDialog
                {
                    Filter = "Spreadsheet File (*.sprd)|*.sprd|All files (*.*)|*.*",
                    Title = "Save " + window.WindowText,
                    OverwritePrompt = true,
                    FileName = window.WindowText
                    
                };

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    if (saveFile.FileName != "")
                    {
                        saveFile.FileName = Path.GetFullPath(saveFile.FileName);
                        window.WindowText = Path.GetFileName(saveFile.FileName);

                        //extract and save filename
                        sheet.Save(saveFile.FileName);
                    }
                }
            }
            catch (SpreadsheetReadWriteException)
            {
                window.ShowErrorMessageBox("Problem occurred while saving the file");
            }
        }

        private void Open()
        {
            try
            {

                OpenFileDialog openFile = new OpenFileDialog
                {
                    Filter = "Spreadsheet File (*.sprd)|*.sprd|All files (*.*)|*.*",
                    Title = "Save Spreadsheet",
                    RestoreDirectory = true
                };


                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (openFile.FileName != "")
                    {
                        openFile.FileName = Path.GetFullPath(openFile.FileName);
                        window.WindowText = Path.GetFileName(openFile.FileName);

                        
                        //extract and save filename
                        sheet = new SS.Spreadsheet(openFile.FileName, CellValidator, CellNormalizer, "ps6");
                        

                        HashSet<string> nonEmpty = new HashSet<string>(sheet.GetNamesOfAllNonemptyCells());
                        SetSpreadsheetPanelValues(nonEmpty);
                        
                        UpdateCurrentCellBoxes();
                    }
                }
            }
            catch (SpreadsheetReadWriteException)
            {
                window.ShowErrorMessageBox("Problem occured while opening file");
            }
        }

        

        private void ModifiedSpreadsheetDiologueBox()
        {
            if (sheet.Changed)
            {
                //prompt to save
                string message = "Unsaved changes detected in current spreadsheet " + window.WindowText;
                message += "\nSave changes?";
                string caption = "Save Changes?";
                bool save = window.ShowOkayCancelMessageBox(message, caption);

                if (save)
                {
                    Save();
                }

            }

        }
    }
}
