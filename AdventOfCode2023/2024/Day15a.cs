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

    internal class Day15a
    {
        class Coord
        {
            public int x;
            public int y;
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
            char[][] map = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.ToCharArray()).ToArray();
            char[] dirs = string.Join("", lines.SkipWhile(l => !string.IsNullOrWhiteSpace(l)).Skip(1)).ToCharArray();

            (int x, int y) pos = (0, 0);

            for(int y = 0; y < map.Length; ++y)
            {
                for(int x = 0; x < map[0].Length; ++x)
                {
                    if (map[x][y] == '@' )
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
                        pos = Move(pos, (0, -1), map);
                        break;
                    case 'v':
                        pos = Move(pos, (0, 1), map);
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
                    if (map[y][x] == 'O')
                    {
                        sum += 100 * y + x;
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


