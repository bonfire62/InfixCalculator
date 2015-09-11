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

        public static int Evaluate(string exp, Lookup variableEvaluatorLookup)
        {
            var finalAns = 0;
 
            Regex regex = new Regex("^[a-zA-Z]+[0-9]+$");
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            for (var i = 0; i < substrings.Length; i++)
            {
                substrings[i] = substrings[i].Trim();
            }
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            try
            {
                foreach (var t in substrings)
                {
                    var token = t;
                    double doubToken;
                    if (double.TryParse(token, out doubToken))
                    {
                        if (valueStack.Count > 0)
                        {
                            if (((operatorStack.Peek() == ("*") || operatorStack.Peek() == ("/"))))
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
                    else if (regex.IsMatch(t))
                    {
                        double num = variableEvaluatorLookup(token);

                    }
                    else if ((token == "+" || token == "-") && valueStack.Count >= 2)
                    {
                        if ((operatorStack.Peek() == "+") || (operatorStack.Peek() == "-"))
                        {
                            var popVal = valueStack.Pop();
                            var popVal1 = valueStack.Pop();
                            var popOp = operatorStack.Pop();
                            var resultVal = MathEvaluator(popOp, popVal, popVal1);
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
                            var popVal = valueStack.Pop();
                            var popVal1 = valueStack.Pop();
                            var popOp = operatorStack.Pop();
                            var resultVal = MathEvaluator(popOp, popVal, popVal1);
                            valueStack.Push(resultVal);
                            if (operatorStack.Peek() == "(")
                            {
                                operatorStack.Pop();
                            }
                        }
                        if (((operatorStack.Peek() == "*" || operatorStack.Peek() == "/") && valueStack.Count() >= 2))
                        {
                            var popVal = valueStack.Pop();
                            var popVal1 = valueStack.Pop();
                            var popOp = operatorStack.Pop();
                            var resultVal = MathEvaluator(popOp, popVal, popVal1);
                            valueStack.Push(resultVal);
                        }
                    }

                    
                    


                }
                if (!operatorStack.Any())
                {
                    finalAns = Convert.ToInt32(valueStack.Pop());  
                }
                if (operatorStack.Any())
                {
                    if (operatorStack.Peek() == "+" || operatorStack.Peek() == "-")
                    {
                        var popVal = valueStack.Pop();
                        var popVal1 = valueStack.Pop();
                        var popOp = operatorStack.Pop();
                        var resultVal = MathEvaluator(popOp, popVal, popVal1);
                        finalAns = Convert.ToInt32(resultVal);
                    }
                }
                
                //if (!operatorStack.Any())
                //{
                //    if (valueStack.Count == 1)
                //    {
                //        finalAns = Convert.ToInt32(valueStack.Pop());

                //    }
                //}
                
            }   
            catch
            {
            }
            return finalAns;
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