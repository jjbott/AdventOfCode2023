using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day4
    {
        private Dictionary<int, int> recursiveCounts = new Dictionary<int, int>();

        private int RecursiveCounts(int index, List<int> counts)
        {
            if (recursiveCounts.ContainsKey(index))
            {
                return recursiveCounts[index];
            }

            var cardCount = 1;

            var winCount = counts[index];

            for (int i = 0; i < winCount; i++)
            {
                cardCount += RecursiveCounts(i + 1 + index, counts);
            }

            return cardCount;
        }

        public void Part1()
        {
            var lines = File.ReadAllLines("Inputs/Day4.txt");

            var splits = lines.Select(line => line.Split(": "));

            var counts = splits
                .Select(split =>
                {
                    var split2 = split[1].Split(" | ");

                    var a = split2[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    var b = split2[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    return b.Where(x => a.Contains(x)).Count();
                })
                .ToList();

            var scores = counts.Where(c => c > 0).Select(c => (int)Math.Pow(2, c - 1)).ToList();

            Console.WriteLine(scores.Sum());           
        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day4.txt");

            var splits = lines.Select(line => line.Split(": "));

            var counts = splits
                .Select(split =>
                {
                    var split2 = split[1].Split(" | ");

                    var a = split2[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    var b = split2[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    return b.Where(x => a.Contains(x)).Count();
                })
                .ToList();

            var scores = counts.Where(c => c > 0).Select(c => (int)Math.Pow(2, c - 1)).ToList();

            var recursiveCounts = new Dictionary<int, int>();

            Console.WriteLine(counts.Select((c, i) => RecursiveCounts(i)).Sum());

            int RecursiveCounts(int index)
            {
                if (recursiveCounts.ContainsKey(index))
                {
                    return recursiveCounts[index];
                }

                var cardCount = 1;

                var winCount = counts[index];

                for (int i = 0; i < winCount; i++)
                {
                    cardCount += RecursiveCounts(i + 1 + index);
                }

                return cardCount;
            }
        }
    }
}
