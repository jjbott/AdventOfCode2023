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
    internal class Day23b
    {
        private HashSet<string> seen = new();
       
        public List<HashSet<string>>? FindLargestSet(HashSet<string> set, Dictionary<string, HashSet<string>> network)
        {
            var key = string.Join("", set.OrderBy(x => x));
            if ( seen.Contains(key) )
            { 
                return null;
            }

            seen.Add(key);

            // find candidates
            var candidates = new HashSet<string>();
            foreach (var n in set)
            {
                foreach (var next in network[n].Where(next => !set.Contains(next) && !candidates.Contains(next)))
                {
                    if (set.All(s => network[s].Contains(next)))
                    {
                        candidates.Add(next);
                    }
                }
            }

            if (!candidates.Any())
            {
                return new List<HashSet<string>> { set };
            }

            var nextSets = candidates.Select(s =>
            {
                var nextSet = new HashSet<string>(set) { s };
                return FindLargestSet(nextSet, network);
            })
                .Where(nextSet => nextSet != null)
                .SelectMany(nextSet => nextSet)
                .ToList();

            return nextSets;
        }

        public void Run()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day23.txt");

            var connections = lines.Select(l => (a: l.Substring(0, 2), b: l.Substring(3))).ToList();

            var nodes = connections.Select(c => c.a).Union(connections.Select(c => c.b)).Distinct().ToList();

            var network = nodes.ToDictionary(n => n, n => connections.Where(c => c.a == n).Select(c => c.b).Union(connections.Where(c => c.b == n).Select(c => c.a)).Distinct().ToHashSet());

            HashSet<string>? largest = null;
            foreach (var node in nodes)
            {
                var sets = FindLargestSet(new HashSet<string> { node }, network);
                if (sets?.Any() == true)
                {
                    var l = sets.OrderByDescending(x => x.Count).First();
                    if (largest == null || l.Count > largest.Count)
                    {
                        largest = l;
                    }
                }       
            }

            Console.WriteLine(string.Join(",", largest.OrderBy(x => x)));
            
        }
    }
}
