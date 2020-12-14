using System;
using System.IO;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine(Part1(lines));
            Console.WriteLine(Part2(lines));
        }

        static int AnyoneAnswer(string[] line, ref int currentLine, bool[] questions)
        {
            while(currentLine < line.Length && !string.IsNullOrWhiteSpace(line[currentLine]))
            {
                foreach(var c in line[currentLine])
                {
                    questions[c-'a'] = true;
                }
                currentLine++;
            }
            currentLine++;
            return questions.Count(c => c);
        }

        static int EveryoneAnswer(string[] line, ref int currentLine, int[] questions)
        {
            int gc = 0;
            while(currentLine < line.Length && !string.IsNullOrWhiteSpace(line[currentLine]))
            {
                foreach(var c in line[currentLine])
                {
                    questions[c-'a']++;
                }
                currentLine++;
                gc++;
            }
            currentLine++;
            return questions.Count(c => c == gc);
        }

        static int Part1(string[] lines)
        {
            int cl = 0;
            int sum = 0;
            while(cl < lines.Length)
            {
                sum += AnyoneAnswer(lines, ref cl, new bool[26]);
            }
            return sum;
        }

        static int Part2(string[] lines)
        {
            int cl = 0;
            int sum = 0;
            while(cl < lines.Length)
            {
                sum += EveryoneAnswer(lines, ref cl, new int[26]);
            }
            return sum;
        }
    }
}
