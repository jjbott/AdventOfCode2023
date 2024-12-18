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

    internal class Day13b
    {
        class Coord
        {
            public long x;
            public long y;
        }

        class Machine
        {
            public Coord ButtonA = new Coord();
            public Coord ButtonB = new Coord();
            public Coord Prize = new Coord();
        }

        public void Run()
        {
            IEnumerable<string> lines = File.ReadAllLines("./2024/Inputs/Day13.txt");
            var machines = new List<Machine>();


            /*
            Button A: X+90, Y+73
            Button B: X+19, Y+81
            Prize: X=8266, Y=12870
            */

            while ( lines.Any())
            {
                var m = new Machine();
                var l = lines.Take(3).ToList();
                lines = lines.Skip(4);

                m.ButtonA.x = int.Parse(l[0].Split("X")[1].Split(",")[0]);
                m.ButtonA.y = int.Parse(l[0].Split("Y")[1]);

                m.ButtonB.x = int.Parse(l[1].Split("X")[1].Split(",")[0]);
                m.ButtonB.y = int.Parse(l[1].Split("Y")[1]);

                m.Prize.x = int.Parse(l[2].Split("X=")[1].Split(",")[0]) + 10000000000000;
                m.Prize.y = int.Parse(l[2].Split("Y=")[1]) + 10000000000000;

                machines.Add(m);
            }

            long cost = 0;

            foreach(var m in machines)
            {
                if( Math.Abs(m.ButtonA.x/(double)m.ButtonA.y - m.ButtonB.x / (double)m.ButtonB.y) > 0.1 )
                {
                    var t1 = CalcNonColinear(m);
                    cost += t1.c;
                }
                else
                {
                    int g = 56;
                }
            }

            Console.WriteLine(cost);
        }

        private static (long a, long b, long c) CalcNonColinear(Machine m)
        {
            var ba = m.ButtonA;
            var bb = m.ButtonB;
            var p = m.Prize;

            // a*ax+b*bx=px
            // a*ay+b*by=py

            // a=(px-b*bx)/ax
            // a=(py-b*by)/ay
            // (px-b*bx)/ax = (py-b*by)/ay

            // b = (px*ay-ax*py)/(bx*ay-ax*by)

            checked
            {
                var b = (p.x * ba.y - ba.x * p.y) / (double)(bb.x * ba.y - ba.x * bb.y);
                var a = (p.x - b * bb.x) / (double)ba.x;

                if (b >= 0 && a >= 0 && a == (long)a && b == (long)b)
                {
                    return ((long)a, (long)b, (long)a * 3 + (long)b);
                }
            }

            
            return (0,0,0);
        }
    }
}

