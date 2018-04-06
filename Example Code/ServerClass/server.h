/*
 * This class represents a server at default port 2112 or a
 * port input by the user.
 *
 * NOTE: Could be static or non-static.
 *
 */


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
		int serverSocket;
		struct sockaddr_in servAddr;
		
	public:
		Server();
		Server(int port);
		~Server();
	    Server(const Server & other);

		void read();
		void write(int socket, std::string message);
		void beginListeningLoop();

};

#endif