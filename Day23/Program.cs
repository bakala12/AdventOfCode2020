using System;
using System.Collections.Generic;
using System.Linq;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "253149867".Select(c => c - '0').ToArray();
            Console.WriteLine(Part1(input.ToList()));
            Console.WriteLine(Part2(input.ToList()));
        }

        static void Print(List<int> list, int currentCup)
        {
            for(int i = 0; i < list.Count; i++)
            {
                if(i == currentCup)
                    Console.Write($"({list[i]}) ");
                else
                    Console.Write($"{list[i]} ");
            }
            Console.WriteLine();
            Console.Read();
        }

        static int Part1(List<int> input)
        {
            int currentCup = 0;
            for(int i = 0; i < 100; i++)
            {
                int current = input[currentCup];
                int n1 = input[(currentCup+1)%input.Count];
                int n2 = input[(currentCup+2)%input.Count];
                int n3 = input[(currentCup+3)%input.Count];
                input.Remove(n1);
                input.Remove(n2);
                input.Remove(n3);
                int destination = current-1;
                int dInd = -1;
                bool found = false;
                while(!found & destination > 0)
                {
                    dInd = input.IndexOf(destination);
                    if(dInd >= 0)
                    {
                        found = true;
                        break;
                    }
                    destination--;
                }
                if(!found)
                {
                    destination = input.Max();
                    dInd = input.IndexOf(destination);
                }
                input.Insert(dInd + 1, n1);
                input.Insert(dInd + 2, n2);
                input.Insert(dInd + 3, n3);
                currentCup = input.IndexOf(current);
                currentCup = (currentCup+1) % input.Count;
            }
            var index = input.IndexOf(1);
            int value = 0;
            for(int i = (index+1)%input.Count; input[i] != 1; i = (i+1)%input.Count)
            {
                value = value * 10 + input[i];
            }
            return value;
        }

        static long Part2(List<int> input)
        {
            int[] data = new int[10000001];
            int first = 0;
            int prev = 0;
            for(int i = 1; i <= 1000000; i++)
            {
                int v = i;
                if(i <= input.Count)
                    v = input[i-1];
                if(prev > 0)
                    data[prev] = v;
                prev = v;
                if(first == 0)
                    first = v;
            }
            data[prev] = first;
            int current = first;
            for(int it = 1; it <= 10000000; it++)
            {
                int r1 = data[current];
                int r2 = data[r1];
                int r3 = data[r2];
                int dest = current;
                do
                {
                    dest--;
                    if(dest == 0) dest = 1000000;
                } while(r1 == dest || r2 == dest || r3 == dest);
                int next = data[r3];
                data[current] = next;
                data[r3] = data[dest];
                data[dest] = r1;
                current = next;
            }
            return (long)data[1]*(long)data[data[1]];
        }
    }
}
