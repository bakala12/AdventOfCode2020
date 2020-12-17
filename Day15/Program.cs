using System;
using System.Linq;

namespace Day15
{
    class Program
    {
        static string Input = "6,3,15,13,1,0";

        static void Main(string[] args)
        {
            var numbers = Input.Split(',').Select(x => int.Parse(x)).ToArray();
            Console.WriteLine(Part1(numbers));
            Console.WriteLine(Part2(numbers));
        }

        static int Part1(int[] numbers, int n = 2020)
        {   
            int[] spoken = new int[n];
            int[] lastIndex = new int[n];
            int[] prevIndex = new int[n];
            for(int i = 0; i < numbers.Length; i++)
            {
                spoken[i] = numbers[i];
                lastIndex[numbers[i]] = i+1;
                prevIndex[numbers[i]] = 0;
            }
            var last = spoken[numbers.Length - 1];
            for(int i = numbers.Length; i < n; i++)
            {
                if(prevIndex[last] > 0)
                {   
                    spoken[i] = i - prevIndex[last];
                }
                else 
                {                    
                    spoken[i] = 0;
                }    
                last = spoken[i];
                prevIndex[last] = lastIndex[last];
                lastIndex[spoken[i]] = i+1;
            }
            return spoken[n-1];
        }

        static int Part2(int[] numbers)
        {
            return Part1(numbers, 30000000);
        }
    }
}
