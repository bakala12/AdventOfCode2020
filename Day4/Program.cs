using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine(Part1(lines));
            Console.WriteLine(Part2(lines));
        }

        private static bool TryParsePassword(string[] lines, ref int currentLine, string[] setFields, List<string> fields, Func<string[], List<string>, bool> validatePassport)
        {
            while(currentLine < lines.Length)
            {
                if(string.IsNullOrWhiteSpace(lines[currentLine]))
                    break;
                var split = lines[currentLine].Split(' ');
                foreach(string s in split)
                {
                    var spl = s.Split(':');
                    var ind = fields.IndexOf(spl[0]);
                    if(ind >= 0)
                        setFields[ind] = spl[1];
                }
                currentLine++;
            }
            currentLine++;
            return validatePassport(setFields, fields);
        }

        static int Process(string[] lines, Func<string[], List<string>, bool> validatePassport)
        {
            int validPassports = 0;
            int currentLine = 0;
            List<string> fields = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid"};
            while(currentLine < lines.Length)
            {
                if(TryParsePassword(lines, ref currentLine, new string[8], fields, validatePassport))
                    validPassports++;
            }
            return validPassports;
        }

        static int Part1(string[] lines)
        {
            Func<string[], List<string>, bool> validatePassport = (sf, f) => 
            {
                var count = sf.Count(f => !string.IsNullOrEmpty(f));
                return count == 8 || (count == 7 && string.IsNullOrEmpty(sf[7]));
            };
            return Process(lines, validatePassport);
        }

        static int Part2(string[] lines)
        {
            //helpers
            Func<string, int, bool> nDigit = (s, n) => s.Count(char.IsDigit) == n;
            Func<string, int, int, bool> inRange = (s, min, max) => int.TryParse(s, out int v) && v >= min && v <= max;
            Func<string, int, int, int, bool> nDigitInRange = (s, d, min, max) => nDigit(s, d) && inRange(s,min,max);
            Func<string, string, int, int, bool> isUnitInRange = (s, u, min, max) => s.EndsWith(u) && inRange(s.Substring(0, s.Length - u.Length), min, max);
            Func<string, List<char>, bool> allIn = (s, lc) => s.All(c => lc.Contains(c));
            Func<string, bool> isColor = s => s.Length == 7 && s[0] == '#' && allIn(s.Substring(1), new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'});

            Func<string, bool>[] subValidate = new Func<string, bool>[]
            {
                s => nDigitInRange(s, 4, 1920, 2002),
                s => nDigitInRange(s, 4, 2010, 2020),
                s => nDigitInRange(s, 4, 2020, 2030),
                s => isUnitInRange(s, "cm", 150, 193) || isUnitInRange(s, "in", 59, 76),
                s => isColor(s),
                s => new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(s),
                s => nDigit(s, 9)
            }; 
            
            Func<string[], List<string>, bool> validatePassport1 = (sf, f) => 
            {
                var count = sf.Count(f => !string.IsNullOrEmpty(f));
                return count == 8 || (count == 7 && string.IsNullOrEmpty(sf[7]));
            };

            Func<string[], List<string>, bool> validatePassport = (sf, f) => 
            {
                bool r = validatePassport1(sf, f);
                if(!r) return false;
                for(int i = 0; i < f.Count - 1; i++)
                {
                    bool v = !string.IsNullOrEmpty(sf[i]) && subValidate[i](sf[i]);
                    r &= v;
                }
                return r;
            };
            return Process(lines, validatePassport);
        }
    }
}
