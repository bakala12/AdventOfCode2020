using System;
using System.Collections.Generic;
using System.IO;

namespace Day13
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
            int timestamp = int.Parse(lines[0]);
            int wait = int.MaxValue;
            int id = -1;
            foreach(var bus in lines[1].Split(','))
            {
                if(bus[0] == 'x')
                    continue;
                int busId = int.Parse(bus);
                int w = busId - timestamp % busId;
                if(w < wait)
                {
                    wait = w;
                    id = busId;
                }
            }
            return id * wait;
        }

        static long Part2(string[] lines)
        {
            int i = -1;
            List<Congruence> congruences = new List<Congruence>();
            foreach(var l in lines[1].Split(','))
            {
                i++;
                if(l == "x") continue;
                int m = int.Parse(l);
                congruences.Add(new Congruence(m - i, int.Parse(l)));
            }
            return ChineseRemainderTheorem.Solve(congruences);
        }
    }

    class Congruence
    {
        public long Remainder;
        public long Modulo;

        public Congruence(long r, long m)
        {
            Remainder = r % m;
            Modulo = m;
        }
    }

    static class ChineseRemainderTheorem
    {
        public static long Solve(List<Congruence> congruences)
        {
            long[] Mi = new long[congruences.Count];
            long M = 1;
            for(int i = 0; i < congruences.Count; i++)
            {
                M *= congruences[i].Modulo;
            }
            long x = 0;
            for(int i = 0; i < congruences.Count; i++)
            {
                Mi[i] = M / congruences[i].Modulo;
                (_, long f, long g) = ExtendedEuclideanAlgorithm(congruences[i].Modulo, Mi[i]);
                x += g * Mi[i] * congruences[i].Remainder; 
            }
            while(x < 0) x += M;
            return x;
        }

        static (long, long, long) ExtendedEuclideanAlgorithm(long a, long b)
        {
            if(a < b)
            {
                (long d, long x1, long y1) = ExtendedEuclideanAlgorithm(b, a);
                return (d, y1, x1);
            }
            long x = 1;
            long y = 0;
            long r = 0;
            long s = 1;
            while(b != 0)
            {
                long c = a % b;
                long q = a / b;
                a = b;
                b = c;
                long r1 = r;
                long s1 = s;
                r = x - q * r;
                s = y - q * s;
                x = r1;
                y = s1;
            }
            return (a, x, y);
        }
    }
}
