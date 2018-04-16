
#include <iostream>
#include "server.h"
#include "spreadsheet.h"

int main(){

	cs3505::server mainServer = cs3505::server();
	//mainServer.master_server_loop();
	std::cout << "Sweet sweet success." << std::endl;

	cs3505::spreadsheet mySheet("AnotherNewFile");
	std::cout << "MORE Sweet sweet success." << std::endl;

	return 1;
}
