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
using NetworkController;
using System.Net.Sockets;

// TODO: all messages from protocol
// TODO: arrow keys
// TODO: add locks on the sheet -- update sheet in the controller, view painting values in the panel (UpdateSpreadsheetPanelValues)

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


        /// <summary>
        /// Maps other client ID's to cellnames
        /// </summary>
        private Dictionary<string, string> otherClientsCurrentCells;
        private const char THREE = (char)3;
        private string[] sheetChoicesForUser;

        /// <summary>
        /// Creates a new controller which controls an ISpreadsheetWindow and contains a reference to 
        /// a spreadsheet model.
        /// </summary>
        public Controller(ISpreadsheetWindow spreadsheetWindow)
        {
            // get the reference of the GUI
            window = spreadsheetWindow;

            // set the window name at the top of the form 
            window.WindowText = "YOU ARE OFFLINE";

            // create a new model
            string version = "ps6";
            sheet = new SS.Spreadsheet(CellValidator, CellNormalizer, version);

            otherClientsCurrentCells = new Dictionary<string, string>();

            // register methods with events
            SpreadsheetPanel panel = window.GetSpreadsheetPanel();
            panel.SelectionChanged += DisplayCurrentCellName;
            panel.SelectionChanged += SetCellValueBox;
            panel.SelectionChanged += SetCellContentsBox;
            panel.SelectionChanged += SendFocusToServer;

            window.EnterContentsAction += SendEditToServer;
            window.SetDefaultAcceptButton();

            window.AddFormClosingAction(FormCloses);
            window.AboutText += OpenAbout;
            window.HowToUseText += OpenHowToUse;

            window.Startup += IPInputBox;
            window.Undo += SendUndoToServer;
            window.Revert += SendRevertToServer;

            // set default locations
            panel.SetSelection(0, 0);
            UpdateCurrentCellBoxes();
        }

        /// <summary>
        /// Send an unfocus and focus message to the server.
        /// </summary>
        /// <param name="sender"></param>
        private void SendFocusToServer(SpreadsheetPanel sender)
        {
            Networking.Send(theServer, "unfocus " + THREE);
            Networking.Send(theServer, "focus " + window.CurrentCellText + THREE);
        }

        private void FormCloses()
        {
            if (theServer != null)
            {
                Networking.Send(theServer, "disconnect " + THREE);
                theServer.Close();
            }
        }


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
        /// Sets the model's cell contents and updates the panel with the correct values.
        /// </summary>
        private void ReceiveChange(string contents)
        {
            // contents = "A1:=hello"
            string[] parsed = contents.Split(':');
            
            // convert row and column to a cell name
            string cellName = parsed[0];

            // reset the contents of the cell and recalculate dependent cells
            ISet<string> cellsToUpdate = sheet.SetContentsOfCell(cellName, parsed[1]);
            SetSpreadsheetPanelValues(cellsToUpdate);
        }

        /// <summary>
        /// Sends an "edit" message to the server.
        /// </summary>
        private void SendEditToServer()
        {
            string name = window.CurrentCellText;
            string contents = window.ContentsBoxText; 
            Networking.Send(theServer, "edit " + name + ":" + contents + THREE);

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
        

        /*************************************Connecting to Server and NETWORKING*****************************************/


        /// <summary>
        /// Shows a dialog box for getting the IP address of the spreadsheet server, and connecting appropriately.
        /// </summary>
        private void IPInputBox()
        {
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
                Networking.Send(theServer, "disconnect " + THREE);
                MessageBox.Show("There was a connection error, please try again.", "Error");

                return;
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

            if (theServer.Connected && !state.hasError)
            {
                // grab the incoming message(s)
                string totalData = state.sBuilder.ToString();

                // Messages are separated by THREE
                string[] parts = Regex.Split(totalData, @"(?<=[\3])");

                foreach (string message in parts)
                {
                    // Ignore empty strings added by the regex splitter
                    if (message.Length == 0)
                        continue;

                    // The regex splitter will include the last string even if it doesn't end with THREE,
                    // So we need to ignore it if this happens. 
                    if (message[message.Length - 1] != THREE)
                        break;

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
                    Networking.Send(theServer, "load " + spreadsheet + THREE);
                    break;

                // Disconnect, ending the session
                case "disconnect":
                    MessageBox.Show("There was a connection error, please try again.", "Error");
                    EmptyAllCells(new HashSet<string>(sheet.GetNamesOfAllNonemptyCells()));
                    theServer.Close();
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
                    ReceiveChange(contents);
                    break;

                case "focus":
                    // contents example: A9:unique_1d
                    string[] parsed = contents.Split(':');
                    
                    // keep track of other client's selected cell
                    if (!otherClientsCurrentCells.ContainsKey(parsed[1]))
                    {
                        otherClientsCurrentCells.Add(parsed[1], parsed[0]);
                    }
                    else
                    {
                        otherClientsCurrentCells[parsed[1]] = parsed[0];
                    }

                    // focus cell
                    FocusCell(parsed[0]);
                    break;

                case "unfocus":
                    // contents example: unique_1
                    // unfocus cell
                    UnfocusCell(otherClientsCurrentCells[contents]);
                    break;
            }



            throw new NotImplementedException();
        }

        /// <summary>
        /// Unfocus this cell in the SpreadsheetPanel in the view.
        /// </summary>
        /// <param name="cellName"></param>
        private void UnfocusCell(string cellName)
        {
            // TODO: oh wait, I can't just unfocus a cell... 
            // I could (easily) end up unfocusing something that is focused on by two users.. UGH
            SpreadsheetPanel panel = window.GetSpreadsheetPanel();
            ConvertCellNameToRowCol(cellName, out int row, out int col);
            panel.Focus(row, col);
        }

        /// <summary>
        /// Focus this cell in the SpreadsheetPanel in the view.
        /// </summary>
        /// <param name="cellName"></param>
        private void FocusCell(string cellName)
        {
            SpreadsheetPanel panel = window.GetSpreadsheetPanel();
            ConvertCellNameToRowCol(cellName, out int row, out int col);
            panel.Focus(row, col);
        }

        /// <summary>
        ///  Attempt to load the spreadsheet.
        /// </summary>
        /// <param name="contents">The spreadsheet as newline-separated 
        /// cell names and contents. A6:3\nA9:=A6/2\n\</param>
        private void LoadSheet(string contents)
        {
            // EmptyAllCells
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
            // this code is borrowed
            char[] invalids = System.IO.Path.GetInvalidFileNameChars();
            string newName = String.Join("_", spreadsheet.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
            return newName;
        }

        /// <summary>
        /// This is registered and called when an Undo event happens.
        /// </summary>
        private void SendUndoToServer()
        {
            Networking.Send(theServer, "undo " + THREE);
        }

        /// <summary>
        /// This is registered and called when a Revert event happens.
        /// </summary>
        private void SendRevertToServer()
        {
            string cellToRevert = window.CurrentCellText;
            Networking.Send(theServer, "revert " + cellToRevert + THREE);
        }
    }
}
