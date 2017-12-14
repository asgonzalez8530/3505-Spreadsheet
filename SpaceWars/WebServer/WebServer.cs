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

        #region Network Action delegates that handle incoming web requests

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

                string html = MakeHTMLforStats();
                Console.WriteLine(html);
                Network.SendAndClose(state.GetSocket(), httpOkHeader + MakeHTMLforStats());
            }


            // If the browser requested a player
            else if (request.StartsWith(@"GET /games?player="))
            {
             
                // maybe split?
                string playerName = GetRequestParameter(request);
                string html = MakeHTMLforPlayer(playerName);
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
            else if (request.StartsWith(@"GET /games?player="))
            {
                

                string html = MakeHTMLforGame(GetRequestParameter(request));
                if (html.Length == 0)
                {
                    Network.SendAndClose(state.GetSocket(), httpBadHeader + "<h2>the passed in game ID does not exist</h2>");
                }
                else
                {
                    Network.SendAndClose(state.GetSocket(), httpOkHeader + html);
                }

            }

            // Otherwise, our very simple web server doesn't recognize any other URL
            else
            {
                Network.SendAndClose(state.GetSocket(), httpBadHeader + "<h2>page not found</h2>");
            }
        }


        /// <summary>
        /// Gets the player name from request
        /// </summary>
        private static string GetRequestParameter(string requestString)
        {
            int playerIndex = requestString.IndexOf('=');

            int i = playerIndex;
            while (requestString[i] != ' ')
            {
                i++;
            }

            return requestString.Substring(playerIndex + 1, i - playerIndex).Trim();


        }

        #endregion

        #region Methods for parsing Data Base information

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


                        // make a title for the table
                        string title = MakeHTMLTableTitleFromString("Overall Stats");

                        string[] columnNames =
                        {
                            "Game ID",
                            "Player Name",
                            "Score",
                            "Accuracy",
                            "Duration"
                        };
                        string rows = MakeColumnRowFromArray(columnNames);

                        // This will be the command that we will execute
                        string commandString = "select gameID, duration, playerName, score, accuracy from " +
                                                "GameStats natural join PlayersInGame " +
                                                "join PlayerStats on PlayersInGame.playerID = PlayerStats.playerID " +
                                                "order by score desc;";

                        // give our comand to the MySqlCommand object
                        command.CommandText = commandString;

                        // Execute the command
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                // turn this record into a table row in html
                                string[] recordRow =
                                {

                                    reader["gameID"].ToString(),
                                    reader["playerName"].ToString(),
                                    reader["score"].ToString(),
                                    reader["accuracy"].ToString(),
                                    reader["duration"].ToString()

                                };

                                rows += MakeTableRowFromArray(recordRow);

                            }
                        }

                        // make a table from the table rows and append it to title
                        html = title + MakeHTMLTableFromRows(rows);
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

                        // make a title for the table
                        string title = MakeHTMLTableTitleFromString(playerName + "'s Stats");
                        string[] columnNames =
                         {
                            "Game ID",
                            "Score",
                            "Accuracy"
                        };
                        string rows = MakeColumnRowFromArray(columnNames);

                        // This will be the command that we will execute
                        string commandString = "select gameID, score, accuracy from PlayersInGame natural join " +
                                                " PlayerStats where playerName=" + playerName + ";";

                        // give our comand to the MySqlCommand object
                        command.CommandText = commandString;

                        // Execute the command
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                // turn this record into a table row in html
                                string[] recordRow =
                                {
                                    reader["gameID"].ToString(),
                                    reader["score"].ToString(),
                                    reader["accuracy"].ToString()
                                };

                                rows += MakeTableRowFromArray(recordRow);

                            }
                        }

                        // make a table from the table rows and append it to title
                        html = title + MakeHTMLTableFromRows(rows);
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
        /// Takes a gameID and uses it to get information from DB
        /// Then returns a string which includes an html formated
        /// title and table
        /// </summary>
        private static string ParseDataBaseForGame(string gameID)
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

                        // make a header for the table
                        string title = MakeHTMLTableTitleFromString("Stats for game number" + gameID);
                        string[] columnNames = { "Game ID", "Duration", };
                        string rows = MakeColumnRowFromArray(columnNames);


                        // This will be the command that we will execute
                        string commandString = "select playerName, score, accuracy from PlayersInGame " +
                                                "natural join PlayerStats where gameID=" + gameID + ";";

                        // give our comand to the MySqlCommand object
                        command.CommandText = commandString;

                        // Execute the command
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // turn this record into a table row in html
                                string[] recordRow =
                                {
                                    reader["playerName"].ToString(),
                                    reader["score"].ToString(),
                                    reader["accuracy"].ToString()
                                };

                                rows += MakeTableRowFromArray(recordRow);

                            }
                        }

                        // make a table from the table rows and append it to title
                        html = title + MakeHTMLTableFromRows(rows);

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

        #endregion


        // below are methods for making properly formatted HTML... 
        // I know there is some sort of HTML writer for this but I couldn't 
        // immediately find it
        #region Methods for making HTML Documents based on request

        /// <summary>
        /// Gets html records and turns it into properly formatted html for a
        /// stats request
        /// </summary>
        private static string MakeHTMLforStats()
        {
            string title = "Space wars - Overall Stats";
            return MakeStatsHTML(title, ParseDataBaseForStats);
        }

        /// <summary>
        /// Gets html records and turns it into properly formatted html for a
        /// player request
        /// </summary>
        private static string MakeHTMLforPlayer(string player)
        {
            string title = "Space wars - Overall Stats";
            return MakeStatsHTML(title, () => ParseDataBaseForPlayer(player));
        }

        /// <summary>
        /// Gets html records and turns it into properly formatted html for a
        /// player request
        /// </summary>
        private static string MakeHTMLforGame(string gameID)
        {
            string title = "Space wars - Overall Stats";
            return MakeStatsHTML(title, () => ParseDataBaseForGame(gameID));
        }


        /// <summary>
        /// General purpose method for making an HTML document for our SpaceWars web server.
        /// Takes a string title which is displayed on the program bar on top of the web
        /// browser. Also takes a method which returns a string containing an html formatted
        /// table.
        /// </summary>
        private static string MakeStatsHTML(string title, Func<string> table)
        {
            // this goes at the type of an html document, may not need in this case
            string doctype = "<!DOCTYPE html>";

            // title displayed by web browser
            string headTitle = MakeTitleTagFromString(title);

            // css formatting information
            string css = GetCSS();

            string head = MakeHTMLHeadFromContentString(headTitle + css);

            string body = MakeHTMLBodyFromContentString(table());
            string html = doctype + MakeHTMLFromHeadAndBody(head, body);

            return html;
        }

        #endregion

        #region Helper Methods that make HTML tag elements
        /// <summary>
        /// Iterates over each element of the array and returns an HTML table row
        /// that contains a td for each element
        /// </summary>
        private static string MakeTableRowFromArray(string[] elements)
        {
            // tag elements for for building a table row
            string tableRowOpen = "<tr class = \"row\">";
            string tableRowClose = "</tr>";
            string tableDetailOpen = "<td> ";
            string tableDetailClose = "</td>";

            string row = tableRowOpen;

            foreach (String element in elements)
            {
                // the element surrounded by opening and closing tr tags
                row = row + tableDetailOpen + element + tableDetailClose;
            }

            row = row + tableRowClose;

            return row;
        }

        /// <summary>
        /// Iterates over each element of the array and returns an HTML table row
        /// that contains a td for each element
        /// </summary>
        private static string MakeColumnRowFromArray(string[] elements)
        {
            // tag elements for for building a table row
            string tableRowOpen = "<tr class = \"columnrow\">";
            string tableRowClose = "</tr>";
            string tableDetailOpen = "<td> ";
            string tableDetailClose = "</td>";

            string row = tableRowOpen;

            foreach (String element in elements)
            {
                // the element surrounded by opening and closing tr tags
                row = row + tableDetailOpen + element + tableDetailClose;
            }

            row = row + tableRowClose;

            return row;
        }

        /// <summary>
        /// Takes in a string which are roperly formatted HTML tr tags
        /// and surrounds them with HTML table tags
        /// </summary>
        private static string MakeHTMLTableFromRows(string rows)
        {

            // opening tag for table
            string tableOpen = "<table class = \"table\">";
            string tableClose = "</table>";

            return tableOpen + rows + tableClose;
        }

        /// <summary>
        /// surrounds headerTitle with HTML h2 tags
        /// </summary>
        private static string MakeHTMLTableTitleFromString(string title)
        {
            // make a header for the table
            // open tag
            string h2TagOpen = "<h2 class = \"title\">";
            string h2TagClose = "</h2>";

            return h2TagOpen + title + h2TagClose;
        }

        /// <summary>
        /// Takes htmlContent and surrounds it with html body tags
        /// </summary>
        private static string MakeHTMLBodyFromContentString(string htmlContent)
        {
            string bodyOpen = "<body>";
            string bodyClose = "</body>";

            return bodyOpen + htmlContent + bodyClose;
        }

        /// <summary>
        /// Takes the head and body elements of an html document and surounds them with html tags
        /// </summary>
        private static string MakeHTMLFromHeadAndBody(string head, string body)
        {
            string htmlOpen = "<html>";
            string htmlClose = "</html>";
            return htmlOpen + head + body + htmlClose;
        }

        /// <summary>
        /// Takes a string containing HTMLhead content and surrounds it with 
        /// head tags
        /// </summary>
        private static string MakeHTMLHeadFromContentString(string headContent)
        {
            string headOpen = "<head>";
            string headClose = "</head>";

            return headOpen + headContent + headClose;
        }

        private static string GetCSS()
        {
            //TODO: Return css 
            return "";
        }

        /// <summary>
        /// Takes a string and surrounds it with html title tags
        /// </summary>
        private static string MakeTitleTagFromString(string title)
        {
            string titleOpen = "<title>";
            string titleClose = "</title>";

            return titleOpen + title + titleClose;
        }

        #endregion

    }
}
