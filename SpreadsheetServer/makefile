a.out: makefile main.o server.o interface.o spreadsheet.o socket_connection.o tester
	g++ main.o server.o interface.o spreadsheet.o socket_connection.o /usr/local/lib/libboost_system.a /usr/local/lib/libboost_serialization.a /usr/local/lib/libboost_filesystem.a -lpthread

tester: makefile tester.o interface.o spreadsheet.o socket_connection.o server.o
	g++ -o tester server.o interface.o tester.o spreadsheet.o socket_connection.o /usr/local/lib/libboost_system.a /usr/local/lib/libboost_serialization.a /usr/local/lib/libboost_filesystem.a -lgtest -lpthread

main.o: main.cpp server.h
	g++ -std=c++11 -c main.cpp

server.o: server.cpp server.h interface.h
	g++ -std=c++11 -c server.cpp

interface.o: interface.cpp interface.h
	g++ -std=c++11 -c interface.cpp

spreadsheet.o: spreadsheet.cpp spreadsheet.h
	g++ -std=c++11 -c spreadsheet.cpp

socket_connection.o: socket_connection.cpp socket_connection.h
	g++ -std=c++11 -c socket_connection.cpp

tester.o: tester.cpp
<<<<<<< HEAD
	g++ -c tester.cpp

#server.o
=======
	g++ -std=c++11 -c tester.cpp
>>>>>>> 79a6474c13cdbcd676b25b768f8cdfff6476c69a
