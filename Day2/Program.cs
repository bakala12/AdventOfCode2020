using System;
using System.IO;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine(Part1(lines));
            Console.WriteLine(Part2(lines));
        }

        static int Part1(string[] lines)
        {
            int sum = 0;
            foreach(var l in lines)
            {
                var split = l.Split(' ');
                var password = split[2];
                var letter = split[1][0];
                int[] limits = split[0].Split('-').Select(x => int.Parse(x)).ToArray();
                var c = password.Count(x => x == letter);
                sum += (c >= limits[0] && c <= limits[1]) ? 1 : 0;
            }
            return sum;
        }

        static int Part2(string[] lines)
        {
            int sum = 0;
            foreach(var l in lines)
            {
                var split = l.Split(' ');
                var password = split[2];
                var letter = split[1][0];
                int[] limits = split[0].Split('-').Select(x => int.Parse(x)).ToArray();
                sum += ((password[limits[0]-1] == letter) ^ (password[limits[1]-1] == letter)) ? 1 : 0;
            }
            return sum;
        }
    }
}
