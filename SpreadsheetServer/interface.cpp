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
#include <boost/archive/text_iarchive.hpp>
#include <boost/archive/text_oarchive.hpp>
#include <boost/serialization/map.hpp>
#include <boost/serialization/deque.hpp>
#include <boost/serialization/stack.hpp>
#include <boost/filesystem.hpp>
#include <boost/filesystem/fstream.hpp>

namespace cs3505
{
    /**
     * Constructor for the interface
     */
    interface::interface() 
	{
		map_lock = PTHREAD_MUTEX_INITIALIZER;
		queue_lock = PTHREAD_MUTEX_INITIALIZER;
		client_lock = PTHREAD_MUTEX_INITIALIZER;
		message_lock = PTHREAD_MUTEX_INITIALIZER;
		disconnect_lock = PTHREAD_MUTEX_INITIALIZER;
		spreadsheet_lock = PTHREAD_MUTEX_INITIALIZER;
	}

    /**
     * Destructor for the interface
     */
    interface::~interface() {}

    /**
     * Returns true if there are clients to client. Otherwise returns false. 
     */
    bool interface::new_clients_isempty()
    {
        bool flag;

        pthread_mutex_lock( &client_lock );
        // check to see if new clients is empty
        flag = new_clients.empty();
        pthread_mutex_unlock( &client_lock );

        return flag;
    }

    /**
     * Adds the inputted client to the new client queue
     */
    void interface::new_clients_add(int socket)
    {
        pthread_mutex_lock( &client_lock );
        // add the new socket to the list
        new_clients.push(socket);
        pthread_mutex_unlock( &client_lock );
    }

    /**
     * Locks the new clients queue and removes each client from the list and finishes the 
     * spreadsheet handshake. (may do the handshake stuff on a seperate thread???)
     */
    void interface::new_clients_finish_handshake()
    {
        pthread_mutex_lock( &client_lock );
        int socket = new_clients.front();
        new_clients.pop();

        // for each socket in the list

            // make new thread?
            // finish TCP and spreadsheet handshake
        pthread_mutex_unlock( &client_lock );
    }

    /**
     * Returns true if there are no clients to disconnect. Otherwise returns false.
     */
    bool interface::disconnect_isempty()
    {
        bool flag;

        pthread_mutex_lock( &disconnect_lock );
        // check to see disconnect is empty
        disconnect.empty();
        pthread_mutex_unlock( &disconnect_lock );

        return flag;
    }

    /**
     * Adds the inputted client to the disconnect set
     */
    void interface::disconnect_add(int socket)
    {
        pthread_mutex_lock( &disconnect_lock );
        // insert the socket to disconnect list
        disconnect.insert(socket);
        pthread_mutex_unlock( &disconnect_lock );
    }

