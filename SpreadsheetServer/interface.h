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

namespace cs3505
{
    class interface
    {
        private:
            // private variables (still need getters and setters for all)
            std::queue<int> new_clients;
            std::map<int, int> map_of_clients;
            std::set<int> disconnect;
            std::queue<std::string> messages;

        public:
            // constructor
            interface();
            bool new_clients_isempty();
            void new_clients_add(int);
            void new_clients_finish_handshake();
            bool disconnect_isempty();
            void disconnect_add(int);
            void disconnect_clients();
            bool messages_isempty();
            std::string get_message();
            void messages_add(std::string);
            


        private:
            // helper methods

    };
} // end of class

#endif