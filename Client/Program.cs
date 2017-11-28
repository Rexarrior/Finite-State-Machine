using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FiniteStateMachineLibrary;



namespace FiniteStateMachine
{
    class Program
    {
        #region Save\load Machine functions
        static void SaveNFMMachine(NFM<string> machine, string filename)
        {

        try
        {
            using (StreamWriter outputFile =new StreamWriter( new FileStream(filename, FileMode.OpenOrCreate)))
            {
                outputFile.WriteLine("# Alphabet");
                outputFile.WriteLine(machine.Alphabet.Literals.Aggregate("", (acum, x)=> acum + x.ToString() + ' '));
                outputFile.WriteLine("# Count of states");
                outputFile.WriteLine(machine.StateTable.Count);
                outputFile.WriteLine("# States");
                var states = machine.StateTable.States;
                states.Reverse() ;
                foreach (State<string>  state in  states )
                {
                    string str = state.Caption;
                    if (state.IsStartingState)
                        str += " 1";
                    else
                        str += " 0";
                    
                    if (state.IsFinalState)
                        str += " 1";
                    else
                        str += " 0";

                    foreach (Pair<string, State<string>> pair in state.NextStates)
                    {
                        str+=' ' + pair.first + ' ' + pair.second.Id;
                    }
                    
                    outputFile.WriteLine(str);
                    
                }
                
                outputFile.WriteLine("# Name");
                outputFile.WriteLine(machine.Name);

            }
            Console.WriteLine("Machine {0} succesfull have saved", machine.Name);  
        }
        catch (System.Exception e)
        {
            
            Console.WriteLine("Error! Machine haven't saved. Error message is {0} ",e.Message);
        }

              
        }

        static NFM<string> loadNFM(string filename)
        {
            
            try
            {
                using (StreamReader inputFile = new StreamReader(new FileStream(filename, FileMode.Open))) 
                {
                    
                    string inputStr = inputFile.ReadLine();
                    while (inputStr[0] == '#')
                        inputStr = inputFile.ReadLine();
                    Alphabet<string> alphabet = new Alphabet<string>(inputStr.Split(' '));
                    inputStr = inputFile.ReadLine();
                    while (inputStr[0] == '#')
                        inputStr = inputFile.ReadLine();
                    int countOfStates = int.Parse(inputStr);
                    inputStr = inputFile.ReadLine();
                    while (inputStr[0] == '#')
                        inputStr = inputFile.ReadLine();
                    List<List<Pair<string, int>>> nextStatesForAll = new List<List<Pair<string, int>>>(countOfStates);
                    List<State<string>> states = new List<State<string>>(countOfStates);
                    
                    for (int i = 1; i <= countOfStates; i++ )
                    {
                        


                        List<string> stateString = inputStr.Split(' ').ToList();
                        inputStr = inputFile.ReadLine();
                        
                        string caption = stateString[0];
                        bool isStarting = stateString[1] == "1";
                        bool isFinal = stateString[2] == "1";

                        int countOfNextStates = ( stateString.Count - 3)  / 2;

                        List<Pair<string,int>> nextStates =
                            new List<Pair<string, int>> (countOfNextStates);


                        for (int j =0 ; j < countOfNextStates; j++)
                        {
                            nextStates.Add(new Pair<string,int>(stateString[2 + j * 2 + 1],
                                int.Parse (stateString[2 + j * 2 + 2]) ));

                        }


                        nextStatesForAll.Add(nextStates);
                        states.Add(new State<string>(alphabet, (uint)i, isStarting, isFinal, caption));
                    
                    }
                    

                    for (int i = 0; i < countOfStates; i++)
                    {
                        states[i].NextStates = nextStatesForAll[i ].
                            Select(x => new Pair<string,
                            State<string>>(x.first, states[x.second-1])).ToList();
                    }

                    inputStr = inputFile.ReadLine();
                    while (inputStr[0] == '#')
                        inputStr = inputFile.ReadLine();
                    string name = inputStr;
                    
                    StateTable<string> stateTable = new StateTable<string>( states);
                    NFM<string> nfm = new NFM<string>(alphabet, stateTable);
                    nfm.Name = name;
                    Console.WriteLine("Machine {0} succesfull have loaded", nfm.Name);    
                    return nfm;
                   
                } 
           
            
            }
            catch (System.Exception e)
            {
                
                Console.WriteLine("Error. Machine haven't loaded. Exception message:{0} ",e.Message);
                return null;
            }
            

        }
        #endregion
        
        
        
