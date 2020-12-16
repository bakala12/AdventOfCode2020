using System;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").Select(x => int.Parse(x)).ToArray();
            Console.WriteLine(Part1(lines));
            Console.WriteLine(Part2(lines));
        }

        static int Part1(int[] lines)
        {
            var adapters = lines.OrderBy(x => x);
            int diff1 = 0, diff3 = 0;
            int start = 0;
            foreach(var a in adapters)
            {
                if(a - start == 1) diff1++;
                if(a - start == 3) diff3++;
                start = a;
            }
            diff3++;
            return diff1 * diff3;
        }

        static long Part2(int[] lines)
        {
            var adapters = lines.OrderBy(x => x).ToArray();
            int maxp = adapters[adapters.Length-1]+3;
            long[,] c = new long[adapters.Length+1, maxp+1];
            c[0,1] = c[0,2] = c[0,3] = 1;
            for(int i = 1; i <= adapters.Length; i++)
            {
                for(int p = 0; p <= maxp; p++)
                    c[i,p] = c[i-1,p];
                int pow = adapters[i-1];
                c[i, pow+1] += c[i-1, pow];
                c[i, pow+2] += c[i-1, pow];
                c[i, pow+3] += c[i-1, pow];
            }
            return c[adapters.Length, maxp];
        }
    }
}
