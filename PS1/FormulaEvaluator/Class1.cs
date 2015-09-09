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
            int loopCount = 0;
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

                    if (valueStack.Count > 0)
                    {
                        if ((operatorStack.Peek() == ("*") || operatorStack.Peek() == ("/") && loopCount > 0))
                        {
                            int popVal = valueStack.Pop();
                            string popString = operatorStack.Pop();

                        }
                    }
                }

            }



        }

        public static int OperatorEvaluator( string givenOperator, int givenNumber, int subjectNumber )
        {
         //will return int that has been operated on give certain int or operator value   
            if (givenOperator == "+")
            {
                int addInt = givenNumber + subjectNumber;
                return addInt;
            }
            if (givenOperator == "*")
            {
                int multInt = givenNumber*subjectNumber;
                return multInt;
            }
            if (givenOperator == "/")
            {
                int divInt = givenNumber/subjectNumber;
                return divInt;
            }
            if (givenOperator == "-")
            {
                int subInt = givenNumber - subjectNumber;
                return subInt;
            }
            return 0;
        }

    }



}
