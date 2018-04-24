/*
 * A simple class that handles inbound and
 * outbound messages.
 * 
 * v7: April 20, 2018
 */

#include "message_queue.h"
#include <queue>
#include <regex>
#include <unistd.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <iostream>

namespace cs3505
{

	// Constructor
	message_queue::message_queue()
	{

	}

	// Destructor
	message_queue::~message_queue()
	{

	}


	/**
	 * Enqueue this message to send to the client later.
	 * 
	 */
	void message_queue::add_to_outbound(int socket, std::string & message)
	{
		Message msg;
		msg.socket = socket;
		msg.message = message;
		outboundMessages.push(msg);
	}

	/* 
	 * Enqueue this message to send to the client later.
	 *
	 */
	void message_queue::add_to_outbound(Message msg)
	{
		outboundMessages.push(msg);
	}

	/**
	 * Return the next message to send.
	 * 
	 */
	Message message_queue::next_outbound()
	{
		Message nextMessage = outboundMessages.front();
		outboundMessages.pop();
		return nextMessage;
	}

	/**
	 * Enqueue this message to parse and process later.
	 * 
	 */
	void message_queue::add_to_inbound(Message msg)	
	{
		inboundMessages.push(msg);
	}

	/** 
	 * Return the next message to process.
	 * 
	 */
	Message message_queue::next_inbound()
	{
		Message nextMessage = inboundMessages.front();
		inboundMessages.pop();
		return nextMessage;
	}

    bool message_queue::outbound_empty()
    {
        return outboundMessages.empty();
    }

    bool message_queue::inbound_empty()
    {
        return inboundMessages.empty();
    }

    void message_queue::send_message(Message message)
    {
		// get socket and message to send
        int socket = message.socket;
        std::string tmp = message.message;
		//std::cout << "Sending " << tmp << " on socket " << socket << "\n";

		// Send the message
        write(socket, tmp.c_str(), tmp.length());
    }
}
