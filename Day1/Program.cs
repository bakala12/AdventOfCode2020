using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Select(x => int.Parse(x)).ToArray();
            int res1 = Part1(input);
            Console.WriteLine(res1);
            int res2 = Part2(input);
            Console.WriteLine(res2);
        }

        static int Part1(int[] input)
        {
            var dic = new Dictionary<int, int>();
            foreach(var l in input)
            {
                if(dic.TryGetValue(l, out int v))
                    return l*(2020-l);
                dic.Add(2020-l, l);
            }
            return -1;
        }

        static int Part2(int[] input)
        {
            int[,] arr = new int[input.Length,input.Length];
            for(int i = 0; i < input.Length; i++)
                for(int j = i+1; j < input.Length; j++)
                {
                    int c = 2020 - input[i] - input[j];
                    for(int k = j+1; k < input.Length; k++)
                    {
                        if(input[k] == c)
                            return input[i] * input[j] * input[k];
                    }
                }
            return -1;
        }
    }
}
