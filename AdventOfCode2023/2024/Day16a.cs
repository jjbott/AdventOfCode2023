using AOC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{

    internal class Day16a
    {
        public IEnumerable<(int x, int y, char dir, int score)> Next(char[][] map, int x, int y, char dir/*, int score*/)
        {
            var score = 0;
            var next = new List<(int x, int y, char dir, int score)>();

            switch (dir)
            {
                case '>':
                    next.Add((x + 1, y, dir, score + 1));
                    next.Add((x, y, '^', score + 1000));
                    next.Add((x, y, 'v', score + 1000));
                    break;
                case 'v':
                    next.Add((x, y + 1, dir, score + 1));
                    next.Add((x, y, '>', score + 1000));
                    next.Add((x, y, '<', score + 1000));
                    break;
                case '<':
                    next.Add((x-1, y, dir, score + 1));
                    next.Add((x, y, '^', score + 1000));
                    next.Add((x, y, 'v', score + 1000));
                    break;
                case '^':
                    next.Add((x, y-1, dir, score + 1));
                    next.Add((x, y, '>', score + 1000));
                    next.Add((x, y, '<', score + 1000));
                    break;
            }

            return next.Where(n =>
                n.x >= 0
                && n.x <= map[0].Length
                && n.y >= 0
                && n.y <= map.Length
                && map[n.y][n.x] != '#')
                .ToList();
        }

        public int Heuristic((int x, int y, char dir) pos, (int x, int y) goal)//, Dictionary<(int x, int y), ((int x, int y) pos, Direction dir)> cameFrom, List<int[]> map)
        {
            var score = 0;
            if ((pos.x, pos.y) == goal) return 0;

            if (pos.x == goal.x)
            {
                score += goal.x - pos.x;
                switch(pos.dir)
                {
                    case '<': score += 2000; break;
                    case '^':
                    case 'v':
                        score += 1000; break;
                }
                return score;
            }
            if (pos.y == goal.y)
            {
                score += pos.y - goal.y;
                switch (pos.dir)
                {
                    case '<':
                    case '>':
                        score += 1000; break;
                    case 'v':
                        score += 2000; break;
                }
                return score;
            }
            else
            {
                score += goal.x - pos.x;
                score += pos.y - goal.y;
                score += 1000; switch (pos.dir)
                {
                    case '<':
                    case 'v':
                        score += 1000; break;
                }

            }

            return score;
                
        }

        private List<(int x, int y, char dir)> Reconstruct((int x, int y, char dir) current, Dictionary<(int x, int y, char dir), (int x, int y, char dir)> cameFrom)
        {
            var path = new List<(int x, int y, char dir)>();
            while (cameFrom.ContainsKey(current))
            {
                path.Add(current);

                current = cameFrom[current];
            }

            return path;
        }

        public List<(int x, int y, char dir)> AStar((int x, int y, char dir) start, (int x, int y) g, char[][] map)
        {
            /*
             * /*
             * function A_Star(start, goal, h)
        // The set of discovered nodes that may need to be (re-)expanded.
        // Initially, only the start node is known.
        // This is usually implemented as a min-heap or priority queue rather than a hash-set.
        openSet := {start}*/
            var openSet = new PriorityQueue<(int x, int y, char dir), int>();
            openSet.Enqueue((start.x, start.y, start.dir), 0);


            var cameFrom = new Dictionary<(int x, int y, char dir), (int x, int y, char dir)>();
            /*
        // For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from the start
        // to n currently known.
        cameFrom := an empty map
            */

            var gScore = new Dictionary<(int x, int y, char dir), int>();
            gScore[start] = 0;
            /*
        // For node n, gScore[n] is the cost of the cheapest path from start to n currently known.
        gScore := map with default value of Infinity
        gScore[start] := 0
            */

            var fScore = new Dictionary<(int x, int y, char dir), int>();
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

                var next = Next(map, current.x, current.y, current.dir/*, current.score*/);

                foreach (var n in next)
                {
                    //for each neighbor of current
                    // d(current,neighbor) is the weight of the edge from current to neighbor
                    // tentative_gScore is the distance from start to the neighbor through current
                    // tentative_gScore := gScore[current] + d(current, neighbor)
                    var tentative_gScore = gScore.GetValueOrDefault((current.x, current.y, current.dir), int.MaxValue) + n.score;

                    //if tentative_gScore < gScore[neighbor]
                    if (tentative_gScore < gScore.GetValueOrDefault((n.x, n.y, n.dir), int.MaxValue))
                    {
                        // This path to neighbor is better than any previous one. Record it!
                        //cameFrom[neighbor] := current
                        cameFrom[(n.x, n.y, n.dir)] = (current.x, current.y, current.dir);
                        //gScore[neighbor] := tentative_gScore
                        gScore[(n.x, n.y, n.dir)] = tentative_gScore;
                        //fScore[neighbor] := tentative_gScore + h(neighbor)
                        fScore[(n.x, n.y, n.dir)] = tentative_gScore + Heuristic((n.x, n.y, n.dir), g);//, cameFrom, map);
                        if (!openSet.UnorderedItems.Any(i => i.Element == (n.x, n.y, n.dir)))
                        {
                            openSet.Enqueue((n.x, n.y, n.dir), fScore[(n.x, n.y, n.dir)]);
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
            var map = File.ReadAllLines("./2024/Inputs/Day16.txt").Select(l => l.ToCharArray()).ToArray();

            char dir = '>';
            (int x, int y) pos = (0,0);
            (int x, int y) end = (0, 0);

            for (int y = 0; y < map.Length; ++y)
            {
                for (int x = 0; x < map[0].Length; ++x)
                {
                    if (map[x][y] == 'S')
                    {
                        pos.x = x;
                        pos.y = y;
                        break;
                    }
                    else if (map[x][y] == 'E')
                    {
                        end.x = x;
                        end.y = y;
                        break;
                    }
                }
            }

            var result = AStar((pos.x, pos.y, '>'), end, map);
            result.Reverse();
            result.Insert(0, (pos.x, pos.y, '>'));

            var score = 0;
            var c = result[0];
            for(int i = 1; i < result.Count; ++i)
            {
                if (result[i-1].dir == result[i].dir) score++;
                else score += 1000;
            }

            Console.WriteLine();
        }
        
        void Print(char[][] map) 
        {
            for(int y = 0; y < map.Length; y++)
            {
                Console.WriteLine(new string(map[y]));
            }
        }
    }
}


