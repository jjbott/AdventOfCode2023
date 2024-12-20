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
    internal class Day20a
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

            for (int y = 0; y < map.Length; ++y)
            {
                for (int x = 0; x < map[0].Length; ++x)
                {
                    if (map[y][x] >= 0)
                    {
                        var cheats = new List<((int x, int y) n1, (int x, int y) n2)>()
                        {
                            ((x-1, y), (x-2,y)),
                            ((x+1, y), (x+2,y)),
                            ((x, y-1), (x,y-2)),
                            ((x, y+1), (x,y+2)),
                        };

                        foreach (var c in cheats)
                        {
                            if (InMap(c.n2.x, c.n2.y) && map[c.n1.y][c.n1.x] == -1 && map[c.n2.y][c.n2.x] > map[y][x])
                            {
                                var savings = map[c.n2.y][c.n2.x] - (map[y][x] + 2);

                                if (savings == 66)
                                {
                                    int g = 56;
                                }

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
