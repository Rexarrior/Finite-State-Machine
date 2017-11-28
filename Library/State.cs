using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace FiniteStateMachineLibrary
{

    public class State<LiteralType> : IComparable
        
    {

        #region private

        #region fields
        private string _caption; // name of state

        private UInt32 _id; // number of state

        
        private List<Pair<LiteralType, State<LiteralType>>> _nextStates; // all possible next states

        private Alphabet<LiteralType> _alphabet; // alphabet of machine includes the state

        private bool _isStartingState;

        private bool _isFinalState;

        #endregion


        #region methods
   
        #endregion


        #endregion


        #region publick

        #region methods
         int IComparable.CompareTo(object anotherState)
        {
            return this.Id.CompareTo(((State<LiteralType>)anotherState).Id);
        }



        public List<State<LiteralType>> GetNext(LiteralType literal)
        {
            
            return _nextStates.Where(x=>x.first.Equals(literal)).Select(x=>x.second).ToList();
            
        }



        public static bool operator < (State<LiteralType> firstState, State<LiteralType> secondState)
        {
            return firstState.Id < secondState.Id;
        }



        public static bool operator >(State<LiteralType> firstState, State<LiteralType> secondState)
        {
            return firstState.Id < secondState.Id;
        }



       
        #endregion


        #region properties

        public string Caption
        {
            get
            {
                return _caption;
            }

            set
            {
                _caption = value;
            }
        }



        public uint Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
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
            }
        }



        public bool IsStartingState
        {
            get
            {
                return _isStartingState;
            }

            set
            {
                _isStartingState = value;
            }
        }



        public bool IsFinalState
        {
            get
            {
                return _isFinalState;
            }

            set
            {
                _isFinalState = value;
            }
        }



       public List<Pair<LiteralType, State<LiteralType>>> NextStates
        {
            get
            {
                return _nextStates;
            }

            set
            {
                _nextStates = value;
            }
        }
        #endregion

        #region constructors
        public State(
            Alphabet<LiteralType> alphabet,
            IEnumerable<Pair<LiteralType, State<LiteralType>>> nextStates,
            uint id, bool isStartingState = false, bool isFinalState = false,
            string caption = "Unnamed"  )
        {
            _alphabet = alphabet;
            _nextStates = new List<Pair<LiteralType, State<LiteralType>>>();
            _nextStates.AddRange(nextStates);
           
            _isStartingState = isStartingState;
            _isFinalState = isFinalState;
            _caption = caption;
            _id = id;
        }
        
        public State(
           Alphabet<LiteralType> alphabet,
           uint id, bool isStartingState = false, bool isFinalState = false,
           string caption = "Unnamed")
        {
            _alphabet = alphabet;
            _nextStates = new List<Pair<LiteralType, State<LiteralType>>>();
            
            _isStartingState = isStartingState;
            _isFinalState = isFinalState;
            _caption = caption;
            _id = id;
        }


        public State()
        {
            _alphabet = new Alphabet<LiteralType>();
            _isStartingState = false;
            _isFinalState = false;
            _caption = "Unnamed";
            _nextStates = new List<Pair<LiteralType, State<LiteralType>>>();
            
        }

        #endregion

        #endregion

    }




}
