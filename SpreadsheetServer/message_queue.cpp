/*
 * A simple class that handles inbound and
 * outbound messages.
 * 
 */

#include "message_queue.h"
#include <queue>

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


	// Enqueue this message to send to the client later.
	void message_queue::add_to_outbound(int socket, std::string & message)
	{
		Message msg;
		msg.socket = socket;
		msg.message = message;
		outboundMessages.push(msg);
	}

	// Return the next message to send.
	Message message_queue::next_outbound()
	{
		Message nextMessage = outboundMessages.front();
		outboundMessages.pop();
		return nextMessage;
	}

	// Enqueue this message to parse and process later.
	void message_queue::add_to_inbound(int socket, std::string & message)	
	{
		Message msg;
		msg.socket = socket;
		msg.message = message;
		inboundMessages.push(msg);
	}

	// Return the next message to process.
	Message message_queue::next_inbound()
	{
		Message nextMessage = inboundMessages.front();
		inboundMessages.pop();
		return nextMessage;
	}
}
