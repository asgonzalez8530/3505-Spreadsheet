/// <summary>
/// This program is a GUI of the client spreadsheet for a multi-client Spreadsheet Server.
/// 
/// 3505 version of ClientGUI by Rebekah Peterson, Jacqulyn Machardy, Anastasia Gonzalez, Michael Raleigh
/// 3500 version of SpreadsheetGUI by Anastasia Gonzalez, Aaron Bellis
/// </summary>

Last Updated: Spring 2018

Shortcuts for Close (CTR+Q), Revert (CTR+R), and Undo (CTR+Z)
Controller updated to connect to a server, send, and receive protocol messages appropriately.
SpreadsheetPanel updated for other client's focus messages.
Arrow keys change current selection.
Editable TextBox moves around with current selection -- editting a cell in a cell
Formulas can now evaluate to FormatError, so the Formula.dll and Spreadsheet.dll have been updated.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////

Name: Anastasia Gonzalez and Aaron Bellis
UID: u0985898 & u0981638
Last Update: 10/27/17

Initial Design Ideas: We will need three text boxes. Two of the text boxes will be non-editable (cell name and value). 
One of the text boxes will be editable (contents). All three text boxes make up the cell properties. There will be a menu that 
offers the options to save, open, close, or begin a new spreadsheet. The open and close features will check to see if there are 
any unsaved changes. If yes, then the GUI will prompt the user to save before moving onto the next action. We will design the 
GUI so that the menu is on top, then the cell properties, then the spreadsheet panel. All three containers will resize accordingly 
when the GUI panel changes sizes. 

PS2 Version: 1.0
PS3 Version: 1.0
PS4 Version: 1.0

Notes for the TAs: Our project was setup in an the proper MVC format. We used the proper abstraction so that the different classes
could speak to each other and work together to create a fuctional spreadsheet GUI. 

How to Run: Press the start button on the top of the screen

