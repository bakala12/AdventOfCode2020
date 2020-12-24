using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day24
{
    enum Direction
    {
        E,
        SE,
        NE,
        W,
        NW,
        SW
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var input = ParseInput(lines);
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }

        static List<Direction>[] ParseInput(string[] lines)
        {
            List<Direction>[] directions = new List<Direction>[lines.Length];
            int ind = 0;
            foreach(var l in lines)
            {
                directions[ind] = new List<Direction>();
                for(int i = 0; i < l.Length; i++)
                {
                    switch(l[i])
                    {
                        case 'n':
                            directions[ind].Add(l[++i] == 'e' ? Direction.NE : Direction.NW);
                            break;
                        case 's':
                            directions[ind].Add(l[++i] == 'e' ? Direction.SE : Direction.SW);
                            break;
                        case 'e':
                            directions[ind].Add(Direction.E);
                            break;
                        case 'w':
                            directions[ind].Add(Direction.W);
                            break;
                    }
                }
                ind++;
            }
            return directions;
        }

        static int Part1(List<Direction>[] input)
        {
            var colors = new Dictionary<(int,int),bool>();
            foreach(var l in input)
            {
                var loc = (0,0);
                foreach(Direction d in l)
                {
                    switch(d)
                    {
                        case Direction.E:
                            loc = (loc.Item1+1,loc.Item2);
                            break;
                        case Direction.W:
                            loc = (loc.Item1-1,loc.Item2);
                            break;
                        case Direction.NW:
                            loc = (loc.Item1, loc.Item2+1);
                            break;
                        case Direction.NE:
                            loc = (loc.Item1+1, loc.Item2+1);
                            break;
                        case Direction.SW:
                            loc = (loc.Item1-1, loc.Item2-1);
                            break;
                        case Direction.SE:
                            loc = (loc.Item1, loc.Item2-1);
                            break;
                    }
                }
                if(colors.TryGetValue(loc, out bool v))
                    colors[loc] = !v;
                else
                    colors[loc] = true;
            }
            return colors.Count(x => x.Value);
        }

        static List<(int, int)> GetStart(List<Direction>[] input)
        {
            Dictionary<(int,int),bool> colors = new Dictionary<(int, int), bool>();
            foreach(var l in input)
            {
                var loc = (0,0);
                foreach(Direction d in l)
                {
                    switch(d)
                    {
                        case Direction.E:
                            loc = (loc.Item1+1,loc.Item2);
                            break;
                        case Direction.W:
                            loc = (loc.Item1-1,loc.Item2);
                            break;
                        case Direction.NW:
                            loc = (loc.Item1, loc.Item2+1);
                            break;
                        case Direction.NE:
                            loc = (loc.Item1+1, loc.Item2+1);
                            break;
                        case Direction.SW:
                            loc = (loc.Item1-1, loc.Item2-1);
                            break;
                        case Direction.SE:
                            loc = (loc.Item1, loc.Item2-1);
                            break;
                    }
                }
                if(colors.TryGetValue(loc, out bool v))
                    colors[loc] = !v;
                else
                    colors[loc] = true;
            }
            return colors.Where(x => x.Value).Select(x => x.Key).ToList();
        }

        static IEnumerable<(int,int)> AllTilesToFlip(List<(int,int)> black)
        {
            List<(int, int)> list = new List<(int, int)>();
            foreach(var w in black)
            {
                list.Add((w.Item1, w.Item2+1));
                list.Add((w.Item1+1, w.Item2+1));
                list.Add((w.Item1+1, w.Item2));
                list.Add((w.Item1, w.Item2-1));
                list.Add((w.Item1-1, w.Item2-1));
                list.Add((w.Item1-1, w.Item2));
            }
            return list.Distinct();
        }

        static bool FlipToBlack((int,int) w, List<(int,int)> black)
        {
            int c = 0;
            if(black.Contains((w.Item1, w.Item2+1))) c++;
            if(black.Contains((w.Item1+1, w.Item2+1))) c++;
            if(black.Contains((w.Item1+1, w.Item2))) c++;
            if(black.Contains((w.Item1, w.Item2-1))) c++;
            if(black.Contains((w.Item1-1, w.Item2-1))) c++;
            if(black.Contains((w.Item1-1, w.Item2))) c++;
            return (!black.Contains(w) && c == 2) || (black.Contains(w) && (c == 1 || c == 2));
        }

        static int Part2(List<Direction>[] input)
        {
            var black = GetStart(input);
            for(int i = 1; i<=100; i++)
            {
                List<(int,int)> newBlack = new List<(int, int)>();
                foreach(var l in AllTilesToFlip(black))
                    if(FlipToBlack(l, black))
                        newBlack.Add(l);
                black = newBlack;
            }
            return black.Count;
        }
    }
}