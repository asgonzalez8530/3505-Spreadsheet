a.out: makefile main.o server.o interface.o spreadsheet.o socket_connection.o tester
	g++ main.o server.o interface.o spreadsheet.o socket_connection.o /usr/local/lib/libboost_system.a /usr/local/lib/libboost_serialization.a /usr/local/lib/libboost_filesystem.a -lpthread

tester: makefile tester.o interface.o spreadsheet.o socket_connection.o server.o
	g++ -o tester server.o interface.o tester.o spreadsheet.o socket_connection.o /usr/local/lib/libboost_system.a /usr/local/lib/libboost_serialization.a /usr/local/lib/libboost_filesystem.a -lgtest -lpthread

main.o: main.cpp server.h
	g++ -c main.cpp

server.o: server.cpp server.h interface.h
	g++ -c server.cpp

interface.o: interface.cpp interface.h
	g++ -c interface.cpp

spreadsheet.o: spreadsheet.cpp spreadsheet.h
	g++ -c spreadsheet.cpp

socket_connection.o: socket_connection.cpp socket_connection.h
	g++ -c socket_connection.cpp

tester.o: tester.cpp
	g++ -c tester.cpp

#server.o
