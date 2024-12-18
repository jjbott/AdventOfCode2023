using AOC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{

    internal class Day14b
    {
        class Coord
        {
            public int x;
            public int y;
        }

        class Bot
        {
            public Coord S = new Coord();
            public Coord V = new Coord();
            public Coord P = new Coord();
        }

        public void Run()
        {
            IEnumerable<string> lines = File.ReadAllLines("./2024/Inputs/Day14.txt");
            int w = 101;
            int h = 103;

            var bots = new List<Bot>();
            foreach (var line in lines)
            {
                //  p=61,7 v=48,98
                var b = new Bot();
                b.S.x = int.Parse(line.Split("p=")[1].Split(",")[0]);
                b.S.y = int.Parse(line.Split(",")[1].Split(" v")[0]);

                b.V.x = int.Parse(line.Split("v=")[1].Split(",")[0]);
                b.V.y = int.Parse(line.Split(",")[2]);

                //b.P.x = b.S.x;
                //b.P.y = b.S.y;

                bots.Add(b);
            }

            long steps = 1000000000000000;
            for (long i = 0; i <= steps; i = i + 1)
            {
                foreach (var b in bots)
                {

                    b.P.x = (int)((b.S.x + (b.V.x * i)) % w);
                    if (b.P.x < 0)
                    {
                        b.P.x += w;
                    }
                    if (b.P.x == w)
                    {
                        int g = 56;
                    }
                    b.P.y = (int)((b.S.y + (b.V.y * i)) % h);
                    if (b.P.y < 0)
                    {
                        b.P.y += h;
                    }
                }

                foreach (var b in bots)
                {
                    if (bots.Count(b2 => b2.P.y == b.P.y - 1 && b2.P.x == b.P.x + 1) == 1
                        && bots.Count(b2 => b2.P.y == b.P.y - 2 && b2.P.x == b.P.x + 2) == 1
                        && bots.Count(b2 => b2.P.y == b.P.y - 3 && b2.P.x == b.P.x + 3) == 1
                        && bots.Count(b2 => b2.P.y == b.P.y - 4 && b2.P.x == b.P.x + 4) == 1
                        && bots.Count(b2 => b2.P.y == b.P.y - 5 && b2.P.x == b.P.x + 5) == 1
                        && bots.Count(b2 => b2.P.y == b.P.y - 6 && b2.P.x == b.P.x + 6) == 1)
                    {
                        Print(bots, w, h);
                        Console.WriteLine(i);
                        Console.ReadLine();
                    }
                }

                    var test = bots.Where(b => b.P.y == 102 && b.P.x == 50).ToList();
                var test2 = bots.Where(b => b.P.y == 102 && b.P.x == 49).ToList();
                var test3 = bots.Where(b => b.P.y == 102 && b.P.x == 51).ToList();
                //var test4 = bots.Where(b => b.P.y == 2 && b.P.x == 48).ToList();
                //var test5 = bots.Where(b => b.P.y == 2 && b.P.x == 52).ToList();

                if ( test.Count() == 1 && test2.Count() == 1 && test3.Count() == 1)
                    //&& test4.Count() == 1
                    //&& test5.Count() == 1)
                {
                    //Console.WriteLine(i);
                    //Print(bots, 10, 10, 45, 0);
                    int g = 56;
                }
                else
                {
                    int g = 56;
                }
                
                var test8 = bots.GroupBy(b => (b.P.x, b.P.y)).Any(g => g.Count() > 1);
                if ( !test8 )
                {
                    Print(bots, w, h);
                    Console.WriteLine(i);

                    var v = bots.Where(b => b.P.x < w / 2 & b.P.y < h / 2).Count();
                    var x = bots.Where(b => b.P.x > w / 2 & b.P.y < h / 2).Count();
                    var y = bots.Where(b => b.P.x < w / 2 & b.P.y > h / 2).Count();
                    var z = bots.Where(b => b.P.x > w / 2 & b.P.y > h / 2).Count();


                    //Print(bots, w, h);
                    Console.WriteLine(v * x * y * z);

                    Console.ReadLine();
                }
                
            }

            
        }

        void Print(List<Bot> bots, int w, int h, int minX = 0, int minY = 0) 
        {
            for(int j = minY; j < minY + h; j++)
            {
                for(int i = minX; i < minX + w; i++)
                {
                    var c = bots.Count(b => b.P.x == i && b.P.y == j);
                    if( c == 0)
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write((char)('0' + c));
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}


