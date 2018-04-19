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

#include "interface.h"
#include "spreadsheet.h"
#include <string>
#include <queue>
#include <set>
#include <map>
#include <iostream>

namespace cs3505
{
    interface::interface() 
    {
		new_clients = std::queue<int>();
        map_of_clients = std::map<spreadsheet, int>();
        disconnect = std::set<int>();
        messages = std::queue<std::string>();
    }


    /**
     * Destructor for the interface
     */
    interface::~interface() 
    {

    }

    /**
     * Returns true if there are clients to client. Otherwise returns false. 
     */
    bool interface::new_clients_isempty()
    {
        return new_clients.empty();
    }

    /**
     * Adds the inputted client to the new client queue
     */
    void interface::new_clients_add(int client)
    {
        new_clients.push(client);
    }

    /**
     * Locks the new clients queue and removes each client from the list and finishes the 
     * spreadsheet handshake. (may do the handshake stuff on a seperate thread???)
     */
    void interface::new_clients_finish_handshake()
    {
        // lock the new_clients list 

        // for each socket in the list

            // make new thread?
            // finish TCP and spreadsheet handshake
    }

    /**
     * Returns true if there are no clients to disconnect. Otherwise returns false.
     */
    bool interface::disconnect_isempty()
    {
        return disconnect.empty();
    }

    /**
     * Adds the inputted client to the disconnect set
     */
    void interface::disconnect_add(int client)
    {
        // lock
        disconnect.insert(client);
    }

    /**
     * Locks the disconnect set and removes each client from the list, the server, & their spreadsheet. 
     */
    void interface::disconnect_clients()
    {
        // lock the disconnected list

        // for each client 
            // remove them from the list, the server, & their spreadsheet. 
    }

    /**
     * Returns true if there are messages to process. Otherwise returns false.
     */
    bool interface::messages_isempty()
    {
        return messages.empty();
    }

    /**
     * Returns first message in the queue.
     */
    std::string interface::get_message()
    {
        return messages.front();
    }

    /**
     * Adds the inputted message to the message queue
     */
    void interface::messages_add(std::string message)
    {
        // lock 
        messages.push(message);
    }

    /**
     * Register socket in flag map
     */
    void interface::flag_map_add(int socket)
    {
        //mtx.lock();
        ping_flags[socket] = 0;
        //mtx.unlock();
    }

    /**
     * Register socket in flag map
     */
    void interface::flag_map_remove(int socket)
    {
        //mtx.lock();
        ping_flags.erase(socket);
        //mtx.unlock();
    }

    /**
     * Checks ping_flags for a response
     */
    int interface::check_ping_response(int socket)
	{
        //mtx.lock();
        if(ping_flags[socket] == 1)
        {
            ping_flags[socket] = 0;
            return true;
        }
        else
        {
		    return false;
        }
        //mtx.unlock();
	}

    /**
     * Send a ping to the passed socket
     */
	void interface::send_ping(int socket)
    {
    }


    /**
     * Update socket ping response flag
     */
    void interface::ping_received(int socket)
    {
        std::cout << "MADE IT IN" << "\n";
        //mtx.lock();
        ping_flags.insert(std::pair<int ,int>(socket, 1));
        //mtx.unlock();
        std::cout << "MADE IT OUT\n";
    }

    /**
     * Propogate the inputted message to all the clients connected to that spreadsheet
     */
	void interface::propogate_to_spreadsheet(spreadsheet * s, std::string message)
    {
        //int clients = map_of_clients[s];
        //map_of_clients.find(s)->second;
        // std::map<spreadsheet, int>::const_iterator pos = map_of_clients.find(*s);
        // if (pos != map_of_clients.end()) 
        // {
        //     int clients = pos->second;
        // }

        // get the client list using the spreadsheet and map_of_clients 

        // lock
        // add the message to outgoing messages of the client
    }
    
    /**
     * Propogate the inputted message to the clients passed in
     */
	void interface::propogate_to_client(int client, std::string message)
    {
        // lock
        // add the message to outgoing messages of the client
    }

} // end of class
