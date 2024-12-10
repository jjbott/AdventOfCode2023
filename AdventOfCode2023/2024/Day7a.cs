using AOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{
    internal class Day7a
    {
        public List<long> Calc(List<long> values, long max)
        {
            if (values.Count == 0)
            {
                return new List<long>();
            }

            if (values.Count == 1)
            {
                return new List<long>() { values[0] };
            }

            if (values.Count == 2)
            {
                return new List<long>() { values[0] + values[1], values[0] * values[1] };
            }

            var newValues1 = new List<long>() { values[0] + values[1] };
            newValues1.AddRange(values.Skip(2));
            var newValues2 = new List<long>() { values[0] * values[1] };
            newValues2.AddRange(values.Skip(2));

            var answers = new List<long>();
            answers.AddRange(Calc(newValues1, max));
            answers.AddRange(Calc(newValues2, max));
            answers = answers.Where(a => a <= max).ToList();

            return answers;
        }

        public void Run()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day7.txt");

            long sum = 0;

            foreach (var l in lines)
            {
                var answer = long.Parse(l.Split(':')[0]);
                var values = l.Split(": ")[1].Split(' ').Select(s => long.Parse(s)).ToList();
                var valid = Calc(values, answer).Any(a => a == answer);
                if (valid)
                {
                    sum += answer;
                }
                int g = 56;

            }

            Console.WriteLine(sum);
        }
    }
}

