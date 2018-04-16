a.out: makefile main.o server.o interface.o spreadsheet.o socket_connection.o tester
	g++ main.o server.o interface.o -lpthread

tester: makefile tester.o server.o interface.o spreadsheet.o socket_connection.o
	g++ -L/usr/local/include -o tester server.o interface.o tester.o /usr/lib64/libboost_system.so.1.53.0 /usr/lib64/libboost_serialization.so.1.53.0 /usr/lib64/libboost_filesystem.so.1.53.0 -lgtest -lpthread

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
