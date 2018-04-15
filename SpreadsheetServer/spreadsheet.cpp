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
 std::stack<string> edits // example value: "A36:I <3 dogs"
 */
 
/*
 * Change cellName's contents to cellContents. Counts as an edit.
 */
std::string spreadsheet::edit(std::string &cellName, std::string &cellContents)
{	
	// TODO checking if it's empty, but may actually need to check if it's NULL too

    // peek the cell's stack, preserve the old value 
    if (sheet[cellName].empty()) 
    {
        edits.push(cellName + ":");
    }
    else
    {
        std::string oldContents = sheet[cellName].top();
        edits.push(cellName + ":" + oldContents); // push the old value onto edits
    }

    sheet[cellName].push(cellContents); // update sheet TODO NULL check here?

    return "change " + cellName + ":" + cellContents; // return the change message
}

/*
 * Revert cellName's contents. Counts as an edit.
 */
std::string spreadsheet::revert(std::string &cellName)
{
	// TODO checking if it's empty, but may actually need to check if it's NULL too

    if (sheet[cellName].empty()) return NULL; // nothing to revert, return null

    else
    {
        std::string oldContents = sheet[cellName].top(); // grab the old value
		sheet[cellName].pop(); // revert the value by popping
        edits.push(cellName + ":" + oldContents); // store the old value onto edits
    }

	std::string cellContents = sheet[cellName].empty() ? "" : sheet[cellName].top(); // grab the current contents		
	
	return "change " + cellName + ":" + cellContents; // return the change message
}

/*
 * Undo the last edit made to this spreadsheet. Not an edit.
 */
std::string spreadsheet::undo()
{
	// TODO checking if it's empty, but may actually need to check if it's NULL too

	if(edits.empty()) return NULL; //nothing to undo

	else
	{
		std::string undo = edits.top(); // grab the last edit
		edits.pop(); // undo the last edit
		std::string cellName = undo.substr(0, undo.find(":") - 1); // grab the cell name
		sheet[cellName].pop(); // undo the last edit to this cell

		return "change " + undo; // return the change message
	}

	return "change A1:This should not be happening."; // should never reach this. . .
}

/*
 * Write the current state of the spreadsheet to file.
 */
void spreadsheet::save() 
{
	/*
	 * Save edits as csv
	 * Save map as xml? or csv?
	 * then, reading would go something like this:
	 *
	 * while !in.fail()
	 * grab cell name and number of elements, make entry in "sheet"
	 * // A1:2,
	 *
	 * loop over elements, adding them to the sheet entry's stack
	 * // "dogs",
	 * // "=1+1",
	 *
	 * resume while loop
	 */
}

//public:

/*
 * Construct a spreadsheet from the given file.
 */
spreadsheet::spreadsheet(std::string fileName) {}

/*
 * Construct an empty spreadsheet.
 */
spreadsheet::spreadsheet() {}

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
        if (update.find("edit ") == 0)
        {
            int colon = update.find(":");                   // useful index to know
            std::string name = update.substr(5, colon - 5); // TODO check for fencepost errors
            std::string value = update.substr(colon + 1, update.length() - colon - 1);

            return edit(name, value);
        }

        if (update.find("revert ") == 0)
        {
            std::string name = update.substr(7, update.length() - 1);

            return revert(name);
        }

        if (update.find("undo ") == 0)
        {
            return undo();
        }

        else
            return NULL; // we didn't find a message that we know how to process :(
    }

    catch (const std::out_of_range &outOfRange)
    {
        return NULL; // the message was not a protocol match
    }
}

/*
 * Process a Full_State message by returning a map.
 *
 */
std::map<std::string, std::string> spreadsheet::full_state()
{
    std::map<std::string, std::string> fullState;
    std::map<std::string, std::stack<std::string> >::iterator sheetRator;

    for (sheetRator = sheet.begin(); sheetRator != sheet.end(); sheetRator++)
    {
        std::string cellName = sheetRator->first;
        std::string cellContents = sheetRator->second.top();

        fullState.insert(std::pair<std::string, std::string>(cellName, cellContents));
    }
    return fullState;
}

/*
 * Destroy this spreadsheet.
 */
spreadsheet::~spreadsheet() {}
}