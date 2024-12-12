using AOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{

    internal class Day12b
    {
        public int CalcPerim(HashSet<(int x, int y)> seen)
        {
            var corners = 0;

            foreach(var pos in seen)
            {
                // UL
                if ( !seen.Contains((pos.x-1, pos.y)) && !seen.Contains((pos.x, pos.y - 1))) corners++;

                // UR
                if (!seen.Contains((pos.x + 1, pos.y)) && !seen.Contains((pos.x, pos.y - 1))) corners++;

                // DL
                if (!seen.Contains((pos.x - 1, pos.y)) && !seen.Contains((pos.x, pos.y + 1))) corners++;

                // DR
                if (!seen.Contains((pos.x + 1, pos.y)) && !seen.Contains((pos.x, pos.y + 1))) corners++;

                // inner UL
                if (seen.Contains((pos.x - 1, pos.y)) && seen.Contains((pos.x, pos.y - 1)) && !seen.Contains((pos.x - 1, pos.y - 1))) corners++;

                // inner UR
                if (seen.Contains((pos.x + 1, pos.y)) && seen.Contains((pos.x, pos.y - 1)) && !seen.Contains((pos.x + 1, pos.y - 1))) corners++;

                // inner DL
                if (seen.Contains((pos.x - 1, pos.y)) && seen.Contains((pos.x, pos.y + 1)) && !seen.Contains((pos.x - 1, pos.y + 1))) corners++;


                // inner DR
                if (seen.Contains((pos.x + 1, pos.y)) && seen.Contains((pos.x, pos.y + 1)) && !seen.Contains((pos.x + 1, pos.y + 1))) corners++;
            }

            return corners;
        }

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

            var perim2 = CalcPerim(seen);
            //PrintPiece(seen);
            return area * perim2;

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

        void PrintPiece(HashSet<(int x, int y)> seen)
        {
            for(int y = seen.Min(p => p.y); y <= seen.Max(p => p.y); ++y)
            {
                for(int x = seen.Min(p => p.x); x <=  seen.Max(p => p.x); ++x)
                {
                    if (seen.Contains((x, y)))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}

