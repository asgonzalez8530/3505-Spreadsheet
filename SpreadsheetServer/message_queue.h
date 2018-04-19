/*
 * A simple class that handles inbound and
 * outbound messages.
 * 
 */

#ifndef MESSAGE_QUEUE_H
#define MESSAGE_QUEUE_H

#include <string>
#include <queue>

namespace cs3505
{
	// this struct represents a message from a socket
	typedef struct _message
	{
		int socket;
		std::string message;

	} Message;


	class message_queue
	{
	private:

		// The queue of messages the Server wants to send to a client.
		std::queue<Message> outboundMessages;

		// The queue of client messages that the Server needs to process.
		std::queue<Message> inboundMessages;
	
	public:

		// Constructor
		message_queue();

		// Destructor
		~message_queue();

		// Enqueue this message to send to the client later.
		void add_to_outbound(int socket, std::string & message);

		// Return the next message to send.
		Message next_outbound();

		// Enqueue this message parse and process later.
		void add_to_inbound(int socket, std::string & message);

		// Return the next message to process.
		Message next_inbound();
	};
}

#endif
