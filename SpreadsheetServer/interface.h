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
#include "message_queue.h"

typedef std::list<int> socket_list;
typedef std::map<std::string, socket_list> client_map;

namespace cs3505
{
    class interface
    {
        private:
            // private variables
            std::queue<int> new_clients; // queue of clients that need to be added
            std::queue<int> disconnect; // set of sockets that need to be disconnected
            std::map<std::string, spreadsheet> all_spreadsheets; // map of spreadsheet names and spreadsheet objects
            socket_list clients; // list of all client sockets for a spreadsheet
            client_map map_of_spreadsheets; // map of client lists for spreadsheets
            message_queue messages; // The inbound/outbound message controller

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
            bool disconnect_isempty();
            void disconnect_add(int);
            void disconnect_clients();
            void disconnect_all();
            void disconnecting();
            void client_wants_to_disconnect(int);

            // propogate methods
            void propogate_to_spreadsheet(std::string, std::string);
            void propogate_to_client(int, std::string);
            void propogate_full_state(std::map<std::string, std::string> *, int);
            void stop_receiving_and_propogate_all_messages();

            // parsing of messages and propogating 
            int parse_and_respond_to_message(std::string, int socket, std::string);

            // spreadsheet getters and setters
            bool spreadsheet_exists(std::string);
            void add_client(std::string, int);
            void add_spreadsheet(std::string);
            spreadsheet * get_spreadsheet(std::string);
            std::string get_spreadsheet(int);
            void save_all_spreadsheets();
            std::set<std::string> get_spreadsheet_names();

            // message queues
            bool outbound_empty();
            void send_message();
            void add_to_outbound_messages(int, std::string);
            bool inbound_empty();
            int get_inbound_message_parse_and_respond();
            void add_to_inbound_messages(int, std::string);

        private:
            // helper methods (without locks to prevent deadlocks)
            void propogate_to_client_without_a_lock(int, std::string);
            void parse_and_respond_to_message_without_lock(std::string, int socket, std::string);
            void propogate_to_spreadsheet_without_lock(std::string, std::string);
            void propogate_full_state_without_lock(std::map<std::string, std::string> *, int);

    };
} // end of class

#endif
