a.out: main.o server.o interface.o spreadsheet.o
	g++ main.o server.o interface.o -lpthread

main.o: main.cpp server.h
	g++ -c main.cpp

server.o: server.cpp server.h interface.h
	g++ -c server.cpp

interface.o: interface.cpp interface.h
	g++ -c interface.cpp

spreadsheet.o: spreadsheet.cpp spreadsheet.h
	g++ -c spreadsheet.cpp
