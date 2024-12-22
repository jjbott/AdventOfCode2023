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
    internal class Day21b
    {
        (string result, long length) NumericToDirectional(string code)
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

            long length = 0;
            var result = "";
            var pos = (x: 2, y: 3, c: 'A');
            var p = (2, 0);
            foreach (var c in code)
            {
                var step = new List<string>();

                var target = map.Single(m => m.c == c);

                // Do up and right first to avoid the empty space.
                // Although for the final calc, I dont think it matters...

                if (pos.x < target.x)
                {
                    step.Add(new string('>', target.x - pos.x));
                }

                if (pos.y > target.y)
                {
                    step.Add(new string('^', pos.y - target.y));
                }

                if (pos.x > target.x)
                {
                    step.Add(new string('<', pos.x - target.x));
                }

                if (pos.y < target.y)
                {
                    step.Add(new string('v', target.y - pos.y));
                }

                step.Add("A");

                // Check if swapping the order of directions is possible, and if so, would produce a shorter sequence
                // Dont allow passing through the space with no buttons
                if (step.Count <= 2 || ((pos.x == 0 || target.x == 0) && (pos.y == 3 || target.y == 3)))
                {
                    result = result + string.Join("", step);

                    //p = (2, 1);
                    foreach (var c2 in string.Join("", step))
                    {
                        var r = DirectionalToDirectional(p, c2, 0);
                        p = r.pos;
                        length += r.length;
                    }
                }
                else
                {
                    var seq1 = string.Join("", step);
                    var seq2 = step[1] + step[0] + step[2];

                    long length1 = 0;
                    long length2 = 0;
                    var p1 = p;
                    foreach (var c2 in seq1)
                    {
                        var r = DirectionalToDirectional(p1, c2, 0);
                        p1 = r.pos;
                        length1 += r.length;
                    }
                    var p2 = p;
                    foreach (var c2 in seq2)
                    {
                        var r = DirectionalToDirectional(p2, c2, 0);
                        p2 = r.pos;
                        length2 += r.length;
                    }

                    if (length1 < length2)
                    {
                        result += seq1;
                        length += length1;
                        p = p1;
                    }
                    else
                    {
                        result += seq2;
                        length += length2;
                        p = p2;
                    }
                }
                pos = target;
            }

            return (result, length);
        }

        private List<(int x, int y, char c)> _directionalMap = new List<(int x, int y, char c)>{
                            (1, 0, '^'),(2, 0, 'A'),
                (0, 1, '<'),(1, 1, 'v'),(2, 1, '>')
            };

        private Dictionary<((int x, int y) pos, char target, int depth), ((int x, int y) pos, long length)> cache = new();

        ((int x, int y) pos, long length) DirectionalToDirectional((int x, int y) pos, char target, int depth)
        {
            if (pos == (2, 0) && target == 'v' && depth == 7)
            {
                int g = 56;
            }
            
            if (!cache.ContainsKey((pos, target, depth)))
            {

                var path = DirectionalToDirectional(pos, target);

                if (depth == 24)
                {
                    return (path.targetPos, path.result.Sum(s => s.Length));
                }

                var p = (2, 0);
                long length = 0;

                var path1 = string.Join("", path.result);

                foreach (var c in path1)
                {
                    var r = DirectionalToDirectional(p, c, depth + 1);
                    p = r.pos;
                    length += r.length;
                }

                if ( p != (2,0) )
                {
                    throw new Exception();
                }

                if (path.result.Count > 2 && !((pos.x == 0 || path.targetPos.x == 0) && (pos.y == 0 || path.targetPos.y == 0)) )
                {
                    // check if switching the first 2 steps will result in a shorter path
                    p = (2, 0);
                    long length2 = 0;
                    var path2 = path.result[1] + path.result[0] + path.result[2];

                    foreach (var c in path2)
                    {
                        var r = DirectionalToDirectional(p, c, depth + 1);
                        p = r.pos;
                        length2 += r.length;
                    }

                    if ( length2 < length)
                    {
                        length = length2;
                    }
                }

                cache[(pos, target, depth)] = (path.targetPos, length);
            }


            return cache[(pos, target, depth)];
            
        }

        private Dictionary<((int x, int y) pos, char target), (List<string> result, (int x, int y) targetPos)> charCache = new();

        (List<string> result, (int x, int y) targetPos) DirectionalToDirectional((int x, int y) startPos, char target)
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

            if (!charCache.ContainsKey((startPos, target)))
            {
                List<string> result = new();
                var pos = startPos;

                var targetPos = _directionalMap.Single(m => m.c == target);

                // Do down and right first to avoid the empty space.
                // Although for the final calc, I dont think it matters...

                if (pos.x < targetPos.x)
                {
                    result.Add(new string('>', targetPos.x - pos.x));
                }

                if (pos.y < targetPos.y)
                {
                    result.Add(new string('v', targetPos.y - pos.y));
                }

                if (pos.y > targetPos.y)
                {
                    result.Add(new string('^', pos.y - targetPos.y));
                }

                if (pos.x > targetPos.x)
                {
                    result.Add(new string('<', pos.x - targetPos.x));
                }

                result.Add("A");

                charCache[(startPos, target)] = (result, (targetPos.x, targetPos.y));
            }

            return charCache[(startPos, target)];
        }

        public void Run()
        {
            var codes = File.ReadAllLines("./2024/Inputs/Day21.txt");

            long sum = 0;

            foreach (var code in codes)
            {
                var step1 = NumericToDirectional(code);
                //var step2 = DirectionalToDirectional(step1);
               // var step3 = DirectionalToDirectional(step2);
                Console.WriteLine(code);
                Console.WriteLine("    " + step1);
                //Console.WriteLine("    " + step2);
                //Console.WriteLine("    " + step3);

                var i = int.Parse(code.Substring(0, 3));
                long score = step1.length * i;
                Console.WriteLine($"    {step1.length} * {i} = {score}");
                sum += score;
            }

            // 172774


            // guessed 442968401642078. Too high. booo.
            //         128443926293806. Too low.  booooooo
            //         260339962233536. boooooooooooooo
            //         210686850124870. oh thank goodness
            Console.WriteLine(sum);
        }
    }
}
