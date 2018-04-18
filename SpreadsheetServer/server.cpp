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
 * v3: April 6, 2018
 * v4: April 13, 2018
 */

#include "server.h"
#include "interface.h"
#include <errno.h> // includes for networking
#include <stdlib.h>
#include <unistd.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <string.h>
#include <string>
#include <iostream>
#include <ctime>

namespace cs3505
{
    // forward declare delegate for thread
    void *client_loop(void *connection_file_descriptor);
    void *ping_loop(void *connection_file_descriptor);
    double getTime(clock_t startTime, clock_t testTime);
    bool parseBuffer(int size, char buff[]);

    // forward declare listener initializer and listener_loop helper
    int init_listener();
    void *listener_loop(void *);

	typedef struct _ThreadData
	{
		int socket;
		interface * data;
	} ThreadData;

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
        server_awaiting_client_loop();

        std::cout << "Entering main server loop.\n";

        // server shutdown listener

        // TODO the "endl" here leads to an error in the listener loop, it's interpreted as a socket operation.
        // std::cout << "Entering main server loop." << std::endl;

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
     * This is a loop that listens for new TCP connections and processes those new
     * connections by connecting and initiating the Protocol Handshake.
     */
    void server::server_awaiting_client_loop()
    {

        // initialize listener socket
        int serverSocket = init_listener();

        // print for debugging
        std::cout << "Finished listener initialize." << std::endl;

		ThreadData * args = new ThreadData();
		args->socket = serverSocket;
		args->data = &data;

        // set up a new thread for the listener loop()
        void *server = &serverSocket; // store as a void * so it can be passed to listener_loop()
        pthread_t new_connection_thread;
        pthread_create(&new_connection_thread, NULL, listener_loop, server);
        pthread_create(&new_connection_thread, NULL, ping_loop, args);

        // Clean up thread resources as they finish
        pthread_detach(new_connection_thread);
    }

