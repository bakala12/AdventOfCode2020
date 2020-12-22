using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var (p1, p2) = Parse(lines);
            Console.WriteLine(Part1(new Queue<int>(p1.ToArray()),new Queue<int>(p2.ToArray())));
            Console.WriteLine(Part2(new Queue<int>(p1.ToArray()),new Queue<int>(p2.ToArray())));
        }

        static (Queue<int>, Queue<int>) Parse(string[] lines)
        {
            int ind = 1;
            Queue<int> player1 = new Queue<int>();
            Queue<int> player2 = new Queue<int>();
            while(!string.IsNullOrWhiteSpace(lines[ind]))
            {
                player1.Enqueue(int.Parse(lines[ind]));
                ind++;
            }
            ind+=2;
            while(ind < lines.Length)
            {
                player2.Enqueue(int.Parse(lines[ind]));
                ind++;
            }
            return (player1, player2);
        }

        static int CalculateValue(Queue<int> cards)
        {
            var l = cards.Count;
            int v = 0;
            foreach(var c in cards)
            {
                v += c * l;
                l--;
            }
            return v;
        }

        static int Part1(Queue<int> player1, Queue<int> player2)
        {
            int r = 100000;
            while(r-- >= 0)
            {
                if(!player1.TryDequeue(out int p1))
                    return CalculateValue(player2);
                if(!player2.TryDequeue(out int p2))
                    return CalculateValue(player1);
                if(p1 > p2)
                {
                    player1.Enqueue(p1);
                    player1.Enqueue(p2);
                }
                else
                {
                    player2.Enqueue(p2);
                    player2.Enqueue(p1);
                }
            }
            return -1;
        }

        static string DeckToString(Queue<int> deck)
        {
            var sb = new StringBuilder();
            foreach(var c in deck)
                sb.Append($"{c} ");
            return sb.ToString();
        }

        static bool RecursiveCombat(Queue<int> player1, Queue<int> player2, int d = 1)
        {
            int r = int.MaxValue;
            List<(string,string)> history = new List<(string, string)>();
            while(r-- >= 0)
            {
                var p = (DeckToString(player1), DeckToString(player2));
                if(history.Contains(p))
                    return true;
                else
                    history.Add(p);
                if(!player1.TryDequeue(out int p1))
                    return false;
                if(!player2.TryDequeue(out int p2))
                    return true;
                if(player1.Count < p1 || player2.Count < p2)
                {
                    if(p1 > p2)
                    {
                        player1.Enqueue(p1);
                        player1.Enqueue(p2);
                    }
                    else
                    {
                        player2.Enqueue(p2);
                        player2.Enqueue(p1);
                    }
                }
                else
                {
                    var s = RecursiveCombat(new Queue<int>(player1.ToArray().Take(p1)), new Queue<int>(player2.ToArray().Take(p2)), d+1);
                    if(s)
                    {
                        player1.Enqueue(p1);
                        player1.Enqueue(p2);
                    }
                    else
                    {
                        player2.Enqueue(p2);
                        player2.Enqueue(p1);
                    }
                }
            }
            return false;
        }

        static int Part2(Queue<int> player1, Queue<int> player2)
        {
            RecursiveCombat(player1, player2);
            return CalculateValue(player1.Count == 0 ? player2 : player1);
        }
    }
}
