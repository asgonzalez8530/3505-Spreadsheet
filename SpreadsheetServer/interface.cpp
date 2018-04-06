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

#include "interface.h"
#include <string>
#include <queue>
#include <set>
#include <map>

namespace cs3505
{
    interface()
    {
        // do we want it on the stack or in the heap?? i dont think we need to use the heap

        // initialize the data structures
        // new clients (queue)
        new_clients = new std::queue<int>;

        // map < spreadsheet, list of clients connected to spreadsheet >
        map_of_clients = new std::map<int, int>;

        // disconnect (set)
        disconnect = new std::set<int>();

        // messages (queue)
        messages = new std::queue<std::string>();
    }

    // helper methods

} // end of class