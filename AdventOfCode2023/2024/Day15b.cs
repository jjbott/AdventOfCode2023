using AOC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{

    internal class Day15b
    {
        (int x, int y) Move2((int x, int y) pos, (int x, int y) dir, char[][] map)
        {
            var seen = new HashSet<(int x, int y)>();

            var lines = new List<List<(int x, int y)>>();
            var queue = new Queue<(int x, int y)>();
            queue.Enqueue((pos.x, pos.y));

            while (queue.TryDequeue(out var c))
            {
                if (seen.Contains(c))
                {
                    // already handling this position
                    continue;
                }

                var line = new List<(int x, int y)>();

                var empty = (x: -1, y: -1);

                //c.x += dir.x;
                //c.y += dir.y;
                while (map[c.y][c.x] != '#')
                {
                    line.Add(c);

                    if (map[c.y][c.x] == '.')
                    {
                        empty = c;
                        break;
                    }
                    else if (map[c.y][c.x] == '[')
                    {
                        queue.Enqueue((c.x + 1, c.y));
                    }
                    else if (map[c.y][c.x] == ']')
                    {
                        queue.Enqueue((c.x - 1, c.y));
                    }

                    c.x += dir.x;
                    c.y += dir.y;


                }

                if (empty.x < 0 && empty.y < 0)
                {
                    // Never found a space, cant move
                    return pos;
                }
                else
                {
                    foreach(var s in line)
                    {
                        seen.Add(s);
                    }
                    lines.Add(line);
                }
            }

            // everyone is happy, shift all lines
            foreach(var l in lines)
            {
                if ( map[l.Last().y][l.Last().x] != '.')
                {
                    // I messed up!
                    throw new Exception();
                }

                for(int i = l.Count- 1; i >= 1; i--)
                {
                    map[l[i].y][l[i].x] = map[l[i - 1].y][l[i - 1].x];
                }

                map[l[0].y][l[0].x] = '.';
            }

            return (pos.x + dir.x, pos.y + dir.y);
        }

        (int x, int y) Move((int x, int y) pos, (int x, int y) dir, char[][] map)
        {
            var c = pos;            

            var empty = (x: -1, y: -1);

            c.x += dir.x;
            c.y += dir.y;
            while (map[c.y][c.x] != '#')
            {
                if (map[c.y][c.x] == '.')
                {
                    empty = c;
                    break;
                }
                c.x += dir.x;
                c.y += dir.y;
            }

            if ( empty.x >= 0 && empty.y >= 0)
            {
                c = empty;

                while ( c != pos )
                {
                    var n = (x: c.x - dir.x, y: c.y - dir.y);

                    map[c.y][c.x] = map[n.y][n.x];
                    
                    c = n;
                }

                map[pos.y][pos.x] = '.';

                pos = (pos.x + dir.x, pos.y + dir.y);
            }

            return pos;
        }

        public void Run()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day15.txt");

            /*
            If the tile is #, the new map contains ## instead.
            If the tile is O, the new map contains [] instead.
            If the tile is ., the new map contains .. instead.
            If the tile is @, the new map contains @. instead.
            */
            char[][] map = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.SelectMany( c =>
                {
                switch (c)
                {
                    case '.': return new char[] { '.', '.' };
                        case '#': return new char[] {'#', '#'};
                        case 'O': return new char[] {'[', ']'};
                        case '@': return new char[] {'@', '.'};
                    }
                    return new char[] { };
                }).ToArray()
                ).ToArray();
            char[] dirs = string.Join("", lines.SkipWhile(l => !string.IsNullOrWhiteSpace(l)).Skip(1)).ToCharArray();

            (int x, int y) pos = (0, 0);

            for(int y = 0; y < map.Length; ++y)
            {
                for(int x = 0; x < map[0].Length; ++x)
                {
                    if (map[y][x] == '@' )
                    {
                        pos.x = x;
                        pos.y = y;
                        break;
                    }
                }
            }

            foreach (var c in dirs)
            {
                switch (c)
                {
                    case '>':
                        pos = Move(pos, (1, 0), map);
                        break;
                    case '<':
                        pos = Move(pos, (-1, 0), map);
                        break;
                    case '^':
                        pos = Move2(pos, (0, -1), map);
                        break;
                    case 'v':
                        pos = Move2(pos, (0, 1), map);
                        break;
                }

                //Print(map);
                //Console.WriteLine();
                //Console.ReadLine();
            }

            var sum = 0;
            for (int y = 0; y < map.Length; ++y)
            {
                for (int x = 0; x < map[0].Length; ++x)
                {
                    if (map[y][x] == '[')
                    {
                        checked
                        {
                            sum += 100 * y + x;
                        }
                    }
                }
            }

            Print(map);
            Console.WriteLine(sum);
        }
        
        void Print(char[][] map) 
        {
            for(int y = 0; y < map.Length; y++)
            {
                Console.WriteLine(new string(map[y]));
            }
        }
    }
}


