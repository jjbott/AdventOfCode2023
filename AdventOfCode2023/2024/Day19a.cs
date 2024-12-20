using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{
    internal class Day19a
    {


        public void Run()
        {

            var lines = File.ReadAllLines("./2024/Inputs/Day19.txt");

            var towels = lines[0].Split(',').Select(t => t.Trim()).ToHashSet();
            var min = towels.Min(x => x.Length);
            var max = towels.Max(x => x.Length);

            var designs = lines.Skip(2).ToList();

            var count = 0;

            foreach(var design in designs)
            {
                var queue = new Queue<string>();
                queue.Enqueue(design);
                var seen = new HashSet<String>();

                while (queue.TryDequeue(out var q))
                {
                    if ( q.Length == 0 )
                    {
                        count++;
                        break;
                    }

                    seen.Add(q);

                    for (int i = min; i <= Math.Min(max, q.Length); i++)
                    {
                        if ( !seen.Contains(q.Substring(i)) && towels.Contains(q.Substring(0, i)) )
                        {
                            queue.Enqueue(q.Substring(i));
                        }
                    }
                }
            }


            Console.WriteLine(count);




            return;
        }
    }
}
