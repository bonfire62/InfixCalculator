using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    public static class Evaluator
    {
        public delegate double Lookup(string v);

        /// <summary>
        /// This class is the main class that evaluates a function (given as a string), and returns a double number as the result. It uses two stacks, sorts the string into these stacks, 
        /// then preforms an operation algorithm that performs the operations in the correct order (order of operation).
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="varLookup"></param>
        /// <returns>double</returns>
        public static double Evaluate(string exp, Lookup varLookup)
        {
            Regex regex = new Regex("^[a-zA-Z]+$");
            
            
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
    
            /// Start of main for loop that iterates over array of split strings
            for (var i = 0; i < substrings.Length; i++)
            {
                substrings[i] = substrings[i].Trim();
                double doubToken;
                /// if string is a number
                if (double.TryParse(substrings[i], out doubToken))
                {
                    /// checks if the valuestack is empty
                    if (operatorStack.Any())
                    {
                        if ((operatorStack.Peek() == ("*") || operatorStack.Peek() == ("/")) && valueStack.Any())
                        {
                            var operatedVar = MathEvaluator(operatorStack.Pop(), doubToken, valueStack.Pop());
                            valueStack.Push(operatedVar);
                        
                    }
                    }
                    else
                    {
                        valueStack.Push(doubToken);
                    }
                }
                else if (regex.IsMatch(substrings[i]))
                {
                    doubToken = varLookup(substrings[i]);
                    if ((operatorStack.Peek() == ("*") || operatorStack.Peek() == ("/")) && valueStack.Any())
                    {
                        valueStack.Push(MathEvaluator(operatorStack.Pop(), doubToken, valueStack.Pop()));
                    }
                    else
                    {
                        valueStack.Push(doubToken);
                    }
                }
                else if ((substrings[i].Equals("+") || substrings[i].Equals("-")))
                {
                    
                    if (operatorStack.Any() &&( (operatorStack.Peek() == "+") || (operatorStack.Peek() == "-")))
                    {
                        if (valueStack.Count < 2)
                        {
                            throw new ArgumentException("Invalid Syntax");
                        }
                        var popVal = valueStack.Pop();
                        var popVal1 = valueStack.Pop();
                        var popOp = operatorStack.Pop();
                        var resultVal = MathEvaluator(popOp, popVal, popVal1);
                        valueStack.Push(resultVal);
                        operatorStack.Push(substrings[i]);
                    }
                    else
                    {
                        operatorStack.Push(substrings[i]);
                    }
                }
                else if (substrings[i].Equals("*") || substrings[i].Equals("/"))
                {
                    operatorStack.Push(substrings[i]);
                }

                else if (substrings[i].Equals("("))
                {
                    operatorStack.Push(substrings[i]);
                }
                else if (substrings[i].Equals(")"))
                {
                    if ((operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                    {
                        var popVal = valueStack.Pop();
                        var popVal1 = valueStack.Pop();
                        var popOp = operatorStack.Pop();
                        var resultVal = MathEvaluator(popOp, popVal, popVal1);
                        valueStack.Push(resultVal);
                        if (operatorStack.Peek() == "(")
                        {
                            operatorStack.Pop();
                        }
                        else
                        {
                            throw new ArgumentException("Invalid Syntax");
                        }
                    }
                    else if (((operatorStack.Peek() == "*" || operatorStack.Peek() == "/") && valueStack.Count() >= 2))
                    {
                        var popVal = valueStack.Pop();
                        var popVal1 = valueStack.Pop();
                        var popOp = operatorStack.Pop();
                        var resultVal = MathEvaluator(popOp, popVal, popVal1);
                        valueStack.Push(resultVal);
                    }
                }
            }

            if (operatorStack.Count == 0 && valueStack.Count == 1)
            {
                return valueStack.Pop();
            }
            else if (operatorStack.Count == 1 && valueStack.Count == 2)
            {

                if (operatorStack.Peek() == "+" || operatorStack.Peek() == "-")
                {
                    var popVal = valueStack.Pop();
                    var popVal1 = valueStack.Pop();
                    var popOp = operatorStack.Pop();
                    var resultVal = MathEvaluator(popOp, popVal, popVal1);
                    return resultVal;
                }
               
            }

            else if (operatorStack.Count > 0 && valueStack.Count > 0)
            {
                throw new ArgumentException("Invaild Syntax");
            }
            else
            {
                throw new ArgumentException("Invalid Syntax");
            }
            return 1;
        }






        /// <summary>
        /// MathEvaluator method takes the operator, and operateson the two numbers passed to it. It is used for encapsulation and readability.
        /// </summary>
        /// <param name="givenOperator"></param>
        /// <param name="givenNumber"></param>
        /// <param name="subjectNumber"></param>
        /// <returns></returns>
        public static double MathEvaluator(string givenOperator, double givenNumber, double subjectNumber)
        {
            switch (givenOperator)
            {
                case "+":
                    var addDoub = givenNumber + subjectNumber;
                    return addDoub;
                case "*":
                    var multDoub = givenNumber*subjectNumber;
                    return multDoub;
                case "/":
                    if (subjectNumber == 0)
                    { throw new ArgumentException("Divide by 0");}
                    var divDoub = givenNumber/subjectNumber;
                    return divDoub;
                case "-":
                    var subDoub = givenNumber - subjectNumber;
                    return subDoub;
            }
            return 1;

        }
    }
}