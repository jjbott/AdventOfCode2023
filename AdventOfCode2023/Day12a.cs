using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day12a
    {
        public List<int> NextIteration(List<int> current, int spacesMax)
        {
            var next = new List<int>(current);
            var sum = current.Sum();
            bool carry = false;
            for (int i = next.Count() - 1; i >= 0; i--)
            {
                if (sum + 1 > spacesMax)
                {
                    next[i]++;
                    for (int j = i - 1; j >= 0 && next.Sum() > spacesMax; --j)
                    {
                        next[j + 1] = (j + 1) == next.Count - 1 ? 0 : 1;
                        next[j]++;
                    }
                    if (next.Sum() == spacesMax) return next;
                    if (next.Sum() > spacesMax) return null;
                    return NextIteration(next, spacesMax);
                }
                while (sum + 1 <= spacesMax)
                {
                    next[i] += 1;
                    sum += 1;
                }
                return next;

            }

            return null;
        }

        public bool Match(string map, List<int> spaces, List<int> values)
        {
            string compare = "";
            for (int i = 0; i < spaces.Count() - 1; i++)
            {
                compare += (spaces[i] > 0 ? new string('.', spaces[i]) : "") + new string('#', values[i]);
            }

            compare += (spaces.Last() > 0 ? new string('.', spaces.Last()) : "");

            if (map.Length != compare.Length) throw new Exception("Dang");

            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] != compare[i] && map[i] != '?')
                {
                    return false;
                }
            }

            return true;
        }

        public int Part1Count(string line, int expandCount = 0)
        {
            var map = line.Split(" ")[0];
            map = string.Join("?", Enumerable.Repeat(map, expandCount + 1));

            var values = line.Split(" ")[1].Split(",").Select(n => int.Parse(n)).ToList();
            values = Enumerable.Repeat(values, expandCount + 1).SelectMany(v => v).ToList();
            return Part1Count(map, values);
        }

        public int Part1Count(string map, List<int> values)
        {
            var spaces = Enumerable.Repeat(1, values.Count() + 1).ToList();
            spaces[0] = 0;
            spaces[spaces.Count() - 1] = -1;
            var spacesMax = map.Length - values.Sum();
            var current = new List<int>(spaces);

            return Count(spaces, values, map);
        }

        public void Part1()
        {
            // ????.??#.?????#? 1,1,1,1,6
            var lines = File.ReadAllLines("Inputs/Day12.txt");
            var count = 0;
            foreach (var l in lines)
            {
                var c = Part1Count(l);
                count += c;
            }

            Console.WriteLine(count);
        }

        public int Count(List<int> spaces, List<int> values, string map)
        {
            var count = 0;
            var current = new List<int>(spaces);
            while ((current = NextIteration(current, map.Length - values.Sum())) != null)
            {
                if (Match(map, current, values))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
