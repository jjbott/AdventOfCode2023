using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{
    internal class Day20b
    {
        public void Run()
        {
            var map = File.ReadAllLines("./2024/Inputs/Day20.txt")
                .Select(
                    l =>
                    {
                        var a = l.ToCharArray().Select(c =>
                            {
                                switch (c)
                                {
                                    case '#':
                                        return -1;
                                    case 'S':
                                        return -2;
                                    //case 'E':
                                    //    return -3;
                                    default:
                                        return 0;
                                }
                            }).ToArray();
                        return a;
                    }
                ).ToArray();

            (int x, int y) start = (0, 0);

            for (int y = 0; y < map.Length; ++y)
            {
                for (int x = 0; x < map[0].Length; ++x)
                {
                    if (map[y][x] == -2)
                    {
                        start = (x, y);
                    }
                }
            }

            var len = 0;
            (int x, int y) pos = start;
            while (true)
            {
                if (map[pos.y][pos.x - 1] == 0)
                {
                    ++len;
                    pos = (pos.x - 1, pos.y);
                    map[pos.y][pos.x] = len;
                    continue;
                }
                else if (map[pos.y][pos.x + 1] == 0)
                {
                    ++len;
                    pos = (pos.x + 1, pos.y);
                    map[pos.y][pos.x] = len;
                    continue;
                }
                else if (map[pos.y - 1][pos.x] == 0)
                {
                    ++len;
                    pos = (pos.x, pos.y - 1);
                    map[pos.y][pos.x] = len;
                    continue;
                }
                else if (map[pos.y + 1][pos.x] == 0)
                {
                    ++len;
                    pos = (pos.x, pos.y + 1);
                    map[pos.y][pos.x] = len;
                    continue;
                }

                break;
            }

            // wait to set start to 0 so pathing doesnt accidentally backtrack
            map[start.y][start.x] = 0;

            var InMap = (int x, int y) =>
            {
                return x >= 0 && x < map[0].Length && y >= 0 && y < map.Length;
            };

            Dictionary<int, int> counts = new();

            var cheatTime = 20;

            for (int y = 0; y < map.Length; ++y)
            {
                for (int x = 0; x < map[0].Length; ++x)
                {
                    if (map[y][x] < 0) continue;

                    for (int y2 = Math.Max(0, y - cheatTime); y2 < Math.Min(map.Length, y + cheatTime + 1); ++y2)
                    {
                        for (int x2 = Math.Max(0, x - cheatTime); x2 < Math.Min(map[0].Length, x + cheatTime + 1); ++x2)
                        {
                            if (map[y2][x2] > 0 && map[y2][x2] > map[y][x])
                            {
                                var distance = Math.Abs(y2 - y) + Math.Abs(x2 - x);

                                if (distance <= cheatTime)
                                {
                                    var savings = map[y2][x2] - (map[y][x] + distance);

                                    if (savings > 0)
                                    {

                                        if (!counts.ContainsKey(savings))
                                        {
                                            counts[savings] = 1;
                                        }
                                        else
                                        {
                                            counts[savings]++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // Exmaple: 285 >= 50
            //Print(map);
            Console.WriteLine(counts.Where(kv => kv.Key >= 100).Sum(kv => kv.Value));

            return;
        }

        void Print(int[][] map)
        {
            foreach(var x in map)
            {
                Console.WriteLine(
                    string.Join(' ',
                x.Select(i =>
                {
                    var s = i.ToString();
                    return s.PadLeft(4, ' ');
                })
                ));
            }
        }
    }
}
