all:
	@echo Please, run "make windows" or "make linux" for the compilation. Run  "make clean" for cleaning executable files


linux:  library_l copyDll client_l tests_l 
windows:  library_w copyDll client_w tests_w


#######################################################################################################################
LIBCOMPILEARGS = -target:library -platform:anycpu -out:./bin/FSM.dll ./Library/Alphabet.cs ./Library/Machine.cs ./Library/State.cs ./Library/StateTable.cs ./Library/Supports.cs 
CLIENTCOMPILEARGS = -target:exe -platform:anycpu -out:./bin/runFCM.exe -reference:./SampleClient/FSM.dll  ./SampleClient/Program.cs ./SampleClient/Support.cs
TESTSCOMPILEARGS = -target:exe -platform:anycpu -out:./bin/runTests.exe -reference:./Tests/FSM.dll  ./Tests/Run.cs ./Tests/testForMyTaskFSM.cs





library_l:	
	mcs $(LIBCOMPILEARGS)
	
	
client_l:
	mcs $(CLIENTCOMPILEARGS)
	
tests_l:
	mcs $(TESTSCOMPILEARGS)


library_w:
	csc $(LIBCOMPILEARGS)
	
	
client_w:
	csc $(CLIENTCOMPILEARGS)
	
tests_w:
	csc $(TESTSCOMPILEARGS)


mkDirs:
	mkdir bin



copyDll:
	cp ./bin/FSM.dll  ./SampleClient/

	cp ./bin/FSM.dll ./Tests/
	

copyAll:
	cp  FSM.dll ./bin
	cp  runFCM.exe ./bin
	cp  runTests.exe ./bin

clean: 
	rm -rf *.exe
	rm -rf *.dll

	