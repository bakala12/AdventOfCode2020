using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day19
{
    class ChomskyGrammar
    {
        public List<int> NonTerminals;
        public List<(int, int, int)> Productions;
        public List<(int, char)> TermProductions;
        public List<(int, int)> NonTerminalInNonTerminalProductions;

        public ChomskyGrammar()
        {
            NonTerminals = new List<int>();
            Productions = new List<(int, int, int)>();
            TermProductions = new List<(int, char)>();
            NonTerminalInNonTerminalProductions = new List<(int,int)>();
        }

        public void CorrectToChomskyForm()
        {
            while(NonTerminalInNonTerminalProductions.Count > 0)
            {
                var p = NonTerminalInNonTerminalProductions[0];
                NonTerminalInNonTerminalProductions.RemoveAt(0);
                var pr = Productions.Where(x => x.Item1 == p.Item2).ToArray();
                foreach(var p1 in pr)
                {
                    Productions.Add((p.Item1, p1.Item2, p1.Item3));
                }
            }
        }

        public ChomskyGrammar Clone()
        {
            return new ChomskyGrammar()
            {
                NonTerminals = NonTerminals.ToArray().ToList(),
                TermProductions = TermProductions.ToArray().ToList(),
                NonTerminalInNonTerminalProductions = NonTerminalInNonTerminalProductions.ToArray().ToList(),
                Productions = Productions.ToArray().ToList()
            };
        }
    
        public void Print()
        {
            var d = new Dictionary<int, List<(int, int)>>();
            foreach(var p in Productions)
            {
                if(d.TryGetValue(p.Item1, out var l))
                    l.Add((p.Item2, p.Item3));
                else
                    d[p.Item1] = new List<(int, int)>() { (p.Item2, p.Item3) };
            }
            foreach(var p in d.OrderBy(x => x.Key))
            {
                System.Console.WriteLine($"{p.Key}: {string.Join(" | ", p.Value.Select(x => $"{x.Item1} {x.Item2}"))}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var (grammar, words) = ParseInput(lines);
            Console.WriteLine(Part1(grammar, words));
            Console.WriteLine(Part2(grammar, words));
        }

        static (ChomskyGrammar, List<string>) ParseInput(string[] lines)
        {
            var grammar = new ChomskyGrammar();
            int ind = 0;
            while(!string.IsNullOrWhiteSpace(lines[ind]))
            {
                var s = lines[ind].Split(":");
                var startNonTerm = int.Parse(s[0]);
                grammar.NonTerminals.Add(startNonTerm);
                var a = s[1].Trim();
                var b = a.Split('|');
                for(int i = 0; i < b.Length; i++)
                {
                    var c = b[i].Trim().Split(' ');
                    if(c.Length == 1 && c[0].Contains('"'))
                        grammar.TermProductions.Add((startNonTerm, c[0][1]));
                    else if(c.Length == 2)
                        grammar.Productions.Add((startNonTerm, int.Parse(c[0]), int.Parse(c[1])));
                    else if(c.Length == 1 && int.TryParse(c[0], out int rr))
                        grammar.NonTerminalInNonTerminalProductions.Add((startNonTerm, rr));
                    else
                        System.Console.WriteLine($"Invalid production {lines[ind]}");
                }
                ind++;
            }
            ind++;
            var words = new List<string>();
            while(ind < lines.Length)
            {
                words.Add(lines[ind++]);
            }
            return (grammar, words);
        }

        static bool CheckWordGeneratedCYKAlgorithm(ChomskyGrammar grammar, string word)
        {
            // tab[startposition, length-1, nonterminal] == true iff nonterminal generates sequence of characters from start of length
            bool[,,] tab = new bool[word.Length, word.Length, grammar.NonTerminals.Max()+1];
            for(int i = 1; i <= word.Length; i++)
            {
                foreach(var w in grammar.TermProductions)
                    if(w.Item2 == word[i-1])
                        tab[i-1, 0, w.Item1] = true;
            }
            for(int i = 2; i <= word.Length; i++)
            {
                for(int j = 1; j <= word.Length - i + 1; j++)
                {
                    for(int k = 1; k <= i - 1; k++)
                        foreach(var p in grammar.Productions)
                        {
                            if(tab[j-1, k-1, p.Item2] && tab[j-1 + k, i - k - 1, p.Item3])
                                tab[j-1, i-1, p.Item1] = true;
                        }
                }
            }
            var b = tab[0, word.Length-1,0];
            return b;
        }

        static int Part1(ChomskyGrammar grammar, List<string> words)
        {
            var g = grammar.Clone();
            //g.CorrectToChomskyForm();
            return words.Count(w => CheckWordGeneratedCYKAlgorithm(g, w));
        }

        static int Part2(ChomskyGrammar grammar, List<string> words)
        {
            //modify wrong productions:
            //8: 42
            //11: 42 31
            //into
            //8: 42 | 42 8
            //11: 42 31 | 42 11 31
            var g = grammar.Clone();
            g.Productions.Add((8, 42, 8));
            var newNonTerm = g.NonTerminals.Max()+1;
            g.NonTerminals.Add(newNonTerm);
            g.Productions.Add((11, newNonTerm, 31));
            g.Productions.Add((newNonTerm, 42, 11));
            //g.CorrectToChomskyForm();
            //g.Print();
            return words.Count(w => CheckWordGeneratedCYKAlgorithm(g, w));
        }
    }
}
