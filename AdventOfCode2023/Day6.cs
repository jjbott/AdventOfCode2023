using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day6
    {
        public void Part1()
        {
            var lines = File.ReadAllLines("Inputs/Day6.txt");
            var times = lines[0]
                .Split(": ")[1]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Int32.Parse(x))
                .ToArray();
            var distances = lines[1]
                .Split(": ")[1]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Int32.Parse(x))
                .ToArray();

            var results = new List<int>();

            for (int i = 0; i < times.Length; i++)
            {
                var a = .5 * (times[i] - Math.Sqrt(times[i] * times[i] - 4 * distances[i]));
                var b = .5 * (times[i] + Math.Sqrt(times[i] * times[i] - 4 * distances[i]));

                results.Add((int)Math.Floor(b) - (int)Math.Ceiling(a) + 1);
            }

            Console.WriteLine(results.Aggregate((a, b) => a * b));
        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day6.txt");
            var time = long.Parse(lines[0].Split(": ")[1].Replace(" ", ""));
            var distance = long.Parse(lines[1].Split(": ")[1].Replace(" ", ""));

            var a = .5 * (time - Math.Sqrt(time * time - 4 * distance));
            var b = .5 * (time + Math.Sqrt(time * time - 4 * distance));

            Console.WriteLine(Math.Floor(b) - Math.Ceiling(a) + 1);
        }
    }
}