    /**
     * This method controls pings and initiates disconnect of unresponsive clients
     *
     * connection_file_descriptor - The socket to ping
     */
    void *ping_loop(void *connection_file_descriptor)
    {
		ThreadData * args = (ThreadData*)connection_file_descriptor;
        int socket = args->socket;
        int failed_pings = 0;
        double secondsToPing = 10;
        double secondsToTimeout = 60;
        clock_t pingTimer, timePassed;

        // begin ping timer
        pingTimer = clock();

        while (true)
        {
            timePassed = clock();

            // check for timeout
            if (failed_pings >= 5)
            {
                // add client to the disconnect list
				(args->data)->disconnect_add(socket);

                pthread_exit(0);
            }

            // check for ping
            else if (getTime(pingTimer, timePassed) >= secondsToPing)
            {
                //Check for a ping response
				if((args->data)->check_ping_response))
                {
                    pingTimer = clock();
                    failed_pings = 0;
                }
                else
                {
                    failed_pings += 1;
                }

                // reset timer clock
                pingTimer = clock();
            }
        }
    }

    /**
     * Takes a connection file descriptor, aka our client's socket.
     *  Basic loop to print client chat messages.
     */
    void *client_loop(void *connection_file_descriptor)
    {
        int socket = *((int *)connection_file_descriptor);

        write(socket, "Hello!\r\n", 8);
        char buffer[1024];
        int result = 0;

        while (true)
        {
            // print for debugging
            std::cout << "Waiting to read reply from client." << std::endl;

            result = read(socket, buffer, 1023);

            if (result < 0)
            {
                std::cerr << "Error: " << strerror(errno) << " Error in client_loop()" << std::endl;
                exit(1);
            }

            // Insert null terminator in buffer
            buffer[result] = 0;

            // Print number of received bytes AND the contents of the buffer
            std::cout << "Received " << result << " bytes:\n" << buffer << std::endl;
        }

        close(socket);
    }

    /**
     * A helper to abstract away the setup for the server's listening socket.
     * Creates a socket, binds it to the listenPort, and begins listening.
     * Returns the listening socket.
     */
    int init_listener()
    {
        // the default port we'll listen on
        int listenPort = 2112;

        // Create a socket
        int serverSocket = socket(AF_INET, SOCK_STREAM, 0);

        // if the socket value is negative, there was an error
        if (serverSocket < 0)
        {
            std::cerr << "Error: " << strerror(errno) << " Error in init_listener()" << std::endl;
            exit(1);
        }

        // Fill in the address structure
        struct sockaddr_in myaddr;
        memset(&myaddr, 0, sizeof(struct sockaddr_in)); //allocate the memory
        myaddr.sin_family = AF_INET;                    // using IPv4
        myaddr.sin_port = htons(listenPort);            // Port to listen
        myaddr.sin_addr.s_addr = htonl(INADDR_ANY);     // ?

        // Bind a socket to the address
        int bindResult = bind(serverSocket, (struct sockaddr *)&myaddr, sizeof(myaddr));

        // if the bind result value is negative, there was an error
        if (bindResult < 0)
        {
            std::cerr << "Error: " << strerror(errno) << " Error in init_listener() bind" << std::endl;
            exit(1);
        }

        // Now, listen for a connection (reusing "bindResult" - consider renaming?)
        bindResult = listen(serverSocket, 1); // "1" is the maximal length of the queue

        // if the listen result value is negative, there was an error
        if (bindResult < 0)
        {
            std::cerr << "Error: " << strerror(errno) << " Error in init_listener() listen" << std::endl;
            exit(1);
        }

        return serverSocket;
    }

    /**
     * The server's listening loop.
     * Accepts new connections, starting a new thread for each one.
     */
    void *listener_loop(void *server)
    {
        // print for debugging
        std::cout << "Begin listening." << std::endl;

        while (true)
        {
            int newClient = 0;
            int serverSocket = *((int *)server);

            // Accept a connection (the "accept" command waits for a connection with
            // no timeout limit...)
            struct sockaddr_in peeraddr;
            socklen_t peeraddr_len;
            newClient = accept(serverSocket, (struct sockaddr *)&peeraddr, &peeraddr_len);

            // if the accept result value is negative, there was an error
            if (newClient < 0)
            {
                std::cerr << "Error: " << strerror(errno) << " Error in listener_loop()" << std::endl;
                exit(1);
            }

            // if the accept result value is positive, we have a new client!
            if (newClient > 0)
            {
                int sock = newClient;  // copy the new client
                void *conn_fd = &sock; // store as a void * so it can be passed to client_loop
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
    // std::string server::parse_message(std::string message)
    // {
    //     // response message that the server will propogate if not an empty string
    //     std::string response = "";

    //     // TODO: parse message here
    //     // register message will add the client to the new clients list

    //     return response;
    // }

  /**
   * This method takes two clock times and returns the difference
   * 
   * startTime - The current clock time
   * testTime - The initial clock time
   * 
   * Returns a double value of seconds passed
   */
  double getTime(clock_t currentTime, clock_t initialTime)
    {
      clock_t timeDiff = currentTime - initialTime;
      // extract time passed based on clock speed
      double seconds = timeDiff / (double)CLOCKS_PER_SEC;
      return seconds;
    }

/**
 * This method parses the inputted message to make sure that it is complete and valid.
 * The inputted message is a reference. 
 * 
 * This method returns one of four values:
 *      1: its a ping_response message
 *      2: its a ping message
 *      3: its a disconnect message
 *      <string>: this a complete message that was not one of the above
 */
std::string parseBuffer(std::string * message)
{   
    // TODO: move this to the right before the method call
    // outside the loop
    //std::string message = "";
    // convert the char buff to a string
    //std::string new_message(buff);
    // combine the old messages with the new messages
    //messages.append(buff);
    
    // get the position of /3
    int position = message->find((char)3);

    // check to see if its a complete message
    if ( position > 0 )
    {
        // pull out and remove the message from the beginning to the current char;
        std::string current_message = message->substr(0, position + 1);
        *message = message->substr(position + 1);

        // ping_response (may be able to remove the char 3)
        if (current_message.find("ping_response ") > 0)
        {
            return 1 + "";
        }

        // ping
        else if (current_message.find("ping ") > 0)
        {
            return 2 + "";
        }

        // disconnect
        else if (current_message.find("ping_response ") > 0)
        {
            return 3 + "";
        }

        // other messages that we will parse later
        else
        {
            return current_message;
        }
    }
    
}

/**
 * parses the inputted message. And determines if its a valid message.
 * Implements the servers response to the message.
 * TODO: Still need to implement
 */
std::string parse_message(spreadsheet s, std::string message)
{
    // register
    if (message.find("register ") > 0)
    {
        int p = message.find("register ");
        message.substr(p, message.length - 1);
    }

    // load
    else if (message.find("load ") > 0)
    {
        int p = message.find("load ");
        message.substr(p, message.length - 1);
    }

    // edit
    else if (message.find("edit ") > 0)
    {
        int p = message.find("edit ");
        message.substr(p, message.length - 1);
    }

    // focus
    else if (message.find("focus ") > 0)
    {
        int p = message.find("focus ");
        message.substr(p, message.length - 1);
    }

    // unfocus
    else if (message.find("unfocus ") > 0)
    {
        int p = message.find("unfocus ");
        message.substr(p, message.length - 1);
    }

    // undo
    else if (message.find("undo ") > 0)
    {
        int p = message.find("undo ");
        message.substr(p, message.length - 1);
    }

    // revert
    else if (message.find("revert ") > 0)
    {
        int p = message.find("revert ");
        message.substr(p, message.length - 1);
    }

    // else not a valid message so we do nothing
    return NULL;
}


} // end of class
