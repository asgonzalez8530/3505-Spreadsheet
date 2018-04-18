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
	 * Change cellName's contents to cellContents. Counts as an edit.
	 */
	std::string spreadsheet::edit(std::string cellName, std::string cellContents)
	{	
		// TODO checking if it's empty, but may actually need to check if it's NULL too

		std::cout << "Hello from edit." << std::endl;


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

		sheet[cellName].push(cellContents); // update sheet TODO NULL check here?

		return "change " + cellName + ":" + cellContents; // return the change message
	}

	/*
	 * Revert cellName's contents. Counts as an edit.
	 */
	std::string spreadsheet::revert(std::string cellName)
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

		return "change A1:This should not be happening."; // TODO should never reach this. . .
	}

	/*
	 * Write the current state of the spreadsheet to file.
	 */
	void spreadsheet::save() 
	{

		if(!edits.empty())
		{
		
			std::cout << "I don't even know." << std::endl;	
			std::cout << edits.top() << std::endl;	
			std::cout << sheet["A1"].top() << std::endl;
			
			std::stack<std::string> temp(edits);
			
			// build file path to edits stack
			boost::filesystem::path myEdits = boost::filesystem::current_path() / (const boost::filesystem::path&)("Spreadsheets/" + myName + "_edits.sprd");



			std::cout << "Set up path." << std::endl;	

			boost::filesystem::ofstream  editsOut(myEdits); // set up out file streams
			std::cout << "Made filestream. " << editsOut.good() << std::endl;	
			boost::archive::text_oarchive editsArchive(editsOut); // set up out archives
			std::cout << "Made edits archive." << std::endl;	

			std::cout << "Writing edits. Edits size = " << edits.size() << std::endl;
			editsArchive << temp;
			std::cout << "Wrote edits." << std::endl;	
		}

		if(!sheet.empty())
		{
			std::map<std::string, std::stack<std::string> > temp(sheet);
			
			// build file path to sheet map
			boost::filesystem::path mySheet = boost::filesystem::current_path() / (const boost::filesystem::path&)("Spreadsheets/" + myName + "_sheet.sprd");

			boost::filesystem::ofstream sheetOut(mySheet); // set up out file streams		
			boost::archive::text_oarchive sheetArchive(sheetOut); // set up out archives

			std::cout << "Writing sheet." << std::endl;
			sheetArchive << temp; // write to archives
		}

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

		// build file path to sheet map
		boost::filesystem::path mySheet = boost::filesystem::current_path() / (const boost::filesystem::path&)("Spreadsheets/" + fileName + "_sheet.sprd");

		// build file path to edits stack
		boost::filesystem::path myEdits = boost::filesystem::current_path() / (const boost::filesystem::path&)("Spreadsheets/" + fileName + "_edits.sprd");


		
		// if the "sheet" file exists, read from it
		if (boost::filesystem::exists(mySheet) && boost::filesystem::file_size(mySheet) > 0)
		{
			boost::filesystem::ifstream in(mySheet);
			boost::archive::text_iarchive meArchive(in);

			meArchive >> sheet; // populate this->sheet

			// archive AND ifstream are closed when we leave scope 
			// (https://www.boost.org/doc/libs/1_66_0/libs/serialization/doc/tutorial.html)
		}

		// otherwise, create the file and close it.
		else
		{
			boost::filesystem::ofstream out(mySheet);
			out.close();
		}

		// if the "edits" file exists, read from it
		if (boost::filesystem::exists(myEdits) && boost::filesystem::file_size(myEdits) > 0)
		{
			boost::filesystem::ifstream in(myEdits);
			boost::archive::text_iarchive meArchive(in);

			meArchive >> edits; // populate this->edits

			// archive AND ifstream are closed when we leave scope 
			// (https://www.boost.org/doc/libs/1_66_0/libs/serialization/doc/tutorial.html)
		}

		// otherwise, create the file and close it
		else
		{
			boost::filesystem::ofstream out(myEdits);
			out.close();
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
			std::cout << "Hello from update." << std::endl;
			
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
