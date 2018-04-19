﻿using SS;
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
using NetworkController;
using System.Net.Sockets;

// TODO: update SpreadsheetPanel.cs and dll for focus messages from other clients
// TODO: send and receive focus messages
// TODO: all messages
// TODO: disable buttons while trying to connect and choose a spreadsheet?

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controller for the Spreadsheet GUI. Contains reference to view and model
    /// </summary>
    class Controller
    {
        private SS.Spreadsheet sheet; // reference to the model 
        private ISpreadsheetWindow window; // reference to the GUI (view)
        private Socket theServer; // reference to Networking

        // TODO: thoughts on this? it's intended to help pick a new sheet after a file_load_error
        private string[] sheetChoicesForUser;

        char THREE = (char)3;

        /// <summary>
        /// Creates a new controller which controls an ISpreadsheetWindow and contains a reference to 
        /// a spreadsheet model.
        /// </summary>
        public Controller(ISpreadsheetWindow spreadsheetWindow)
        {
            // get the reference of the GUI
            window = spreadsheetWindow;

            // set the window name at the top of the form 
            window.WindowText = "YOU NEED TO CONNECT TO A SPREADSHEET STILL";

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

            //TODO: we do probably want to have a Closing action, just not this one!
            //window.AddFormClosingAction(ModifiedSpreadsheetDialogueBox);
            window.AboutText += OpenAbout;
            window.HowToUseText += OpenHowToUse;
            // added for 3505
            window.Startup += IPInputBox;
            window.Undo += UndoLastChange;
            window.Revert += RevertCell;

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
            else 
            {
                window.ValueBoxText = "CellError";
            }
            
        }

        /// <summary>
        /// Gets the contents of a given cell from the model and places it in the current cell contents box
        /// for updating
        /// </summary>
        private void SetCellContentsBox(SpreadsheetPanel panel)
        {
            // set the location of this textbox
            int x, y, width, height;
            panel.GetSelectionLocation(out x, out y, out width, out height);

            window.UpdateEditBoxLocation(x, y, width, height);

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

            // reset the contents of the cell and recalculate dependent cells
            ISet<string> cellsToUpdate = sheet.SetContentsOfCell(cellName, window.ContentsBoxText);
            SetSpreadsheetPanelValues(cellsToUpdate);
            UpdateCurrentCellBoxes();

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

            window.SetFocusToContentBox();
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
            else
            {
                window.SetCellText(row, col, "CellError");
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
            // TODO: take this method out
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
            // TODO: take this out
            if (sheet.Changed)
            {
                //prompt to save
                string message = "Unsaved changes detected in current spreadsheet: " + window.WindowText;
                message += "\n\nSave changes?";
                string caption = "Save Changes?";
                bool save = window.ShowOkayCancelMessageBox(message, caption);

                //// if user clicks on save then save the changes
                //if (save)
                //{
                //    Save();
                //}
            }
        }

        /*************************************Connecting to Server and NETWORKING*****************************************/


        /// <summary>
        /// Shows a dialog box for getting the IP address of the spreadsheet server, and connecting appropriately.
        /// </summary>
        private void IPInputBox()
        {
            ////////TODO: remove this hacky test for ChooseSpreadsheetBox//////////
            //string[] stringarr = { "one", "two", "three" };

            //string result = ChooseSpreadsheetBox(stringarr);

            //window.WindowText = result;
            //return;
            //////////////////////////////////////////////////////////////////
            Form2 getIP = new Form2();

            if (getIP.ShowDialog() == DialogResult.OK)
            {
                string ipAddress = getIP.ipTextBox.Text;
                
                try
                {
                    theServer = Networking.ConnectToServer(FirstContact, ipAddress);
                }
                catch (Exception)
                {
                    MessageBox.Show("There was a connection error, please try again.", "Error");
                }
            }
            
            getIP.Dispose();
        }

        /// <summary>
        /// Called Asynchronously by the Networking Library.
        /// Send "register \3" message according to protocol.
        /// </summary>
        /// <param name="state"></param>
        private void FirstContact(SocketState state)
        {
            if (theServer.Connected && !state.hasError)
            {
                state.callMe = ReceiveStartup;
                Networking.Send(state.theSocket, "register " + THREE);
                Networking.GetData(state);
            }
            else
            {
                MessageBox.Show("There was a connection error, please try again.", "Error");
            }
        }

        /// <summary>
        /// Called Asynchronously by the Networking library.
        /// Retrieves list of available spreadsheets and sends load message.
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveStartup(SocketState state)
        {
            // state error or sbuilder null
            if (state.hasError)
            {
                MessageBox.Show("There was a connection error, please try again.", "Error");
                // TODO: do we need to do any disconnecting cleanup if we get to this point and have an error?
            }
            if (state.sBuilder == null)
            {
                return;
            }

            // get info from sbuilder
            string fromServer = state.sBuilder.ToString();

            if (!fromServer.Contains(THREE))
            {
                return;
            }

            // "connect_accepted spreadsheet\nother\ncrazy\3"
            // parse and use info for combo box
            string connect_accepted = "connect_accepted ";
            string[] parsed = fromServer.Split(THREE);
            string connectAcceptMessage = parsed[0];
            string sheets = "";

            if (!connectAcceptMessage.StartsWith(connect_accepted))
            {
                return; // TODO: Server sent a bogus message.
            }
            else
            {
                sheets = connectAcceptMessage.Remove(0, connect_accepted.Length);
            }
            sheetChoicesForUser = sheets.Split('\n');

            // in combo box dialog, choose a spreadsheet
            string spreadsheet = ChooseSpreadsheetBox(sheetChoicesForUser);
            
            window.WindowText = spreadsheet;
            Networking.Send(theServer, "load " + spreadsheet + THREE);

            // TODO: could this clear too much?
            // clear sbuilder
            state.sBuilder.Clear();

            state.callMe = ProcessMessage;
            Networking.GetData(state);
        }

        /// <summary>
        /// Parse data and update this Spreadsheet.
        /// </summary>
        /// <param name="state"></param>
        private void ProcessMessage(SocketState state)
        {
            // TODO: test up to this point? Make a fake server?

            if (theServer.Connected && !state.hasError)
            {
                // grab the incoming message(s)
                string totalData = state.sBuilder.ToString();

                // Messages are separated by THREE
                string[] parts = Regex.Split(totalData, @"(?<=[\n])"); //TODO: figure out Regex for THREE

                foreach (string message in parts)
                {
                    ProcessNext(message);
                    state.sBuilder.Remove(0, message.Length);
                }

                // now ask for more data. this will start an event loop.
                Networking.GetData(state);
            }
        }

        /// <summary>
        /// A helper to decide which action to take, based on the message contents.
        /// </summary>
        /// <param name="message">example: focus A9:unique_1\3</param>
        private void ProcessNext(string message)
        {
            // Ignore empty strings added by the regex splitter
            if (message.Length == 0)
                return;

            // The regex splitter will include the last string even if it doesn't end with THREE,
            // So we need to ignore it if this happens. 
            if (message[message.Length - 1] != THREE)
                return;

            // Find the first space and switch on the command found
            string command = message.Substring(0, message.IndexOf(" "));
            string contents = message.Substring(message.IndexOf(" "), message.Length - 1);

            switch (command)
            {
                // Error loading the file, prompt user to select a file again
                case "file_load_error":
                    // Show an error dialog
                    // in combo box dialog, choose another spreadsheet
                    string spreadsheet = ChooseSpreadsheetBox(sheetChoicesForUser);

                    window.WindowText = spreadsheet;
                    Networking.Send(theServer, "load " + spreadsheet + THREE); // TODO: is it ok to send here?
                    break;

                // Disconnect, ending the session
                case "disconnect":
                    // TODO: how do we want to do this? could we just set the hasError flag?
                    break;

                // Reply with ping_response
                case "ping":
                    Networking.Send(theServer, "ping_response ");
                    break;

                // A ping response arrived, so reset our ping timer
                case "ping_response":
                    break;

                // Load the new sheet
                case "full_state":
                    LoadSheet(contents);
                    break;

                // Apply the Change
                case "change":
                    UpdateSheet(contents);
                    break;

                case "focus":
                    // TODO: contents example: A9:unique_1
                    break;

                case "unfocus":
                    // TODO: contents example: unique_1
                    break;
            }



            throw new NotImplementedException();
        }

        /// <summary>
        ///  Attempt to load the spreadsheet.
        /// </summary>
        /// <param name="contents">The spreadsheet as newline-separated 
        /// cell names and contents. A6:3\nA9:=A6/2\n\</param>
        private void LoadSheet(string contents)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Apply "change" to the sheet.
        /// </summary>
        /// <param name="change">example: A4:=A1+A3\</param>
        private void UpdateSheet(string change)
        {
            // TODO: lock spreadsheet model before changing
            // TODO: add lock in local change mechanism as well, if that's a thing
            throw new NotImplementedException();
        }

        /// <summary>
        /// Displays a Dialog box for choosing/ creating a new spreadsheet to connect to.
        /// </summary>
        /// <param name="sheetChoices"></param>
        /// <returns></returns>
        private string ChooseSpreadsheetBox(string[] sheetChoices)
        {
            Form3 comboForm = new Form3(sheetChoices);
            string spreadsheet = "";

            if (comboForm.ShowDialog() == DialogResult.OK)
            {
                spreadsheet = comboForm.comboBox.Text;
                spreadsheet = CleanFileName(spreadsheet);
            }
            else
            {
                spreadsheet = ChooseSpreadsheetBox(sheetChoices);
            }

            comboForm.Dispose();

            return spreadsheet;
        }

        private string CleanFileName(string spreadsheet)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This is registered and called when an Undo event happens.
        /// </summary>
        private void UndoLastChange()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This is registered and called when a Revert event happens.
        /// </summary>
        private void RevertCell()
        {
            throw new NotImplementedException();
        }
    }
}
