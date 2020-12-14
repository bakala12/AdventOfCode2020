using System;
using System.IO;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine(Part1(lines, 3, 1));
            Console.WriteLine(Part2(lines));
        }

        static int Part1(string[] map, int vx, int vy)
        {
            int trees = 0;
            int x = 0;
            int y = 0;
            int len = map[0].Length;
            while(y < map.Length)
            {
                if(map[y][x] == '#') trees++;
                x += vx;
                if(x >= len) x %= len;
                y+= vy;
            }
            return trees;
        }

        static long Part2(string[] map)
        {
            long res = 1;
            var vectors = new (int,int)[] {(1,1), (3,1), (5,1), (7,1), (1,2)};
            foreach(var v in vectors)
            {
                int r = Part1(map, v.Item1, v.Item2);
                res *= r;
            }
            return res;
        }
    }
}
