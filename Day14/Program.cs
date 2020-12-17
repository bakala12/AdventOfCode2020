using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine(Part1(lines));
            Console.WriteLine(Part2(lines));
        }

        static void SetMemory(Dictionary<long, long> mem, long address, long value, string mask)
        {
            long v = value;
            long m = 1;
            for(int i = 0; i < 36; i++, m <<= 1)
            {
                if(mask[35-i] == '1')
                    v |= m;
                else if(mask[35-i] == '0')
                    v &= ~m;
            }
            mem[address] = v;
        }

        static void SetOnAddresses(Dictionary<long, long> mem, long[] addressBits, int currentBit, long currentAddress, long value)
        {
            if(currentBit == 36)
            {
                mem[currentAddress] = value;
                return;
            }
            if(addressBits[currentBit] == -1)
            {
                SetOnAddresses(mem, addressBits, currentBit + 1, currentAddress, value);
                SetOnAddresses(mem, addressBits, currentBit + 1, currentAddress + (1L << currentBit), value);
                return;
            }
            SetOnAddresses(mem, addressBits, currentBit + 1, currentAddress + addressBits[currentBit] * (1L << currentBit), value);
        }

        static void SetMemory2(Dictionary<long, long> mem, long address, long value, string mask)
        {
            long[] addressBits = new long[36];
            long mm = 1;
            for(int i = 0; i < 36; i++, mm <<= 1)
            {
                if(mask[35-i] == '0')
                    addressBits[i] = (address & mm) >> i;
                else if(mask[35-i] == '1')
                    addressBits[i] = 1;
                else
                    addressBits[i] = -1;
            }
            SetOnAddresses(mem, addressBits, 0, 0, value);
        }

        static long Part1(string[] lines)
        {
            Dictionary<long, long> mem = new Dictionary<long, long>();
            string mask = new string('X', 36);
            foreach(var l in lines)
            {
                var split = l.Split('=');
                if(split[0].Trim() == "mask")
                    mask = split[1].Trim();
                else
                {
                    var s = split[0].Replace("mem[", "").Replace("]", "").Trim();
                    SetMemory(mem, long.Parse(s), long.Parse(split[1].Trim()), mask);
                }
            }
            return mem.Values.Aggregate(0L, (acc, l) => acc + l);
        }

        static long Part2(string[] lines)
        {
            Dictionary<long, long> mem = new Dictionary<long, long>();
            string mask = new string('X', 36);
            foreach(var l in lines)
            {
                var split = l.Split('=');
                if(split[0].Trim() == "mask")
                    mask = split[1].Trim();
                else
                {
                    var s = split[0].Replace("mem[", "").Replace("]", "").Trim();
                    SetMemory2(mem, long.Parse(s), long.Parse(split[1].Trim()), mask);
                }
            }
            return mem.Values.Aggregate(0L, (acc, l) => acc + l);
        }
    }
}
