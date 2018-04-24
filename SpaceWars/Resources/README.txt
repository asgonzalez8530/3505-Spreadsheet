Author: Rebekah Peterson, Jacqulyn MacHardy
Last Updated: 12/6/2017

Design Decisions: MVC design. Model consists of the World, which has dictionaries of Ships, Projectiles, and Stars. 
The View consists of a windows form app called View, which has a connect button (for connecting to the server) and 
a drawingPanel (for drawing information from the server) and a scoreboardPanel. DrawingPanel redraws on a timer. 
The controller is NetworkController which has methods used by the View to connect to the server and communicate 
between server and client. Separated concerns of receiving and applying command requests: the Server processes
the requests and then the World is in charge of applying the requests. (examples: FireProjectile, RotateShip, RemoveShip)

Testing Strategy: Unit tested all pieces of the Model including World, Projectile, Star, Ship, and Vector2D. 
Also, tested the server console app and client GUI by hand. Used the provided Client and AIClient to test our Server, 
to mitigate any issues that might be introduced by our Client.  When we were sure the Server was working, then
we tested with our Client. Had hoped to test with other teams' clients, but ran out of time.

Client Polish: Sounds have been included for when projectiles are fired and when a hit happens. Each ship's HP surrounds the ship. The scoreboard is sorted.

Server Polish: The XML settings file includes required and additional settings to adjust game play design. This is where SafeRespawn Game mode can be turned on. 
This game mode keeps a ship from losing HP for 100 frames after respawning. 

What works and doesn't work:
	11/7/2017	SpaceWars client's solution created
				Before implementing, we plan to stub out classes and wireframe. Starting with Model.
	11/10/2017	Started View GUI.
				Implemented ConnectButton, ConnectToServer, ConnectedCallback, FirstContact - from Network Diagram.
				Implemented Send and SendCallback, adapated from Lec20.
	11/11/2017	Implemented Receive Start up (basically)
				Copied in DrawingPanel from lab10
				World now contains dictionaries and properties instead of lists (Stars, Ships, and Projectiles)
				Implemented ReceiveWorld with a helper method UpdateWorld that makes updates to theWorld with each Json Object that comes.
				Also improved World with a constructor and UpdateStar, UpdateShip, and UpdateProjectile methods.
	11/13/2017	Added frameTimer to redraw the form
				Added needed getters for the Model (World, Ship, Star, Projectile)
				Added sprites to resources
				Implemented onPaint with locks
				Implemented ShipDrawer, ProjectileDrawer and StarDrawer
	11/14/2017	Now deleting dead projectiles and only loading sprites once 
				Added command requests functionality
	11/16/2017	Added error checking
				Added a scoreboard panel
				Ships are now comparable, and World.GetShips() is sorted
				In the form, commandRequests are now sent together (RL) rather than (R) and then (L)
	11/17/2017	OnPaint in Scoreboard now uses DrawString instead of Labels
				Relocated SendCommandRequests() call. Ship turning now matches sample client.
				Created NetworkError() method for reconnecting, uses MethodInvoker
				Now resizing scoreboardPanel
				Added a hasError flag to socketstate
				Normalizing vectors before using ToAngle()
	
************************BEGIN PS8************************
	11/28/2017	Made Server project.
				Added ConnectionState and methods for Server's side of the handshake.
				Added new constructor for Ship to facilitate when a client first joins.
	11/30/2017	Added to projectiles and stars to UpdateClients.
				Added setters to Projectile, Ship, and Star.
				Finished secondary Ship constructor.
				Finished HandleCommandRequests.
				Started UpdateClients.
				Finished implementing ReceiveName()
	12/1/2017	Created Server constructor
				Now displays random objects (server sends to client)
				Added a timer for updating!
				Separated ProcessCommands and ApplyCommands
				Created shipCommands dictionary
	12/2/2017	ApplyCommands in Server now turns thrust on
				Ship now contains the velocity
				World now contains an Update method
				Ship now wraps around
				Implemented projectiles and projectile clean up.
				Made helper for random spawnpoints.
	12/4/2017	Created FireProjectile and RotateShip methods (moved functionality to the Model)
				Created another World constructor for inputting settings from XML file.
				Implemented Collisions.
				Fixed a bug where ship could shoot while dead.
				World.Update() takes the current frame count.
				Added TimeOfDeath to Ship.
				Ship now respawns!
	12/5/2017	Model tests, documentation, and clarity of code improved.
	12/6/2017	Added initial TimeOfDeath to Ship constructor.
				Added tests. 100% code coverage on the Model!
				Implemented XML Settings File.
				Handled bad settings.
				Handling disconnecting clients.
				Made spawn point safety check.
				Enforced the protocol with the client.
				Updated Networking.Send.
				Implemented Extra Game Mode: SafeRespawn.
	12/7/2017	Implemented reading Stars from XML settings file.

				
Resources: 
	Newtonsoft's JSON library
	Provided SpaceWars sprites	11/13/2017
	Reason for sound effects	11/17/2017
	ServerSettings.xml			12/6/2017
