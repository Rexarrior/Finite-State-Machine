using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Tests
{

    class Programm
    {

        public static void Main(string[] args)
        {
            MyTaskTest test = new MyTaskTest("test_logs.txt");
            test.Test();

        }



    }
    
}