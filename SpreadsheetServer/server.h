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

#ifndef SERVER_H
#define SERVER_H

namespace cs3505
{
    class server
    {
        // private variables (still need getters and setters for all)
        // data structure to store the new client sockets (new_clients)
        // data structure to store the clients that are no longer connected (not_connected)
        // data structure (queue?) to store the incoming messages (incoming_messages)
        // boolean that tells you if the server was asked to terminate (terminate)

        public:
            // constructor
            server();

            // master server loop
            void master_server_loop();


        private:
            // helper methods
            void check_for_new_clients();
            void verify_connections();
            bool process_message();
            void check_for_shutdown();
            std::string parse_message(std::string message);


    };
}


#endif

// end of class