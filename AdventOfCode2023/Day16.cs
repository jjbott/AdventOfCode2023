using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace AdventOfCode2023
{
    internal class Day16
    {
        public enum Direction {
            Up,Left,Down,Right
        }

        public void Part1()
        {
            var map = File.ReadAllLines("Inputs/Day16.txt").Select(l => l.ToCharArray()).ToList();

            var beams = new HashSet<((int x, int y) pos, Direction dir)>();

            var queue = new List<((int x, int y) pos, Direction dir)> { ((0, 0), Direction.Right) };

            while (queue.Count > 0)
            {
                var current = queue.First();
                queue.RemoveAt(0);

                if(current.pos.x < 0 || current.pos.x >= map[0].Length || current.pos.y < 0 || current.pos.y >= map.Count)
                {
                    continue;
                }

                if ( !beams.Contains(current))
                {
                    beams.Add(current);

                    var c = map[current.pos.y][current.pos.x];

                    if ( c == '.')
                    {
                        switch(current.dir)
                        {
                            case Direction.Left: queue.Add(((current.pos.x - 1, current.pos.y), current.dir)); break;
                            case Direction.Right: queue.Add(((current.pos.x + 1, current.pos.y), current.dir)); break;
                            case Direction.Up: queue.Add(((current.pos.x, current.pos.y - 1), current.dir)); break;
                            case Direction.Down: queue.Add(((current.pos.x, current.pos.y + 1), current.dir)); break;
                        }
                    }
                    else if ( c == '\\')
                    {
                        switch (current.dir)
                        {
                            case Direction.Left: queue.Add(((current.pos.x, current.pos.y - 1), Direction.Up)); break;
                            case Direction.Right: queue.Add(((current.pos.x, current.pos.y + 1), Direction.Down)); break;
                            case Direction.Up: queue.Add(((current.pos.x - 1, current.pos.y), Direction.Left)); break;
                            case Direction.Down: queue.Add(((current.pos.x + 1, current.pos.y), Direction.Right)); break;
                        }
                    }
                    else if (c == '/')
                    {
                        switch (current.dir)
                        {
                            case Direction.Left: queue.Add(((current.pos.x, current.pos.y + 1), Direction.Down)); break;
                            case Direction.Right: queue.Add(((current.pos.x, current.pos.y - 1), Direction.Up)); break;
                            case Direction.Up: queue.Add(((current.pos.x + 1, current.pos.y), Direction.Right)); break;
                            case Direction.Down: queue.Add(((current.pos.x - 1, current.pos.y), Direction.Left)); break;
                        }
                    }
                    else if (c == '|')
                    {
                        switch (current.dir)
                        {
                            case Direction.Left:
                            case Direction.Right:
                                {
                                    queue.Add(((current.pos.x, current.pos.y + 1), Direction.Down));
                                    queue.Add(((current.pos.x, current.pos.y - 1), Direction.Up));
                                }
                                break;                       
                            case Direction.Up: queue.Add(((current.pos.x, current.pos.y-1), current.dir)); break;
                            case Direction.Down: queue.Add(((current.pos.x, current.pos.y+1), current.dir)); break;
                        }
                    }
                    else if (c == '-')
                    {
                        switch (current.dir)
                        {
                            case Direction.Up:
                            case Direction.Down:
                                {
                                    queue.Add(((current.pos.x-1, current.pos.y), Direction.Left));
                                    queue.Add(((current.pos.x+1, current.pos.y), Direction.Right));
                                }
                                break;
                            case Direction.Left: queue.Add(((current.pos.x-1, current.pos.y), current.dir)); break;
                            case Direction.Right: queue.Add(((current.pos.x+1, current.pos.y), current.dir)); break;
                        }
                    }
                }
            }

            Console.WriteLine(beams.Select(b => b.pos).Distinct().Count());
        }

        public int CountEnergized(List<char[]> map, ((int x, int y) pos, Direction dir) initial )
        {
            var beams = new HashSet<((int x, int y) pos, Direction dir)>();

            var queue = new List<((int x, int y) pos, Direction dir)> { initial };

            while (queue.Count > 0)
            {
                var current = queue.First();
                queue.RemoveAt(0);

                if (current.pos.x < 0 || current.pos.x >= map[0].Length || current.pos.y < 0 || current.pos.y >= map.Count)
                {
                    continue;
                }

                if (!beams.Contains(current))
                {
                    beams.Add(current);

                    var c = map[current.pos.y][current.pos.x];

                    if (c == '.')
                    {
                        switch (current.dir)
                        {
                            case Direction.Left: queue.Add(((current.pos.x - 1, current.pos.y), current.dir)); break;
                            case Direction.Right: queue.Add(((current.pos.x + 1, current.pos.y), current.dir)); break;
                            case Direction.Up: queue.Add(((current.pos.x, current.pos.y - 1), current.dir)); break;
                            case Direction.Down: queue.Add(((current.pos.x, current.pos.y + 1), current.dir)); break;
                        }
                    }
                    else if (c == '\\')
                    {
                        switch (current.dir)
                        {
                            case Direction.Left: queue.Add(((current.pos.x, current.pos.y - 1), Direction.Up)); break;
                            case Direction.Right: queue.Add(((current.pos.x, current.pos.y + 1), Direction.Down)); break;
                            case Direction.Up: queue.Add(((current.pos.x - 1, current.pos.y), Direction.Left)); break;
                            case Direction.Down: queue.Add(((current.pos.x + 1, current.pos.y), Direction.Right)); break;
                        }
                    }
                    else if (c == '/')
                    {
                        switch (current.dir)
                        {
                            case Direction.Left: queue.Add(((current.pos.x, current.pos.y + 1), Direction.Down)); break;
                            case Direction.Right: queue.Add(((current.pos.x, current.pos.y - 1), Direction.Up)); break;
                            case Direction.Up: queue.Add(((current.pos.x + 1, current.pos.y), Direction.Right)); break;
                            case Direction.Down: queue.Add(((current.pos.x - 1, current.pos.y), Direction.Left)); break;
                        }
                    }
                    else if (c == '|')
                    {
                        switch (current.dir)
                        {
                            case Direction.Left:
                            case Direction.Right:
                                {
                                    queue.Add(((current.pos.x, current.pos.y + 1), Direction.Down));
                                    queue.Add(((current.pos.x, current.pos.y - 1), Direction.Up));
                                }
                                break;
                            case Direction.Up: queue.Add(((current.pos.x, current.pos.y - 1), current.dir)); break;
                            case Direction.Down: queue.Add(((current.pos.x, current.pos.y + 1), current.dir)); break;
                        }
                    }
                    else if (c == '-')
                    {
                        switch (current.dir)
                        {
                            case Direction.Up:
                            case Direction.Down:
                                {
                                    queue.Add(((current.pos.x - 1, current.pos.y), Direction.Left));
                                    queue.Add(((current.pos.x + 1, current.pos.y), Direction.Right));
                                }
                                break;
                            case Direction.Left: queue.Add(((current.pos.x - 1, current.pos.y), current.dir)); break;
                            case Direction.Right: queue.Add(((current.pos.x + 1, current.pos.y), current.dir)); break;
                        }
                    }
                }
            }
            return beams.Select(b => b.pos).Distinct().Count();
        }

        public void Part2()
        {
            var map = File.ReadAllLines("Inputs/Day16.txt").Select(l => l.ToCharArray()).ToList();

            var max = 0;

            for(int x = 0; x < map[0].Length; x++)
            {
                var c = CountEnergized(map, ((x, 0), Direction.Down));
                if ( c >  max ) max = c;

                c = CountEnergized(map, ((x, map.Count -1), Direction.Up));
                if (c > max) max = c;
            }

            for (int y = 0; y < map.Count; y++)
            {
                var c = CountEnergized(map, ((0, y), Direction.Right));
                if (c > max) max = c;

                c = CountEnergized(map, ((map[0].Length - 1, y), Direction.Left));
                if (c > max) max = c;
            }


            Console.WriteLine(max);
        }
    }
}
