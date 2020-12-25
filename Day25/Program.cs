using System;
using System.IO;
using System.Linq;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            var publicKeys = File.ReadAllLines("input.txt").Select(x => long.Parse(x)).ToArray();
            Console.WriteLine(Part1(publicKeys[0], publicKeys[1]));
        }

        static long Part1(long cardKey, long doorKey)
        {
            int cardLoopSize = 0;
            long n = 7;
            long r = 1;
            while(r != cardKey)
            {
                r *= n;
                r %= 20201227;
                cardLoopSize++; 
            }
            n = doorKey;
            r = 1;
            for(int i = 0; i < cardLoopSize; i++)
            {
                r *= n;
                r %= 20201227;
            }
            return r;
        }
    }
}
