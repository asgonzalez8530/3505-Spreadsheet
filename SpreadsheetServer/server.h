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
#include "spreadsheet.h"
#include "ping.h"
#include "message_queue.h"

namespace cs3505
{

	typedef struct _ThreadData
	{
		int socket;
		interface * data;
		ping * png;
        message_queue * q;

	} ThreadData;    

	class server
    {
        private:

			ThreadData * connfd;

            // track whether the "quit" command has been issued
            bool terminate;

	        // the hub for server-client interactions
            interface data;

            // the ping controller
            ping pings;

            // The inbound/outbound message controller
            message_queue messages;

        public:
            // constructor
            server();

			// destructor
			~server();

            // master server loop
            void master_server_loop();


        private:
            // helper methods
	        int server_awaiting_client_loop();
            void check_for_new_clients();
            void verify_connections();
            bool process_message();
            void check_for_shutdown();
            void shutdown();
			void parse_and_respond_to_message(spreadsheet * s, int socket, std::string message);
            std::set<std::string> get_spreadsheet_names();


    };
} // end of class

#endif
