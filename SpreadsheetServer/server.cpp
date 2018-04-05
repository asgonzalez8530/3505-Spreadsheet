/* 
 * This class handles the four tasks of a server:
 *  1) check/handle new clients
 *  2) verify connections with connected clients
 *  3) process incoming messages
 *  4) check for server shutdown
 *
 * Pineapple upside down cake
 * v1: April 4, 2018
 */

#include "server.h"
#include <string>

namespace cs3505
{
    //**** public methods ****//

    // constructor
    server::server()
    {
        // may not want this here unless we make a server object on a seperate thread
        // most likely will want to just call the master server loop on a seperate thread 
        // rather than initializing it when the server object is made
        server::master_server_loop();

        // new thread were we start the ping loop

        // new thread were we start listening for multiple clients

    }

    server::master_server_loop()
    {   
        // this boolean will determine whether or not the loop will run immediatily 
        // after execution or it will sleep 10 ms before running again
        bool sleeping = false;

        while (!sleeping)
        {
            check_for_new_clients();
            verify_connections();
            sleeping = process_message();
            check_for_shutdown();
            
            // if no new message then we sleep for 10ms
            if (sleeping)
            {
                //sleep(10ms);
                sleeping = false;
            }
        }
    }

    //**** private & helper methods ****//
    
    /**
     * checks if the client list has a new client.
     * if the new_client list size is not zero (there is/are new client/s) then it locks the list and 
     * removes each client (socket) from the list and connects the client to the server. It then 
     * proceeds to finish the TCP and spreadsheet handshake. (may do the handshake stuff on a seperate thread???)
     */
    void server::check_for_new_clients()
    {
        // there are new clients 
        if (new_clients.size() != 0)
        {
            // lock the new_clients list 

            // for each socket in the list
            
            // make a new connection

            // make new thread?
            // finish TCP and spreadsheet handshake
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
        if (not_connected.size() != 0)
        {
            // lock the not connected list

            // for each client not connected remove them from being connected to the server and spreadsheet
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
        if (!incoming_messages.empty())
        {
            // pop the message off the stack
            std::string message = incoming_message.get();

            // parse the message
            std::string response = parse_message(message);  

            if (response != NULL)
            {
                // propogate the message on new thread 
            }

            return true;
        }

        return false;
    }

    /**
     * checks if the server was asked to shut down.
     * if the boolean terminate is true then we shut down then we clean up the spreadsheet messages (stop receiving messages). 
     * We propogate the appropriate messages. We then proceed to disconnect all the clients, save the spreadsheet, and close 
     * our program in a clean manner.
     */
    void server::check_for_shutdown()
    {
        //TODO: remove this declaration for terminate
        // terminate should be a global variable that is used as a flag 
        bool terminate = false;
        if (terminate)
        {
            // stop receiving messages and propogate appropriate changes
            // (i.e. call the process message method to process all previous messages)

            // disconnect all clients

            // save the spreadsheet

            // close our out of this program in a clean way

        }
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
        // response message that the server will propogate if not null
        std::string response = NULL;

        // TODO: parse message here

        return response;
    }

}