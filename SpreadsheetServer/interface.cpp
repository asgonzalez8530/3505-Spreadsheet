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
 * v7: April 20, 2018
 */

#include "interface.h"
#include "spreadsheet.h"
#include <string>
#include <queue>
#include <set>
#include <map>
#include <unistd.h>
#include <iostream>
#include <regex>
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
    // bool interface::new_clients_isempty()
    // {
    //     bool flag;

    //     pthread_mutex_lock( &client_lock );
    //     // check to see if new clients is empty
    //     flag = new_clients.empty();
    //     pthread_mutex_unlock( &client_lock );

    //     return flag;
    // }

    /**
     * Adds the inputted client to the new client queue
     */
    // void interface::new_clients_add(int socket)
    // {
    //     pthread_mutex_lock( &client_lock );
    //     // add the new socket to the list
    //     new_clients.push(socket);
    //     pthread_mutex_unlock( &client_lock );
    // }

    /**
     * Locks the new clients queue and removes each client from the list and finishes the 
     * spreadsheet handshake. (may do the handshake stuff on a seperate thread???)
     */
    // void interface::new_clients_finish_handshake()
    // {
    //     pthread_mutex_lock( &client_lock );
    //     int socket = new_clients.front();
    //     new_clients.pop();

    //     // for each socket in the list

    //         // make new thread?
    //         // finish TCP and spreadsheet handshake
    //     pthread_mutex_unlock( &client_lock );
    // }

    /**
     * Returns true if there are no clients to disconnect. Otherwise returns false.
     */
    bool interface::disconnect_isempty()
    {
        bool flag;

        pthread_mutex_lock( &disconnect_lock );
        // check to see disconnect is empty
        flag = disconnect.empty();
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
        disconnect.push(socket);
        pthread_mutex_unlock( &disconnect_lock );
    }

    /**
     * Locks the disconnect set and removes each client from the list, the server, & their spreadsheet. 
     */
    void interface::disconnect_clients()
    {
        pthread_mutex_lock( &disconnect_lock );

        // for each client remove them from the list, the server, & their spreadsheet
        //std::set<int>::iterator it;
        //for (it = disconnect.begin(); it != disconnect.end(); it++)
        while (!disconnect.empty())
        {
            // pull out the socket
            int socket = disconnect.front();
            disconnect.pop();

            std::string str = "disconnect ";
            str.push_back((char)3);

            pthread_mutex_lock( &message_lock );
            Message msg;

            msg.socket = socket;
            msg.message = str;

            std::cout << "Disconnect!\n";
            messages.add_to_outbound(msg);
            pthread_mutex_unlock( &message_lock );

            pthread_mutex_lock( &spreadsheet_lock );

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
                        break;
                    }
                }
            }
            pthread_mutex_unlock( &spreadsheet_lock );
        }

        pthread_mutex_unlock( &disconnect_lock );
    }

    /**
     * removes each client from the server 
     */
    // void interface::disconnect_all()
    // {
    //     pthread_mutex_lock( &spreadsheet_lock );

    //     // for each spreadsheet in the map of spreadsheets
    //     std::map<std::string, std::list<int>>::iterator it;
    //     for (it = map_of_spreadsheets.begin(); it != map_of_spreadsheets.end(); it++)
    //     {
    //         std::list<int> clients = it->second;

    //         // check to see if the client is in the list conenct to the spreadsheet
    //         std::list<int>::iterator j;
    //         for (j = clients.begin(); j != clients.end(); j++)
    //         {
    //             // now we close the socket and remove them from the server
    //             close(*j);
    //         }
    //     }

    //     pthread_mutex_unlock( &spreadsheet_lock );
    // }

    /**
     * send the disconnect message to each client
     */
    void interface::disconnecting()
    {
        std::string message = "disconnect ";
        message.push_back((char)3);

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
     * removes the client from the spreadsheet and tells the other clients on the spreadsheet to 
     * unfocus the client
     */
    void interface::client_wants_to_disconnect(int socket)
    {
        // message to send 
        std::string unfocus = "unfocus " + std::to_string(socket);
        unfocus.push_back((char)3);
        
        // get the spreadsheet the client is connected to 
        std::string spreadsheet_name = get_spreadsheet(socket);

        // send to each client the unfocus message
        propogate_to_spreadsheet(spreadsheet_name, unfocus);
    }

    /**
     * Returns true if there are messages to process. Otherwise returns false.
     */
    // bool interface::messages_isempty()
    // {
    //     bool flag;

    //     pthread_mutex_lock( &message_lock );
    //     // checks to see if the message queue is empty
    //     flag = messages.empty();
    //     pthread_mutex_unlock( &message_lock );

    //     return flag;
    // }

    /**
     * Returns first message in the queue.
     */
    // std::string interface::get_message()
    // {
    //     std::string result;

    //     pthread_mutex_lock( &message_lock );
    //     // return and remove the first item in the message queue
    //     result = messages.front();
    //     pthread_mutex_unlock( &message_lock );
        
    //     return result;
    // }

    /**
     * Adds the inputted message to the message queue
     */
    // void interface::messages_add(std::string message)
    // {
    //     pthread_mutex_lock( &message_lock );
    //     // add message to the queue
    //     messages.push(message);
    //     pthread_mutex_unlock( &message_lock );
    // }

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
     * Propogate the inputted message to all the clients connected to that spreadsheet
     */
	void interface::propogate_to_spreadsheet_without_lock(std::string spreadsheet_name, std::string message)
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
                    propogate_to_client_without_a_lock(*j, message);
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
        // add the message to outgoing messages of the client
        add_to_outbound_messages(socket, message);
    }

    /**
     * adds the message to the outbound queue without locking the message object
     */
    void interface::propogate_to_client_without_a_lock(int socket, std::string message)
    {
        Message msg;
        msg.socket = socket;
        msg.message = message; 

        messages.add_to_outbound(msg);
    }

    /**
     * propogate all together the full state message to the client
     */
    void interface::propogate_full_state(std::map<std::string, std::string> * contents, int socket)
    {
        pthread_mutex_lock( &message_lock );

        // build up the response message
        std::string result  = "full_state ";

        // propogate to the client the result response 
        propogate_to_client_without_a_lock(socket, result);

        for(std::map<std::string, std::string>::iterator iter = contents->begin(); iter != contents->end(); iter++)
        {
            // get cell 
            result = iter->first;

            // propogate to the client the result response 
            propogate_to_client_without_a_lock(socket, result);
            
            // get cell contents
            result = iter->second;

            // propogate to the client the result response 
            propogate_to_client_without_a_lock(socket, result);
        }

        std:: string last_char = "";
        last_char.push_back((char)3);
        // propogate to the client the result response 
        propogate_to_client_without_a_lock(socket, last_char);

        pthread_mutex_unlock( &message_lock );
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
        pthread_mutex_lock( &message_lock );

        // parse all the inbound messages
        while (!messages.inbound_empty())
        {
            // get a message
            Message inbound = messages.next_inbound();

            // get the spreadsheet name that correlates to the inbound message socket
            std::string spreadsheet_name = get_spreadsheet(inbound.socket);
            // parse the message and have the server respond apporiately
            parse_and_respond_to_message_without_lock(spreadsheet_name, inbound.socket, inbound.message);
        }

        pthread_mutex_unlock( &message_lock );
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
    int interface::parse_and_respond_to_message(std::string spreadsheet_name, int socket, std::string message)
    {
        int ret_val = -1;
        // isolate the header 
        int position = message.find(" ");
        std::string header = message.substr(0, position + 1);

        // register
        if (std::regex_match(header, std::regex("register ")))
        {
            std::cout << "register message... preparing to respond\n";
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
            std::cout << "got filesnames... about to send\n";

            result.push_back((char)3);
            // propogate to the client the result response 
            propogate_to_client(socket, result);
        }
        // load
        else if (std::regex_match(header, std::regex("load ")))
        {
            std::cout << "got load message\n";
            // find where the message begins
            int p = message.find("load ");

            if (p + 6 >= message.length())
            {
                return ret_val;
            }

            // get the cell id
            std::string spreadsheet_name = message.substr(p + 6);

            // try to make a open spreadsheet
            try 
            {
                std::cout << "in try\n";
                if (spreadsheet_exists(spreadsheet_name))
                {
                    std::cout << "spreadsheet exists\n";
                    // add client 
                    add_client(spreadsheet_name, socket);

                    // get the spreadsheet object
                    spreadsheet * s = get_spreadsheet(spreadsheet_name);

                    // load full state (iterate)
                    std::map<std::string, std::string> contents = s->full_state();

                    propogate_full_state(&contents, socket);

                    // Check whether a ping loop is running
                    ret_val = socket;
                }
                else
                {
                    std::cout << "spreadsheet does NOT exists\n";
                    // add spreadsheet
                    add_spreadsheet(spreadsheet_name);

                    // add client
                    add_client(spreadsheet_name, socket);

                    // get the spreadsheet object
                    spreadsheet * s = get_spreadsheet(spreadsheet_name);
                    
                    // load full state (iterate)
                    std::map<std::string, std::string> contents = s->full_state();
                    propogate_full_state(&contents, socket);

                    ret_val = socket;
                }
            }
            catch (...)
            {
                std::cout << "in catch... something went wrong!\n";
                // propogate to the client the file error message response 
                std::string result = "file_load_error ";
                result.push_back((char)3);
                propogate_to_client(socket, result);
            }
        }

        // edit
        else if (std::regex_match(header, std::regex("edit ")))
        {
            std::cout << "got edit message\n";
            // find where the message begins
            int p = message.find("edit ");

            // remove white space at the beginning of the message
            std::string cleaned_up_message = message.substr(p);

            // get the spreadsheet object
            spreadsheet * s = get_spreadsheet(spreadsheet_name);

            // ignore the message
            if (s == NULL)
            {
                return ret_val;
            }

            // update spreadsheet with the change 
            std::string result = s->update(cleaned_up_message);
            result.push_back((char)3);

            // propgate the result to the other clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // focus
        else if (std::regex_match(header, std::regex("focus ")))
        {
            std::cout << "got focus message\n";
            // find where the message begins
            int p = message.find("focus ");

            if (p + 6 >= message.length())
            {
                return ret_val;
            }

            // get the cell id
            std::string cell_id = message.substr(p + 6);
            std::cout << "cell id" << cell_id << "\n";

            // build up the response message
            std::string result  = "focus ";
            result += cell_id + ":" + std::to_string(socket);
            result.push_back((char)3);

            std::cout << result << "\n";
            
            // propogate the message to all the clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // unfocus
        else if (std::regex_match(header, std::regex("unfocus ")))
        {
            std::cout << "got unfocus message\n";
            // build up the response message
            std::string result  = "unfocus ";
            result += std::to_string(socket);
            result.push_back((char)3);

            std::cout << result << std::endl;
            
            // propogate the message to all the clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // undo
        else if (std::regex_match(header, std::regex("undo ")))
        {
            std::cout << "got undo message\n";
            // find where the message begins
            int p = message.find("undo ");

            // remove white space at the beginning of the message
            std::string cleaned_up_message = message.substr(p);

            // get the spreadsheet object
            spreadsheet * s = get_spreadsheet(spreadsheet_name);

            // ignore the message
            if (s == NULL)
            {
                return ret_val;
            }

            // update spreadsheet with the change 
            std::string result = s->update(cleaned_up_message);
            result.push_back((char)3);

            // propgate the result to the other clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // revert
        else if (std::regex_match(header, std::regex("revert ")))
        {
            std::cout << "got revert message\n";
            // find where the message begins
            int p = message.find("revert ");

            // remove white space at the beginning of the message
            std::string cleaned_up_message = message.substr(p);

            // get the spreadsheet object
            spreadsheet * s = get_spreadsheet(spreadsheet_name);
            
            // ignore the message
            if (s == NULL)
            {
                return ret_val;
            }

            // update spreadsheet with the change 
            std::string result = s->update(cleaned_up_message);
            result.push_back((char)3);

            // propgate the result to the other clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }
        // else not a valid message so we do nothing
        return ret_val;
    }

    /**
     * parses the inputted message. And determines if its a valid message.
     * Implements the servers response to the message.
     */
    void interface::parse_and_respond_to_message_without_lock(std::string spreadsheet_name, int socket, std::string message)
    {
        // isolate the header 
        int position = message.find(" ");
        std::string header = message.substr(0, position + 1);

        // register
        if (std::regex_match(header, std::regex("register ")))
        {
            std::cout << "register message... preparing to respond\n";
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
            std::cout << "got filesnames... about to send\n";

            result.push_back((char)3);
            // propogate to the client the result response 
            propogate_to_client(socket, result);
        }
        // load
        else if (std::regex_match(header, std::regex("load ")))
        {
            std::cout << "got load message\n";
            // find where the message begins
            int p = message.find("load ");

            if (p + 6 >= message.length())
            {
                return;
            }

            // get the cell id
            std::string spreadsheet_name = message.substr(p + 6);

            // try to make a open spreadsheet
            try 
            {
                std::cout << "in try\n";
                if (spreadsheet_exists(spreadsheet_name))
                {
                    std::cout << "spreadsheet exists\n";
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
                    std::cout << "spreadsheet does NOT exists\n";
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
                std::cout << "in catch... something went wrong!\n";
                // propogate to the client the file error message response 
                std::string result = "file_load_error ";
                result.push_back((char)3);
                propogate_to_client(socket, result);
            }
        }

        // edit
        else if (std::regex_match(header, std::regex("edit ")))
        {
            std::cout << "got edit message\n";
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
            result.push_back((char)3);

            // propgate the result to the other clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // focus
        else if (std::regex_match(header, std::regex("focus ")))
        {
            std::cout << "got focus message\n";
            // find where the message begins
            int p = message.find("focus ");

            if (p + 6 >= message.length())
            {
                return;
            }

            // get the cell id
            std::string cell_id = message.substr(p + 6);
            std::cout << "cell id" << cell_id << "\n";

            // build up the response message
            std::string result  = "focus ";
            result += cell_id + ":" + std::to_string(socket);
            result.push_back((char)3);

            std::cout << result << "\n";
            
            // propogate the message to all the clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // unfocus
        else if (std::regex_match(header, std::regex("unfocus ")))
        {
            std::cout << "got unfocus message\n";
            // build up the response message
            std::string result  = "unfocus ";
            result += std::to_string(socket);
            result.push_back((char)3);

            std::cout << result << std::endl;
            
            // propogate the message to all the clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // undo
        else if (std::regex_match(header, std::regex("undo ")))
        {
            std::cout << "got undo message\n";
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
            result.push_back((char)3);

            // propgate the result to the other clients in the spreadsheet
            propogate_to_spreadsheet(spreadsheet_name, result);
        }

        // revert
        else if (std::regex_match(header, std::regex("revert ")))
        {
            std::cout << "got revert message\n";
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
            result.push_back((char)3);

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

    /**
     * returns true if there is nothing in the outbound queue
     */
    bool interface::outbound_empty()
    {
        bool flag;

        pthread_mutex_lock( &message_lock );

        flag = messages.outbound_empty();

        pthread_mutex_unlock( &message_lock );

        return flag;
    }

    /**
     * pulls out on message from the outbound queue and sends it using the indicated socket
     */
    void interface::send_message()
    {
        pthread_mutex_lock( &message_lock );

        // pop 
        Message msg = messages.next_outbound();

        // send 
        messages.send_message(msg);

        pthread_mutex_unlock( &message_lock );
    }

    /**
     * add message to outbound messages
     */
    void interface::add_to_outbound_messages(int socket, std::string message)
    {
        pthread_mutex_lock( &message_lock );

        Message msg;
        msg.socket = socket;
        msg.message = message; 

        messages.add_to_outbound(msg);
        
        pthread_mutex_unlock( &message_lock );
    }

    /**
     * returns true if there is nothing in the inbound queue
     */
    bool interface::inbound_empty()
    {
        bool flag;

        pthread_mutex_lock( &message_lock );

        flag = messages.inbound_empty();

        pthread_mutex_unlock( &message_lock );

        return flag;
    }

    /**
     * gets the next message in the inbound message queue
     */
    int interface::get_inbound_message_parse_and_respond()
    {
        Message inbound;
        
        pthread_mutex_lock( &message_lock );

        inbound = messages.next_inbound();

        pthread_mutex_unlock( &message_lock );

        // get the spreadsheet name that coorelates to the socket
        std::string spreadsheet_name = get_spreadsheet(inbound.socket);
        // parse the message and have the server respond apporiately
        int result = parse_and_respond_to_message(spreadsheet_name, inbound.socket, inbound.message);

        return result;
    }

    /**
     * add the message and socket to the inbound queue of messages
     */
    void interface::add_to_inbound_messages(int socket, std::string message)
    {
        pthread_mutex_lock( &message_lock );

        Message msg;
        msg.socket = socket;
        msg.message = message;

        messages.add_to_inbound(msg);

        pthread_mutex_unlock( &message_lock );
    }


} // end of class
