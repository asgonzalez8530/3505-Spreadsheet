/*
 * A simple class that stores information
 * about this connection with this client.
 *
 * 
 */

#include "socket_connection.h"

namespace cs3505
{
/*
private:
	
	// The TCP socket we're streaming over.
	int socket;

	// The queue of messages the Server wants to send to the client.
	std::queue<string> outboundMessages;

	// The queue of messages from the client that the Server needs to process.
	std::queue<string> inboundMessages;

public:
*/

	// Constructor
	socket_connection::socket_connection(int socket)
	{
		this->socket = socket;
		std::queue<std::string> outboundMessages; // TODO is this how to initialize these? 
		std::queue<std::string> inboundMessages;
	}


	// Enqueue this message to send to the client later.
	void socket_connection::add_to_outbound(std::string& message)
	{
		outboundMessages.push(message);
	}

	// Return the next message to send.
	std::string socket_connection::next_to_send(std::string& message)
	{
		std::string nextMessage = outboundMessages.front();
		outboundMessages.pop();
		return nextMessage;
	}

	// Enqueue this message parse and process later.
	void socket_connection::add_to_inbound(std::string& message)	
	{
		inboundMessages.push(message);
	}

	// Return the next message to process.
	std::string socket_connection::next_to_process(std::string& message)
	{
		std::string nextMessage = inboundMessages.front();
		inboundMessages.pop();
		return nextMessage;
	}
}
