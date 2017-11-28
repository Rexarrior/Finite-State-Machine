using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FiniteStateMachineLibrary
{ 
   
  public  class Alphabet<LiteralType> 
        
    {
        #region private; 
        #region fields
        private List<LiteralType> _literals;

       
        #endregion
        #endregion

        #region public

        #region exeptions

        public class LiteralAlreadyExistExeption : Exception
        {
            LiteralType literal;

            public LiteralAlreadyExistExeption(string message, LiteralType literal) : base(message)
            {
                this.literal = literal;

            }
        }
        #endregion

        #region methods
        public LiteralType this[int i]
        {
            get { return _literals[i]; }

        }



        public bool ContainsLiteral( LiteralType literal)
        {
            return _literals.Contains(literal);
        }



        public void Add(LiteralType literal) //Add a literal to the alphabet 
        {
            if (!_literals.Contains(literal))
                _literals.Add(literal);
            else
                throw new LiteralAlreadyExistExeption("Литералы словаря должны быть уникальны.", literal);
        }




        public bool Erase(LiteralType literal) // Delete a literal from the alphabet.
        {
            if (_literals.Remove(literal))
                return true;
            else
                return false;
        }




        public override string ToString()
        
        {
            return _literals.Aggregate("", (acum, x) => acum + x.ToString());
        }


        
        #endregion

        #region properties
        public List<LiteralType> Literals
        {
            get
            {
                return _literals;
            }

            set
            {
                _literals = value;
            }
        }
        #endregion

        #region constructors
        public Alphabet()
        {
            _literals = new List<LiteralType>();
        }


        public Alphabet(IEnumerable<LiteralType> literals)
        {
            _literals = new List<LiteralType>(literals.Distinct()); // create a list of unique literals


        }
        #endregion
        #endregion
    }

}
