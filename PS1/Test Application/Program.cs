﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;


namespace Test_Application
{
    /// <summary>
    /// This Program is the tester program
    /// </summary>
    class T
    {
        static void Main(string[] args)
        {
           Console.WriteLine(Evaluator.Evaluate("6/3 + a",Lookup));
           Console.Read();
        }

        static double Lookup(string s)
        {
            var value = new Dictionary<string, int>
            {
                {"X", 4},
                {"Z", 6},
                {"a",1 }
            };
            if (!value.ContainsKey(s)) throw new ArgumentException("Variable not found");
            return value[s];
        }
       

        
    }
}
