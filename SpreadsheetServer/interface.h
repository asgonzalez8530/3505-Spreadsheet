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
#include <pthread.h>
#include "spreadsheet.h"

typedef std::list<int> socket_list;
typedef std::map<std::string, socket_list> client_map;

namespace cs3505
{
    class interface
    {
        private:
            // private variables
            std::queue<int> new_clients; // queue of clients that need to be added
            std::set<int> disconnect; // set of sockets that need to be disconnected
            std::queue<std::string> messages; // queue of messages that the server needs to parse
            std::map<std::string, spreadsheet> all_spreadsheets; // map of spreadsheet names and spreadsheet objects
            socket_list clients; // list of all client sockets for a spreadsheet
            client_map map_of_spreadsheets; // map of client lists for spreadsheets

            // Data structure locks
			pthread_mutex_t map_lock;
            pthread_mutex_t queue_lock;
            pthread_mutex_t client_lock;
            pthread_mutex_t message_lock;
            pthread_mutex_t disconnect_lock;
            pthread_mutex_t spreadsheet_lock;

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
            void disconnect_all();
            void disconnecting();
            bool messages_isempty();
            std::string get_message();
            void messages_add(std::string);
            void propogate_to_spreadsheet(std::string, std::string);
            void propogate_to_client(int, std::string);
            bool spreadsheet_exists(std::string);
            void add_client(std::string, int);
            void add_spreadsheet(std::string);
            void propogate_full_state(std::map<std::string, std::string> *, int);
            spreadsheet * get_spreadsheet(std::string);
            std::string get_spreadsheet(int);
            void stop_receiving_and_propogate_all_messages();
            void save_all_spreadsheets();

        private:
            // helper methods

    };
} // end of class

#endif
