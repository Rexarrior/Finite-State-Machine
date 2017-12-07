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
        /// <summary>
        /// save the NFM to file
        /// </summary>
        /// <param name="machine">NFM to be saved</param>
        /// <param name="filename">Output file </param>
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
                #region save states
                outputFile.WriteLine("# States");
                var states = machine.StateTable.States;
                states.Reverse() ;  // Need for conservation input order
                foreach (State<string>  state in  states )
                {//saving states

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
                #endregion
                outputFile.WriteLine("# Name");
                outputFile.WriteLine(machine.Name);

            }
            Console.WriteLine("Machine {0} succesfull saved", machine.Name);  
        }
        catch (System.Exception e)
        {
            //something was wrong :(
            Console.WriteLine("Error! Machine haven't saved. Error message is {0} ",e.Message);
        }

              
        }

        /// <summary>
        /// Load NFM from the input file
        /// </summary>
        /// <param name="filename">Name of file contains the NFM</param>
        /// <returns>Loaded NFM</returns>
        static NFM<string> loadNFM(string filename)
        {
            
            try
            {
                using (StreamReader inputFile = new StreamReader(new FileStream(filename, FileMode.Open))) 
                {
                    string inputStr;                    
                    // This block need to skip the comments                   
                    #region skip comment
                    inputStr = inputFile.ReadLine();
                    while (inputStr[0] == '#')                        
                        inputStr = inputFile.ReadLine();
                    #endregion                    
                    Alphabet<string> alphabet = new Alphabet<string>(inputStr.Split(' '));
                    
                    #region skip comment
                    inputStr = inputFile.ReadLine();
                    while (inputStr[0] == '#')                        
                        inputStr = inputFile.ReadLine();
                    #endregion                     
                    int countOfStates = int.Parse(inputStr);

                    #region skip comment
                    inputStr = inputFile.ReadLine();
                    while (inputStr[0] == '#')                        
                        inputStr = inputFile.ReadLine();
                    #endregion                    
                    List<List<Pair<string, int>>> nextStatesForAll = new List<List<Pair<string, int>>>(countOfStates);
                    List<State<string>> states = new List<State<string>>(countOfStates);
                    
                    //collecting state-input strings and make state without next states
                    #region input states
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
                    
                    // adding the next states to states
                    for (int i = 0; i < countOfStates; i++)
                    {
                        states[i].NextStates = nextStatesForAll[i ].
                            Select(x => new Pair<string,
                            State<string>>(x.first, states[x.second-1])).ToList();
                    }
                    #endregion
                    
                    #region skip comment
                    inputStr = inputFile.ReadLine();
                    while (inputStr[0] == '#')
                        inputStr = inputFile.ReadLine();
                    #endregion                    
                    string name = inputStr;
                    

                    // make the machine
                    StateTable<string> stateTable = new StateTable<string>( states);
                    NFM<string> nfm = new NFM<string>(alphabet, stateTable);
                    nfm.Name = name;
                    Console.WriteLine("Machine {0} succesfull have loaded", nfm.Name);    
                    return nfm;
                   
                } 
           
            
            }
            catch (System.Exception e)
            {
                //something was wronk :(
                Console.WriteLine("Error. Machine haven't loaded. Exception message:{0} ",e.Message);
                return null;
            }
            

        }
        #endregion
        
        
        /// <summary>
        /// Construct new NFM by parameters, that will be read from Console input
        /// </summary>
        /// <returns>NFM</returns>
        static NFM<string> ConstrucktNFM()
        {
            Console.Write(Notes.contructingAlphabetStartNote);
           
            #region  collecting Alphabet
            Console.Write(Notes.constructingAlphabetEnteringNote);
            string literals = Console.ReadLine();
            Alphabet<string> alphabet = new Alphabet<string>(literals.Split(' '));
            Console.Write(Notes.constructingAlphabetEndNote);
            #endregion
           
            #region collecting states
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
            #endregion


            //make machine
            StateTable<string> stateTable = new StateTable<string>( states);
            NFM<string> nfm = new NFM<string>(alphabet, stateTable);

            Console.Write(Notes.machineIsReadyNote);
            nfm.Name = Console.ReadLine();
                
            Console.Write(Notes.constructingTableStateEndNote);

            return nfm; 
        }

        /// <summary>
        /// Read and analize an input strings by NFM
        /// </summary>
        /// <param name="nfm">Used NFM</param>
        static void useNFM(NFM<string> nfm)
        {
            Console.Write(Notes.useMachineUseNote);

            nfm.InputString = Console.ReadLine().Select(x=>"" + x).ToList();
            nfm.processString(); // Compute allowing of input string 
            Console.WriteLine("Input string is{0} acceping. \n", nfm.IsFinal?"":" not");
            nfm.ReInitialize();
            List<List<string>> substrs = nfm.GetAcceptingSubstrFromInput().ToList(); 
            if(substrs.Count == 0 )
                Console.WriteLine("Input string isn't contain any accepting substring.");
            else
            {
                foreach (var item in substrs)
                {
                    Console.Write("The follow substring was accepting: "); 
                    Console.WriteLine(item.Aggregate(" ", (acum, x)=>acum + x));
                }
            }

            // Back all parameters of the machine to default to future use
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
                                    {
                                        #region load from file 
                                        Console.Write("Please, enter  file name: ");
                                        string name = Console.ReadLine();

                                        NFM<string> machine = loadNFM(name);
                                        if (machine != null)
                                            machines.Add(machine);
                                        #endregion
                                    }
                                    break;

                                case "2":
                                    {
                                        #region use already exists machine
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
                                        #endregion
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
