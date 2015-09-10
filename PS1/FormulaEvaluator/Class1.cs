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
            int finalAns = 0;
            int loopCount = 0;
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            for (int i = 0; i < substrings.Length; i++)
            {
                substrings[i] = substrings[i].Trim();
            }
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();

            for (int i = substrings.Length - 1; i >= 0; i--)
            {
                string token = substrings[i];
                double doubToken;
                if (double.TryParse(substrings[i], out doubToken))
                {
                    
                    if (valueStack.Count > 0)
                    {
                        if (((operatorStack.Peek() == ("*") || operatorStack.Peek() == ("/")) && loopCount > 0))
                        {
                            double popVal = valueStack.Pop();
                            string popOp = operatorStack.Pop();
                            double operatedVar = MathEvaluator(popOp, doubToken, popVal);
                            valueStack.Push(operatedVar);
                        }
                        else
                        {
                            valueStack.Push(doubToken);
                        }
                    }
                }
                /*if (/*regex explession here#1#))
                {
                    
                }*/
                if ((token == "+" || token == "-") && valueStack.Count >= 2)
                {
                    if ((operatorStack.Peek() == "+") || (operatorStack.Peek() == "-"))
                    {
                        double popVal = valueStack.Pop();
                        double popVal1 = valueStack.Pop();
                        string popOp = operatorStack.Pop();
                        double resultVal = MathEvaluator(popOp, popVal, popVal1);
                        valueStack.Push(resultVal);
                        operatorStack.Push(token);
                    }
                }
                if (token == "*" || token == "/")
                {
                    operatorStack.Push(token);
                }

                if (token == "(")
                {
                    operatorStack.Push(token);
                }
                if (token == ")")
                {
                    if ((operatorStack.Peek() == "+" || operatorStack.Peek() == "-") && valueStack.Count() >= 2)
                    {
                        double popVal = valueStack.Pop();
                        double popVal1 = valueStack.Pop();
                        string popOp = operatorStack.Pop();
                        double resultVal = MathEvaluator(popOp, popVal, popVal1);

                    }
                }






                return finalAns;
            }

        }

        public static double MathEvaluator( string givenOperator, double givenNumber, double subjectNumber )
        {
            switch (givenOperator)
            {
                case "+":
                    double addDoub = givenNumber + subjectNumber;
                    return addDoub;
                case "*":
                    double multDoub = givenNumber*subjectNumber;
                    return multDoub;
                case "/":
                    double divDoub = givenNumber/subjectNumber;
                    return divDoub;
                case "-":
                    double subDoub = givenNumber - subjectNumber;
                    return subDoub;
            }
            return 0;
        }

    }



}
