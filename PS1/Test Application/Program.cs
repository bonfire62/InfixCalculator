using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;


namespace PS1_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
           Console.WriteLine(Evaluator.Evaluate("4+3",Lookup));
           Console.Read();


        }

        static int Lookup(string s)
        {
            var value = new Dictionary<string, int>
            {
                {"X", 4},
                {"Z", 6}
            };
            if (!value.ContainsKey(s)) throw new ArgumentException("Variable not found");
            return value[s];
        }
       

        
    }
}
