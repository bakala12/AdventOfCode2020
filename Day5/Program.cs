using System;
using System.IO;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine(Part1(lines));
            Console.WriteLine(Part2(lines));
        }

        static int DecodeSeat(string line)
        {
            int lr = 0, lc = 0, rr=128, rc = 8;
            for(int i = 0; i<7; i++)
            {
                if(line[i] == 'F') rr = (lr+rr)/2;
                else lr = (lr+rr)/2;
            }
            for(int i = 7; i < 10; i++)
            {
                if(line[i] == 'L') rc = (lc+rc)/2;
                else lc = (lc+rc)/2;
            }
            return 8*lr+lc;
        }

        static int Part1(string[] lines)
        {
            int maxSeat = -1;
            foreach(var line in lines)
            {
                int s = DecodeSeat(line);
                if(maxSeat < s)
                    maxSeat = s;
            }
            return maxSeat;
        }

        static int Part2(string[] lines)
        {
            bool[] visited = new bool[1024];
            foreach(var line in lines)
            {
                visited[DecodeSeat(line)] = true;
            }
            for(int i=1; i<1023; i++)
            {
                if(!visited[i] && visited[i-1] && visited[i+1])
                    return i;
            }
            return -1;
        }
    }
}
