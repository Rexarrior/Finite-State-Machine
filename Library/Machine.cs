using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



//notice: serialization by xml serializer not implemented.
namespace FiniteStateMachineLibrary
{
    
   public class NFM<LiteralType>
    {
        #region private

        #region fields
        StateTable<LiteralType> _stateTable; // table of station for NFM. Must be correct anytime. 

        Alphabet<LiteralType> _alphabet; // Alphabet of NFM

        
        List<State<LiteralType>> _currentStates;

      
        bool _isBroken; // True if input string is wrong

       
        bool _isFinal;

        List<LiteralType> _inputString;  // Input string will be processing by NFM.


        string _name; 
        #endregion


        #region methods
        void _checkRelationsAlphabetAndStateTable()
        {
            if (!_stateTable.States.Select(x => x.NextStates).All(x => x.All(y => Alphabet.ContainsLiteral(y.first))))
            {
                var ex= new AlphabetAndStateTableDiscrepancyException(
                    "The state contains a literal the alphabet not including",
                    _stateTable.States.First(x => x.NextStates.Any(y => !Alphabet.ContainsLiteral(y.first)))
                    );
                throw ex; 
            }
            if (!_stateTable.CheckAll())
                throw new Exception("StateTable has broken");
                    
                    
                    //  throw new StateTable<LiteralType>.NextStatesDiscrepancyExeption("StateTable contain state have  contained unknown next state ");
        }



        private void _reInit ()
        {
            _isBroken = false;
            _isFinal = false;
            _stateTable.SetStartingState();
            _currentStates = new List<State<LiteralType>>();
            _currentStates.Add(_stateTable.StartingState );

        }

      
        #endregion


        #endregion


        #region publick

        #region exeptions
        public class AlphabetAndStateTableDiscrepancyException: Exception
        {
            public State<LiteralType> wrongState;

            public AlphabetAndStateTableDiscrepancyException(string message, State<LiteralType> state): base(message)
            {
                wrongState = state; 
            }
        }
        #endregion

        #region methods
        public void processString()
        {
            Queue<LiteralType> workQueue = new Queue<LiteralType>(_inputString);

            while (workQueue.Count > 0 && !_isBroken)
            {
                LiteralType literal = workQueue.Dequeue();

                List<State<LiteralType>> nextStates = _currentStates.Select(
                    x => x.GetNext(literal)).Aggregate(
                    new List<State<LiteralType>>(),
                    (acum, x) => acum.Concat(x).ToList());

                if (nextStates.Count == 0)
                    _isBroken = true;
                _currentStates = nextStates;  
            }

            _isFinal = !_isBroken && _currentStates.Any(x => x.IsFinalState);
        }



        public  void ReInitialize()
        {
            _reInit(); 
        }
        #endregion


        #region properties

        
        public List<LiteralType> InputString
        {
            get
            {
                return _inputString;
            }

            set
            {
                _inputString = value;
                _reInit();
                
            }
        }



        public bool IsBroken
        {
            get
            {
                return _isBroken;
            }

            
        }


      
        public Alphabet<LiteralType> Alphabet
        {
            get
            {
                return _alphabet;
            }

            set
            {
                _alphabet = value;
                if (value != null && _stateTable != null)
                    _checkRelationsAlphabetAndStateTable();
                
            }
        }



        public bool IsFinal
        {
            get
            {
                return _isFinal;
            }

          
        }




        public List<State<LiteralType>> CurrentStates
        {
            get
            {
                return _currentStates;
            }

           
        }

        public StateTable<LiteralType> StateTable
        {
            get
            {
                return _stateTable;
            }

            set
            {
                _stateTable = value;

                
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
        
        #endregion

        #region constructors
        public NFM(Alphabet<LiteralType> alphabet, StateTable<LiteralType> stateTable)
        {
            _alphabet = alphabet;
            _stateTable = stateTable;
           

        }



        public NFM()
        {
            _alphabet = null;
            _stateTable = null;
          

        }


       

        #endregion
        
        #endregion

    }
}
