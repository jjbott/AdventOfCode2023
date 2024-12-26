using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{
    internal class Day25a
    {


        public void Run()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day25.txt");

            var lockStrings = new List<List<string>>();
            var keyStrings = new List<List<string>>();

            List<string> current = null;
            foreach(var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    current = null;
                    continue;
                }

                if (current == null)
                {
                    if (line == new string('#', line.Length))
                    {
                        current = new List<string>();
                        lockStrings.Add(current);
                        current.Add(line);
                    }
                    else if (line == new string('.', line.Length))
                    {
                        current = new List<string>();
                        keyStrings.Add(current);
                        current.Add(line);
                    }
                }
                else
                {
                    current.Add(line);
                }
            }

            var locks = new List<List<int>>();
            var keys = new List<List<int>>();

            foreach(var l in lockStrings)
            {
                locks.Add(
                    Enumerable.Range(0, l[0].Length)
                    .Select(i => 
                        l.Select(l2 => l2[i])
                        .Where(c => c == '#').Count()
                    ).ToList());
            }

            foreach (var k in keyStrings)
            {
                keys.Add(
                    Enumerable.Range(0, k[0].Length)
                    .Select(i =>
                        k.Select(l2 => l2[i])
                        .Where(c => c == '#').Count()
                    ).ToList());
            }

            var count = 0;
            for(int li = 0; li < locks.Count; ++li)
            {
                var l = locks[li];
                for(int ki  = 0; ki < keys.Count; ++ki)
                {
                    var k = keys[ki];
                    if (l.Zip(k).All((pair) => pair.First + pair.Second <= 7))
                    {
                        count++;
                    }
                }
            }
            Console.WriteLine(count);
        }
    }
}
