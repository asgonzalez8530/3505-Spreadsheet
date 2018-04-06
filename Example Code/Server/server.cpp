#include <iostream>
#include <errno.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/socket.h>
#include <netinet/in.h>
 
static void usage();
void* client_loop(void* conn_fd);
 
int main(int argc, char *argv[])
{

    if (argc > 1 && *(argv[1]) == '-')
    {
        usage(); exit(1);
    }
 
    int listenPort = 2112;

    // Create a socket
    int s0 = socket(AF_INET, SOCK_STREAM, 0);
    if (s0 < 0)
    {
        std::cerr << "Error: " << strerror(errno) << std::endl;
        exit(1);
    }
 
    // Fill in the address structure containing self address
    struct sockaddr_in myaddr;
    memset(&myaddr, 0, sizeof(struct sockaddr_in));
    myaddr.sin_family = AF_INET;
    myaddr.sin_port = htons(listenPort);        // Port to listen
    myaddr.sin_addr.s_addr = htonl(INADDR_ANY);
 
    // Bind a socket to the address
    int res = bind(s0, (struct sockaddr*) &myaddr, sizeof(myaddr));
    if (res < 0)
    {
        std::cerr << "Error: " << strerror(errno) << std::endl;
        exit(1);
    }
 
    // TODO don't need linger - take it out :D
    // Set the "LINGER" timeout to zero, to close the listen socket
    // immediately at program termination.
    struct linger linger_opt = { 1, 0 }; // Linger active, timeout 0
    setsockopt(s0, SOL_SOCKET, SO_LINGER, &linger_opt, sizeof(linger_opt));
 
    // Now, listen for a connection
    res = listen(s0, 1);    // "1" is the maximal length of the queue


    while(1) 
	{
	    int s1 = 0;
	    if (res < 0)
	    {
		std::cerr << "Error: " << strerror(errno) << std::endl;
		exit(1);
	    }
	 
	    // Accept a connection (the "accept" command waits for a connection with
	    // no timeout limit...)
	    struct sockaddr_in peeraddr;
	    socklen_t peeraddr_len;
	    s1 = accept(s0, (struct sockaddr*) &peeraddr, &peeraddr_len);

	    if (s1 < 0)
	    {
		std::cerr << "Error: " << strerror(errno) << std::endl;
		exit(1);
	    }

	    // if s1 > 0 someone is trying to connect, start a new thread
	    if (s1 > 0) 
	    {
		  // Create new client thread in scope
          {
			int sock = s1;
	        void * conn_fd = &sock;
		    pthread_t new_connection_thread;
		    pthread_create(&new_connection_thread, NULL, client_loop, conn_fd);
		    // Clean up thread resources as they finish
		    pthread_detach(new_connection_thread);
          }
	    }
	 
	    // A connection is accepted. The new socket "s1" is created
	    // for data input/output. The peeraddr structure is filled in with
	    // the address of connected entity, print it.
	    std::cout << "Connection from IP "
		      << ( ( ntohl(peeraddr.sin_addr.s_addr) >> 24) & 0xff ) << "."  // High byte of address
		      << ( ( ntohl(peeraddr.sin_addr.s_addr) >> 16) & 0xff ) << "."
		      << ( ( ntohl(peeraddr.sin_addr.s_addr) >> 8) & 0xff )  << "."
		      <<   ( ntohl(peeraddr.sin_addr.s_addr) & 0xff ) << ", port "   // Low byte of addr
		      << ntohs(peeraddr.sin_port) << std::endl;
	}

	//res = close(s0);    // Close the listen socket 

	//return 0;
}


/*
*  Takes a connection file descriptor, aka our client's socket.
*  Basic loop to print client chat messages.
*/
void* client_loop(void* conn_fd)
{
    int socket = *((int*)conn_fd);
    //int socket = (long)conn_fd; // cast from void* to long? XD
    write(socket, "Hello!\r\n", 8);
    char buffer[1024];
    int res = 0;


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
