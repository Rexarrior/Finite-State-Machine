using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;



namespace FiniteStateMachineLibrary
{

   public  class StateTable<LiteralType>
    {



        #region private

        #region fields
        List<State<LiteralType>> _states;  // list of all states in table

      
        private State<LiteralType> _StartingState;
        #endregion

        #region methods
        private bool _checkAll()
        {
            bool isSingleStartingStateContain = _states.Count(x => x.IsStartingState) == 1;
            bool isFinalStateContain = _states.Any(x => x.IsFinalState);
            bool isNextStatesContain = _states.Select(x => x.NextStates).All(x => x.All(y => _states.Contains(y.second)));
            
            return isFinalStateContain && isNextStatesContain && isSingleStartingStateContain; 
        }  // verify table
        #endregion

        #endregion


        #region public


        #region exeptions
        public class StateAlreadyExistsExeption : Exception
        {
            State<LiteralType> state;

            public StateAlreadyExistsExeption(string message, State<LiteralType> state) : base(message)
            {
                this.state = state;
            }
        }



        public class NextStatesDiscrepancyExeption : Exception
        {
            public NextStatesDiscrepancyExeption(string message) : base(message) { }
        }



        public class SoManyStartStateException : Exception
        {
            public SoManyStartStateException(string message) : base(message) { }
        }



        public class StartingStateDoesntExistsExeption : Exception
        {
            public StartingStateDoesntExistsExeption(string message) : base(message) { }

        }
        #endregion


        #region methods
        public void AddState(State<LiteralType> state) // Add a new state in table
        {
            if (_states.Any(x => x.Id == state.Id))
                throw new StateAlreadyExistsExeption("State defined by it's id. This id already exists",state); 

            int i = 0;
            while (i < _states.Count && _states[i] < state)
                i++;
            if (i == _states.Count)
                _states.Add(state);
            else
                _states.Insert(i, state);

        }  


        public bool RemoveState(int id)
        {
            if (_states.Select(x => x.Id).Any(x => x == id))
                _states.Remove(_states.Find(x => x.Id == id));
            else
                return false;
            return true;
        }



        public State<LiteralType> this[int i]
        {
            get { return _states[i]; }
            set { _states[i] = value; }
        }
       

        
        public void SetStartingState()
        {
            try
            {
                _StartingState = _states.Find(x => x.IsStartingState);
            }
            catch (ArgumentNullException)
            {
                throw new StartingStateDoesntExistsExeption("State Table doesn't contain the starting state");
            }
        }



        public bool CheckAll()
        {
            return _checkAll();
        }
        #endregion

        

        #region properties
        public int Count
        {
            get { return _states.Count; }
        }



       public  List<State<LiteralType>> States
        {
            get
            {
                return _states;
            }

            set
            {
                _states = value;
            }
        }



        public State<LiteralType> StartingState
        {
            get
            {
                return _StartingState;
            }

         
        }






        #endregion

        #region constructors
        public StateTable()
        {
            _states = new List<State<LiteralType>>();
        }



        public StateTable(IEnumerable<State<LiteralType>> states )
        {
            _states = new List<State<LiteralType>>( states.OrderByDescending( x => x.Id));
            
        }

        #endregion

        #endregion


    }
}
