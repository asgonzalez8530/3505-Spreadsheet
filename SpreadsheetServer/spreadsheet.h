/**
 * This class represents the SpreadSheet model for the Server.
 * The responsibilities of this class are:
 *  1) represent the current state of this spreadsheet  
 *  2) receive and apply cell updates from the Server
 *  3) write the current state of this spreadsheet to file
 *  4) read spreadsheet data from a file
 *
 *
 * Executive decision, storing all cell contents as strings (including doubles).
 *
 * Pineapple upside down cake
 * v1: April 11, 2018
 */

#ifndef SPREADSHEET_H
#define SPREADSHEET_H

#include <string>
#include <map>
#include <stack>
#include <stdexcept>

namespace cs3505
{
    class spreadsheet
    {
	private:
	    // TODO decided not to use map of cell names to cell contents,
	    // to send full state, just popping top cell change off of each
	    // cell's stack

		/*
		 * The name of this spreadsheet. Used for saving and identification
		 */	
		std::string myName;

	    /**
	     * "sheet" is a map of cell names (strings) to a stack of changes
	     * for that cell (stack<string>)
	     */
	    std::map<std::string, std::stack<std::string> > sheet;


	    /** 
	     * stack of edits (used for undo)
	     */
	    std::stack<std::string> edits; // example value: "A36:I <3 dogs"


	    /*
	     * Change cellName's contents to cellContents. Counts as an edit.
	     */
	    std::string edit(std::string cellName, std::string cellContents);

	    /*
	     * Revert cellName's contents. Counts as an edit.
	     */
	    std::string revert(std::string cellName);

	    /*
	     * Undo the last edit made to this spreadsheet. Not an edit.
	     */
	    std::string undo();


	public: 


	    /*
	     * Write the current state of the spreadsheet to file.
	     */
	    void save();

	    /*
	     * Construct a spreadsheet from the given file.
	     */
	    spreadsheet(std::string fileName);

	    /*
	     * Construct an empty spreadsheet.
	     */
	    spreadsheet();

	    /*
	     * Driver method for updating this spreadsheet.
	     *
	     * Takes a Edit, Revert, or Undo message (see page 5 of protocol for examples) 
	     * as an argument.
	     *
	     * Returns a Change message
	     */
	    std::string update(std::string);

	    /*
	     * Process a Full_State message by returning this spreadsheet
	     * as a map
	     */
	    std::map<std::string, std::string> full_state();

	    /*
	     * Destroy this spreadsheet.
	     */
	    ~spreadsheet();
    };
}
#endif

