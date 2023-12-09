using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace AdventOfCode2023
{
    internal class Day8
    {
        public void Part1()
        {
            var lines = File.ReadAllLines("Inputs/Day8.txt");
            var directions = lines[0];
            var map = lines
                .Skip(2)
                .ToDictionary(
                    l => l.Split(" = ")[0],
                    l =>
                    {
                        var split = l.Split(" = ")[1].Replace("(", "").Replace(")", "").Split(", ");
                        return new Dictionary<char, string>
                        {
                            { 'L', split[0] },
                            { 'R', split[1] }
                        };
                    }
                );
            var current = "AAA";
            long count = GetCount(directions, map, current);

            Console.WriteLine(count);
        }

        private static long GetCount(
            string directions,
            Dictionary<string, Dictionary<char, string>> map,
            string start
        )
        {
            string current = start;
            long count = 0;
            while (current != "ZZZ")
            {
                current = map[current][directions[(int)(count % directions.Length)]];
                count++;
            }
            Console.WriteLine("{start}, {count}");
            return count;
        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day8.txt");
            var directions = lines[0];
            var map = lines
                .Skip(2)
                .ToDictionary(
                    l => l.Split(" = ")[0],
                    l =>
                    {
                        var split = l.Split(" = ")[1].Replace("(", "").Replace(")", "").Split(", ");
                        return new Dictionary<char, string>
                        {
                            { 'L', split[0] },
                            { 'R', split[1] }
                        };
                    }
                );
            var currents = map.Keys.Where(k => k.EndsWith('A')).ToList();
            long count = 0;
            //var counts = current.Select(c => GetCount(directions, map, c)).ToList();
            //current = current.Take(1).ToList();
            //var start = current.First();

            var counts = currents.Select(c =>
            {
                var current = c;
                var c1 = 0;
                var c2 = 0;
                int count = 0;
                while (!current.EndsWith('Z') || c2 == 0) // (current.Any(c => !c.EndsWith('Z')))
                {
                    current = map[current][directions[(int)(count % directions.Length)]];
                    count++;

                    if (current.EndsWith('Z') && c1 != 0 && c2 == 0)
                    {
                        c2 = count;
                        if (c2 != c1 * 2)
                        {
                            int g = 56;
                        }
                    }

                    if (current.EndsWith('Z') && c1 == 0)
                    {
                        c1 = count;
                    }
                }

                return c1;
            }).ToList();

            var lcm = Utility.LCM(counts.Select(c => (long)c));

            Console.WriteLine(lcm);
        }
    }
}
