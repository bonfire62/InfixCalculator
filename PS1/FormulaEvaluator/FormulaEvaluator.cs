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

        public static double Evaluate(string exp, Lookup variableEvaluatorLookup)
        {
            Regex regex = new Regex("^[a-zA-Z]+[0-9]+$");
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            for (var i = 0; i < substrings.Length; i++)
            {
                substrings[i] = substrings[i].Trim();
            }
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();


            for (int i = 0; i < substrings.Length; i++)
            {
                double doubToken;
                if (double.TryParse(substrings[i], out doubToken))
                {
                    if (valueStack.Count > 0)
                    {
                        if ((operatorStack.Peek() == ("*") || operatorStack.Peek() == ("/")))
                        {
                            var operatedVar = MathEvaluator(operatorStack.Pop(), doubToken, valueStack.Pop());
                            valueStack.Push(operatedVar);
                        }
                        else
                        {
                            valueStack.Push(doubToken);
                        }
                    }
                }
                else if (regex.IsMatch(substrings[i]))
                {
                    doubToken = variableEvaluatorLookup(substrings[i]);
                    if (operatorStack.Peek() == ("*") || operatorStack.Peek() == ("/"))
                    {
                        valueStack.Push(MathEvaluator(operatorStack.Pop(), doubToken, valueStack.Pop()));
                    }
                    else
                    {
                        valueStack.Push(doubToken);
                    }
                }
                else if (((substrings[i].Equals("+") || substrings[i].Equals("-")) && operatorStack.Any()))
                {
                    if ((operatorStack.Peek() == "+") || (operatorStack.Peek() == "-"))
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

        }






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