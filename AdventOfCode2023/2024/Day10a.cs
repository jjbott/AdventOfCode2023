using AOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{

    internal class Day10a
    {
        Dictionary<(int x, int y), List<(int x, int y)>> _scores = new();

        IEnumerable<(int x, int y)> Next(int x, int y, int current, int[][] map)
        {
            var next = current + 1;

            if (x > 0 && map[y][x - 1] == next) yield return (x: x-1, y: y); 

            if (y > 0 && map[y-1][x] == next) yield return (x: x, y: y-1);

            if (x < (map[0].Length - 1) && map[y][x+1] == next) yield return (x: x+1, y: y);

            if (y < (map.Length - 1) && map[y+1][x] == next) yield return (x: x, y: y+1);
        }

        List<(int x, int y)> Calc(int x, int y, int[][] map)
        {
            if (!_scores.ContainsKey((x, y)))
            {
                int current = map[y][x];

                if (current == 9) 
                { 
                    return new List<(int x, int y)> { (x, y) };
                }

                var reachable9s = Next(x, y, current, map).SelectMany(n => Calc(n.x, n.y, map)).Distinct().ToList();

                if ( !reachable9s.Any())
                {
                    // clear spot on map so nothing else looks at it
                    map[y][x] = -1;
                    return reachable9s;
                }

                _scores[(x, y)] = reachable9s;
            }

            return _scores[(x, y)];
        }


        public void Run()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day10.txt");

            var map = lines.Select(l => l.ToCharArray().Select(c => (int)(c - '0')).ToArray()!).ToArray()!;

            var sum = 0;

            for(int y = 0; y < map.Length; y++)
            {
                for(int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == 0)
                    {
                        sum += Calc(x, y, map).Count();
                    }
                }
            }

            Console.WriteLine(sum);
        }
    }
}

