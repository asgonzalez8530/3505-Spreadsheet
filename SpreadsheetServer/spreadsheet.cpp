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


/*
 * Boost libraries used for reading and writing.
 */
#include <boost/archive/text_iarchive.hpp>
#include <boost/archive/text_oarchive.hpp>
#include <boost/serialization/map.hpp>
#include <boost/serialization/deque.hpp>
#include <boost/serialization/stack.hpp>
#include <boost/filesystem.hpp>
#include <boost/filesystem/fstream.hpp>

namespace cs3505
{

/*
 ************************** PRIVATE METHODS ***********************
 */
	/*
	 * Change cellName's contents to cellContents. Counts as an edit.
	 */
	std::string spreadsheet::edit(std::string cellName, std::string cellContents)
	{	
		sheet[cellName];

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

		sheet[cellName].push(cellContents); // update sheet 

		return "change " + cellName + ":" + cellContents; // return the change message
	}

	/*
	 * Revert cellName's contents. Counts as an edit.
	 */
	std::string spreadsheet::revert(std::string cellName)
	{
		if (sheet[cellName].empty()) return ""; // nothing to revert, return null

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
		if(edits.empty()) return ""; //nothing to undo

		else
		{
			std::string undo = edits.top(); // grab the last edit
			edits.pop(); // undo the last edit

			std::string cellName = undo.substr(0, undo.find(":")); // grab the cell name
			sheet[cellName].pop(); // undo the last edit to this cell

			return "change " + undo; // return the change message
		}

		return "change A1:This should not be happening."; // TODO should never reach this. . .
	}

/*
 ************************** PUBLIC METHODS ***********************
 */


	/*
	 * Write the current state of the spreadsheet to file.
	 */
	void spreadsheet::save() 
	{
		// build file path to edits stack
		boost::filesystem::path myEdits = boost::filesystem::current_path() / (const boost::filesystem::path&)("Spreadsheets/" + myName + "_edits.sprd");

		// build file path to sheet map
		boost::filesystem::path mySheet = boost::filesystem::current_path() / (const boost::filesystem::path&)("Spreadsheets/" + myName + "_sheet.sprd");


		boost::filesystem::ofstream  editsOut(myEdits); // set up out file streams
		boost::filesystem::ofstream sheetOut(mySheet);

		boost::archive::text_oarchive editsArchive(editsOut); // set up out archives
		boost::archive::text_oarchive sheetArchive(sheetOut); 

		editsArchive << edits; // write to archives	
		sheetArchive << sheet; 
	}

	/*
	 * Construct a spreadsheet from the given file.
	 * 
	 * 1) Get the paths to both the "sheet" and "edits" files. Set myName.
	 * 2) Deserialize the data there into this spreadsheet's "sheet" and "edits" member variables.
	 * 3) Close the files.
	 */
	spreadsheet::spreadsheet(std::string fileName)
	{
		
		this->myName = fileName;
		//std::cout << fileName << std::endl;

		// build file path to sheet map
		boost::filesystem::path mySheet = boost::filesystem::current_path() / (const boost::filesystem::path&)("Spreadsheets/" + fileName + "_sheet.sprd");

		// build file path to edits stack
		boost::filesystem::path myEdits = boost::filesystem::current_path() / (const boost::filesystem::path&)("Spreadsheets/" + fileName + "_edits.sprd");


		// do the files exist
		bool sheetExists = boost::filesystem::exists(mySheet);		
		bool editsExists = boost::filesystem::exists(myEdits);

		// if the "sheet" and "edits" files exist, read from it
		if (sheetExists && editsExists)
		{
			//std::cout << fileName << " spreadsheet exists so we load up the info." << std::endl;
			boost::filesystem::ifstream sheetIn(mySheet); // set up in file streams
			boost::archive::text_iarchive sheetArchive(sheetIn); // set up in archives
			sheetArchive >> sheet; // populate this->sheet and this-> edits

			boost::filesystem::ifstream editsIn(myEdits); // set up in file streams
			boost::archive::text_iarchive editsArchive(editsIn); // set up in archives
			editsArchive >> edits; // populate this->sheet and this-> edits

			// archive AND ifstream are closed when we leave scope 
		}
	}


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
		        int colon = update.find(":");  // useful index to know
		        std::string name = update.substr(5, colon - 5); 
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
		        return ""; // we didn't find a message that we know how to process :(
		}

		catch (...)
		{
		    return ""; // the message was not a protocol match
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
			if(!(sheetRator->second.empty()))
			{
		    	std::string cellName = sheetRator->first;
		    	std::string cellContents = sheetRator->second.top();

			    fullState.insert(std::pair<std::string, std::string>(cellName, cellContents));
			}
		}
		return fullState;
	}

	/*
	 * Destroy this spreadsheet.
	 */
	spreadsheet::~spreadsheet() {}
}
