/* 
 * This class stores and tracks all the data structures that the server uses:
 *  1) new clients (queue)
 *  2) map < spreadsheet, list of clients connected to spreadsheet >
 *  3) disconnect (set)
 *  4) messages (queue)
 * 
 * It also handles all the locking and logic that the server does.
 *
 * Pineapple upside down cake
 * v1: April 5, 2018
 */

#ifndef INTERFACE_H
#define INTERFACE_H

#include <string>
#include <queue>
#include <set>
#include <map>
#include <list>
#include "spreadsheet.h"

typedef std::list<int> socket_list;
typedef std::map<std::string, socket_list> client_map;


namespace cs3505
{
    class interface
    {
        private:
            // private variables (still need getters and setters for all)


			/* TODO: Do we want to use data structures like these? */
			 // list of all client sockets for a spreadsheet
			socket_list clients;
			 // map of client lists for spreadsheets
			client_map map_of_spreadsheets;

            std::queue<int> new_clients;
            std::map<spreadsheet, int> map_of_clients;
            std::set<int> disconnect;
            std::queue<std::string> messages;

        public:
            // constructor
            interface();
            ~interface();
            bool new_clients_isempty();
            void new_clients_add(int);
            void new_clients_finish_handshake();
            bool disconnect_isempty();
            void disconnect_add(int);
            void disconnect_clients();
            bool messages_isempty();
            std::string get_message();
            void messages_add(std::string);
            void propogate_to_spreadsheet(spreadsheet * s, std::string message);
            void propogate_to_client(int client, std::string message);

        private:
            // helper methods

    };
} // end of class

#endif
