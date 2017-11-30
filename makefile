all: library copyDll client tests 

library:
	mcs -target:library -platform:anycpu -out:./bin/FSM.dll ./Library/Alphabet.cs ./Library/Machine.cs ./Library/State.cs ./Library/StateTable.cs ./Library/Supports.cs 
	
	
client:
	mcs -target:exe -platform:anycpu -out:./bin/runFCM.exe -reference:./Client/FSM.dll  ./Client/Program.cs ./Client/Support.cs
	
tests:
	mcs -target:exe -platform:anycpu -out:./bin/runTests.exe -reference:./Tests/FSM.dll  ./Tests/Run.cs ./Tests/testForMyTaskFSM.cs
copyDll:
	cp ./bin/FSM.dll ./Client/
	cp ./bin/FSM.dll ./Tests/
	

copyAll:
	cp  FSM.dll ./bin
	cp  runFCM.exe ./bin
	cp  runTests.exe ./bin

clear: 
	rm -rf *.exe
	rm -rf *.dll

	