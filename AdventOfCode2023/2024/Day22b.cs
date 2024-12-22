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
    internal class Day22b
    {
        public void Run()
        {
            var initials = File.ReadAllLines("./2024/Inputs/Day22.txt").Select(l => long.Parse(l)).ToArray();

            List<Dictionary<(long a, long b, long c, long d), int>> sequences = new();

            foreach (var initial in initials)
            {
                Dictionary<(long a, long b, long c, long d), int> sequence = new();

                List<(long diff, int digit)> diffs = new();

                var s = initial;
                var s0 = s;
                for (var i = 0; i < 2000; ++i)
                {
                    s = ((s * 64) ^ s) % 16777216;
                    s = ((s / 32) ^ s) % 16777216;
                    s = ((s * 2048) ^ s) % 16777216;

                    diffs.Add(((int)(s % 10) - (int)(s0 % 10), (int)(s % 10)));
                    s0 = s;
                }

                for (var i = 0; i < diffs.Count - 3; ++i)
                {
                    var t = (diffs[i].diff, diffs[i+1].diff, diffs[i+2].diff, diffs[i+3].diff);
                    if ( !sequence.ContainsKey(t) )
                    {
                        sequence[t] = diffs[i + 3].digit;
                    }
                }

                sequences.Add(sequence);

                Console.WriteLine($"{initial}: {sequence.Count}");
            }

            Dictionary<(long a, long b, long c, long d), int> globalCounts = new();

            foreach ( var s in sequences )
            {
                foreach(var kv in s)
                {
                    if (!globalCounts.ContainsKey(kv.Key))
                    {
                        globalCounts[kv.Key] = 0;
                    }

                    globalCounts[kv.Key]++;
                }
            }

            foreach (var s in sequences)
            {
                foreach (var kv in s.ToList())
                {
                    if (globalCounts[kv.Key] == 1)
                    {
                        s.Remove(kv.Key);
                    }
                }

                Console.WriteLine($"{s.Count}");
            }

            sequences = sequences.Where(s => s.Count > 0).ToList();

            // recalc counts now that we've removed sequences that will never add value

            globalCounts = new Dictionary<(long a, long b, long c, long d), int>();

            foreach (var s in sequences)
            {
                foreach (var kv in s)
                {
                    if (!globalCounts.ContainsKey(kv.Key))
                    {
                        globalCounts[kv.Key] = 0;
                    }

                    globalCounts[kv.Key]++;
                }
            }

            (long a, long b, long c, long d) maxKey = (0, 0, 0, 0);
            int maxValue = 0;

            foreach (var kv in globalCounts.OrderByDescending(kv => kv.Value))
            {
                if (maxValue >= (kv.Value * 9))
                {
                    // Cant get a higher value anymore. Were done!
                    break;
                }

                int sum = 0;
                foreach(var s in sequences)
                {
                    foreach(var diff in s)
                    {
                        if ( diff.Key == kv.Key)
                        {
                            sum += diff.Value;
                            break;
                        }
                    }
                }
                if ( sum > maxValue)
                {
                    maxValue = sum;
                    maxKey = kv.Key;

                    Console.WriteLine($"{maxKey} {maxValue}");
                }
            }

            int g = 56;
        }
    }
}
