/* 
 * This class handles the four tasks of a server:
 *  1) check/handle new clients
 *  2) verify connections with connected clients
 *  3) process incoming messages
 *  4) check for server shutdown
 *
 * Pineapple upside down cake
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
    // This struct holds the data needed by threads
	typedef struct _ThreadData
	{
		int socket;
		interface * data;
		ping * png;

	} ThreadData;

	class server
    {
        private:

            // a pointer to the data to pass to threads
			ThreadData * connfd;

            // track whether the "quit" command has been issued
            bool terminate;

	        // the hub for server-client interactions
            interface data;

            // the ping controller
            ping pings;

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
            bool send_message();
            void shutdown(int);


    };
} // end of class

#endif
