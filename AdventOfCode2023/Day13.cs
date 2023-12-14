using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day13
    {

        public void Part1()
        {
            var lines = File.ReadAllLines("Inputs/Day13.txt");
            var maps = new List<List<string>>();
            var current = new List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    maps.Add(current);
                    current = new List<string>();
                }
                else
                {
                    current.Add(line);
                }
            }
            maps.Add(current);

            long totalScore = 0;
            foreach (var m in maps)
            {
                var yReflection = FindReflection(m);

                if (yReflection >= 0)
                {
                    totalScore += 100 * (yReflection + 1);
                    continue;
                }

                var rotatedMap = new List<string>();
                for(int x = 0; x < m[0].Length; ++x)
                {
                    rotatedMap.Add(string.Join("", m.Select(l => l[x])));
                }

                var xReflection = FindReflection(rotatedMap);

                if ( xReflection < 0) {
                    throw new Exception("Dang");
                }

                var score = xReflection + 1;
                totalScore += score;
            }

            Console.WriteLine(totalScore);

        }

        public int FindReflection(List<string> map)
        {
            for(int y = 0; y < map.Count - 1; y++)
            {
                bool mirrored = true;
                for(int y2 = y + 1; y2 < map.Count; y2++)
                {
                    if (y - (y2 - y - 1) >= 0 && map[y - (y2 - y - 1)] != map[y2])
                    {
                        mirrored = false;
                        break;
                    }

                    

                }

                if (mirrored)
                {
                    return y;
                }


            }

            return -1;
        }

        public int FindReflection2(List<string> map, int maxDiffCount = 0)
        {
            for (int y = 0; y < map.Count - 1; y++)
            {
                int diffCount = 0;

                for (int y2 = y + 1; y2 < map.Count; y2++)
                {
                    if (y - (y2 - y - 1) >= 0)
                    {
                        if (map[y - (y2 - y - 1)] == map[y2])
                        {
                            continue;
                        }
                        else
                        {
                            for (int x = 0; x < map[y2].Length; x++)
                            {
                                if (map[y2][x] != map[y - (y2 - y - 1)][x])
                                {
                                    diffCount++;
                                    if (diffCount > maxDiffCount)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                    }

                    



                }

                if (diffCount == maxDiffCount)
                {
                    return y;
                }
            }

            return -1;
        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day13.txt");
            var maps = new List<List<string>>();
            var current = new List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    maps.Add(current);
                    current = new List<string>();
                }
                else
                {
                    current.Add(line);
                }
            }
            maps.Add(current);

            long totalScore = 0;
            foreach (var m in maps)
            {
                var yReflection = FindReflection2(m, 1);

                if (yReflection >= 0)
                {
                    totalScore += 100 * (yReflection + 1);
                    continue;
                }

                var rotatedMap = new List<string>();
                for (int x = 0; x < m[0].Length; ++x)
                {
                    rotatedMap.Add(string.Join("", m.Select(l => l[x])));
                }

                var xReflection = FindReflection2(rotatedMap, 1);

                if (xReflection < 0)
                {
                    throw new Exception("Dang");
                }

                var score = xReflection + 1;
                totalScore += score;
            }

            Console.WriteLine(totalScore);
        }
        
    }
}
