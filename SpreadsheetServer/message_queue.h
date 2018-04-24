/*
 * A simple class that handles inbound and outbound messages.
 * Keeps track of the life of a message while in the server. 
 * Stores the socket (client) and the message that was from the socket
 * 
 * Pineapple upside down cake
 */

#ifndef MESSAGE_QUEUE_H
#define MESSAGE_QUEUE_H

#include <string>
#include <queue>
#include <pthread.h>

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
		void add_to_outbound(int, std::string &);

		// Enqueue this message object to send to the client later.
		void add_to_outbound(Message);

		// Return the next message to send.
		Message next_outbound();

		// Enqueue this message parse and process later.
		void add_to_inbound(Message);

		// Return the next message to process.
		Message next_inbound();

		// Return whether outbound is empty
        bool outbound_empty();

		// Return whether inbound is empty
        bool inbound_empty();

        // Sends a message to a specific socket
        void send_message(Message);

	};
}

#endif
