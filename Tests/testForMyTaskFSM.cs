using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using FiniteStateMachineLibrary;
namespace Tests
{
    public class  MyTaskTest
    {
        public string filename; 
        Random randomizer = new Random(DateTime.Now.Millisecond); 

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






        public string GetRandomString(int length, bool isLongTest)
        {
    
            List<string> elems = new List<string>(3) {"%", "*a*", "*"};
            string str = "";

            




            if (isLongTest)
            {
                while (length > 0)
                {
                    int choice = randomizer.Next() % 3;
                    // Console.WriteLine(choice);
                    str += elems[choice];
                    if (choice == 1)
                        length -= 3; 
                    else
                        length -= 1; 
                }
                    
            }
            else 
            {
                str = "%";
                length -= 2; 
                while (length > 0)
                {
                    int choice = randomizer.Next() % 2;
                    // Console.WriteLine(choice);
                    str += elems[1 + choice];
                    if (choice == 0)
                        length -= 3; 
                    else
                        length -= 1; 
                     

                }
                str +="%";

            }
            return str; 

        }

        private bool VerifySubstr(Regex regex, List<List<string>> substr)
        {
            bool isPassed = true; 
            foreach (var item in substr)
            {
                isPassed = isPassed && regex.IsMatch(item.Aggregate("",(acum,x)=>acum  + x)); 
            }

            return isPassed;
        }

        public void Test(uint countLevel = 100, uint countCycle = 100)
        {

           
            


            #region makingMachine
            //     Alphabet<string> alphabet = new Alphabet<string>("* % a".Split(' ').ToList());
            //     List<State<string>> states = new List<State<string>>(7); 
            //     states.Add(new State<string>(alphabet, 1, true, false, "A"));
            //     states.Add(new State<string>(alphabet, 2, false, false, "Q1"));
            //     states.Add(new State<string>(alphabet, 3, false, false, "Q2"));
            //     states.Add(new State<string>(alphabet, 4, false, false, "Q3"));
            //     states.Add(new State<string>(alphabet, 5, false, false, "Q4"));
            //     states.Add(new State<string>(alphabet, 6, false, false, "Q5"));
            //     states.Add(new State<string>(alphabet, 7, false, true, "Q6"));

            //     states[0].NextStates = new List<Pair<string, State<string>>>() { 
            //         new Pair<string, State<string>>("%", states[1])};
            //     states[1].NextStates = new List<Pair<string, State<string>>>() {
            //         new Pair<string, State<string>>("*", states[2])};
            //     states[2].NextStates = new List<Pair<string, State<string>>>() {
            //         new Pair<string, State<string>>("a", states[3])};
            //     states[3].NextStates = new List<Pair<string, State<string>>>() {
            //         new Pair<string, State<string>>("*", states[4])};
  
          //     states[4].NextStates = new List<Pair<string, State<string>>>(){
            //         new Pair<string, State<string>>("*", states[1]),
            //         new Pair<string, State<string>>("*", states[5])};
            //     states[5].NextStates = new List<Pair<string, State<string>>>(){
            //         new Pair<string, State<string>>("*", states[4]),
            //         new Pair<string, State<string>>("%", states[6])};
                
            //     StateTable<string> table = new StateTable<string>(states); 
            //     NFM<string> machine = new NFM<string>(alphabet, table) ;

            #endregion

            
            Regex verifer = new Regex("^%(\\*a\\*\\*(\\*\\*)*)+%$");
            NFM<string> machine = loadNFM("MyTask.txt"); 

            using (StreamWriter output = new StreamWriter(new FileStream(filename, FileMode.OpenOrCreate)))
            {
                

                        // string str = "*a**%";

                        // machine.InputString = str.Select(x=>"" + x).ToList(); 
                       
                        
                        
                        // machine.processString();
                        // bool answVer = verifer.IsMatch(str) ;
                        // Console.WriteLine(machine.IsFinal);
                        // Console.WriteLine(machine.IsBroken);
                        // foreach (var item in machine.StateTable.States)
                        // {
                            
                        //     Console.Write("\n{0} :",item.Id);
                        //     foreach (var item1 in item.NextStates)
                        //     {
                        //     Console.Write("{0} :  {1} ; ",item1.first, item1.second.Id);
                                
                        //     }
                        // }
                        // bool answMach =  machine.IsFinal && !machine.IsBroken; 
                        // string answStr = ""; 
                        // machine.ReInitialize();
                        // List<List<string>> substrs = machine.GetAcceptingSubstrFromInput().ToList(); 


                       
                        // if (answVer == answMach)
                        // {         
                        //     answStr = "PASS"; 
                        // }
                        // else
                        // { 
                        //     answStr = "FAILED"; 
                        // }

                        // string answSubstrs = "";
                        
                        // if (VerifySubstr(verifer, substrs))
                        // {         
                        //     answSubstrs = "PASS"; 
                        // }
                        // else
                        // { 
                        //     answSubstrs = "FAILED"; 
                        // }
                        // output.WriteLine(" main test {0} ; substr test {4} sequence = {1} ; Verifer answer = {2} ; Machine answer = {3}; substrs:\n  ",
                        // answStr, str, answVer, answMach, answSubstrs);
                        // foreach (var item in substrs)
                        // {
                        //     string substr = item.Aggregate("",(acum, x)=> acum + x); 
                        //     // Console.WriteLine(substr);
                        //     output.WriteLine(substr);    
                        // }}



          
                for (int level = 5; level <= countLevel; level++ )
                {
                    for (int i = 0; i < countCycle; i++)
                    {
                        
                        string str = GetRandomString(level, true).Aggregate("",(acum,x)=>(acum + x));

                        machine.InputString = str.Select(x=>"" + x).ToList(); 
                       
                        
                        
                        machine.processString();
                        bool answVer = verifer.IsMatch(str) ;
                        bool answMach =  machine.IsFinal && !machine.IsBroken; 
                        string answStr = ""; 
                        machine.ReInitialize();
                        List<List<string>> substrs = machine.GetAcceptingSubstrFromInput().ToList(); 

                       
                        if (answVer == answMach)
                        {         
                            answStr = "PASS"; 
                        }
                        else
                        { 
                            answStr = "FAILED"; 
                        }

                        string answSubstrs = "";
                        
                        if (VerifySubstr(verifer, substrs))
                        {         
                            answSubstrs = "PASS"; 
                        }
                        else
                        { 
                            answSubstrs = "FAILED"; 
                        }

                        output.WriteLine(" main test {0} ; substr test {4} sequence = {1} ; Verifer answer = {2} ; Machine answer = {3}; substrs:\n  ",
                        answStr, str, answVer, answMach, answSubstrs);
                        foreach (var item in substrs)
                        {
                            string substr = item.Aggregate("",(acum, x)=> acum + x); 
                            // Console.WriteLine(substr);
                            output.WriteLine(substr);    
                        }
                    }
                }
            }
        }


        
        public MyTaskTest(string filename)
        {
            this.filename = filename;
        }
    }
}
