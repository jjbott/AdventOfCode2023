using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day9
    {
        public List<int> Diffs(List<int> sequence)
        {
            var diffs = new List<int>();
            for (int i = 0; i < sequence.Count - 1; i++)
            {
                diffs.Add(sequence[i + 1] - sequence[i]);
            }
            return diffs;
        }

        public void Part1()
        {
            var lines = File.ReadAllLines("Inputs/Day9.txt");
            var sequences = lines
                .Select(l => l.Split(" ").Select(n => int.Parse(n)).ToList())
                .ToList();

            var values = new List<int>();

            foreach (var sequence in sequences)
            {
                var diffList = new List<List<int>> { sequence };

                while (diffList.Last().Any(i => i != 0))
                {
                    diffList.Add(Diffs(diffList.Last()));
                }

                diffList.Reverse();
                values.Add(diffList.Aggregate(0, (a, b) => a += b.Last()));
            }

            Console.WriteLine(values.Sum());
        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day9.txt");
            var sequences = lines
                .Select(l => l.Split(" ").Select(n => int.Parse(n)).ToList())
                .ToList();

            var values = new List<int>();

            foreach (var sequence in sequences)
            {
                var diffList = new List<List<int>> { sequence };

                while (diffList.Last().Any(i => i != 0))
                {
                    diffList.Add(Diffs(diffList.Last()));
                }

                diffList.Reverse();
                values.Add(diffList.Aggregate(0, (a, b) => a = b.First() - a));
            }

            Console.WriteLine(values.Sum());
        }
    }
}
