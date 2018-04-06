/* 
 * This class stores and tracks all the datastructures that the server uses:
 *  1) new clients (queue)
 *  2) map < spreadsheet, list of clients connected to spreadsheet >
 *  3) disconnect (set)
 *  4) messages (queue)
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

            


        private:
            // helper methods

    };
} // end of class

#endif