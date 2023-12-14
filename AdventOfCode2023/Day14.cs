using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace AdventOfCode2023
{
    internal class Day14
    {
        private List<char[]> Rotate(List<char[]> map)
        {
            var rotated = new List<char[]>();

            for (int x = 0; x < map[0].Length; x++)
            {
                rotated.Add(new string('.', map.Count).ToCharArray());
            }

            for (int x = 0; x < map[0].Length; x++)
            {
                for (int y = 0; y < map.Count; y++)
                {
                    rotated[x][map.Count - y - 1] = map[y][x];
                }
            }

            return rotated;
        }

        public void Print(List<char[]> map)
        {
            foreach (var l in map)
            {
                Console.WriteLine(new string(l));
            }
            Console.WriteLine();
        }

        public void Part1()
        {
            var lines = File.ReadAllLines("Inputs/Day14.txt").Select(l => l.ToCharArray()).ToList();

            for (int x = 0; x < lines[0].Length; x++)
            {
                var minY = 0;
                for (int y = 0; y < lines.Count(); y++)
                {
                    if (lines[y][x] == '#')
                    {
                        minY = y + 1;
                    }
                    else if (lines[y][x] == 'O')
                    {
                        if (minY != y)
                        {
                            lines[minY][x] = 'O';
                            lines[y][x] = '.';
                        }
                        minY++;
                    }
                }
            }

            foreach (var l in lines)
            {
                Console.WriteLine(new string(l));
            }

            var load = 0;
            for (int y = 0; y < lines.Count; y++)
            {
                load += lines[y].Count(c => c == 'O') * (lines.Count - y);
            }

            int g = 56;
            Console.WriteLine(load);
        }

        public void ShiftNorth(List<char[]> map)
        {
            for (int x = 0; x < map[0].Length; x++)
            {
                var minY = 0;
                for (int y = 0; y < map.Count(); y++)
                {
                    if (map[y][x] == '#')
                    {
                        minY = y + 1;
                    }
                    else if (map[y][x] == 'O')
                    {
                        if (minY != y)
                        {
                            map[minY][x] = 'O';
                            map[y][x] = '.';
                        }
                        minY++;
                    }
                }
            }
        }

        public int CalcLoad(List<char[]> map)
        {
            var load = 0;
            for (int y = 0; y < map.Count; y++)
            {
                load += map[y].Count(c => c == 'O') * (map.Count - y);
            }
            return load;
        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day14.txt").Select(l => l.ToCharArray()).ToList();

            Print(lines);

            var loadCalcs = new List<int>();
            var periodFound = -1;

            for (int i = 0; i < 1000000000; ++i)
            {
                ShiftNorth(lines);
                //Print(lines);
                lines = Rotate(lines);
                //Print(lines);
                ShiftNorth(lines); // W
                //Print(Rotate(Rotate(Rotate(lines))));
                lines = Rotate(lines);
                ShiftNorth(lines); // S
                //Print(Rotate(Rotate(lines)));
                lines = Rotate(lines);
                ShiftNorth(lines); // E
                //Print(Rotate(lines));
                lines = Rotate(lines);

                var load = CalcLoad(lines);
                loadCalcs.Add(load);
                Console.WriteLine($"{i}: {load}");

                //Print(lines);


                for (int period = 1; period < 1000 && periodFound < 0; ++period)
                {
                    // just checking the most recent load calc. Assuming that finding 10 matches is a good indicator of a period.
                    if (i - (period * 10) > 0)
                    {
                        periodFound = period;
                        for (int p = 1; p <= 10; ++p)
                        {
                            if (load != loadCalcs[loadCalcs.Count - p * period])
                            {
                                periodFound = -1;
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (periodFound > 0)
                {
                    i += ((1000000000 - i) / periodFound) * periodFound;
                }
            }
        }
    }
}
