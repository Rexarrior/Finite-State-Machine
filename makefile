all: library copyDll client tests copyAll

library:
	mcs -target:library -platform:anycpu -out:FSM.dll ./Library/Alphabet.cs ./Library/Machine.cs ./Library/State.cs ./Library/StateTable.cs ./Library/Supports.cs 
	
	
client:
	mcs -target:exe -platform:anycpu -out:runFCM.exe -reference:FSM.dll  ./Client/Program.cs ./Client/Support.cs
	
tests:
	mcs -target:exe -platform:anycpu -out:runTests.exe -reference:FSM.dll  ./Tests/Run.cs ./Tests/testForMyTaskFSM.cs
copyDll:
	cp FSM.dll ./Client/
	cp FSM.dll ./Tests/
	

copyAll:
	cp  FSM.dll ./bin
	cp  runFCM.exe ./bin
	cp  runTests.exe ./bin

clear: 
	rm -rf *.exe
	rm -rf *.dll

	