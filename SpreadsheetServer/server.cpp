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
 * v5: April 18, 2018
 * v6: April 19, 2018
 * v7: April 20, 2018
 */

#include "server.h"
#include "interface.h"
#include "spreadsheet.h"
#include "ping.h"
#include <errno.h> // includes for networking
#include <stdlib.h>
#include <unistd.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <string.h>
#include <string>
#include <iostream>
#include <ctime>
//#include <mutex>
#include <stack>
#include <fstream>
#include <thread> 
#include <chrono> 
#include <regex>

namespace cs3505
{
    // forward declare delegate for thread
    void * client_loop(void *connection_file_descriptor);
    void * ping_loop(void *connection_file_descriptor);
    void * check_for_shutdown(void * connection_file_descriptor);
    double getTime(clock_t startTime, clock_t testTime);
    std::string parseBuffer(std::string * message);

    // forward declare listener initializer and listener_loop helper
    int init_listener();
    void *listener_loop(void *);

    //**** public methods ****//

    // constructor
    server::server()
    {
		connfd = new ThreadData();
		connfd->data = &data;
		connfd->png = &pings;

        // this boolean will tell us when we want to shut down the server
        terminate = false;
    }

	    // constructor
    server::~server()
    {
    }

    void server::master_server_loop()
    {
        // this boolean will determine whether or not the loop will run immediatily
        // after execution or it will sleep 10 ms before running again
        bool inbound_sleep = false;
        bool outbound_sleep = false;
        bool terminate_flag = false;

            {
                // Create a thread to monitor for shutdown input
                void * connection_file_descriptor = &terminate_flag;
                pthread_t th;
                pthread_create(&th, NULL, check_for_shutdown, connection_file_descriptor);

                // Clean up thread resources as they finish
                pthread_detach(th);
            }

        std::set<std::string> file_names = data.get_spreadsheet_names();

        if (!file_names.empty())
        {
            // get all the available spreadsheets
            for(std::set<std::string>::iterator iter = file_names.begin(); iter != file_names.end(); iter++)
            {
                // get the spreadsheet name
                std::string name = *iter;

                // build and add the spreadsheet to the server
                data.add_spreadsheet(name);
            }
        }

        // new thread were we start listening for multiple clients
        int serverSocket = server_awaiting_client_loop();

        std::cout << "Entering main server loop.\n";

        // run the main server loop
        while (!terminate)
        {
            inbound_sleep = process_message();
            outbound_sleep = send_message();
            verify_connections();

            // if no new message then we sleep for 10ms
            if (inbound_sleep && outbound_sleep)
            {
                std::this_thread::sleep_for (std::chrono::milliseconds(10));
                inbound_sleep = false;
                outbound_sleep = false;
            }

            // check the shutdown thread flag
            terminate = terminate_flag;
        }

        shutdown(serverSocket);
    }

    //**** private & helper methods ****//

    /**
     * This is a loop that listens for new TCP connections and processes those new
     * connections by connecting and initiating the Protocol Handshake.
     */
    int server::server_awaiting_client_loop()
    {
        // initialize listener socket
        int serverSocket = init_listener();

        // print for debugging
        std::cout << "Finished listener initialize." << std::endl;

        // Create a thread to listen for new connections
        connfd->socket = serverSocket;

        pthread_t new_connection_thread;
        pthread_create(&new_connection_thread, NULL, listener_loop, connfd);

        // Clean up thread resources as they finish
        pthread_detach(new_connection_thread);

        return serverSocket;
    }

