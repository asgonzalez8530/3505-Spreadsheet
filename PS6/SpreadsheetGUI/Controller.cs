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
using SpreadsheetUtilities;


namespace SpreadsheetGUI
{
    /// <summary>
    /// Controller for the Spreadsheet GUI. Contains reference to view and model
    /// </summary>
    class Controller
    {
        private SS.Spreadsheet sheet; // reference to the model 
        private ISpreadsheetWindow window; // reference to the GUI (view)

        /// <summary>
        /// Creates a new controller which controls an ISpreadsheetWindow and contains a reference to 
        /// a spreadsheet model.
        /// </summary>
        public Controller(ISpreadsheetWindow spreadsheetWindow)
        {
            // get the reference of the GUI
            window = spreadsheetWindow;

            // set the window name at the top of the form 
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
            //TODO: we do probably want to have a Closing action, just not this one!
            //window.AddFormClosingAction(ModifiedSpreadsheetDialogueBox);
            window.AboutText += OpenAbout;
            window.HowToUseText += OpenHowToUse;

            // set default locations
            panel.SetSelection(0, 0);
            UpdateCurrentCellBoxes();
        }


        //******************************** Private Methods **************************//

        /// <summary>
        /// Default Cell Validator for the spreadsheet GUI. Takes in a cellName and 
        /// returns True if the cell name is within the range of column a-z and row 1-99 
        /// </summary>
        private bool CellValidator(string cellName)
        {
            string validCell = @"^[a-zA-Z][1-9]\d?$";
            return Regex.IsMatch(cellName, validCell);
        }

        /// <summary>
        /// Default normalizer for spreadsheet GUI. Takes in a cell name and returns 
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
        /// the CurrentCellText to the normalized cell name. 
        /// </summary>
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
        /// Replaces the current value text box to the looked-up value of the cell
        /// </summary>
        private void SetCellValueBox(SpreadsheetPanel panel)
        {
            // locates the current cell in the grid and converts it to a variable
            panel.GetSelection(out int col, out int row);
            string cellName = ConvertRowColToCellName(row, col);

            // set the "value" object to the value of the variable
            object value = sheet.GetCellValue(cellName);

            // if value is a string or double then convert the object to a string
            if (value is string || value is double)
            {
                window.ValueBoxText = value.ToString();
            }
            // else text box value will be set to Error
            else if (value is FormulaError)
            {
                window.ValueBoxText = "FormulaError";
            }
            else
            {
                window.ValueBoxText = "FormatError";
            }
        }

        /// <summary>
        /// Gets the contents of a given cell from the model and places it in the current cell contents box
        /// for updating
        /// </summary>
        private void SetCellContentsBox(SpreadsheetPanel panel)
        {
            // locate the current cell in the grid and convert to a variable
            panel.GetSelection(out int col, out int row);
            string cellName = ConvertRowColToCellName(row, col);

            // set the contents text to the current contents of the cell
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
                // reset the contents of the cell and recalculate dependent cells
                ISet<string> cellsToUpdate = sheet.SetContentsOfCell(cellName, window.ContentsBoxText);
                SetSpreadsheetPanelValues(cellsToUpdate);
                UpdateCurrentCellBoxes();

            }
            catch (CircularException)
            {
                window.ShowErrorMessageBox("Circular dependency detected");
            }
            catch (Exception e) // TODO take this out
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
        /// text for those cells to that value
        /// </summary>
        private void SetSpreadsheetPanelValues(ISet<string> cellsToUpdate)
        {
            foreach (string cell in cellsToUpdate)
            {
                SetSpreadsheetPanelValue(cell);
            }
        }

        /// <summary>
        /// takes in a cell name, looks up its value and sets the value to the corresponding cell in the view
        /// </summary>
        private void SetSpreadsheetPanelValue(string cell)
        {
            object value = sheet.GetCellValue(cell);
            ConvertCellNameToRowCol(cell, out int row, out int col);

            // if value is a string or double then convert the object to a string
            if (value is string || value is double)
            {
                window.SetCellText(row, col, value.ToString());
            }
            // else text box value will be set to Error
            else if (value is FormulaError)
            {
                window.SetCellText(row, col, "FormulaError");
            }
            else
            {
                window.SetCellText(row, col, "FormatError");
            }


        }

