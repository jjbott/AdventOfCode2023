using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day11
    {

        public void Part1()
        {
            var expandCount = 1;

            var lines = File.ReadAllLines("Inputs/Day11Example2b.txt");
            var expandedLines = new List<string>();
            for(int i = 0; i < lines.Length; i++)
            {
                expandedLines.Add(lines[i]);
                if (!lines[i].Any(c => c != '.'))
                {
                    for (int j = 0; j < expandCount; ++j)
                    {
                        expandedLines.Add(lines[i]);
                    }
                }                        
            }

            var expandedRows = new List<string>(Enumerable.Repeat("", expandedLines.Count));

            for (int i = 0; i < expandedLines[0].Length; i++)
            {
                for(var li = 0; li < expandedLines.Count; ++li)
                {
                    expandedRows[li] += expandedLines[li][i];
                }

                if (!expandedLines.Any(l => l[i] != '.'))
                {
                    for (int j = 0; j < expandCount; ++j)
                    {
                        for (var li = 0; li < expandedLines.Count; ++li)
                        {
                            expandedRows[li] += expandedLines[li][i];
                        }
                    }
                }
            }

            var galaxies = new List<(int x, int y)>();
            for(int y = 0; y < expandedRows.Count; y++)
            {
                for (int x = 0; x < expandedRows[0].Length;  x++)
                {
                    if (expandedRows[y][x] == '#')
                    {
                        galaxies.Add((x, y));
                    }
                }
            }

            int maxDistance = 50;
            int minDistance = int.MaxValue;

            long distance = 0;
            int c = 0;
            for (int i = 0; i < galaxies.Count; i++)
            {
                var start = galaxies[i];
                foreach (var g in galaxies.Skip(i + 1))
                {
                    c++;
                    distance += Distance(start, g);
                }

            }

            Console.WriteLine(distance);

        }

        public int Distance((int x, int y) p1, (int x, int y) p2)
        {
            return Math.Abs(p1.y - p2.y) + Math.Abs(p1.x - p2.x);
        }

        public int Distance2((int x, int y) p1, (int x, int y) p2, List<string> map)
        {
            var expansion = 1000000;

            var xCount = 0;
            var yCount = 0;

            var xs = new List<int> { p1.x, p2.x };
            xs.Sort();

            for(int x = xs[0]; x < xs[1]; ++x)
            {
                if (map[p1.y][x] == 'M') xCount++;
            }

            var ys = new List<int> { p1.y, p2.y };
            ys.Sort();

            for (int y = ys[0]; y < ys[1]; ++y)
            {
                if (map[y][p1.x] == 'M') yCount++;
            }

            return (Math.Abs(p1.y - p2.y) - yCount) + (Math.Abs(p1.x - p2.x) - xCount) + (yCount* expansion) + (xCount* expansion); 
        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day11.txt");
            var expandedLines = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                
                if (!lines[i].Any(c => c != '.'))
                {
                    expandedLines.Add(new string('M', lines[i].Length));
                }
                else
                {
                    expandedLines.Add(lines[i]);
                }
            }

            var expandedRows = new List<string>(Enumerable.Repeat("", expandedLines.Count));

            for (int i = 0; i < expandedLines[0].Length; i++)
            {
                if (!expandedLines.Any(l => l[i] != '.' && l[i] != 'M'))
                {
                    for (var li = 0; li < expandedLines.Count; ++li)
                    {
                        expandedRows[li] += 'M';// expandedLines[li][i] == 'M' ? 'G' : 'M';
                    }
                }
                else
                {
                    for (var li = 0; li < expandedLines.Count; ++li)
                    {
                        expandedRows[li] += expandedLines[li][i];
                    }
                }
            }

            var galaxies = new List<(int x, int y)>();
            for (int y = 0; y < expandedRows.Count; y++)
            {
                for (int x = 0; x < expandedRows[0].Length; x++)
                {
                    if (expandedRows[y][x] == '#')
                    {
                        galaxies.Add((x, y));
                    }
                }
            }

            int maxDistance = 50;
            int minDistance = int.MaxValue;

            long distance = 0;
            int c = 0;
            for (int i = 0; i < galaxies.Count; i++)
            {
                var start = galaxies[i];
                foreach (var g in galaxies.Skip(i + 1))
                {
                    c++;
                    distance += Distance2(start, g, expandedRows);
                }

            }

            Console.WriteLine(distance);
        }

        
    }
}
