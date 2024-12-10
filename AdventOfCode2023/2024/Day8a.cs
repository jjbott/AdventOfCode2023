using AOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{
    public static class Ex
    {
        public static IEnumerable<List<T>> DifferentCombinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new List<T>() } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => (new[] { e }).Concat(c).ToList()));
        }
    }

    internal class Day8a
    {

        class Node
        {
            public int x;
            public int y;
            public char t;
        }

        public void Run()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day8.txt");

            List<Node> nodes = new List<Node>();

            for(int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] != '.')
                    {
                        nodes.Add(new Node { x = x, y = y, t = lines[y][x] });
                    }
                }
            }

            var antinodes = new List<Node>();

            foreach(var g in nodes.GroupBy(x => x.t).Where(g => g.Count() >= 2))
            {
                var combinations = g.DifferentCombinations(2).ToList();
                foreach(var c in combinations)
                {
                    var rise = c[0].y - c[1].y;
                    var run = c[0].x - c[1].x;

                    var an1 = new Node { x = c[0].x + run, y = c[0].y + rise, t = c[0].t };
                    var an2 = new Node { x = c[1].x - run, y = c[1].y - rise, t = c[0].t };

                    antinodes.Add(an1);
                    antinodes.Add(an2);
                }
            }

            var distinct = antinodes.Select(an => (an.x, an.y)).ToHashSet();

            Console.WriteLine(distinct.Count(an => an.x >= 0 && an.x < lines[0].Length && an.y >= 0 && an.y < lines.Length));
        }
    }
}