        /// <summary>
        /// Opens a save file dialogue and saves the model to a file
        /// </summary>
        private void Save()
        {
            try
            {
                // open file explorer
                SaveFileDialog saveFile = new SaveFileDialog
                {
                    Filter = "Spreadsheet File (*.sprd)|*.sprd|All files (*.*)|*.*",
                    Title = "Save " + window.WindowText,
                    OverwritePrompt = true,
                    FileName = window.WindowText

                };

                // when ok is pressed and the file name is not empty
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    if (saveFile.FileName != "")
                    {
                        // get the file name and the file's absolute path
                        saveFile.FileName = Path.GetFullPath(saveFile.FileName);
                        window.WindowText = Path.GetFileName(saveFile.FileName);

                        // save file using the absolute path
                        sheet.Save(saveFile.FileName);
                    }
                }
            }
            catch (SpreadsheetReadWriteException)
            {
                window.ShowErrorMessageBox("Problem occurred while saving the file");
            }
        }

        /// <summary>
        /// Opens a file dialogue box and opens the chosen file in this window. 
        /// If information will be changed, prompts user to save.
        /// </summary>
        private void Open()
        {
            // if there exists any unsaved changes then promt user to save
            ModifiedSpreadsheetDialogueBox();

            try
            {
                // open file explorer
                OpenFileDialog openFile = new OpenFileDialog
                {
                    Filter = "Spreadsheet File (*.sprd)|*.sprd|All files (*.*)|*.*",
                    Title = "Open Spreadsheet",
                    RestoreDirectory = true
                };

                // when ok is pressed and a file is selected
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (openFile.FileName != "")
                    {
                        // get filename and files absolute path
                        openFile.FileName = Path.GetFullPath(openFile.FileName);
                        window.WindowText = Path.GetFileName(openFile.FileName);
                        
                        // open new spreadsheet
                        OpenSpreadsheetFromFile(openFile.FileName);


                    }
                }
            }
            catch (SpreadsheetReadWriteException)
            {
                window.ShowErrorMessageBox("Problem occured while opening file");
            }
        }

        /// <summary>
        /// Helper method for OpenSpreadsheetFromFile. 
        /// Empties the contents of the spreadsheet pane.
        /// </summary>
        /// <param name="cellsToEmpty"></param>
        private void EmptyAllCells(ISet<string> cellsToEmpty)
        {
            foreach (string cell in cellsToEmpty)
            {
                ConvertCellNameToRowCol(cell, out int row, out int col);
                window.SetCellText(row, col, "");
            }
        }

        /// <summary>
        /// Empties the spreadsheet pane and sets its contents to the new spreadsheet model at fileLocation
        /// </summary>
        private void OpenSpreadsheetFromFile(string fileLocation)
        {
            // empty the current spreadsheetpane
            EmptyAllCells(new HashSet<string>(sheet.GetNamesOfAllNonemptyCells()));

            // window text is now the name of the new file
            window.WindowText = Path.GetFileName(fileLocation);

            // open the spreadsheet
            sheet = new SS.Spreadsheet(fileLocation, CellValidator, CellNormalizer, "ps6");

            // set the contents of the spreadsheet pane
            HashSet<string> nonEmpty = new HashSet<string>(sheet.GetNamesOfAllNonemptyCells());
            SetSpreadsheetPanelValues(nonEmpty);

            // update the current window selection
            window.SetCellSelectionToDefault();
            UpdateCurrentCellBoxes();

        }

        /// <summary>
        /// Opens the about file in the default text editor.
        /// </summary>
        private void OpenAbout()
        {
            string fileName = @"..\..\..\Resources\About.txt";
            string path = Path.Combine(Environment.CurrentDirectory, fileName);
            Process.Start(path);
        }

        /// <summary>
        /// Opens the about file in the default text editor.
        /// </summary>
        private void OpenHowToUse()
        {
            string fileName = @"..\..\..\Resources\HowToUse.txt";
            string path = Path.Combine(Environment.CurrentDirectory, fileName);
            Process.Start(path);
        }

        /// <summary>
        /// Dialogue box that prompts the user to save current spreadsheet
        /// </summary>
        private void ModifiedSpreadsheetDialogueBox()
        {
            if (sheet.Changed)
            {
                //prompt to save
                string message = "Unsaved changes detected in current spreadsheet: " + window.WindowText;
                message += "\n\nSave changes?";
                string caption = "Save Changes?";
                bool save = window.ShowOkayCancelMessageBox(message, caption);

                // if user clicks on save then save the changes
                if (save)
                {
                    Save();
                }
            }
        }
    }
}
