using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Day1
    {
        public void RunPart1()
        {
            var lines = File.ReadAllLines(@".\Inputs\Day1.txt");
            var digitsOnly = lines.Select(l => l.Where(c => char.IsDigit(c)).Select(d => d - '0'));
            var numbers = digitsOnly.Select(d => (d.First() * 10) + d.Last());
            Console.WriteLine(numbers.Sum());
        }

        public void RunPart2()
        {
            var lines = File.ReadAllLines(@".\Inputs\Day1.txt");
            var converted = lines
                .Select(
                    l =>
                        l.Replace("one", "o1e")
                            .Replace("two", "t2o")
                            .Replace("three", "t3e")
                            .Replace("four", "f4r")
                            .Replace("five", "f5e")
                            .Replace("six", "s6x")
                            .Replace("seven", "s7n")
                            .Replace("eight", "e8t")
                            .Replace("nine", "n9e")
                )
                .ToList();
            var digitsOnly = converted
                .Select(l => l.Where(c => char.IsDigit(c)).Select(d => d - '0'))
                .ToList();
            var numbers = digitsOnly.Select(d => (d.First() * 10) + d.Last()).ToList();

            var test = lines
                .Select((l, i) => (l, converted[i], string.Join("", digitsOnly[i]), numbers[i]))
                .ToList();

            Console.WriteLine(numbers.Sum());
        }
    }
}
