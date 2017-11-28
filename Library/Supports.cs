using System;
using System.Collections.Generic;

using System.Text;

namespace FiniteStateMachineLibrary
{

    public interface IInitialStringUser
    {
        string ToInitialString();

        void InitializeByInitialString(string InitialString); 
    }



   
   public  class Pair<First, Second> 
    {
        public First first;
        public Second second;



        
        public Pair(First first, Second second)
        {
            this.first = first;
            this.second = second; 
        }
        public Pair()
        {
            first = default(First);
            second = default(Second);

        }

    }





}
