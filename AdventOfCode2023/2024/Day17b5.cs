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
    internal class Day17b5
    {
        long ArgVal(int arg, (long a, long b, long c, int pc) state)
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

        List<int> RunProgram(long a, List<int> program)
        {
            (long a, long b, long c, int pc) state = (a, 0, 0, 0);

            var output = new List<int>();

            while (state.pc < program.Count)
            {
                var op1 = program[state.pc];
                var arg = program[state.pc + 1];

                switch (op1)
                {
                    case 0: // adv
                        //Console.WriteLine($"a = {state.a} / 2^^{ArgVal(arg, state)})");
                        state.a = (long)(state.a / Math.Pow(2, ArgVal(arg, state)));
                        state.pc += 2;
                        break;
                    case 1: // bxl
                        //Console.WriteLine($"b = {state.b} ^ {arg}");
                        state.b = state.b ^ arg;
                        state.pc += 2;
                        break;
                    case 2: //bst
                        //Console.WriteLine($"b = {ArgVal(arg, state)} % 8");
                        state.b = ArgVal(arg, state) % 8;
                        state.pc += 2;
                        break;
                    case 3: // jnz
                        if (state.a != 0)
                        {
                            //Console.WriteLine($"jump to {arg}: {state.a}");
                            state.pc = arg;
                        }
                        else
                        {
                            //Console.WriteLine($"noop (jump to {arg}): {state.a}");
                            state.pc += 2;
                        }
                        break;
                    case 4: //bxc
                        //Console.WriteLine($"b = {state.b} ^ {state.c}");
                        state.b = state.b ^ state.c;
                        state.pc += 2;
                        break;
                    case 5: //out
                        //Console.WriteLine($"out << {ArgVal(arg, state) % 8}");
                        output.Add((int)(ArgVal(arg, state) % 8));
                        state.pc += 2;
                        break;
                    case 6: // bdv
                        //Console.WriteLine($"b = {state.a} / 2^^{ArgVal(arg, state)})");
                        state.b = (long)(state.a / Math.Pow(2, ArgVal(arg, state)));
                        state.pc += 2;
                        break;
                    case 7: // cdv
                        //Console.WriteLine($"c = {state.a} / 2^^{ArgVal(arg, state)})");
                        state.c = (long)(state.a / Math.Pow(2, ArgVal(arg, state)));
                        state.pc += 2;
                        break;
                }
            }

            return output;
        }

        List<int> NextBits(List<int> known, List<int> program)
        {
            var next = new List<int>();

            for (int i = 0; i <= 0b111; ++i)
            {
                long a = 0;
                foreach (var k in known)
                {
                    a += k;
                    a = a << 3;
                }
                a += i;

                var output = RunProgram(a, program);

                if (
                    output.Count == known.Count + 1
                    && output.Count <= program.Count
                    && output.SequenceEqual(program.TakeLast(known.Count + 1))
                )
                {
                    next.Add(i);
                }
            }
            return next;
        }

        public void Run()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day17.txt");

            List<int> program = lines[4]
                .Split(":")[1]
                .Split(",")
                .Select(n => int.Parse(n))
                .ToList();

            List<int> val = new();

            List<int> expected = new List<int>(program);

            var solutions = new List<List<int>>();

            var queue = new Queue<List<int>>();
            queue.Enqueue(new List<int>());

            while (queue.TryDequeue(out var q))
            {
                if (q.Count == program.Count)
                {
                    solutions.Add(q);
                }
                else
                {
                    var nb = NextBits(q, program);

                    foreach (var n in nb)
                    {
                        var nextVals = new List<int>(q);
                        nextVals.Add(n);
                        queue.Enqueue(nextVals);
                    }
                }
            }

            foreach (var s in solutions)
            {
                long a = 0;
                foreach (var k in s)
                {
                    a = a << 3;
                    a += k;
                }
                Console.WriteLine(a);
            }

            return;
        }
    }
}
