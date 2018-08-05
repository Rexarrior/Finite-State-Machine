using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FiniteStateMachine
{

    public class Notes
    {

        public const string startNote = " Please, select the action: \n" +
            "1 - Use saved machine. \n" +
            "2 - Construct new machine \n" +
            "3 - Close and save all machine, then quit. \n" +
            "Enter command: "
            ;

        public const string selectMachineNote = "Please, select type of machine:\n" +
            "1 - Non-determine finite machine \n" +
            "another - Cancel and  back to the main menu \n" +
            "Enter command: "
            ;

        public const string contructingAlphabetStartNote =
            "Now you should enter a alphabet for the machine.\n"
            ;


        public const string constructingAlphabetEnteringNote =
            "Please, enter the alphabet's symbols  combined  " +
            "to a string with spase as a delimiter. \n" +
            "Enter symbols: "
            ;

        public const string constructingAlphabetEndNote =
            "Alphabet entering have ended.\n\n"
            ;

        public const string constructingTableStateBeginNote =
            "Now you should construct Table of Machine state.\n " +
            "Please, don't forget, that you should specify only one " +
            "starting state and at least one final state. Also you" +
            "should be careful and avoid specify unknown states. \n\n " +
            "In first, you should enter a count of states in table. \n " +
            "Enter count: "
            ;

        public const string constructingTableStateEnteringStateNote =
            "Advice: Enumerate your states before entering" +
            "and enter one by one. Every state automatically receive a id," +
            "which is number from one.\n" +
            "Enter states one by line in next format: \n\n\n" +
            "caption isStarting isFinal [Next states] \n\n\n " +
            "Where: \n" +
            "caption is any string \n" +
            " isStarting is one of symbols \"1\" \"0\" ;" +
            "Enter \"0\" if state isn't starting state," +
            "otherwise enter \"1\" \n" +
             " isFinal is one of symbols \"1\" \"0\" ;" +
            "Enter \"0\" if state isn't final state," +
            "otherwise enter \"1\" \n" +
            "[Next states] is a sequence of next pair\n" +
            "with spase as determine:\n " +
            "literal id \n" +
            "Where: \n" +
            "literal is a symbol of alphabet; \n" +
            "id is automatically received id \n"

            ;

        public const string constructingTableStateEndNote =
            "Table of states of machine entering have ended.\n"
            ;

        public const string machineIsReadyNote =
            "Your machine is ready. Please, enter it's name: "
            ;

        public const string useMachineSelectModeNote =
            "Please, select mode: \n" +
            "1 - Load a machine from file\n" +
            "2 - Use already exists machine\n " +
            "another - Cancel and  back to the main menu \n" +
            "Enter command: "
            ;

        public const string useInSessionMachineSelect =
            "Please, select a machine: \n"
            ;

        public const string useLoadedMachineSelect =
            "Please, enter the file name: "
            ;

        public const string useLoadedMachineSelectSucces =
            "Your machine have loaded. It's name is "
            ;
        public const string useLoadedMachineSelectFailure =
            "Loading of your machine have failed. "
            ;

        public const string useMachineUseNote =
            "Please, enter input string and see machine answer. \n" +
            "Enter input string: ";


    }
}
