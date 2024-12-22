using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{
    internal class Day21a
    {
        string NumericToDirectional(string code)
        {
            /*
            +---+---+---+         +---+---+  
            | 7 | 8 | 9 |         | ^ | A |  
            +---+---+---+     +---+---+---+  
            | 4 | 5 | 6 |     | < | v | > |  
            +---+---+---+     +---+---+---+  
            | 1 | 2 | 3 |       
            +---+---+---+       
                | 0 | A |       
                +---+---+
            */

            var map = new List<(int x, int y, char c)>{
                (0, 0, '7'),(1, 0, '8'),(2, 0, '9'),
                (0, 1, '4'),(1, 1, '5'),(2, 1, '6'),
                (0, 2, '1'),(1, 2, '2'),(2, 2, '3'),
                            (1, 3, '0'),(2, 3, 'A')
            };

            var result = "";
            var pos = (x:2, y:3, c:'A');
            foreach(var c in code)
            {
                var step = new List<string>();

                var target = map.Single(m => m.c == c);

                // Do up and right first to avoid the empty space.
                // Although for the final calc, I dont think it matters...

                if ( pos.x < target.x)
                {
                    step.Add(new string('>', target.x - pos.x));
                }

                if (pos.y > target.y)
                {
                    step.Add(new string('^', pos.y - target.y));
                }

                if (pos.x > target.x)
                {
                    step.Add(new string('<', pos.x - target.x ));
                }

                if (pos.y < target.y)
                {
                    step.Add(new string('v', target.y - pos.y));
                }

                step.Add("A");

                // Check if swapping the order of directions is possible, and if so, would produce a shorter sequence
                if ( step.Count <= 2 )
                {
                    result = result + string.Join("", step);
                }
                // Dont allow passing through the space with no buttons
                else if ((pos.x == 0 || target.x == 0) && (pos.y == 3 || target.y == 3))
                {
                    result = result + string.Join("", step);
                }
                else
                {
                    var seq1 = string.Join("", step);
                    var seq2 = step[1] + step[0] + step[2];

                    if (DirectionalToDirectional(DirectionalToDirectional(seq1)).Length < DirectionalToDirectional(DirectionalToDirectional(seq2)).Length)
                    {
                        result += seq1;
                    }
                    else
                    {
                        result += seq2;
                    }
                }
                pos = target;
            }

            return result;
        }

        string DirectionalToDirectional(string code, bool optimize = false)
        {
            /*
            +---+---+---+         +---+---+  
            | 7 | 8 | 9 |         | ^ | A |  
            +---+---+---+     +---+---+---+  
            | 4 | 5 | 6 |     | < | v | > |  
            +---+---+---+     +---+---+---+  
            | 1 | 2 | 3 |       
            +---+---+---+       
                | 0 | A |       
                +---+---+
            */

            var map = new List<(int x, int y, char c)>{
                            (1, 0, '^'),(2, 0, 'A'),
                (0, 1, '<'),(1, 1, 'v'),(2, 1, '>')
            };

            var result = "";
            var pos = (x:2, y:0, c:'A');
            foreach (var c in code)
            {
                var step = new List<string>();

                var target = map.Single(m => m.c == c);

                // Do down and right first to avoid the empty space.
                // Although for the final calc, I dont think it matters...

                if (pos.x < target.x)
                {
                    step.Add(new string('>', target.x - pos.x));
                }

                if (pos.y < target.y)
                {
                    step.Add(new string('v', target.y - pos.y));
                }

                if (pos.y > target.y)
                {
                    step.Add(new string('^', pos.y - target.y));
                }

                if (pos.x > target.x)
                {
                    step.Add(new string('<', pos.x - target.x));
                }

                step.Add("A");

                // Check if swapping the order of directions is possible, and if so, would produce a shorter sequence
                if ( !optimize )
                {
                    // Dont stack overflow
                    result = result + string.Join("", step);
                }
                else if (step.Count <= 2)
                {
                    result = result + string.Join("", step);
                }
                // Dont allow passing through the space with no buttons
                else if ((pos.x == 0 || target.x == 0) && (pos.y == 0 || target.y == 0))
                {
                    result = result + string.Join("", step);
                }
                else
                {
                    var seq1 = string.Join("", step);
                    var seq2 = step[1] + step[0] + step[2];

                    if (DirectionalToDirectional(seq1, false).Length < DirectionalToDirectional(seq2, false).Length)
                    {
                        result += seq1;
                    }
                    else
                    {
                        result += seq2;
                    }
                }

                pos = target;
            }

            return result;
        }

        public void Run()
        {
            var codes = File.ReadAllLines("./2024/Inputs/Day21.txt");

            var sum = 0;

            foreach (var code in codes)
            {
                var step1 = NumericToDirectional(code);
                var step2 = DirectionalToDirectional(step1);
                var step3 = DirectionalToDirectional(step2);
                Console.WriteLine(code);
                Console.WriteLine("    " + step1);
                Console.WriteLine("    " + step2);
                Console.WriteLine("    " + step3);

                var i = int.Parse(code.Substring(0, 3));
                var score = step3.Length * i;
                Console.WriteLine($"    {step3.Length} * {i} = {score}");
                sum += score;
            }

            // 172774

            Console.WriteLine(sum);
        }
    }
}
