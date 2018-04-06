
#include <iostream>
#include "server.h"

int main(){

	cs3505::server mainServer = cs3505::server();
	mainServer.master_server_loop();
	std::cout << "Sweet sweet success." << std::endl;

	return 1;
}
