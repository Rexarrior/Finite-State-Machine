using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


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

        List<LiteralType> _inputSequence;  // Input string will be processing by NFM.


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


        private void _processNextSymbol(LiteralType literal)
        {

             List<State<LiteralType>> nextStates = _currentStates.Select(
                    x => x.GetNext(literal)).Aggregate(
                    new List<State<LiteralType>>(),
                    (acum, x) => acum.Concat(x).ToList());

            if (nextStates.Count == 0)
                _isBroken = true;
            _currentStates = nextStates;  
            ChangeStateDelegate temp_ch = StateChahged;
            if (temp_ch != null)
            {
                temp_ch(_currentStates);
            }

            _isFinal = !_isBroken && _currentStates.Any(x => x.IsFinalState);
            
            
            FinalStateAchivedDelegate temp = FinalStateAchived;
            if (temp != null && _isFinal)
            {
                temp();
            }

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

        #region events
        #region delegates
            public delegate void FinalStateAchivedDelegate ();

            public delegate void ChangeStateDelegate (ICollection<State<LiteralType>> achivedStates);

        #endregion
            public  event FinalStateAchivedDelegate  FinalStateAchived; 
            public  event ChangeStateDelegate  StateChahged;  
        #endregion


        #region methods
        public void processString()
        {
           Queue<LiteralType> workQueue = new Queue<LiteralType>(_inputSequence);

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



        public ICollection<List<LiteralType>> GetAcceptingSubstrFromInput()
        {
            List<List<LiteralType>> acceptedSubstring = new List<List<LiteralType>>(); 
           for (int i = 0; i < _inputSequence.Count; i++)
           {   _reInit();
               int j = i; 
               while (j < _inputSequence.Count)
               {
                   this._processNextSymbol(_inputSequence[j]); 
                   if (_isFinal)
                   {
                       acceptedSubstring.Add(_inputSequence.Skip(i).Take(j - i + 1).ToList());
                    //    break;                     
                   }
                   if (_isBroken)
                   {
                       break; 
                   }

                   j++;
                       
               } 
           }
           return acceptedSubstring;
        }
        #endregion


        #region properties

        
        public List<LiteralType> InputString
        {
            set
            {
                _inputSequence = value;
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
