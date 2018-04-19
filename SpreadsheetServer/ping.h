
#ifndef PING_H
#define PING_H

#include <pthread.h>
#include <map>

typedef std::map<int, int> ping_f;

namespace cs3505
{
	class ping
	{
		private:
		    // map of ping flags for sockets
			ping_f ping_flags;

		    pthread_mutex_t lock;

		public:
		    ping();
		    ~ping();
		    ping(const ping & other);

		    /* Ping control functions */
		    void flag_map_add(int socket);
		    void flag_map_remove(int socket);
			int check_ping_response(int socket);
			void send_ping(int socket);
		    void ping_received(int socket);


	};
}

#endif

