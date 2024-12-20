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
    internal class Day19b
    {
        public long Count(string design, HashSet<string> towels, Dictionary<string, long> counts)
        {
            if (design.Length == 0)
            {
                return 1;
            }
            else if ( counts.ContainsKey(design))
            {
                return counts[design];
            }
            else
            {
                long count = 0;

                for (int i = 1; i <= Math.Min(8, design.Length); i++)
                {
                    if (towels.Contains(design.Substring(0, i)))
                    {
                        if (design.Substring(i).Length == 0)
                        {
                            count++;
                            //matches.Add(new List<string>() { design.Substring(0, i) });
                        }
                        else
                        {
                            count += Count(design.Substring(i), towels, counts);
                            /*
                            var newCount = Count(design.Substring(i), towels, counts);
                            
                            foreach (var subMatch in newCount.towels)
                            {
                                var newMatches = new List<string> { design.Substring(0, i) };
                                newMatches.AddRange(subMatch);
                                matches.Add(newMatches);
                            }*/
                        }
                    }
                }

                counts[design] = count;
                return count;

            }
        }

        public void Run()
        {

            var lines = File.ReadAllLines("./2024/Inputs/Day19.txt");

            var towels = lines[0].Split(',').Select(t => t.Trim()).ToHashSet();
            var min = towels.Min(x => x.Length);
            var max = towels.Max(x => x.Length);

            var designs = lines.Skip(2).ToList();

            long count = 0;
            Dictionary<string, long> counts = new();

            foreach (var design in designs)
            {
                count += Count(design, towels, counts);
            }


            Console.WriteLine(count);




            return;
        }
    }
}
