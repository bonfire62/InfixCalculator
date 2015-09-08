using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
    public static class Evaluator
    {
        public delegate int Lookup(string v);

        public static int Evaluate(string exp, Lookup variableEvaluatorLookup)
        {
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            for (int i = 0; i < substrings.Length; i++)
            {
                substrings[i] = substrings[i].Trim();
            }
            Stack<int> valueStack = new Stack<int>();
            Stack<string> operatorStack = new Stack<string>();

            for (int i = substrings.Length - 1; i >= 0; i--)
            {
                int intPop;
                if (int.TryParse(substrings[i], out intPop))
                {
                    if (operatorStack.Peek() == ("*") || operatorStack.Peek() == ("/"))
                    {
                        valueStack.Pop();
                    }
                }
                
            }
            {
               

            }
            
       
            
            
        }

    



}
