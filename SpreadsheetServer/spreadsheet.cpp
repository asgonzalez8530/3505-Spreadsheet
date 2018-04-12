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

#include "spreadsheet.h"
#include <string>
#include <map>
#include <stack>

namespace cs3505
{
//private:

    // TODO decided not to use map of cell names to cell contents,
    // to send full state, just popping top cell change off of each
    // cell's stack

    /**
     * "sheet" is a map of cell names (strings) to a stack of changes
     * for that cell (stack<string>)
     *
    std::map<std::string, std::stack<std::string>> sheet;

    /** 
     * stack of edits (used for undo)
     *
     * when a cell change comes in from the Server:
     *	1) peek the cell's stack (if empty, use empty string)
     *	2) push that value and the cell name onto the stack of edits
     *  3) send Server the Change data (from step 2)
     *
     * when a cell revert comes in from the Server:
     *	1) pop the cell's stack (if empty, use empty string)
     *	2) push that value and the cell name onto the stack of edits
     *  3) peek the cell's stack (if empty, use empty string)
     *  4) send Server the Change data (from step 3)
     *
     * when a cell undo comes in from the Server:
     *	1) pop the cell's stack (if empty, use empty string)
     *	2) pop the stack of edits
     *  3) peek the cell's stack (if empty, use empty string)
     *  4) send Server the Change data (from step 3)
     *
    std::stack<string> edits // example value: "A36:I <3 dogs"
	*/

    /*
     * Change cellName's contents to cellContents. Counts as an edit.
     */
    std::string spreadsheet::edit(std::string & cellName, std::string & cellContents){}

    /*
     * Revert cellName's contents. Counts as an edit.
     */
    std::string spreadsheet::revert(std::string & cellName){}

    /*
     * Undo the last edit made to this spreadsheet. Not an edit.
     */
    std::string spreadsheet::undo(){}

    /*
     * Write the current state of the spreadsheet to file.
     */
    void spreadsheet::save(){}

//public: 

    /*
     * Construct a spreadsheet from the given file.
     */
    spreadsheet::spreadsheet(std::string fileName){}

    /*
     * Construct an empty spreadsheet.
     */
    spreadsheet::spreadsheet(){}

    /*
     * Driver method for updating this spreadsheet.
     *
     * Takes a Edit, Revert, or Undo message (see page 5 of protocol for examples) 
     * as an argument.
     *
     * Returns a Change message.
     *
     */
    std::string spreadsheet::update(std::string update)
    {
	try
	{
	    if(update.find("edit ") == 0)
	    {
		int colon = update.find(":"); // useful index to know
		std::string name = update.substr(5, colon - 5); // TODO check for fencepost errors
		std::string value = update.substr(colon + 1, update.length() - colon - 1);
		
		edit(name, value);
	    }

	    else if(update.find("revert ") == 0)
	    {
		std::string name = update.substr(7, update.length() - 1);

		revert(name);
	    }
	    
	    else if(update.find("undo ") == 0)
	    {
		undo();
	    }

	    else return NULL; // we didn't find a message that we know how to process :(
	}

	catch(const std::out_of_range& outOfRange)
	{
	    return NULL; // the message was not a protocol match
	}
    }

    /*
     * Process a Full_State message by returning a pair of iterators.
     *
     * Example of use in another class:
     *	pair<iterator, iterator> rators = mySheet.full_state()
     *  for (; rators.first != rators.second; rators.first++)
     * 	{
     *		string cellName = rators.first->first;
     *		string cellContents = rators.first->second.peek();
     *		// send this info to client	
     * 	}
     *
     */
    std::pair<std::map<std::string, std::stack<std::string> >::iterator,
	      std::map<std::string, std::stack<std::string> >::iterator> spreadsheet::full_state()
    {
	return std::make_pair(sheet.begin(), sheet.end());
    }

    /*
     * Destroy this spreadsheet.
     */
    spreadsheet::~spreadsheet(){}
}

