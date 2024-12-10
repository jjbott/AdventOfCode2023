using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2023
{
    internal class Day21
    {
        public void Part1(int stepCount = 64)
        {

            var lines = File.ReadAllLines("Inputs/Day21.txt");

            int startX = 0;
            int startY = 0;

            var map = new int[lines[0].Length, lines.Length];
            for (int y = 0; y < lines.Length; ++y)
            {
                for (int x = 0; x < lines[y].Length; ++x)
                {
                    if ( lines[y][x] == 'S')
                    {
                        startX = x;
                        startY = y;
                        map[y, x] = 0;
                    }
                    map[y, x] = lines[y][x] == '#' ? -1 : 0;
                }
            }

            for (int i = 0; i < stepCount; ++i)
            {
                for (int y = 0; y < map.GetLength(1); ++y)
                {
                    for (int x = 0; x < map.GetLength(0); ++x)
                    {
                        if ((i > 0 && map[x, y] == i) || (i == 0 && x == startX && y == startY))
                        {
                            if (x > 0 && map[x - 1, y] >= 0)
                            {
                                map[x-1, y] = i + 1;
                            }

                            if (x < map.GetLength(0) -1 && map[x + 1, y]>= 0)
                            {
                                map[x + 1, y] = i + 1;
                            }

                            if (y > 0 && map[x, y - 1] >= 0)
                            {
                                map[x, y - 1] = i + 1;
                            }

                            if (y < map.GetLength(1) - 1 && map[x, y + 1] >= 0)
                            {
                                map[x, y + 1] = i + 1;
                            }

                        }
                    }
                }
                /*
                for (int x = 0; x < map.GetLength(0); ++x)
                {
                    for (int y = 0; y < map.GetLength(1); ++y)
                    {

                        if (map[x, y] == i + 1)
                        {
                            Console.Write("O");
                        }
                        else if (map[x, y] == -1)
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
                */
                int g = 56;


            }

            Console.WriteLine(map.Cast<int>().Where(i => i == stepCount).Count());

        }

        public void Part2(int stepCount)
        {
            var lines = File.ReadAllLines("Inputs/Day21.txt");

            int startX = 0;
            int startY = 0;

            var map = new List<List<int>>();

            for (int y = 0; y < lines.Length; ++y)
            {
                map.Add(lines[y].Select((c, x) =>
                {
                    if (c == 'S')
                    {
                        startX = x;
                        startY = y;
                        return 0;
                    }
                    else
                    {
                        return c == '#' ? -1 : 0;
                    }
                }).ToList());
            }

            Dictionary<(int x, int y), List<List<int>>> gridMaps = new Dictionary<(int x, int y), List<List<int>>>();

            void AddGridMap(int x, int y)
            {
                var data = map.Select(l => new List<int>(Enumerable.Repeat(0, l.Count)).ToList()).ToList();
                gridMaps[(x, y)] = data;
            }

            AddGridMap(0, 0);

            // '%' will return negative values, and we don't want that
            int mod(int x, int mod)
            {
                return (x % mod + mod) % mod;
            }

            Dictionary<(int x, int y), (List<int> counts, bool alive)> gridData = new Dictionary<(int x, int y), (List<int> counts, bool alive)>();

            for (int i = 0; i < stepCount; ++i)
            {
                /*if (i % 1000 == 0)
                {
                    Console.WriteLine(i);
                }*/

                Dictionary<(int x, int y), int> counts = new Dictionary<(int x, int y), int>();

                void UpdateMap(int realX, int realY, int steps)
                {
                    var mapX = mod(realX, map[0].Count);
                    var mapY = mod(realY, map.Count);

                    var gridX = (int)Math.Floor((realX) / (double)map[0].Count);
                    var gridY = (int)Math.Floor((realY) / (double)map.Count);

                    if ( gridData.ContainsKey((gridX, gridY)) && !gridData[(gridX, gridY)].alive )
                    {
                        // Dead cell, nothing to do
                        return;
                    }

                    if (!gridMaps.ContainsKey((gridX, gridY)))
                    {
                        AddGridMap(gridX, gridY);
                    }

                    var gridMap = gridMaps[(gridX, gridY)];

                    if (map[mapY][mapX] >= 0 && gridMap[mapY][mapX] != steps)
                    {
                        gridMap[mapY][mapX] = steps;

                        if (!counts.ContainsKey((gridX, gridY)))
                        {
                            counts[(gridX, gridY)] = 1;
                        }
                        else
                        {
                            counts[(gridX, gridY)]++;
                        }
                    }
                }

                // Extra ToList to ignore any new gridmaps that are added as we go
                foreach (var kv in gridMaps.ToList())
                {
                    for (int y = 0; y < map.Count; ++y)
                    {
                        for (int x = 0; x < map[0].Count; ++x)
                        {
                            if ((i > 0 && kv.Value[y][x] == i) || (i == 0 && x == startX && y == startY))
                            {
                                var realX = (kv.Key.x * map[0].Count) + x;
                                var realY = (kv.Key.y * map.Count) + y;

                                UpdateMap(realX - 1, realY, i + 1);
                                UpdateMap(realX + 1, realY, i + 1);
                                UpdateMap(realX, realY - 1, i + 1);
                                UpdateMap(realX, realY + 1, i + 1);
                            }
                        }
                    }
                }

                foreach (var kv in counts)
                {
                    if (!gridData.ContainsKey(kv.Key))
                    {
                        gridData[kv.Key] = (new List<int>(), true);
                    }

                    gridData[kv.Key].counts.Add(kv.Value);
                    if (gridData[kv.Key].counts.Count >= 3 && kv.Value == gridData[kv.Key].counts[gridData[kv.Key].counts.Count - 3])
                    {
                        // cell is dead
                        //gridData[kv.Key] = (gridData[kv.Key].counts.Skip(gridData[kv.Key].counts.Count - 2).ToList(), false);
                        //gridMaps.Remove(kv.Key);
                    }
                }
                /*
                var zcount = 0;
                for (int x = 0; x < map[0].Count; ++x)
                {
                    for (int y = 0; y < map.Count; ++y)
                    {
                        if (map[y][x][(0, 0)] == i + 1 )
                        {
                            zcount++;
                        }
                    }
                }

                Console.WriteLine(zcount);
                */

                var minX = gridMaps.Keys.Min(k => k.x);
                var minY = gridMaps.Keys.Min(k => k.x);

                var realMinX = minX * map[0].Count;
                var realMinY = minY * map.Count;
                /*
                for (int y = realMinY; y < realMinY + (map.Count * 2); ++y)
                    {   
                        for (int x = realMinX; x < realMinX + (map[0].Count * 2); ++x)
                        {
                            var mapX = mod(x, map[0].Count);
                            var mapY = mod(y, map.Count);

                            var gridX = (int)Math.Floor(x / (double)map[0].Count);
                            var gridY = (int)Math.Floor(y / (double)map.Count);

                            if (!gridMaps.ContainsKey((gridX, gridY)))
                            {
                                Console.Write(map[mapY][mapX] == -1 ? '#' : '.');
                            }
                            else
                            {
                                var gridMap = gridMaps[(gridX, gridY)];
                                if (map[mapY][mapX] == -1)
                                {
                                    Console.Write('#');
                                }
                                else if (gridMap[mapY][mapX] == i + 1)
                                {
                                    Console.Write("O");
                                }
                                else
                                {
                                    Console.Write(".");
                                }
                            }
                        }
                    Console.WriteLine();
                }
                Console.WriteLine();
                */

                int g = 56;
            }

            var count = 0;
            foreach (var kv in gridMaps)
            {
                for (int y = 0; y < kv.Value.Count; ++y)
                {
                    count += kv.Value[y].Where(i => i == stepCount).Count();
                }
            }

            Console.WriteLine(count);

        }
    }
}
