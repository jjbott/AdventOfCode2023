using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Day2
    {
        public void RunPart1()
        {
            var lines = File.ReadAllLines(@".\Inputs\Day2.txt");
            var maxRed = 12;
            var maxGreen = 13;
            var maxBlue = 14;

            var parsed = lines.Select(l =>
            {
                var idSplit = l.Split(": ");
                var id = int.Parse(idSplit[0].Split(" ")[1]);
                var rounds = idSplit[1]
                    .Split("; ")
                    .Select(round =>
                    {
                        var selections = round.Split(", ");
                        return selections
                            .Select(s => s.Split(" "))
                            .Select(s => (count: int.Parse(s[0]), color: s[1]));
                    });
                return (id: id, rounds: rounds);
            });

            var filtered = parsed.Where(p =>
            {
                return p.rounds.SelectMany(r => r.Where(c => c.color == "blue")).Max(r => r.count)
                        <= maxBlue
                    && p.rounds.SelectMany(r => r.Where(c => c.color == "red")).Max(r => r.count)
                        <= maxRed
                    && p.rounds.SelectMany(r => r.Where(c => c.color == "green")).Max(r => r.count)
                        <= maxGreen;
            });

            Console.WriteLine(filtered.Sum(p => p.id));
        }

        public void RunPart2()
        {
            var lines = File.ReadAllLines(@".\Inputs\Day2.txt");

            var parsed = lines.Select(l =>
            {
                var idSplit = l.Split(": ");
                var id = int.Parse(idSplit[0].Split(" ")[1]);
                var rounds = idSplit[1]
                    .Split("; ")
                    .Select(round =>
                    {
                        var selections = round.Split(", ");
                        return selections
                            .Select(s => s.Split(" "))
                            .Select(s => (count: int.Parse(s[0]), color: s[1]));
                    });
                return (id: id, rounds: rounds);
            });

            var maxes = parsed.Select(p =>
            {
                return (
                    maxBlue: p.rounds
                        .SelectMany(r => r.Where(c => c.color == "blue"))
                        .Max(r => r.count),
                    maxRed: p.rounds
                        .SelectMany(r => r.Where(c => c.color == "red"))
                        .Max(r => r.count),
                    maxGreen: p.rounds
                        .SelectMany(r => r.Where(c => c.color == "green"))
                        .Max(r => r.count)
                );
            });

            var powers = maxes.Select(m => m.maxGreen * m.maxBlue * m.maxRed);

            Console.WriteLine(powers.Sum());
        }
    }
}
