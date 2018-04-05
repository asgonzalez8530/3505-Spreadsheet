
#include "server.h"

void * clientLoop(void * connfd);
	
Server::Server(){
	port = 2112;
	
	// Create a socket
    serverSocket = socket(AF_INET, SOCK_STREAM, 0);
    if (serverSocket < 0)
    {
        std::cerr << "Error: " << strerror(errno) << std::endl;
        exit(1);
    }
 
    // Fill in the address structure containing self address
    memset(&servAddr, 0, sizeof(struct sockaddr_in));
    servAddr.sin_family = AF_INET;
    servAddr.sin_port = htons(port);        // Port to listen
    servAddr.sin_addr.s_addr = htonl(INADDR_ANY);
	
	    // Bind a socket to the address
    int res = bind(serverSocket, (struct sockaddr*) &servAddr, sizeof(servAddr));
    if (res < 0)
    {
        std::cerr << "Error: " << strerror(errno) << std::endl;
        exit(1);
    }
}


Server::Server(int port){
	this->port = port;
}

Server::~Server(){

}

Server::Server(const Server & other){
	*this = other;
}

void Server::beginListeningLoop(){
	int res = listen(serverSocket, 3);
	int clientSocket;
	
	while(1) 
	{
	    clientSocket = 0;
	    if (res < 0)
	    {
		std::cerr << "Error: " << strerror(errno) << std::endl;
		exit(1);
	    }
	 
	    // Accept a connection (the "accept" command waits for a connection with
	    // no timeout limit...)
	    struct sockaddr_in clientAddr;
	    socklen_t clientAddr_len;
	    clientSocket = accept(serverSocket, (struct sockaddr*) &clientAddr, &clientAddr_len);

	    if (clientSocket < 0)
	    {
		std::cerr << "Error: " << strerror(errno) << std::endl;
		exit(1);
	    }

	    // if s1 > 0 someone is trying to connect, start a new thread
	    if (clientSocket > 0) 
	    {
		  // Create new client thread in scope
          {
			 // Copy clientSocket
			int sock = clientSocket;
			 // Cast socket pointer to void *
	        void * conn_fd = &sock;
		    pthread_t new_connection_thread;
			 // Create new client thread
		    pthread_create(&new_connection_thread, NULL, clientLoop, conn_fd);
		    // Clean up thread resources as they finish
		    pthread_detach(new_connection_thread);
          }
	    }
	
	}
}

void * clientLoop(void * connfd){
	 // Cast void * pointer back to int
	int socket = *((int *)connfd);
	int res;
	char buffer[1024];
	
	write(socket, "Hello!\r\n", 8);
	
	while(1)
	{

	    res = read(socket, buffer, 1023);

	    if (res < 0) {
		std::cerr << "Error: " << strerror(errno) << std::endl;
		exit(1);
	    }

	    // Insert null terminator in buffer
	    buffer[res] = 0;

	    // Print number of received bytes AND the contents of the buffer
	    std::cout << "Received " << res << " bytes:\n" << buffer;

	}
    
    close(socket); 
	
}

void Server::read(){

}


void Server::write(int socket, std::string message){
	//write(socket, message, 8);
}