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

    internal class Day14a
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

            long steps = 100;

            foreach (var b in bots)
            {
                b.P.x = (int)((b.S.x + (b.V.x * steps)) % w);
                if (b.P.x < 0)
                {
                    b.P.x += w;
                }
                if ( b.P.x == w )
                {
                    int g = 56;
                }
                b.P.y = (int)((b.S.y + (b.V.y * steps)) % h);
                if ( b.P.y < 0)
                {
                    b.P.y += h;
                }
            }

            var v = bots.Where(b => b.P.x < w / 2 & b.P.y < h / 2).Count();
            var x = bots.Where(b => b.P.x > w / 2 & b.P.y < h / 2).Count();
            var y = bots.Where(b => b.P.x < w / 2 & b.P.y > h / 2).Count();
            var z = bots.Where(b => b.P.x > w / 2 & b.P.y > h / 2).Count();


            Print(bots, w, h);
            Console.WriteLine(v*x*y*z);
        }

        void Print(List<Bot> bots, int w, int h) 
        {
            for(int j = 0; j < h; j++)
            {
                for(int i = 0; i < w; i++)
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
        }
    }
}