    /**
     * Locks the disconnect set and removes each client from the list, the server, & their spreadsheet. 
     */
    void interface::disconnect_clients()
    {
        pthread_mutex_lock( &disconnect_lock );

        // for each client remove them from the list, the server, & their spreadsheet
        std::set<int>::iterator it;
        for (it = disconnect.begin(); it != disconnect.end(); it++)
        {
            // pull out the socket
            int socket = *it;

            // remove the socket from the disconnect list
            disconnect.erase(*it);

            // lock map of spreadsheets

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

        pthread_mutex_unlock( &disconnect_lock );
    }

    /**
     * removes each client from the server 
     */
    void interface::disconnect_all()
    {
        pthread_mutex_lock( &spreadsheet_lock );

        // for each spreadsheet in the map of spreadsheets
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

        pthread_mutex_unlock( &spreadsheet_lock );
    }

    /**
     * send the disconnect message to each client
     */
    void interface::disconnecting()
    {
        std::string message = "disconnect " + (char)3;

        pthread_mutex_lock( &spreadsheet_lock );

        // for each client in each spreadsheet send them the disconnect message
        std::map<std::string, std::list<int>>::iterator it;
        for (it = map_of_spreadsheets.begin(); it != map_of_spreadsheets.end(); it++)
        {
            std::list<int> clients = it->second;

            std::list<int>::iterator j;
            for (j = clients.begin(); j != clients.end(); j++)
            {
                propogate_to_client(*j, message);
            }
        }

        pthread_mutex_unlock( &spreadsheet_lock );
    }

    /**
     * Returns true if there are messages to process. Otherwise returns false.
     */
    bool interface::messages_isempty()
    {
        bool flag;

        pthread_mutex_lock( &message_lock );
        // checks to see if the message queue is empty
        flag = messages.empty();
        pthread_mutex_unlock( &message_lock );

        return flag;
    }

    /**
     * Returns first message in the queue.
     */
    std::string interface::get_message()
    {
        std::string result;

        pthread_mutex_lock( &message_lock );
        // return and remove the first item in the message queue
        result = messages.front();
        pthread_mutex_unlock( &message_lock );
        
        return result;
    }

    /**
     * Adds the inputted message to the message queue
     */
    void interface::messages_add(std::string message)
    {
        pthread_mutex_lock( &message_lock );
        // add message to the queue
        messages.push(message);
        pthread_mutex_unlock( &message_lock );
    }

    /**
     * Propogate the inputted message to all the clients connected to that spreadsheet
     */
	void interface::propogate_to_spreadsheet(std::string spreadsheet_name, std::string message)
    {
        pthread_mutex_lock( &spreadsheet_lock );

        // get the list of clients connected to each spreadsheet
        std::map<std::string, std::list<int>>::iterator it;
        for (it = map_of_spreadsheets.begin(); it != map_of_spreadsheets.end(); it++)
        {
            if (it->first.compare(spreadsheet_name) == 0)
            {
                std::list<int> clients = it->second;

                // send the message to each client in the list
                std::list<int>::iterator j;
                for (j = clients.begin(); j != clients.end(); j++)
                {
                    propogate_to_client(*j, message);
                }
            }
        }
        pthread_mutex_unlock( &spreadsheet_lock );
    }
    
    /**
     * Propogate the inputted message to the clients passed in
     */
	void interface::propogate_to_client(int socket, std::string message)
    {
        // lock the outgoing messages

        // add the message to outgoing messages of the client
    }

    /**
     * propogate all together the full state message to the client
     */
    void interface::propogate_full_state(std::map<std::string, std::string> * contents, int socket)
    {
        // lock outbound messages (may cause a deadlock?)

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
        bool flag;

        pthread_mutex_lock( &spreadsheet_lock );
        // trys to see if the spreadsheet is in the server
        std::map<std::string, socket_list>::iterator location;
        location = map_of_spreadsheets.find(spreadsheet_name);
        flag = location != map_of_spreadsheets.end();
        pthread_mutex_unlock( &spreadsheet_lock );
        
        return flag;
    }

    /**
     * Adds the inputted socket (client) to the spreadsheet
     */
    void interface::add_client(std::string spreadsheet_name, int socket)
    {
        pthread_mutex_lock( &spreadsheet_lock );
        // iterate through the spreadsheets and add the client to the proper spreadsheet
        std::map<std::string, socket_list>::iterator location;
        location = map_of_spreadsheets.find(spreadsheet_name);
        if (location != map_of_spreadsheets.end())
        {
            map_of_spreadsheets.at(spreadsheet_name).push_back(socket);
        }
        pthread_mutex_unlock( &spreadsheet_lock );
    }

    /**
     * add the inputted spreadsheet to the list of spreadsheets
     */
    void interface::add_spreadsheet(std::string spreadsheet_name)
    {
        pthread_mutex_lock( &spreadsheet_lock );

        // add the spreadsheet to all spreadsheet list and map of spreadsheets with no clients
        spreadsheet s(spreadsheet_name);
        all_spreadsheets.insert( std::pair<std::string, spreadsheet>(spreadsheet_name, s) );
        // unlock and lock map of spreadsheets

        std::list<int> empty_list({});
        map_of_spreadsheets.insert(std::pair<std::string, std::list<int>>(spreadsheet_name, empty_list));
        pthread_mutex_unlock( &spreadsheet_lock );
    }

    /**
     * returns the spreadsheet object that is associated with the inputted spreadsheet name
     */
    spreadsheet * interface::get_spreadsheet(std::string name)
    {
        spreadsheet * ptr = NULL;

        pthread_mutex_lock( &spreadsheet_lock );
        // iterate through trying to find the spreadsheet object
        std::map<std::string, spreadsheet>::iterator it;
        for (it = all_spreadsheets.begin(); it != all_spreadsheets.end(); it++)
        {
            if (it->first == name)
            {
                ptr = &(it->second);
            }
        }
        pthread_mutex_unlock( &spreadsheet_lock );

        return ptr;
    }

    std::string interface::get_spreadsheet(int socket)
    {
        std::string result = "";

        pthread_mutex_lock( &spreadsheet_lock );
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
                    result = it->first;
                }
            }
        }
        pthread_mutex_unlock( &spreadsheet_lock );

        return result;
    }

    /**
     * stops receiveing messages from the sockets (clients) goes and propogates all the messages properly
     */
    void interface::stop_receiving_and_propogate_all_messages()
    {
        // lock messages (may cause a deadlock?)

        // parse and propogate the message
        
    }

    /**
     * Saves the current states of all the spreadsheets to proper files
     */
    void interface::save_all_spreadsheets()
    {
        pthread_mutex_lock( &spreadsheet_lock );
        // iterate through each spreadsheet
        std::map<std::string, spreadsheet>::iterator it;
        for (it = all_spreadsheets.begin(); it != all_spreadsheets.end(); it++)
        {
            // save the spreadsheet
            spreadsheet s = it->second;
            s.save();
        }
        pthread_mutex_unlock( &spreadsheet_lock );
    }

    /**
     * parses the inputted message. And determines if its a valid message.
     * Implements the servers response to the message.
     */
    void interface::parse_and_respond_to_message(std::string spreadsheet_name, int socket, std::string message)
    {
        // register
        if (message.find("register ") > 0)
        {
            // find where the message begins
            int p = message.find("register ");

            std::set<std::string> file_names = get_spreadsheet_names();

            // build of the response
            std::string result = "connect_accepted ";

            if (!file_names.empty())
            {
                // get all the available spreadsheets
                for(std::set<std::string>::iterator iter = file_names.begin(); iter != file_names.end(); iter++)
                {
                    result += *iter;
                    result += "\n";
                }
            }
            result += (char) 3;

            // propogate to the client the result response 
            propogate_to_client(socket, result);
        }

        // load
        else if (message.find("load ") > 0)
        {
            // find where the message begins
            int p = message.find("load ");

            // get the cell id
            std::string spreadsheet_name = message.substr(p + 6);

            // try to make a open spreadsheet
            try 
            {
                if (spreadsheet_exists(spreadsheet_name))
                {
                    // add client 
                    add_client(spreadsheet_name, socket);

                    // get the spreadsheet object
                    spreadsheet * s = get_spreadsheet(spreadsheet_name);

                    // load full state (iterate)
                    std::map<std::string, std::string> contents = s->full_state();

                    propogate_full_state(&contents, socket);
                }
                else
                {
                    // add spreadsheet
                    add_spreadsheet(spreadsheet_name);

                    // add client
                    add_client(spreadsheet_name, socket);

                    // get the spreadsheet object
                    spreadsheet * s = get_spreadsheet(spreadsheet_name);
                    
                    // load full state (iterate)
                    std::map<std::string, std::string> contents = s->full_state();
                    propogate_full_state(&contents, socket);
                }
            }
            catch (...)
            {
                // propogate to the client the file error message response 
                propogate_to_client(socket, "file_load_error" + (char) 3);
            }
        }

        // edit
        else if (message.find("edit ") > 0)
        {
            // find where the message begins
            int p = message.find("edit ");

            // remove white space at the beginning of the message
            std::string cleaned_up_message = message.substr(p);

            // get the spreadsheet object
            spreadsheet * s = get_spreadsheet(spreadsheet_name);

            // ignore the message
            if (s == NULL)
            {
                return;
            }

            // update spreadsheet with the change 
            std::string result = s->update(cleaned_up_message);

            // propgate the result to the other clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // focus
        else if (message.find("focus ") > 0)
        {
            // find where the message begins
            int p = message.find("focus ");

            // get the cell id
            std::string cell_id = message.substr(p + 6);

            // build up the response message
            std::string result  = "focus ";
            result += message.substr(p + 6) + ":" + std::to_string(socket);
            
            // propogate the message to all the clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // unfocus
        else if (message.find("unfocus ") > 0)
        {
            // find where the message begins
            int p = message.find("unfocus ");
        
            // build up the response message
            std::string result  = "unfocus ";
            result += std::to_string(socket);
            
            // propogate the message to all the clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // undo
        else if (message.find("undo ") > 0)
        {
            // find where the message begins
            int p = message.find("undo ");

            // remove white space at the beginning of the message
            std::string cleaned_up_message = message.substr(p);

            // get the spreadsheet object
            spreadsheet * s = get_spreadsheet(spreadsheet_name);

            // ignore the message
            if (s == NULL)
            {
                return;
            }

            // update spreadsheet with the change 
            std::string result = s->update(cleaned_up_message);

            // propgate the result to the other clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // revert
        else if (message.find("revert ") > 0)
        {
            // find where the message begins
            int p = message.find("revert ");

            // remove white space at the beginning of the message
            std::string cleaned_up_message = message.substr(p);

            // get the spreadsheet object
            spreadsheet * s = get_spreadsheet(spreadsheet_name);
            
            // ignore the message
            if (s == NULL)
            {
                return;
            }

            // update spreadsheet with the change 
            std::string result = s->update(cleaned_up_message);

            // propgate the result to the other clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // else not a valid message so we do nothing
    }

    /**
     * Gets all the spreadsheets inside the spreadsheet directory
     */
    std::set<std::string> interface::get_spreadsheet_names()
    {
        boost::filesystem::path directory(boost::filesystem::current_path() / (const boost::filesystem::path&)("Spreadsheets"));
        
        std::set<std::string> meSprds;

        if(boost::filesystem::is_directory(directory))
        {	
            for(boost::filesystem::directory_iterator rator(directory); rator != boost::filesystem::directory_iterator(); rator++)	
            {
                boost::filesystem::directory_entry file = *rator;
                std::string filename = ((boost::filesystem::path)file).filename().string();
                std::string next = filename.substr(0, filename.length() - 11);

                meSprds.insert(next);
            }
        }

        return meSprds;
    }


} // end of class
