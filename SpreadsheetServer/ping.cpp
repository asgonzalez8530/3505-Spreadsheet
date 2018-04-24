/**
 * This class helps a server handle pinging and
 * receiving pings from clients on separate threads.
 * 
 */

#include "ping.h"
#include <pthread.h>
#include <iostream>
#include <map>


namespace cs3505
{

    /**
     * Ping class constructor
     */
    ping::ping()
    {
        // initialize the lock
        lock = PTHREAD_MUTEX_INITIALIZER;
    }

    /**
     * Ping class destructor
     */
    ping::~ping()
	{
	}

    /**
     * Ping class copy constructor
     */
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
        ping_flags[socket] = 0;
        pthread_mutex_unlock( &lock );

    }

    /**
     * Remove socket from flag map
     */
    void ping::flag_map_remove(int socket)
    {
        pthread_mutex_lock( &lock );
        ping_flags.erase(socket);
        pthread_mutex_unlock( &lock );
    }

    /**
     * Checks ping_flags for a response from given socket
     */
    int ping::check_ping_response(int socket)
	{
        int holder;
        pthread_mutex_lock( &lock );
        holder = ping_flags[socket];
        ping_flags[socket] = 0;
        pthread_mutex_unlock( &lock );

        return holder;
	}

    /**
     * Indicate ping response recieved from socket in ping_flags
     */
    void ping::ping_received(int socket)
    {
        pthread_mutex_lock( &lock );
        ping_flags[socket] = 1;
        pthread_mutex_unlock( &lock );
    }

}

