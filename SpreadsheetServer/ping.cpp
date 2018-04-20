
#include "ping.h"
#include <pthread.h>
#include <iostream>
#include <map>


namespace cs3505
{
    ping::ping()
    {
        lock = PTHREAD_MUTEX_INITIALIZER;
    }

    ping::~ping()
	{
	}

    ping::ping(const ping & other)
    {
        this->ping_flags = other.ping_flags;
        this->lock = other.lock;
    }

    /**
     * Register socket in flag map
     */
    void ping::flag_map_add(int socket)
    {
        pthread_mutex_lock( &lock );
        std::cout << "Flag map add!\n";
        ping_flags[socket] = 0;
        pthread_mutex_unlock( &lock );

    }

    /**
     * Register socket in flag map
     */
    void ping::flag_map_remove(int socket)
    {
        pthread_mutex_lock( &lock );
        std::cout << "Flag map remove!\n";
        ping_flags.erase(socket);
        pthread_mutex_unlock( &lock );
    }

    /**
     * Checks ping_flags for a response
     */
    int ping::check_ping_response(int socket)
	{
        pthread_mutex_lock( &lock );
        std::cout << "Check ping response\n";
        if(ping_flags[socket] == 1)
        {
            ping_flags[socket] = 0;
            return true;
        }
        else
        {
		    return false;
        }
        pthread_mutex_unlock( &lock );
	}

    /**
     * Send a ping to the passed socket
     */
	void ping::send_ping(int socket)
    {
        pthread_mutex_lock( &lock );
		std::cout << "Send ping\n";
        pthread_mutex_unlock( &lock );
    }


    /**
     * Update socket ping response flag
     */
    void ping::ping_received(int socket)
    {
        pthread_mutex_lock( &lock );
        ping_flags[socket] = 1;
        pthread_mutex_unlock( &lock );
    }

}

