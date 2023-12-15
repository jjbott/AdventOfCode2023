using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace AdventOfCode2023
{
    internal class Day15
    {
        public int Hash(string s)
        {
            var h = 0;
            foreach (var c in s)
            {
                h += (int)c;
                h *= 17;
                h %= 256;
            }
            return h;
        
        }

        public void Part1()
        {
            var steps = File.ReadAllLines("Inputs/Day15.txt")[0].Split(",");
            Console.WriteLine(steps.Sum(s => Hash(s)));

        }

        public void Part2()
        {
            var lenses = new List<List<(string l, int f)>>();
            for (int i = 0; i < 256; i++) lenses.Add(new List<(string l, int f)>());

            var steps = File.ReadAllLines("Inputs/Day15.txt")[0].Split(",").Select(s => (l: s.Split("=")[0].Split("-")[0], h:Hash(s.Split("=")[0].Split("-")[0]), i:s.Substring(s.Split("=")[0].Split("-")[0].Length))).ToList();

            foreach(var s in steps)
            {
                var index = lenses[s.h].FindIndex(l => l.l == s.l);
                if (s.i[0] == '=')
                {
                    if (index >= 0)
                    {
                        lenses[s.h][index]= ( l: s.l, f: int.Parse(s.i.Substring(1)));
                    }
                    else
                    {
                        lenses[s.h].Add((l: s.l, f: int.Parse(s.i.Substring(1))));
                    }
                }
                else
                {
                    if (index >= 0)
                    {
                        lenses[s.h].RemoveAt(index);
                    }
                    else
                    {
                        int g = 56;
                    }
                }
            }

            long power = 0;
            for(int i = 0; i < lenses.Count; ++i)
            {
                for (int j = 0; j < lenses[i].Count; ++j)
                {
                    power += (long)(i + 1) * (long)(j+1) * (long)lenses[i][j].f;
                }
            }
            Console.WriteLine(power);
        }
    }
}
