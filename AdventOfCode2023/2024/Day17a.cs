using AOC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{

    internal class Day17a
    {
        int ArgVal(int arg, (int a, int b, int c, int pc) state)
        {
            switch (arg)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return arg;
                case 4:
                    return state.a;
                case 5:
                    return state.b;
                case 6:
                    return state.c;
            }
            throw new ArgumentOutOfRangeException();
        }

        public void Run()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day17.txt");

            (int a, int b, int c, int pc) state = (
                int.Parse(lines[0].Split(":")[1]),
                int.Parse(lines[1].Split(":")[1]),
                int.Parse(lines[2].Split(":")[1]),
                0
                );

            

            List<int> program = lines[4].Split(":")[1].Split(",").Select(n => int.Parse(n)).ToList();

            var output = new List<int>();

            while (state.pc < program.Count)
            {
                var op1 = program[state.pc];
                var arg = program[state.pc+1];

                switch (op1)
                {
                    case 0: // adv
                        state.a = (int)(state.a / Math.Pow(2, ArgVal(arg, state)));
                        state.pc += 2;
                        break;
                    case 1: // bxl
                        state.b = state.b ^ arg;
                        state.pc += 2;
                        break;
                    case 2: //bst
                        state.b = ArgVal(arg, state) % 8;
                        state.pc += 2;
                        break;
                    case 3: // jnz
                        if ( state.a != 0) { state.pc = arg; } 
                        else
                        {
                            state.pc += 2;
                        }
                        break;
                    case 4: //bxc
                        state.b = state.b ^ state.c;
                        state.pc += 2;
                        break;
                    case 5: //out
                        output.Add(ArgVal(arg, state) % 8);
                        state.pc += 2;                        
                        break;
                    case 6: // bdv
                        state.b = (int)(state.a / Math.Pow(2, ArgVal(arg, state)));
                        state.pc += 2;
                        break;
                    case 7: // cdv
                        state.c = (int)(state.a / Math.Pow(2, ArgVal(arg, state)));
                        state.pc += 2;
                        break;
                }
            }

            Console.WriteLine(string.Join(",", output));
        }
    }
}


