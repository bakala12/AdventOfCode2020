using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    class Field
    {
        public string Name;
        public int Min1;
        public int Max1;
        public int Min2;
        public int Max2;

        public static Field Parse(string line)
        {
            var s = line.Split(':');
            var s1 = s[1].Split("or");
            var m1 = s1[0].Trim().Split('-');
            var m2 = s1[1].Trim().Split('-');
            var f = new Field()
            {
                Name = s[0],
                Min1 = int.Parse(m1[0]),
                Max1 = int.Parse(m1[1]),
                Min2 = int.Parse(m2[0]),
                Max2 = int.Parse(m2[1])
            };
            return f;
        }

        public bool IsValueValid(int x)
        {
            return (x >= Min1 && x <= Max1) || (x >= Min2 && x <= Max2);
        }
    }

    class Ticket
    {
        public int[] Values;

        public Ticket(string line)
        {
            Values = line.Split(',').Select(x => int.Parse(x)).ToArray();
        }

        public int GetScanningRate(List<Field> fields)
        {
            int scanningRate = 0;
            for(int i = 0; i < Values.Length; i++)
            {
                if(fields.All(f => !f.IsValueValid(Values[i])))
                    scanningRate += Values[i];
            }
            return scanningRate;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var (f, my, near) = Parse(lines);
            Console.WriteLine(Part1(near, f));
            Console.WriteLine(Part2(near, f, my));
        }

        static (List<Field> fields, Ticket myTicket, List<Ticket> nearbyTickets) Parse(string[] lines)
        {
            int i = 0;
            var fields = new List<Field>();
            while(i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
            {
                fields.Add(Field.Parse(lines[i++]));
            }
            i += 2;
            var ownTicket = new Ticket(lines[i++]);  
            i += 2;
            List<Ticket> nearbyTickets = new List<Ticket>();
            while(i < lines.Length)
            {
                nearbyTickets.Add(new Ticket(lines[i++]));
            }
            return (fields, ownTicket, nearbyTickets);
        }

        static int Part1(List<Ticket> nearby, List<Field> fields)
        {
            return nearby.Sum(t => t.GetScanningRate(fields));
        }

        static int OnlyOneTrue(bool[] tab)
        {
            int ind = -1;
            for(int x = 0; x < tab.Length; x++)
            {
                if(tab[x] && ind >= 0)
                    return -1;
                else if(tab[x])
                    ind = x;
            }
            return ind;
        }

        static long Part2(List<Ticket> nearby, List<Field> fields, Ticket myTicket)
        {
            var onlyValid = nearby.Where(t => t.GetScanningRate(fields) == 0).ToArray();
            int fc = fields.Count;
            bool[][] isFieldMatchingValueOn = new bool[fc][];
            for(int f = 0; f < fc; f++)
            {    
                isFieldMatchingValueOn[f] = new bool[fc];
                for(int ff = 0; ff < fc; ff++)
                    isFieldMatchingValueOn[f][ff] = true;
                foreach(var t in onlyValid)
                {
                    for(int fv = 0; fv < fc; fv++)
                    {
                        isFieldMatchingValueOn[f][fv] &= fields[f].IsValueValid(t.Values[fv]);
                    }
                }
            }
            bool[] toSkip = new bool[fc];
            int[] fieldIndex = new int[fc];
            int notSkipped = fc;
            while(notSkipped > 0)
            {
                for(int f = 0; f < fc; f++)
                {
                    if(toSkip[f]) continue;
                    var ind = OnlyOneTrue(isFieldMatchingValueOn[f]);
                    if(ind >= 0)
                    {
                        toSkip[f] = true;
                        notSkipped--;
                        fieldIndex[f] = ind;
                        for(int ff = 0; ff < fc; ff++)
                        {
                            if(ff != f)
                            {
                                isFieldMatchingValueOn[ff][ind] = false;
                            }
                        }
                    }
                }
            }
            long s = 1;
            for(int f = 0; f < fc; f++)
            {
                if(fields[f].Name.StartsWith("departure"))
                {
                    s *= myTicket.Values[fieldIndex[f]];
                }
            }
            return s;
        }
    }
}
