using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{
    internal class Day18b
    {


        public int Heuristic((int x, int y) pos, (int x, int y) goal)//, Dictionary<(int x, int y), ((int x, int y) pos, Direction dir)> cameFrom, List<int[]> map)
        {
            return (goal.x - pos.x) + (goal.y - pos.y);
        }

        private List<(int x, int y)> Reconstruct((int x, int y) current, Dictionary<(int x, int y), (int x, int y)> cameFrom)
        {
            var path = new List<(int x, int y)>();
            while (cameFrom.ContainsKey(current))
            {
                path.Add(current);

                current = cameFrom[current];
            }

            return path;
        }

        public List<(int x, int y)> AStar((int x, int y) start, (int x, int y) g, char[][] map)
        {
            /*
             * /*
             * function A_Star(start, goal, h)
        // The set of discovered nodes that may need to be (re-)expanded.
        // Initially, only the start node is known.
        // This is usually implemented as a min-heap or priority queue rather than a hash-set.
        openSet := {start}*/
            var openSet = new PriorityQueue<(int x, int y), int>();
            openSet.Enqueue((start.x, start.y), 0);


            var cameFrom = new Dictionary<(int x, int y), (int x, int y)>();
            /*
        // For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from the start
        // to n currently known.
        cameFrom := an empty map
            */

            var gScore = new Dictionary<(int x, int y), int>();
            gScore[start] = 0;
            /*
        // For node n, gScore[n] is the cost of the cheapest path from start to n currently known.
        gScore := map with default value of Infinity
        gScore[start] := 0
            */

            var fScore = new Dictionary<(int x, int y), int>();
            fScore[start] = Heuristic(start, g);//, cameFrom, map);

            /*
            // For node n, fScore[n] := gScore[n] + h(n). fScore[n] represents our current best guess as to
            // how cheap a path could be from start to finish if it goes through n.
            fScore := map with default value of Infinity
        fScore[start] := h(start)
            */

            while (openSet.Count > 0)
            {
                // while openSet is not empty

                // This operation can occur in O(Log(N)) time if openSet is a min-heap or a priority queue
                //current := the node in openSet having the lowest fScore[] value

                var current = openSet.Dequeue();

                if ((current.x, current.y) == g)
                {
                    return Reconstruct(current, cameFrom);
                }
                /*
            if current = goal
                return reconstruct_path(cameFrom, current)
                */

                var next = new List<(int x, int y)>();
                if ( current.x > 0 ) next.Add((current.x - 1, current.y));
                if ( current.y > 0 ) next.Add((current.x, current.y - 1));
                if ( current.x < map[0].Length - 1) next.Add((current.x + 1, current.y));
                if (current.y < map.Length - 1) next.Add((current.x, current.y + 1));

                next = next.Where(n => map[n.y][n.x] == '.').ToList();

                foreach (var n in next)
                {
                    //for each neighbor of current
                    // d(current,neighbor) is the weight of the edge from current to neighbor
                    // tentative_gScore is the distance from start to the neighbor through current
                    // tentative_gScore := gScore[current] + d(current, neighbor)
                    var tentative_gScore = gScore.GetValueOrDefault((current.x, current.y), int.MaxValue) + 1;

                    //if tentative_gScore < gScore[neighbor]
                    if (tentative_gScore < gScore.GetValueOrDefault((n.x, n.y), int.MaxValue))
                    {
                        // This path to neighbor is better than any previous one. Record it!
                        //cameFrom[neighbor] := current
                        cameFrom[(n.x, n.y)] = (current.x, current.y);
                        //gScore[neighbor] := tentative_gScore
                        gScore[(n.x, n.y)] = tentative_gScore;
                        //fScore[neighbor] := tentative_gScore + h(neighbor)
                        fScore[(n.x, n.y)] = tentative_gScore + Heuristic((n.x, n.y), g);//, cameFrom, map);
                        if (!openSet.UnorderedItems.Any(i => i.Element == (n.x, n.y)))
                        {
                            openSet.Enqueue((n.x, n.y), fScore[(n.x, n.y)]);
                        }
                        //if neighbor not in openSet
                        //    openSet.add(neighbor)
                    }
                }
            }
            // Open set is empty but goal was never reached
            //return failure*/
            return null;
        }

        public void Run()
        {
            
            int w = 71;
            int h = 71;

            var coords = File.ReadAllLines("./2024/Inputs/Day18.txt").Select(l => (x: int.Parse(l.Split(',')[0]), y: int.Parse(l.Split(',')[1]))).ToList();

            for (int j = 1024; j < coords.Count; ++j)
            {
                var map = Enumerable.Range(0, h).Select(i => new string('.', w).ToCharArray()).ToArray();

                for (var i = 0; i < j; ++i)
                {
                    map[coords[i].y][coords[i].x] = '#';
                }

                var path = AStar((0, 0), (70, 70), map);
                if ( path == null )
                {
                    Console.WriteLine($"{coords[j-1].x},{coords[j-1].y}");
                    break;
                }
            }

            return;
        }
    }
}
