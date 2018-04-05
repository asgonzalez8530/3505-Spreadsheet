a.out: main.o server.o
	g++ main.o server.o -lpthread

main.o: main.cpp server.h
	g++ -c main.cpp

server.o: server.cpp server.h
	g++ -c server.cpp