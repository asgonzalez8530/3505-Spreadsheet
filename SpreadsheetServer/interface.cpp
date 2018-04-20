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
#include <unistd.h>
#include <iostream>

namespace cs3505
{
    /**
     * Constructor for the interface
     */
    interface::interface() {}

    /**
     * Destructor for the interface
     */
    interface::~interface() {}

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
    void interface::disconnect_add(int socket)
    {
        // lock
        disconnect.insert(socket);
    }

    /**
     * Locks the disconnect set and removes each client from the list, the server, & their spreadsheet. 
     */
    void interface::disconnect_clients()
    {
        // lock the disconnected list

        // for each client remove them from the list, the server, & their spreadsheet
        std::set<int>::iterator it;
        for (it = disconnect.begin(); it != disconnect.end(); it++)
        {
            // pull out the socket
            int socket = *it;

            // remove the socket from the disconnect list
            disconnect.erase(*it);

            // lock the spreadsheet

            // remove the client from the spreadsheet
            std::map<std::string, std::list<int>>::iterator it;
            for (it = map_of_spreadsheets.begin(); it != map_of_spreadsheets.end(); it++)
            {
                std::list<int> clients = it->second;

                // check to see if the client is in the list conenct to the spreadsheet
                std::list<int>::iterator j;
                for (j = clients.begin(); j != clients.end(); j++)
                {
                    if (*j == socket)
                    {
                        clients.remove(*j);
                        map_of_spreadsheets.insert(std::pair<std::string, std::list<int>> (it->first, clients));
                    }
                }
            }

            // now we close the socket and remove them from the server
            close(socket);
        }
    }

    /**
     * removes each client from the server 
     */
    void interface::disconnect_all()
    {
        // lock the spreadsheet

        // remove the client from the spreadsheet
        std::map<std::string, std::list<int>>::iterator it;
        for (it = map_of_spreadsheets.begin(); it != map_of_spreadsheets.end(); it++)
        {
            std::list<int> clients = it->second;

            // check to see if the client is in the list conenct to the spreadsheet
            std::list<int>::iterator j;
            for (j = clients.begin(); j != clients.end(); j++)
            {
                // now we close the socket and remove them from the server
                close(*j);
            }
        }
    }

    /**
     * send the disconnect message to each client
     */
    void interface::disconnecting()
    {
        std::string message = "disconnect " + (char)3;

        // lock the spreadsheet

        // remove the client from the spreadsheet
        std::map<std::string, std::list<int>>::iterator it;
        for (it = map_of_spreadsheets.begin(); it != map_of_spreadsheets.end(); it++)
        {
            std::list<int> clients = it->second;

            // check to see if the client is in the list conenct to the spreadsheet
            std::list<int>::iterator j;
            for (j = clients.begin(); j != clients.end(); j++)
            {
                propogate_to_client(*j, message);
            }
        }
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
     * Propogate the inputted message to all the clients connected to that spreadsheet
     */
	void interface::propogate_to_spreadsheet(std::string spreadsheet_name, std::string message)
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
	void interface::propogate_to_client(int socket, std::string message)
    {
        // lock
        // add the message to outgoing messages of the client
    }

    /**
     */
    void interface::propogate_full_state(std::map<std::string, std::string> * contents, int socket)
    {
        // build up the response message
        std::string result  = "full_state ";

        // propogate to the client the result response 
        propogate_to_client(socket, result);

        for(std::map<std::string, std::string>::iterator iter = contents->begin(); iter != contents->end(); iter++)
        {
            // get cell 
            result = iter->first;

            // propogate to the client the result response 
            propogate_to_client(socket, result);
            
            // get cell contents
            result = iter->second;

            // propogate to the client the result response 
            propogate_to_client(socket, result);
        }

        // propogate to the client the result response 
        propogate_to_client(socket, "" + (char) 3);
    }

    /**
     * Returns true if the spreadsheet already exists in the server
     */
    bool interface::spreadsheet_exists(std::string spreadsheet_name)
    {
        std::map<std::string, socket_list>::iterator location;
        location = map_of_spreadsheets.find(spreadsheet_name);
        return location != map_of_spreadsheets.end();
    }

    /**
     * Adds the inputted socket (client) to the spreadsheet
     */
    void interface::add_client(std::string spreadsheet_name, int socket)
    {
        std::map<std::string, socket_list>::iterator location;
        location = map_of_spreadsheets.find(spreadsheet_name);
        if (location != map_of_spreadsheets.end())
        {
            map_of_spreadsheets.at(spreadsheet_name).push_back(socket);
        }
    }

    /**
     * add the inputted spreadsheet to the list of spreadsheets
     */
    void interface::add_spreadsheet(std::string spreadsheet_name)
    {
        spreadsheet s(spreadsheet_name);
        all_spreadsheets.insert( std::pair<std::string, spreadsheet>(spreadsheet_name, s) );
        std::list<int> empty_list({});
        map_of_spreadsheets.insert(std::pair<std::string, std::list<int>>(spreadsheet_name, empty_list));
    }

    /**
     * returns the spreadsheet object that is associated with the inputted spreadsheet name
     */
    spreadsheet * interface::get_spreadsheet(std::string name)
    {
        std::map<std::string, spreadsheet>::iterator it;
        for (it = all_spreadsheets.begin(); it != all_spreadsheets.end(); it++)
        {
            if (it->first == name)
            {
                return &(it->second);
            }
        }
        return NULL;
    }

    std::string interface::get_spreadsheet(int socket)
    {
        // get the list of clients connected to each spreadsheet
        std::map<std::string, std::list<int>>::iterator it;
        for (it = map_of_spreadsheets.begin(); it != map_of_spreadsheets.end(); it++)
        {
            std::list<int> clients = it->second;

            // check to see if the client is in the list conenct to the spreadsheet
            std::list<int>::iterator j;
            for (j = clients.begin(); j != clients.end(); j++)
            {
                if (*j == socket)
                {
                    return it->first;
                }
            }
        }

        return "";
    }

    /**
     * stops receiveing messages from the sockets (clients) goes and propogates all the messages properly
     */
    void interface::stop_receiving_and_propogate_all_messages()
    {

    }

    /**
     * Saves the current states of all the spreadsheets to proper files
     */
    void interface::save_all_spreadsheets()
    {
        // iterate through each spreadsheet
        std::map<std::string, spreadsheet>::iterator it;
        for (it = all_spreadsheets.begin(); it != all_spreadsheets.end(); it++)
        {
            // save the spreadsheet
            spreadsheet s = it->second;
            s.save();
        }
    }


} // end of class
