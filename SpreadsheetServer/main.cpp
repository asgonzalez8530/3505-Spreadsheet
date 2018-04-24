
#include <iostream>
#include "server.h"
#include "spreadsheet.h"

int main(){

	cs3505::server mainServer = cs3505::server();

	// Start the server
	mainServer.master_server_loop();

	return 1;
}
