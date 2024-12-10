using AOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{

    internal class Day8b
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
                antinodes.AddRange(g);

                var combinations = g.DifferentCombinations(2).ToList();
                foreach(var c in combinations)
                {
                    var rise = c[0].y - c[1].y;
                    var run = c[0].x - c[1].x;


                    var anx = c[0].x + run;
                    var any = c[0].y + rise;
                    while(anx >= 0 && anx < lines[0].Length && any >= 0 && any < lines.Length)
                    {
                        antinodes.Add(new Node { x = anx, y = any, t = c[0].t });
                        anx = anx + run;
                        any = any + rise;
                    }

                    anx = c[1].x - run;
                    any = c[1].y - rise;
                    while (anx >= 0 && anx < lines[0].Length && any >= 0 && any < lines.Length)
                    {
                        antinodes.Add(new Node { x = anx, y = any, t = c[0].t });
                        anx = anx - run;
                        any = any - rise;
                    }
                }
            }

            var countPer = antinodes.GroupBy(an => (an.x, an.y)).ToList();

            var count = countPer.Where(g => g.Select(an => an.t).Distinct().Count() == 1).Count();

            var distinct = antinodes.Select(an => (an.x, an.y)).ToHashSet();

            Console.WriteLine(distinct.Count(an => an.x >= 0 && an.x < lines[0].Length && an.y >= 0 && an.y < lines.Length));
        }
    }
}

