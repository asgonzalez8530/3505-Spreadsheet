/*
 * A simple class that stores information
 * about this connection with this client.
 *
 */

#ifndef SOCKET_CONNECTION_H
#define SOCKET_CONNECTION_H

#include <string>
#include <queue>

namespace cs3505
{
	class socket_connection
	{
	private:
	
		// The TCP socket we're streaming over.
		int socket;

		// The queue of messages the Server wants to send to this client.
		std::queue<std::string> outboundMessages;

		// The queue of messages from the client that the Server needs to process.
		std::queue<std::string> inboundMessages;
	
	public:

		// Constructor
		socket_connection(int socket);

		// Enqueue this message to send to the client later.
		void add_to_outbound(std::string message);

		// Return the next message to send.
		std::string next_to_send(std::string message);

		// Enqueue this message parse and process later.
		void add_to_inbound(std::string message);

		// Return the next message to process.
		std::string next_to_process(std::string message);
	};
}

#endif
