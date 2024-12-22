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
    internal class Day22a
    {
        public void Run()
        {
            var initials = File.ReadAllLines("./2024/Inputs/Day22.txt").Select(l => long.Parse(l)).ToArray();

            long sum = 0;
            foreach (var initial in initials)
            {
                var s = initial;
                for (var i = 0; i < 2000; ++i)
                {
                    s = ((s * 64) ^ s) % 16777216;
                    s = ((s / 32) ^ s) % 16777216;
                    s = ((s * 2048) ^ s) % 16777216;
                }

                sum += s;

                Console.WriteLine($"{initial}: {s}");
            }

            Console.WriteLine(sum);
        }
    }
}
