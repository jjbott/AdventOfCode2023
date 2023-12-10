using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day10
    {

        public void Part1()
        {
            var lines = File.ReadAllLines("Inputs/Day10.txt");
            var distances = lines.Select(l => Enumerable.Repeat(-1, l.Length).ToList()).ToList();

            (int x, int y) start = (-1, -1);
            for(int y = 0; y < lines.Length; y++)
            {
                if (lines[y].IndexOf('S') >= 0 )
                {
                    start = (lines[y].IndexOf('S'), y);
                    break;
                }
            }

            // TODO: This isnt generic
            lines[start.y] = lines[start.y].Replace('S', 'J');
            var currentDistance = 0;

            var queue = new List<(int dist, (int x, int y) pos, (int x, int y) last)>();
            queue.Add((0, start, start));
            

            while(queue.Count > 0)
            {
                var c = queue[0];
                queue.RemoveAt(0);

                if ( c.dist == 4)
                { 
                    int g = 56;
                }    

                Console.WriteLine($"{c.dist} {c.pos.x},{c.pos.y}");
                if ( c.pos.x < 0 || c.pos.x >= lines[0].Length || c.pos.y < 0 || c.pos.y >= lines.Length)
                {
                    continue;
                }                

                if (distances[c.pos.y][c.pos.x] >= 0)
                {
                    continue;
                }

                distances[c.pos.y][c.pos.x] = c.dist;
                char p = lines[c.pos.y][c.pos.x];

                /*
                 | is a vertical pipe connecting north and south.
- is a horizontal pipe connecting east and west.
L is a 90-degree bend connecting north and east.
J is a 90-degree bend connecting north and west.
7 is a 90-degree bend connecting south and west.
F is a 90-degree bend connecting south and east.*/

                var next = new List<(int dist, (int x, int y) pos)>();
                if ( p == 'J' )
                {
                    next.Add((c.dist + 1, (c.pos.x - 1, c.pos.y)));
                    next.Add((c.dist + 1, (c.pos.x, c.pos.y - 1)));
                }
                else if (p == '|')
                {
                    next.Add((c.dist + 1, (c.pos.x, c.pos.y - 1)));
                    next.Add((c.dist + 1, (c.pos.x, c.pos.y + 1)));
                }
                else if (p == '-')
                {
                    next.Add((c.dist + 1, (c.pos.x - 1, c.pos.y)));
                    next.Add((c.dist + 1, (c.pos.x + 1, c.pos.y)));
                }
                else if (p == 'L')
                {
                    next.Add((c.dist + 1, (c.pos.x, c.pos.y - 1)));
                    next.Add((c.dist + 1, (c.pos.x + 1, c.pos.y)));
                }
                else if (p == '7')
                {
                    next.Add((c.dist + 1, (c.pos.x - 1, c.pos.y)));
                    next.Add((c.dist + 1, (c.pos.x, c.pos.y + 1)));
                }
                else if (p == 'F')
                {
                    next.Add((c.dist + 1, (c.pos.x + 1, c.pos.y)));
                    next.Add((c.dist + 1, (c.pos.x, c.pos.y + 1)));
                }

                // dont backtrack
                queue.AddRange(next.Where(n => n.pos != c.pos && n.pos != c.last).Select(n => (n.dist, n.pos, c.pos)));
            }

        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day10.txt");
            var distances = lines.Select(l => Enumerable.Repeat(-1, l.Length).ToList()).ToList();

            (int x, int y) start = (-1, -1);
            for (int y = 0; y < lines.Length; y++)
            {
                if (lines[y].IndexOf('S') >= 0)
                {
                    start = (lines[y].IndexOf('S'), y);
                    break;
                }
            }

            // TODO: This isnt generic
            lines[start.y] = lines[start.y].Replace('S', 'J');
            var currentDistance = 0;

            var queue = new List<(int dist, (int x, int y) pos, (int x, int y) last)>();
            queue.Add((0, start, start));


            while (queue.Count > 0)
            {
                var c = queue[0];
                queue.RemoveAt(0);

                if (c.dist == 4)
                {
                    int g = 56;
                }

                //Console.WriteLine($"{c.dist} {c.pos.x},{c.pos.y}");
                if (c.pos.x < 0 || c.pos.x >= lines[0].Length || c.pos.y < 0 || c.pos.y >= lines.Length)
                {
                    continue;
                }

                if (distances[c.pos.y][c.pos.x] >= 0)
                {
                    continue;
                }

                distances[c.pos.y][c.pos.x] = c.dist;
                char p = lines[c.pos.y][c.pos.x];

                /*
                 | is a vertical pipe connecting north and south.
- is a horizontal pipe connecting east and west.
L is a 90-degree bend connecting north and east.
J is a 90-degree bend connecting north and west.
7 is a 90-degree bend connecting south and west.
F is a 90-degree bend connecting south and east.*/

                var next = new List<(int dist, (int x, int y) pos)>();
                if (p == 'J')
                {
                    next.Add((c.dist + 1, (c.pos.x - 1, c.pos.y)));
                    next.Add((c.dist + 1, (c.pos.x, c.pos.y - 1)));
                }
                else if (p == '|')
                {
                    next.Add((c.dist + 1, (c.pos.x, c.pos.y - 1)));
                    next.Add((c.dist + 1, (c.pos.x, c.pos.y + 1)));
                }
                else if (p == '-')
                {
                    next.Add((c.dist + 1, (c.pos.x - 1, c.pos.y)));
                    next.Add((c.dist + 1, (c.pos.x + 1, c.pos.y)));
                }
                else if (p == 'L')
                {
                    next.Add((c.dist + 1, (c.pos.x, c.pos.y - 1)));
                    next.Add((c.dist + 1, (c.pos.x + 1, c.pos.y)));
                }
                else if (p == '7')
                {
                    next.Add((c.dist + 1, (c.pos.x - 1, c.pos.y)));
                    next.Add((c.dist + 1, (c.pos.x, c.pos.y + 1)));
                }
                else if (p == 'F')
                {
                    next.Add((c.dist + 1, (c.pos.x + 1, c.pos.y)));
                    next.Add((c.dist + 1, (c.pos.x, c.pos.y + 1)));
                }

                // dont backtrack
                queue.AddRange(next.Where(n => n.pos != c.pos && n.pos != c.last).Select(n => (n.dist, n.pos, c.pos)));
            }

            var area = 0;

            for(int y = 0; y < lines.Length; ++y)
            {
                //var horizWalls = new char[] { '-', 'J', 'L', '7', 'F' };
                char wallStart = '?';

                var wallCount = 0;
                for(int x = 0; x < lines[y].Length; ++x)
                {
                    if (distances[y][x] >= 0) 
                    {
                        if ('|' == lines[y][x])
                        {
                            wallCount++;
                        }
                        else if (lines[y][x] == 'L' || lines[y][x] == 'F') 
                        {
                            wallStart = lines[y][x];
                        }
                        else if (lines[y][x] == '7' || lines[y][x] == 'J')
                        {
                            if (wallStart == 'L' && lines[y][x] == '7') wallCount++;
                            if (wallStart == 'F' && lines[y][x] == 'J') wallCount++;
                            wallStart = '?';
                        }

                    }
                    else if ( wallCount %2 == 1)
                    {
                        area++;
                    }
                }
            }
            Console.WriteLine(area);
        }
    }
}
