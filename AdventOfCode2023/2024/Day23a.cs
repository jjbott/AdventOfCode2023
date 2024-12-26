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
    internal class Day23a
    {


        public void Run()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day23.txt");

            var connections = lines.Select(l => (a: l.Substring(0, 2), b: l.Substring(3))).ToList();

            var nodes = connections.Select(c => c.a).Union(connections.Select(c => c.b)).Distinct().ToList();

            var network = nodes.ToDictionary(n => n, n => connections.Where(c => c.a == n).Select(c => c.b).Union(connections.Where(c => c.b == n).Select(c => c.a)).Distinct().ToList());

            int g = 56;

            int count = 0;
            HashSet<string> loops = new();

            foreach(var node in network.Where(kv => kv.Key.StartsWith("t")))
            {
                var queue = new Queue<(string first, HashSet<string> n, string next)>();
                foreach(var n in node.Value)
                {
                    queue.Enqueue((node.Key, new HashSet<string> {node.Key }, n));
                }

                while(queue.TryDequeue(out var q))
                {
                    if ( q.first == q.next && q.n.Count == 3) 
                    {
                        var loop = string.Join("", q.n.OrderBy(n => n));
                        if ( !loops.Contains(loop) )
                        {
                            loops.Add(loop);
                        }

                        continue;
                    }
                    else if ( q.n.Count > 3 || q.n.Contains(q.next))
                    {
                        continue;
                    }
                    else
                    {
                        q.n.Add(q.next);
                        foreach(var n in network[q.next])
                        {
                            queue.Enqueue((q.first, new HashSet<string>(q.n), n));
                        }
                    }
                }

            }
            

            Console.WriteLine(loops.Count);
        }
    }
}