        static NFM<string> ConstrucktNFM()
        {
            Console.Write(Notes.contructingAlphabetStartNote);
           

            Console.Write(Notes.constructingAlphabetEnteringNote);
            string literals = Console.ReadLine();
            Alphabet<string> alphabet = new Alphabet<string>(literals.Split(' '));
            Console.Write(Notes.constructingAlphabetEndNote);

            Console.Write(Notes.constructingTableStateBeginNote);
            int countOfStates = int.Parse(Console.ReadLine());

            List<State<string>> states = new List<State<string>>(countOfStates);

            Console.Write(Notes.constructingTableStateEnteringStateNote);

           
            List<List<Pair<string, int>>> nextStatesForAll = new List<List<Pair<string, int>>>(countOfStates);

            for (int i = 1; i <= countOfStates; i++ )
            {
                Console.Write("Enter state by id {0}: ", i);

                List<string> stateString = Console.ReadLine().Split(' ').ToList();
                string caption = stateString[0];
                bool isStarting = stateString[1] == "1";
                bool isFinal = stateString[2] == "1";

                int countOfNextStates = ( stateString.Count - 3)  / 2;

                List<Pair<string,int>> nextStates =
                    new List<Pair<string, int>> (countOfNextStates);


                for (int j =0 ; j < countOfNextStates; j++)
                {
                    nextStates.Add(new Pair<string,int>(stateString[2 + j * 2 + 1],
                        int.Parse (stateString[2 + j * 2 + 2]) ));

                }


                nextStatesForAll.Add(nextStates);
                states.Add(new State<string>(alphabet, (uint)i, isStarting, isFinal, caption));
               
            }


            for (int i = 0; i < countOfStates; i++)
            {
                states[i].NextStates = nextStatesForAll[i ].
                    Select(x => new Pair<string,
                    State<string>>(x.first, states[x.second-1])).ToList();
            }


            StateTable<string> stateTable = new StateTable<string>( states);
            NFM<string> nfm = new NFM<string>(alphabet, stateTable);

            Console.Write(Notes.machineIsReadyNote);
            nfm.Name = Console.ReadLine();
            

                
            Console.Write(Notes.constructingTableStateEndNote);

            return nfm; 
        }


        static void useNFM(NFM<string> nfm)
        {
            Console.Write(Notes.useMachineUseNote);

            nfm.InputString = Console.ReadLine().Select(x=>"" + x).ToList();
            nfm.processString();

            Console.WriteLine("Input string is{0} alllowed. \n", nfm.IsFinal?"":" not");
            nfm.ReInitialize();
             
        } 

        static void Main(string[] args)
        {
            List<NFM<string>> machines = new List<NFM<string>>();

            while (true)
            { 
                  

                Console.Write(Notes.startNote);
                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            #region use mode

                            Console.Write(Notes.useMachineSelectModeNote);

                            switch (Console.ReadLine())
                            {
                               case "1":
                                    {// load from file mode

                                        Console.Write("Please, enter  file name: ");
                                        string name = Console.ReadLine();

                                        NFM<string> machine = loadNFM(name);
                                        if (machine != null)
                                            machines.Add(machine);


                                    }
                                    break;

                                case "2":
                                    {// use already exists machine mode

                                        while (true)
                                        {
                                            Console.Write(Notes.useInSessionMachineSelect);
                                            for (int i = 0; i < machines.Count; i++)
                                            {
                                                Console.WriteLine("machines number {0} : {1}; ", i + 1, machines[i].Name);
                                            }
                                            Console.Write("Enter num selected machine. Q to back to the main menu: ");
                                            string answ = Console.ReadLine();
                                            if (answ.ToLower() == "q")
                                                break;
                                            try
                                            {
                                            int num = int.Parse(answ) -1;
                                            useNFM(machines[num]);
                                            }
                                            catch(System.Exception)
                                            {
                                                Console.WriteLine("Input Error."); 
                                            }
                                            
                                            
                                        }
                                    }break;

                                default: continue;
                            }

                            #endregion
                        } break;

                    case "2":
                        {
                            #region construct mode
                            machines.Add(ConstrucktNFM());
                            // Console.Write(Notes.selectMachineNote);

                            // switch (Console.ReadLine())
                            // {
                            //     case "1":
                            //         {//construct NFM mode
                            //             machines.Add(ConstrucktNFM());

                            //         }break;
                            //     default: continue;
                            // }

                            #endregion
                        }
                        break;

                    case "3":
                        {
                            #region quiet request
                            string saveDir = "./lastSessionMachines/";
                            Directory.CreateDirectory(saveDir);

                            foreach (var x in machines)
                            {
                                SaveNFMMachine(x, saveDir + x.Name + ".txt");    
                            }
                            return;
                            #endregion
                        }
                        

                    default:
                        continue;
                }


            }

            

        }
    }
}
