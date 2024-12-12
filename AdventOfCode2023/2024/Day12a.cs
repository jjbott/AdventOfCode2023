using AOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{

    internal class Day12a
    {
        public int Perim(int x, int y, char c, char[][] map)
        {
            var perim = 0;
            if (x == 0 || map[y][x - 1] != c) perim++;
            if (x == (map[0].Length - 1) || map[y][x + 1] != c) perim++;

            if (y == 0 || map[y - 1][x] != c) perim++;
            if (y == (map.Length - 1) || map[y + 1][x] != c) perim++;

            return perim;
        }

        public int Calc(char[][] map, int x, int y)
        {
            var c = map[y][x];

            if ( c == '#' )
            {
                return 0;
            }

            var seen = new HashSet<(int x, int y)>();
            seen.Add((x, y));

            var perim = Perim(x, y, c, map);
            var area = 1;

            var queue = new List<(int x, int y)>();
            queue.Add((x - 1, y));
            queue.Add((x + 1, y));
            queue.Add((x, y - 1));
            queue.Add((x, y + 1));

            while (queue.Count > 0)
            {
                var pos = queue[0];
                queue.RemoveAt(0);

                if (pos.x < 0  || pos.y < 0 || pos.x >= map[0].Length || pos.y >= map.Length) continue;
                if ( seen.Contains((pos.x, pos.y))) continue;

                if (map[pos.y][pos.x] != c) continue;

                seen.Add((pos.x, pos.y));
                area++;
                perim += Perim(pos.x, pos.y, c, map);

                queue.Add((pos.x - 1, pos.y));
                queue.Add((pos.x + 1, pos.y));
                queue.Add((pos.x, pos.y - 1));
                queue.Add((pos.x, pos.y + 1));
            }

            foreach(var s in seen)
            {
                map[s.y][s.x] = '#';
            }

            return area * perim;

        }

        public void Run()
        {
            Dictionary<char, int> areas = new();
            Dictionary<char, int> perimiters = new();

            var map = File.ReadAllLines("./2024/Inputs/Day12.txt").Select(l=>l.ToCharArray()).ToArray();

            var sum = 0;

            for(int y = 0; y < map.Length; ++y)
            {
                for(int x = 0;  x < map[y].Length; ++x)
                {
                    sum += Calc(map, x, y);                
                }
            }

            Console.WriteLine(sum);
        }
    }
}

