using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    enum Operator
    {
        Addition,
        Multiplication,
        None
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine(Part1(lines));
            Console.WriteLine(Part2(lines));
        }

        static (long, int) EvaluateExpressionEqualPriority(string line, int currentIndex)
        {
            long v = 0;
            var oper = Operator.None;
            while(currentIndex < line.Length)
            {
                if(line[currentIndex] == '+') oper = Operator.Addition;
                else if(line[currentIndex] == '*') oper = Operator.Multiplication;
                else if(line[currentIndex] >= '0' && line[currentIndex] <= '9')
                {
                    long val = line[currentIndex] - '0';
                    switch(oper)
                    {
                        case Operator.None:
                            v = val;
                            break;
                        case Operator.Addition:
                            v += val;
                            break;
                        case Operator.Multiplication:
                            v *= val;
                            break;
                    }
                }
                else if(line[currentIndex] == '(')
                {
                    (long val, int ind) = EvaluateExpressionEqualPriority(line, currentIndex+1);
                    switch(oper)
                    {
                        case Operator.None:
                            v = val;
                            break;
                        case Operator.Addition:
                            v += val;
                            break;
                        case Operator.Multiplication:
                            v *= val;
                            break;
                    }
                    currentIndex = ind;
                }
                else if(line[currentIndex] == ')')
                {
                    return (v, currentIndex);
                }
                currentIndex++;
            }           
            return (v, currentIndex);
        }

        static long Part1(string[] lines)
        {
            return lines.Sum(l => EvaluateExpressionEqualPriority(l, 0).Item1);
        }

        static (long, int) EvaluateExpressionAdditionBeforeMultiplication(string line, int startIndex)
        {
            List<long> operands = new List<long>();
            List<Operator> operators = new List<Operator>();
            int endIndex = line.Length;
            while(startIndex < line.Length)
            {
                if(line[startIndex] >= '0' && line[startIndex] <= '9')
                    operands.Add(line[startIndex] - '0');
                else if(line[startIndex] == '+') 
                    operators.Add(Operator.Addition);
                else if(line[startIndex] == '*')
                    operators.Add(Operator.Multiplication);
                else if(line[startIndex] == '(')
                {
                    var (val, ind) = EvaluateExpressionAdditionBeforeMultiplication(line, startIndex+1);
                    operands.Add(val);
                    startIndex = ind;
                }
                else if(line[startIndex] == ')')
                {
                    endIndex = startIndex+1;
                    break;
                }
                startIndex++;
            }
            while(operators.Count > 0)
            {
                var addition = -1;
                for(int i = 0; i < operators.Count; i++)
                {
                    if(operators[i] == Operator.Addition)
                        addition = i;
                }
                if(addition >= 0)
                {
                    operators.RemoveAt(addition);
                    var v1 = operands[addition];
                    var v2 = operands[addition+1];
                    operands.RemoveAt(addition+1);
                    operands[addition] = v1 + v2;
                }
                else
                {
                    operators.RemoveAt(0);
                    var v1 = operands[0];
                    var v2 = operands[1];
                    operands.RemoveAt(1);
                    operands[0] = v1 * v2;
                }
            }
            return (operands[0], startIndex);
        }

        static long Part2(string[] lines)
        {
            return lines.Sum(l => EvaluateExpressionAdditionBeforeMultiplication(l, 0).Item1);
        }
    }
}
