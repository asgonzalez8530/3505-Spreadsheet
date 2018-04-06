/* 
 * This class handles the four tasks of a server:
 *  1) check/handle new clients
 *  2) verify connections with connected clients
 *  3) process incoming messages
 *  4) check for server shutdown
 *
 * Pineapple upside down cake
 * v1: April 4, 2018
 * v2: April 5, 2018
 */

#include "server.h"
#include "interface.h"
#include <string.h>
#include <iostream>
#include <errno.h> // includes for networking
#include <stdlib.h>
#include <unistd.h>
#include <sys/socket.h>
#include <netinet/in.h>

void* client_loop(void * connection_file_descriptor);



namespace cs3505
{
    //**** public methods ****//

    // constructor
    server::server()
    {
        // this boolean will tell us when we want to shut down the server
        terminate = false;

	// TODO moved stubs relating to starting new threads to master_server_loop()
    }

    void server::master_server_loop()
    {   
        // this boolean will determine whether or not the loop will run immediatily 
        // after execution or it will sleep 10 ms before running again
        bool sleeping = false;


        // new thread were we start the ping loop

        // new thread were we start listening for multiple clients

        // server shutdown listener

	// run the main server loop
        while (!terminate && !sleeping)
        {
            check_for_new_clients();
            verify_connections();
            sleeping = process_message();
            
            // if no new message then we sleep for 10ms
            if (sleeping)
            {
                //sleep(10ms);
                sleeping = false;
            }
        }

        shutdown();
    }

    //**** private & helper methods ****//

    /**
     *
     */
    void* client_loop(void * connection_file_descriptor)
    {
	//return connection_file_descriptor;
    }

    /**
     * This is a loop that listens for new TCP connections and processes those new
     * connections by connecting and initiating the Protocol Handshake.
     */
    void server::server_awaiting_client_loop()
    {
	// the default port we'll listen on
	int listenPort = 2112;

	// Create a socket
        int serverSocket = socket(AF_INET, SOCK_STREAM, 0);

	// if the socket value is negative, there was an error
        if (serverSocket < 0)
        {
            std::cerr << "Error: " << strerror(errno) << std::endl;
            exit(1);
        }

	// Fill in the address structure
	struct sockaddr_in myaddr;
	memset(&myaddr, 0, sizeof(struct sockaddr_in)); //allocate the memory
	myaddr.sin_family = AF_INET; // using IPv4
	myaddr.sin_port = htons(listenPort);        // Port to listen
	myaddr.sin_addr.s_addr = htonl(INADDR_ANY); // ?

	// Bind a socket to the address
	int bindResult = bind(serverSocket, (struct sockaddr*) &myaddr, sizeof(myaddr));
	
	// if the bind result value is negative, there was an error
	if (bindResult < 0)
	{
	    std::cerr << "Error: " << strerror(errno) << std::endl;
	    exit(1);
	}

	// Now, listen for a connection (reusing "bindResult" - consider renaming?)
	bindResult = listen(serverSocket, 1);    // "1" is the maximal length of the queue

	// if the listen result value is negative, there was an error
	if (bindResult < 0)
	{
	    std::cerr << "Error: " << strerror(errno) << std::endl;
	    exit(1);
	}

	while(true)
	{
	    int newClient = 0;
    	    
	    // Accept a connection (the "accept" command waits for a connection with
	    // no timeout limit...)
	    struct sockaddr_in peeraddr;
	    socklen_t peeraddr_len;
	    newClient = accept(serverSocket, (struct sockaddr*) &peeraddr, &peeraddr_len);

	    // if the accept result value is negative, there was an error
	    if (newClient < 0)
	    {
		std::cerr << "Error: " << strerror(errno) << std::endl;
		exit(1);
	    }

	    // if the accept result value is positive, we have a new client!
	    if (newClient > 0)
	    {
		int sock = newClient; // copy the new client
	        void* conn_fd = &sock; // store as a void * so it can be passed to client_loop
		pthread_t new_connection_thread;
		pthread_create(&new_connection_thread, NULL, client_loop, conn_fd);
		
		// Clean up thread resources as they finish
		pthread_detach(new_connection_thread);
	    }	    
	}
    }



    
    /**
     * checks if the client list has a new client.
     * if the new_client list size is not zero (there is/are new client/s) then it locks the list and 
     * removes each client (socket) from the list and connects the client to the server. It then 
     * proceeds to finish the TCP and spreadsheet handshake. (may do the handshake stuff on a seperate thread???)
     */
    void server::check_for_new_clients()
    {
        // there are new clients 
        if (!data.new_clients_isempty())
        {
            data.new_clients_finish_handshake();
        } 
    }

    /**
     * checks if the not_connectioned list to see if there are any clients who are no longer connected.
     * if the not_connected list size is not zero (there is/are disconnected client/s) then it locks the list and 
     * removes each client from the list and from being connected to the server and spreadsheet. 
     */
    void server::verify_connections()
    {
        // clients have disconnected from the server
        if (!data.disconnect_isempty())
        {
            data.disconnect_clients();
        }
    }

    /**
     * THIS METHODS WAY OF PARSING/PROPOGATING MESSAGES MAY NEED TO CHANGE
     * 
     * checks if there is a new message to process.
     * if there is a new message to process then it locks the list and takes one message. 
     * it then proceeds to parse and process the message.
     * if the message needs to be propagated to the other clients then it propagates the message on a new thread
     */
    bool server::process_message()
    {
        // incoming messages will most likely be an object of interface so we will be using the getter here
        // there are messages to process
        if (!data.messages_isempty())
        {
            // pop the message off the stack
            std::string message = data.get_message();

            // parse the message
            std::string response = parse_message(message);  

            if (!response.empty())
            {
                // propogate the message on new thread 
            }

            return true;
        }

        return false;
    }

    /**
     * waits and listens for the "quit" keyword to tell the server to terminate the program
     */
    void server::check_for_shutdown()
    {
        while (true)
        {
            std::string input = "";
            std::getline(std::cin, input);

            if (input.compare("quit") == 0)
            {
                // lock terminate

                // flip the boolean flag terminate to tell the program to terminate
                terminate = true;
            }
        }
    }

    /**
     * checks if the server was asked to shut down.
     * if the boolean terminate is true then we shut down then we clean up the spreadsheet messages (stop receiving messages). 
     * We propogate the appropriate messages. We then proceed to disconnect all the clients, save the spreadsheet, and close 
     * our program in a clean manner.
     */
    void server::shutdown()
    {
        // stop receiving messages and propogate appropriate changes
        // (i.e. call the process message method to process all previous messages)

        // disconnect all clients

        // save the spreadsheet

        // close our out of this program in a clean way

    }

    /**
     * THIS METHODS FUNCTIONALITY WILL MOST LIKELY NEED TO CHANGE
     * 
     * parses the inputted message and if the message requires a server response then we return the server's response as a string
     * 
     * the following messages should be parsed by this message and result in the following response
     */
    std::string server::parse_message(std::string message)
    {
        // response message that the server will propogate if not an empty string
        std::string response = "";

        // TODO: parse message here
        // register message will add the client to the new clients list


        return response;
    }

} // end of class
