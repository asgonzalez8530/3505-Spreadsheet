Name: Anastasia Gonzalez and Aaron Bellis
UID: u0985898 & u0981638
Last Update: 12/10/17

SpaceWars Client - PS7 
Created for CS 3500 2017 Fall Semester

Initial Design Ideas: The GUI will need three panels. One panel is the top bar which will be a tableLayoutPanel and  
the side and center panel will be custom. The side panel will contian the scoreboard and the center one will be the 
game. The size of the form will resize when the server send the proper information. 

What works: We were able to get everything to work. We did a lot of troubleshooting and debugging to get everything 
working. The hardest part to get correct was the GUI. We struggled with updating the information to the GUI correctly 
without causing problems or errors. 

What didn't work: We really struggled with getting the scoreboard to work properly since the health points would 
not update. We did a lot of debugging to get it to work. We also had problems with the black color player not filling 
in the scoreboard color in everytime. 

Notes for the TAs: Our project was setup in an the proper MVC format. We used the proper abstraction so that the 
different classes could speak to each other and work together to create a fuctional spreadsheet GUI. 

Added Features: Our scoreboard is color coordinated and our GUI has a fun Santa Claus theme. We picked a white 
background to represent the snow. 

How to Run: Start the server. Once the server is ready, press start on the client code. Enter in a name and press connect.


SpaceWars Server - PS8
Created for CS 3500 2017 Fall Semester

Initial Design Ideas: The GUI will need three panels. One panel is the top bar which will be a tableLayoutPanel and  
the side and center panel will be custom. The side panel will contian the scoreboard and the center one will be the 
game. The size of the form will resize when the server send the proper information. 
We tested part of the network controller when possible.

What works: We were able to get everything to work. We did a lot of troubleshooting and debugging to get everything 
working. The hardest part to get correct was the GUI. We struggled with updating the information to the GUI correctly 
without causing problems or errors. 

What we struggled with: We really struggled with getting the the ships and projectiles to be represented as dead at the
proper moment. At the beginning we would remove them from the world before we sent the dead representation to the clients. 
After re-evalutating the process we decided set then as dead then send the info to the clients. At the very end we would
go through all ships and projectiles and remove the dead ones from the world. We also had a hard time speeding up the 
servering while handling 10+ clients so we made some methods asynchronous. 

Areas to improve in: We would imporve the code by having two severs. One that would run when the king of the hill setting
was on and the other would run when it was just the normal space wars game. 

Notes for the TAs: Our project was setup in an the proper MVC format. We used the proper abstraction so that the 
different classes could speak to each other and work together to create a fuctional server. We were given an extension 
by Dr. Kopta to turn in our assignment on Sunday.

Added Feature: Our special feature is the king of the hill. The rules are as follows:
1) The king is the first player who enters the world
2) The king gets 3 extra hit points
3) The other players can only fire projectiles to hurt the king
4) The king can fire at anyone
5) When the king is dead a new ship is named king randomly


How to Run: Press the play button (make sure that the server solution is set as the StartUp project). Once the server is
ready, wait for a client to connect. 


/************************************** REMOVE BELOW *****************************************/
TODO:
*	Testing
*	Speedup?
