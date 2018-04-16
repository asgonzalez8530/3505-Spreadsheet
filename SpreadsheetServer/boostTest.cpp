#include <boost/archive/text_iarchive.hpp>
#include <boost/archive/text_oarchive.hpp>
#include <boost/serialization/map.hpp>
#include <boost/serialization/deque.hpp>
#include <boost/serialization/stack.hpp>
#include <boost/filesystem.hpp>
#include <boost/filesystem/fstream.hpp>
#include <stack>
#include <fstream>

// to run:
// g++ -o boostTest boostTest.cpp -lboost_system -lboost_serialization -lboost_filesystem
// g++ -o boostTest boostTest.cpp /usr/local/lib/libboost_system.a /usr/local/lib/libboost_serialization.a /usr/local/lib/libboost_filesystem.a 


int main()
{
	std::ofstream helloOut("HELLOWORLD.txt");
	helloOut << "Hello, World!" << std::endl;

	std::cout << "wrote hello world." << std::endl;

	std::stack<std::string> temp;
	temp.push("Bekah is so GOOOOOOD!");
	
	// build file path to edits stack
	//boost::filesystem::path myStack = boost::filesystem::current_path() / (const boost::filesystem::path&)("testerStack.dat");


	std::cout << "Set up path." << std::endl;	

	std::ofstream  editsOut("testerStack2.dat"); // set up out file streams
	std::cout << "Made filestream. " << editsOut.good() << std::endl;	
	boost::archive::text_oarchive editsArchive(editsOut); // set up out archives
	std::cout << "Made edits archive." << std::endl;	

	std::cout << "Writing edits. Edits size = " << temp.size() << std::endl;
	editsArchive << temp;
	std::cout << "Wrote edits!!!" << std::endl;
}

/*
[machardy@lab1-15 SpreadsheetServer]$ g++ -L/usr/local/include -o boostTest boostTest.cpp /usr/lib64/libboost_system.so.1.53.0 /usr/lib64/libboost_serialization.so.1.53.0 /usr/lib64/libboost_filesystem.so.1.53.0
[machardy@lab1-15 SpreadsheetServer]$ ./boostTest                               wrote hello world.
Set up path.
Made filestream. 1
Made edits archive.
Writing edits. Edits size = 1
terminate called after throwing an instance of 'boost::archive::archive_exception'
  what():  output stream error
Abort
[machardy@lab1-15 SpreadsheetServer]$ 



*/