    /**
     * This method controls pings and initiates disconnect of unresponsive clients
     *
     * connection_file_descriptor - The socket to ping
     */
    void *ping_loop(void *connection_file_descriptor)
    {
		ThreadData * args = (ThreadData*)connection_file_descriptor;
        int socket = (args->socket);
        ping * png = (args->png);
		interface * data = (args->data);

        int failed_pings = 0;
        double secondsToPing = 20;
        clock_t pingTimer, timePassed;

        // Setup the ping message
        std::string ping_message = "ping ";
        ping_message.push_back((char)3);

        // begin ping timer
        pingTimer = clock();

        // Add socket to ping flag map
        (args->png)->flag_map_add(socket);

        while (true)
        {

            timePassed = clock();

            // check for timeout
            if (failed_pings > 5)
            {
                // remove socket from flag map
				(args->png)->flag_map_remove(socket);

                // add client to the disconnect list
				(args->data)->disconnect_add(socket);

                pthread_exit(0);
            }

            // check for ping
            else if (getTime(timePassed, pingTimer) >= secondsToPing)
            {
                // Send a ping message
                (args->data)->add_to_outbound_messages(socket, ping_message);

                //Check for a ping response
				if((args->png)->check_ping_response(socket) == 1)
                {
                    failed_pings = 0;
                }
                else
                {
                    failed_pings++;
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
        ThreadData * args = (ThreadData*)connection_file_descriptor;
        int socket = args->socket;
		ping * png = (args->png);
		interface * data = (args->data);

        char buffer[1024];
        int size = 0;

        while (true)
        {

            size = read(socket, buffer, 1023);

            if (size <= 0)
            {
                (args->data)->client_wants_to_disconnect(socket);
                close(socket);
                pthread_exit(0);
            }

            // Insert null terminator in buffer
            buffer[size] = 0;

            std::string buff = buffer;

            std::string result = parseBuffer(&buff);

            if (result.empty())
			{
                continue;
			}
            else if (result.compare("1") == 0)
            {
                // current client pinged a response so we flag ping as true
                (args->png)->ping_received(socket);
            }
            else if (result.compare("2") == 0)
            {
                // ping the client back
                std::string result = "ping_response ";
                result.push_back((char)3);
                (args->data)->propogate_to_client(socket, result);
            }
            else if (result.compare("3") == 0)
            {
                // send to all the other clients connected to the same spreadsheet that client disconnected
                (args->data)->client_wants_to_disconnect(socket);
                break;
            }
            else
            {
                // add message to the list of messages that needs to be processed
                (args->data)->add_to_inbound_messages(socket, result);
            }

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
    void *listener_loop(void * connection_file_descriptor)
    {
        ThreadData * args = (ThreadData*)connection_file_descriptor;
        int serverSocket = args->socket;

        while (true)
        {
            int newClient = 0;

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
                args->socket = newClient;
                pthread_t new_connection_thread;
                pthread_create(&new_connection_thread, NULL, client_loop, args);

                // Clean up thread resources as they finish
                pthread_detach(new_connection_thread);
            }
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
     * checks if there is a new message to process.
     * if there is a new message to process then it locks the list and takes one message. 
     * it then proceeds to parse and process the message.
     * if the message needs to be propagated to the other clients then it propagates the message on a new thread
     */
    bool server::process_message()
    {
        // there are messages in the inbound queue to process, parse, and add response to the outbound queue
        if (!data.inbound_empty())
        {
            int ping_result = data.get_inbound_message_parse_and_respond();
            if(ping_result > 0)
            {
                ThreadData * args = new ThreadData();
                args->socket = ping_result;
                args->data = connfd->data;
                args->png = connfd->png;
                pthread_t new_connection_thread;
                pthread_create(&new_connection_thread, NULL, ping_loop, args);

                // Clean up thread resources as they finish
                pthread_detach(new_connection_thread);
            }
            return true;
        }

        return false;
    }

    /**
     * checks if there is a new message to propogate.
     * if there is a new message to process then it locks the list and takes one message. 
     * it then proceeds to send the message to the appropriate socket.
     */
    bool server::send_message()
    {
        // there are messages in the outbound queue to send
        if (!data.outbound_empty())
        {
            data.send_message();

            return true;
        }

        return false;
    }

    /**
     * waits and listens for the "quit" keyword to tell the server to terminate the program
     */
    void * check_for_shutdown(void * connfd)
    {

        std::string input = "";
        std::getline(std::cin, input);

        if (input.compare("quit") == 0)
        {
            // flip the boolean flag terminate to tell the program to terminate
            *((bool *)connfd) = true;
        }
    }

    /**
     * checks if the server was asked to shut down.
     * if the boolean terminate is true then we shut down then we clean up the spreadsheet messages (stop receiving messages). 
     * We propogate the appropriate messages. We then proceed to disconnect all the clients, save the spreadsheet, and close 
     * our program in a clean manner.
     */
    void server::shutdown(int serverSocket)
    {
        // stop receiving messages and propogate appropriate changes
        // (i.e. call the process message method to process all previous messages)
        data.stop_receiving_and_propogate_all_messages();

        // propogate to each client that the server is disconnecting
        data.disconnecting();

        // save the spreadsheet
        data.save_all_spreadsheets();
    }

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
        // get the position of \3
        int position = message->find((char)3);

        // check to see if its a complete message
        if ( position > 0 )
        {
            // pull out and remove the message from the beginning to right before the \3
            std::string current_message = message->substr(0, position);
            *message = message->substr(position + 1);

            // ping_response (may be able to remove the char 3)
            if (std::regex_match(current_message, std::regex("ping_response ")))
            {
                return std::to_string(1);
            }

            // ping
            else if (std::regex_match(current_message, std::regex("ping ")))
            {
                return std::to_string(2);
            }

            // disconnect
            else if (std::regex_match(current_message, std::regex("disconnect ")))
            {
                return std::to_string(3);
            }

            // other messages that we will parse later
            else
            {
                return current_message;
            }
        }

        return "";
    }

} // end of class
