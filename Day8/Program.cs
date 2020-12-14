using System;
using System.Collections.Generic;
using System.IO;

namespace Day8
{
    class Instruction
    {
        public int Id;
        public string Instr;

        public void Execute(ProgramState state)
        {
            var instr = Instr.Split(' ');
            var param = int.Parse(instr[1]);
            switch(instr[0])
            {
                case "nop":
                    state.Address++;
                    break;
                case "acc":
                    state.Accumulator += param;
                    state.Address++;
                    break;
                case "jmp":
                    state.Address += param;
                    break;
            }
        }

        public void Change()
        {
            var s = Instr.Split(' ')[0];
            if(s == "jmp") s = "nop";
            else s = "jmp";
            Instr = $"{s} {Instr.Split(' ')[1]}";
        }
    }

    class ProgramState
    {
        public int Accumulator;
        public int Address;

        public ProgramState(int accumulator, int address)
        {
            Accumulator = accumulator;
            Address = address;
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

        static List<Instruction> PrepareInstructions(string[] lines)
        {
            List<Instruction> list = new List<Instruction>();
            for(int i = 1; i <= lines.Length; i++)
                list.Add(new Instruction() { Id = i, Instr = lines[i-1]});
            return list;
        }

        static int Part1(string[] lines)
        {
            var instructions = PrepareInstructions(lines);
            var state = new ProgramState(0,0);
            List<int> addresses = new List<int>();
            while(!addresses.Contains(state.Address))
            {
                addresses.Add(state.Address);
                instructions[state.Address].Execute(state);
            }
            return state.Accumulator;
        }

        static ProgramState ExecuteProgram(List<Instruction> instructions)
        {
            ProgramState state = new ProgramState(0,0);
            List<int> addresses = new List<int>();
            while(!addresses.Contains(state.Address) && state.Address < instructions.Count)
            {
                addresses.Add(state.Address);
                instructions[state.Address].Execute(state);
            }
            return state;
        }

        static int Part2(string[] lines)
        {
            var instructions = PrepareInstructions(lines);
            for(int i = 0; i < instructions.Count; i++)
            {
                if(instructions[i].Instr.StartsWith("acc"))
                    continue;
                instructions[i].Change();
                var state = ExecuteProgram(instructions);
                if(state.Address >= instructions.Count)
                    return state.Accumulator;
                instructions[i].Change();
            }
            return -1;
        }
    }
}
