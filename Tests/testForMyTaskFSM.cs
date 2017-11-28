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

        public void Test(uint countLevel = 100, uint countCycle = 100)
        {

            string str = ""; 
            bool answ = false; 
            

            Regex verifer = new Regex("%(\\*a\\*\\*(\\*\\*)*)+%");

            #region makingMachine
                Alphabet<string> alphabet = new Alphabet<string>("* % a".Split(' ').ToList());
                List<State<string>> states = new List<State<string>>(7); 
                states.Add(new State<string>(alphabet, 1, true, false, "A"));
                states.Add(new State<string>(alphabet, 2, false, false, "Q1"));
                states.Add(new State<string>(alphabet, 3, false, false, "Q2"));
                states.Add(new State<string>(alphabet, 4, false, false, "Q3"));
                states.Add(new State<string>(alphabet, 5, false, false, "Q4"));
                states.Add(new State<string>(alphabet, 6, false, false, "Q5"));
                states.Add(new State<string>(alphabet, 7, false, true, "Q6"));

                states[0].NextStates = new List<Pair<string, State<string>>>() { 
                    new Pair<string, State<string>>("%", states[1])};
                states[1].NextStates = new List<Pair<string, State<string>>>() {
                    new Pair<string, State<string>>("*", states[2])};
                states[2].NextStates = new List<Pair<string, State<string>>>() {
                    new Pair<string, State<string>>("a", states[3])};
                states[3].NextStates = new List<Pair<string, State<string>>>() {
                    new Pair<string, State<string>>("*", states[4])};
                states[4].NextStates = new List<Pair<string, State<string>>>(){
                    new Pair<string, State<string>>("*", states[1]),
                    new Pair<string, State<string>>("*", states[5])};
                states[5].NextStates = new List<Pair<string, State<string>>>(){
                    new Pair<string, State<string>>("*", states[4]),
                    new Pair<string, State<string>>("%", states[6])};
                
                StateTable<string> table = new StateTable<string>(states); 
                NFM<string> machine = new NFM<string>(alphabet, table) ;

            #endregion


            using (StreamWriter output = new StreamWriter(new FileStream(filename, FileMode.OpenOrCreate)))
            {
                for (int level = 5; level <= countLevel; level++ )
                {
                    for (int i = 0; i < countCycle; i++)
                    {
                        str = GetRandomString(level, false);
                        machine.InputString = str.Select(x=> "" + x).ToList(); 
                        machine.processString();
                        bool answVer = verifer.IsMatch(str) ;
                        bool answMach =  machine.IsFinal; 
                        string answStr = ""; 

                        if (answVer == answMach)
                            answStr = "PASS"; 
                        else
                            answStr = "FAILED"; 


                        output.WriteLine(" test {0} ; sequence = {1} ; Verifer answer = {2} ; Machine answer = {3}",
                        answStr, str, answVer, answMach);
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