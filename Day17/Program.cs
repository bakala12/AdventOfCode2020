using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var cubes = ParseState(lines);
            Console.WriteLine(Part1(cubes));
            var cubes2 = ParseState2(lines);
            Console.WriteLine(Part2(cubes2));
        }

        static List<(int, int, int)> ParseState(string[] lines)
        {
            List<(int,int,int)> active = new List<(int, int, int)>();
            for(int y = 0; y < lines.Length; y++)
                for(int x = 0; x < lines[y].Length; x++)
                    if(lines[y][x] == '#')
                        active.Add((x,y,0));
            return active;
        }

        static List<(int, int, int)> GetNeighbours((int, int, int) cube)
        {
            var neighbours = new List<(int, int, int)>();
            for(int x = cube.Item1 - 1; x <= cube.Item1 + 1; x++)
            {
                for(int y = cube.Item2 - 1; y <= cube.Item2 + 1; y++)
                {
                    for(int z = cube.Item3 - 1; z <= cube.Item3 + 1; z++)
                    {
                        if((x,y,z) != cube)
                            neighbours.Add((x,y,z));
                    }
                }
            }
            return neighbours;
        }

        static int NumberOfActiveNeighbours((int,int,int) cube, List<(int,int,int)> activeCubes)
        {
            return GetNeighbours(cube).Count(nc => activeCubes.Contains(nc));
        }

        static List<(int,int,int)> RunCycle(List<(int,int,int)> activeCubes)
        {
            var newActive = new List<(int,int,int)>();
            Dictionary<(int,int,int), bool> visited = new Dictionary<(int,int,int), bool>();
            foreach (var cube in activeCubes)
            {
                foreach (var c in GetNeighbours(cube))
                {
                    if(visited.TryGetValue(c, out bool v) && v)
                        continue;
                    visited[c] = true;
                    int activeNeighbours = NumberOfActiveNeighbours(c, activeCubes);
                    if((activeCubes.Contains(c) && (activeNeighbours == 2 || activeNeighbours == 3)) ||
                        (!activeCubes.Contains(c) && activeNeighbours == 3))
                    {
                        newActive.Add(c);
                    }
                }
            }
            return newActive;
        }

        static List<(int, int, int, int)> ParseState2(string[] lines)
        {
            List<(int,int,int,int)> active = new List<(int, int, int, int)>();
            for(int y = 0; y < lines.Length; y++)
                for(int x = 0; x < lines[y].Length; x++)
                    if(lines[y][x] == '#')
                        active.Add((x,y,0,0));
            return active;
        }

        static List<(int, int, int, int)> GetNeighbours2((int, int, int, int) cube)
        {
            var neighbours = new List<(int, int, int, int)>();
            for(int x = cube.Item1 - 1; x <= cube.Item1 + 1; x++)
            {
                for(int y = cube.Item2 - 1; y <= cube.Item2 + 1; y++)
                {
                    for(int z = cube.Item3 - 1; z <= cube.Item3 + 1; z++)
                    {
                        for(int w = cube.Item4 - 1; w <= cube.Item4 + 1; w++)
                        {
                            if((x,y,z,w) != cube)
                                neighbours.Add((x,y,z,w));
                        }
                    }
                }
            }
            return neighbours;
        }

        static int NumberOfActiveNeighbours2((int,int,int,int) cube, List<(int,int,int,int)> activeCubes)
        {
            return GetNeighbours2(cube).Count(nc => activeCubes.Contains(nc));
        }

        static List<(int,int,int, int)> RunCycle2(List<(int,int,int, int)> activeCubes)
        {
            var newActive = new List<(int,int,int,int)>();
            Dictionary<(int,int,int,int), bool> visited = new Dictionary<(int,int,int,int), bool>();
            foreach (var cube in activeCubes)
            {
                foreach (var c in GetNeighbours2(cube))
                {
                    if(visited.TryGetValue(c, out bool v) && v)
                        continue;
                    visited[c] = true;
                    int activeNeighbours = NumberOfActiveNeighbours2(c, activeCubes);
                    if((activeCubes.Contains(c) && (activeNeighbours == 2 || activeNeighbours == 3)) ||
                        (!activeCubes.Contains(c) && activeNeighbours == 3))
                    {
                        newActive.Add(c);
                    }
                }
            }
            return newActive;
        }

        static int Part1(List<(int,int,int)> cubes)
        {
            List<(int,int,int)> active = cubes;
            for(int i = 1; i <= 6; i++)
            {
                active = RunCycle(active);
            }
            return active.Count();
        } 

        static int Part2(List<(int,int,int, int)> cubes)
        {
            List<(int,int,int, int)> active = cubes;
            for(int i = 1; i <= 6; i++)
            {
                active = RunCycle2(active);
            }
            return active.Count();
        } 
    }
}
