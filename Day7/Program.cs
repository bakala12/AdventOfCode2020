using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
{
    class Node
    {
        public string Color;
        public List<(int,Node)> ContainedBy = new List<(int,Node)>();
        public List<(int, Node)> Contains = new List<(int, Node)>();

        public override string ToString()
        {
            return Color;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine(Part1(lines));
            Console.WriteLine(Part2(lines));
        }
        
        static void ParseLine(string line, List<Node> nodes)
        {
            var split = line.Split(' ');
            var mainColor = $"{split[0]} {split[1]}";
            var mainNode = nodes.Find(n => n.Color == mainColor);
            if(mainNode == null)
            {
                mainNode = new Node() { Color = mainColor };
                nodes.Add(mainNode);
            }
            for(int i = 4; i < split.Length; i+= 4)
            {
                if(split[i] == "no" && split[i+1] == "other")
                    break;
                int num = int.Parse(split[i]);
                string containedColor = $"{split[i+1]} {split[i+2]}";
                var cnode = nodes.Find(n => n.Color == containedColor);
                if(cnode == null)
                {    
                    cnode = new Node() { Color = containedColor };
                    nodes.Add(cnode);
                }
                cnode.ContainedBy.Add((num, mainNode));
                mainNode.Contains.Add((num, cnode));
            }
        }

        static List<Node> BuildTree(string[] lines)
        {
            List<Node> allNodes = new List<Node>(); 
            foreach(var line in lines)
                ParseLine(line, allNodes);
            return allNodes;
        }

        static List<string> ReachableColors(Node node, List<string> reachable)
        {
            foreach(var r in node.ContainedBy)
            {
                reachable.Add(r.Item2.Color);
                ReachableColors(r.Item2, reachable);
            }
            return reachable;
        }

        static int Part1(string[] lines)
        {
            var nodes = BuildTree(lines);
            var color = "shiny gold";
            var n = nodes.Find(n => n.Color == color);
            return ReachableColors(n, new List<string>()).Distinct().Count();
        }

        static int HowManyContains(Node n)
        {
            int s = 1;
            if(n.Contains.Count == 0)
                return 1;
            foreach(var c in n.Contains)
            {
                s += c.Item1 * HowManyContains(c.Item2);
            }
            return s;
        }

        static int Part2(string[] lines)
        {
            var nodes = BuildTree(lines);
            var color = "shiny gold";
            var n = nodes.Find(n => n.Color == color);
            return HowManyContains(n) - 1;
        }
    }
}
