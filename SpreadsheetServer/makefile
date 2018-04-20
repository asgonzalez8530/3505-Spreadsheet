a.out: makefile main.o server.o interface.o spreadsheet.o message_queue.o ping.o tester
	g++ main.o server.o interface.o spreadsheet.o message_queue.o ping.o /usr/local/lib/libboost_system.a /usr/local/lib/libboost_serialization.a /usr/local/lib/libboost_filesystem.a -lpthread

tester: makefile tester.o interface.o spreadsheet.o message_queue.o server.o ping.o
	g++ -g -o tester server.o interface.o tester.o spreadsheet.o message_queue.o ping.o /usr/local/lib/libboost_system.a /usr/local/lib/libboost_serialization.a /usr/local/lib/libboost_filesystem.a -lgtest -lpthread
>>>>>>> 194cdd98a5a985e92695d0c603428b82f5a42a42

main.o: main.cpp server.h
	g++ -std=c++11 -c main.cpp

server.o: server.cpp server.h interface.h ping.h
	g++ -std=c++11 -c server.cpp

interface.o: interface.cpp interface.h
	g++ -std=c++11 -c interface.cpp

spreadsheet.o: spreadsheet.cpp spreadsheet.h
	g++ -std=c++11 -c spreadsheet.cpp

message_queue.o: message_queue.cpp message_queue.h
	g++ -std=c++11 -c message_queue.cpp

ping.o: ping.cpp ping.h
	g++ -std=c++11 -c ping.cpp

tester.o: tester.cpp
	g++ -std=c++11 -c tester.cpp

clean:
	rm a.out *.o tester
