/* 
 * This class handles the four tasks of a server:
 *  1) check/handle new clients
 *  2) verify connections with connected clients
 *  3) process incoming messages
 *  4) check for server shutdown
 *
 * Pineapple upside down cake
 * v1: April 4, 2018
 * v2: April 7, 2018
 */

#ifndef SERVER_H
#define SERVER_H

#include <string>
#include <ctime>
#include "interface.h"

namespace cs3505
{
    class server
    {
        private:
            // track whether the "quit" command has been issued
            bool terminate;

	    // the hub for server-client interactions
            interface data;

        public:
            // constructor
            server();

            // master server loop
            void master_server_loop();


        private:
            // helper methods
	        void server_awaiting_client_loop();
            void check_for_new_clients();
            void verify_connections();
            bool process_message();
            void check_for_shutdown();
            void shutdown();
            std::string parseBuffer(std::string * message)
            void parse_and__message(std::string message)


    };
} // end of class

#endif
