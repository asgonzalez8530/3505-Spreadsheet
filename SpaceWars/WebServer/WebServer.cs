using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SpaceWarsServer
{
    /// <summary>
    /// A very simple web server
    /// </summary>
    public static class WebServer
    {
        // The server must follow the HTTP protocol when replying to a client (web browser).
        // That involves sending an HTTP header before any web content.

        /// <summary>
        /// The connection string to our stats DB.
        /// </summary>
        public const string connectionString = "server=atr.eng.utah.edu;" +
          "database=cs3500_u0981638;" +
          "uid=cs3500_u0981638;" +
          "password=AaronAndAnastasia";

        // This is the header to use when the server recognizes the browser's request
        private const string httpOkHeader =
          "HTTP/1.1 200 OK\r\n" +
          "Connection: close\r\n" +
          "Content-Type: text/html; charset=UTF-8\r\n" +
          "\r\n";

        // This is the header to use when the server does not recognize the browser's request
        private const string httpBadHeader =
          "HTTP/1.1 404 Not Found\r\n" +
          "Connection: close\r\n" +
          "Content-Type: text/html; charset=UTF-8\r\n" +
          "\r\n";


        public static void Main(string[] args)
        {
            // Start an event loop that listens for socket connections on port 80
            // This requires a slight modification to the networking library to take the port argument
            Network.ServerAwaitingClientLoop(HandleHttpConnection, Network.HTTP_PORT);

            Console.Read();
        }

        /// <summary>
        /// This is the delegate for when a new socket is accepted
        /// The networking library will invoke this method when a browser connects
        /// </summary>
        /// <param name="state"></param>
        public static void HandleHttpConnection(SocketState state)
        {
            // Before receiving data from the browser, we need to change what we do when network activity occurs.
            state.SetNetworkAction(ServeHttpRequest);

            Network.GetData(state);
        }


        /// <summary>
        /// This method parses the HTTP request sent by the broswer, and serves the appropriate web page.
        /// </summary>
        /// <param name="state">The socket state representing the connection with the client</param>
        private static void ServeHttpRequest(SocketState state)
        {
            string request = state.GetStringBuilder().ToString();

            // Print it for debugging/examining
            Console.WriteLine("received http request: " + request);


            // If the browser requested the server stats
            if (request.Contains("GET /scores HTTP/1.1"))
            {
                Network.SendAndClose(state.GetSocket(), httpOkHeader + ParseDataBaseForStats());
            }

            // If the browser requested a player
            else if (Regex.IsMatch(request, @"GET /games?player=[\w\W]+ HTTP/ 1.1"))
            {
                // TODO: find a way to parse out the player name 
                // maybe split?
                string playerName = "";
                string html = ParseDataBaseForPlayer(playerName);
                if (html.Length == 0)
                {
                    Network.SendAndClose(state.GetSocket(), httpBadHeader + "<h2>the passed in player does not exist</h2>");

                }
                else
                {
                    Network.SendAndClose(state.GetSocket(), httpOkHeader + html);
                }
            }

            // If the browser requested a game
            else if (Regex.IsMatch(request, @"GET /game?id=\d+ HTTP/ 1.1"))
            {
                // TODO: find a better way because the number could be greater than ten
                // see if the id is a valid number
                if (int.TryParse(request.Substring(12, 1), out int gameID))
                {
                    string html = ParseDataBaseForGame(gameID);
                    if (html.Length == 0)
                    {
                        Network.SendAndClose(state.GetSocket(), httpBadHeader + "<h2>the passed in game ID does not exist</h2>");
                    }
                    else
                    {
                        Network.SendAndClose(state.GetSocket(), httpOkHeader + html);
                    }
                }
            }

            // Otherwise, our very simple web server doesn't recognize any other URL
            else
            {
                Network.SendAndClose(state.GetSocket(), httpBadHeader + "<h2>page not found</h2>");
            }
        }

        private static string ParseDataBaseForStats()
        {
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    string html = "";
                    // Create a command
                    using (MySqlCommand command = conn.CreateCommand())
                    {
                        // This will be the command that we will execute
                        string commandString =  "select gameID, duration, playerName, score, accuracy from" + 
                                                "GameStats natural join PlayersInGame" + 
                                                "join PlayerStats on PlayersInGame.playerID = PlayerStats.playerID" + 
                                                "order by score desc;";

                        // give our comand to the MySqlCommand object
                        command.CommandText = commandString;

                        // execute the command
                        command.ExecuteNonQuery();

                        // make a header for the table
                        html = "<h2>Overall Stats</h2><table>";

                        // Execute the command
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //TODO: make a table in html
                                // make a new row
                                html += "<tr>";

                                // add the game id
                                html += "<td>" + reader["gameID"].ToString() + "</td>";

                                // add the game duration
                                html += "<td>" + reader["duration"].ToString() + "</td>";

                                // add the name of the player
                                html += "<td>" + reader["playerName"].ToString() + "</td>";

                                // add the score
                                html += "<td>" + reader["score"].ToString() + "</td>";

                                // add the accuracy
                                html += "<td>" + reader["accuracy"].ToString() + "</td>";

                                // end the row
                                html += "</tr>";
                            }
                        }

                        html += "<table border = 1>";
                    }

                    // close our connection
                    conn.Close();
                    return html;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message + " : Stats");
                    return "";
                }
            }
        }

        private static string ParseDataBaseForPlayer(string playerName)
        {
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    string html = "";
                    // Create a command
                    using (MySqlCommand command = conn.CreateCommand())
                    {
                        // This will be the command that we will execute
                        string commandString =  "select gameID, score, accuracy from PlayersInGame natural join" +
                                                "PlayerStats where playerName=" + playerName + ";";

                        // give our comand to the MySqlCommand object
                        command.CommandText = commandString;

                        // execute the command
                        command.ExecuteNonQuery();

                        // make a header for the table
                        html = "<h2>" + playerName + "'s Stats</h2><table>";

                        // Execute the command
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //TODO: make a table in html
                                // make a new row
                                html += "<tr>";

                                // add the game id
                                html += "<td>" + reader["gameID"].ToString() +"</td>";

                                // add the score
                                html += "<td>" + reader["score"].ToString() + "</td>";

                                // add the accuracy
                                html += "<td>" + reader["accuracy"].ToString() + "</td>";

                                // end the row
                                html += "</tr>";
                            }
                        }

                        html += "<table border = 1>";
                    }

                    // close our connection
                    conn.Close();
                    return html;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message + " : Stats");
                    return "";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string ParseDataBaseForGame(int gameID)
        {
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    string html = "";
                    // Create a command
                    using (MySqlCommand command = conn.CreateCommand())
                    {
                        // This will be the command that we will execute
                        string commandString = "select playerName, score, accuracy from PlayersInGame" +
                                                "natural join PlayerStats where gameID=" + gameID + ";";

                        // give our comand to the MySqlCommand object
                        command.CommandText = commandString;

                        // execute the command
                        command.ExecuteNonQuery();

                        // make a header for the table
                        html = "<h2>Game " + gameID + " Stats</h2><table>";

                        // Execute the command
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //TODO: make a table in html
                                // make a new row
                                html += "<tr>";

                                // add the game id
                                html += "<td>" + reader["playerName"].ToString() + "</td>";

                                // add the score
                                html += "<td>" + reader["score"].ToString() + "</td>";

                                // add the accuracy
                                html += "<td>" + reader["accuracy"].ToString() + "</td>";

                                // end the row
                                html += "</tr>";
                            }
                        }

                        html += "<table border = 1>";
                    }

                    // close our connection
                    conn.Close();
                    return html;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message + " : Game");
                    return "";
                }
            }
        }

        // NOTE: The above SendAndClose calls are an addition to the Networking library.
        //       The only difference from the basic Send method is that this method uses a callback
        //       that closes the socket after ending the send.

    }
}
