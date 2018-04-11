
#include <iostream>
#include "server.h"

int main(){

	Server serv(2112);
	serv.beginListeningLoop();

	return 1;
}