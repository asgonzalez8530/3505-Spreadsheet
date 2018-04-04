
#ifndef SERVER
#define SERVER

#include <iostream>
#include <errno.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/socket.h>
#include <netinet/in.h>


class Server{
	private:
		int port;
		int sockfd;
		struct sockaddr_in serverAddr;
	
	public:
		
		Server server(int port);
		void initialize();
		int getPort();
		int getSocket();
		void sendData();

};


#endif