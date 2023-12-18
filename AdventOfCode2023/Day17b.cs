using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace AdventOfCode2023
{
    public enum Direction
    {
        Up, Left, Down, Right
    }

    /*
    function reconstruct_path(cameFrom, current)
    total_path := {current}
    while current in cameFrom.Keys:
        current := cameFrom[current]
        total_path.prepend(current)
    return total_path

    // A* finds a path from start to goal.
    // h is the heuristic function. h(n) estimates the cost to reach goal from node n.
    function A_Star(start, goal, h)
        // The set of discovered nodes that may need to be (re-)expanded.
        // Initially, only the start node is known.
        // This is usually implemented as a min-heap or priority queue rather than a hash-set.
        openSet := {start}

        // For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from the start
        // to n currently known.
        cameFrom := an empty map

        // For node n, gScore[n] is the cost of the cheapest path from start to n currently known.
        gScore := map with default value of Infinity
        gScore[start] := 0

        // For node n, fScore[n] := gScore[n] + h(n). fScore[n] represents our current best guess as to
        // how cheap a path could be from start to finish if it goes through n.
        fScore := map with default value of Infinity
        fScore[start] := h(start)

        while openSet is not empty
            // This operation can occur in O(Log(N)) time if openSet is a min-heap or a priority queue
            current := the node in openSet having the lowest fScore[] value
            if current = goal
                return reconstruct_path(cameFrom, current)

            openSet.Remove(current)
            for each neighbor of current
                // d(current,neighbor) is the weight of the edge from current to neighbor
                // tentative_gScore is the distance from start to the neighbor through current
                tentative_gScore := gScore[current] + d(current, neighbor)
                if tentative_gScore < gScore[neighbor]
                    // This path to neighbor is better than any previous one. Record it!
                    cameFrom[neighbor] := current
                    gScore[neighbor] := tentative_gScore
                    fScore[neighbor] := tentative_gScore + h(neighbor)
                    if neighbor not in openSet
                        openSet.add(neighbor)

        // Open set is empty but goal was never reached
        return failure
    */

    internal class Day17b
    {
        private List<(int x, int y)> Reconstruct((int x, int y) current, Dictionary<(int x, int y), ((int x, int y) pos, Direction dir)> cameFrom)
        {
            List<Direction> dirs = new List<Direction>();
            var path = new List<(int x, int y)>();
            while (cameFrom.ContainsKey(current))
            {
                path.Add(current);
                dirs.Add(cameFrom[current].dir);

                current = cameFrom[current].pos;
                
                
            }

            path.Reverse();
            dirs.Reverse();
            foreach(var d in dirs)
            {
                if (d != null)
                {
                    Console.WriteLine(Enum.GetName(typeof(Direction), d));
                }
            }
            return path;
        }

        public int Heuristic((int x, int y) pos, (int x, int y) goal, Dictionary<(int x, int y), ((int x, int y) pos, Direction dir)> cameFrom, List<int[]> map)
        {
            /*
            Dictionary<(int x, int y), ((int x, int y) pos, Direction dir)> fakeCameFrom = new Dictionary<(int x, int y), ((int x, int y) pos, Direction dir)>(cameFrom);

            int score = 0;
            while(pos !=  goal)
            {
                score += map[pos.y][pos.x];

                foreach(var dir in new List<Direction> { Direction.Down, Direction.Right, Direction.Left, Direction.Up }) 
                {
                    if (IsValidDirection(pos, dir, fakeCameFrom, map))
                    {
                        var next = FollowDir(pos, dir);
                        fakeCameFrom[next] = (pos, dir);
                        pos = next;
                        break;
                    }
                }
            }
            return score;
            */

            return 0;// Math.Abs(pos.x - goal.x) + Math.Abs(pos.y - goal.y) - 1;
        }

        private bool IsValidDirection((int x, int y) current, Direction dir, Dictionary<(int x, int y), ((int x, int y) pos, Direction dir)> cameFrom, List<int[]> map)
        {
            var next = FollowDir(current, dir);
            if(!InMap(next.x, next.y, map))
            {
                return false;
            }

            var count = 1;
            while (cameFrom.ContainsKey(current))
            {
                var prev = cameFrom[current];

                if (dir == Direction.Left && prev.dir == Direction.Right)
                {
                    return false;
                }

                if (dir == Direction.Right && prev.dir == Direction.Left)
                {
                    return false;
                }

                if (dir == Direction.Up && prev.dir == Direction.Down)
                {
                    return false;
                }

                if (dir == Direction.Down && prev.dir == Direction.Up)
                {
                    return false;
                }

                if ( prev.dir != dir )
                {
                    return true;
                }
                else
                {
                    count++;
                    // this might be off by 1
                    if ( count > 3)
                    {
                        return false;
                    }
                }

                current = prev.pos;
            }
            

            return true;
        }

        public (int x, int y) FollowDir((int x, int y) pos, Direction dir)
        {
            switch(dir)
            {
                case Direction.Left: return (pos.x - 1, pos.y);
                case Direction.Right: return (pos.x + 1, pos.y);
                case Direction.Up: return (pos.x, pos.y -1);
                case Direction.Down: return (pos.x, pos.y+1);
            }

            throw new Exception("wat");
        }

        public bool InMap(int x, int y, List<int[]> map)
        {
            if (x < 0 || x >= map[0].Length || y < 0 || y >= map.Count)
            {
                return false;
            }
            return true;
        }

        public List<(int x, int y)> AStar((int x, int y) start, (int x, int y) g, List<int[]> map)
        {
            /*
             * /*
             * function A_Star(start, goal, h)
        // The set of discovered nodes that may need to be (re-)expanded.
        // Initially, only the start node is known.
        // This is usually implemented as a min-heap or priority queue rather than a hash-set.
        openSet := {start}*/
            var openSet = new PriorityQueue<((int x, int y) pos, Direction? dir), int>();
            openSet.Enqueue((start, null), 0);


            var cameFrom = new Dictionary<(int x, int y), ((int x, int y), Direction dir)>();
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
            fScore[start] = Heuristic(start, g, cameFrom, map);

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

                if (current.pos == g)
                {
                    return Reconstruct(current.pos, cameFrom);
                }
                /*
            if current = goal
                return reconstruct_path(cameFrom, current)
                */

                var validDirs = new List<Direction>();

                if ((current.dir == null || current.dir != Direction.Left) && IsValidDirection(current.pos, Direction.Right, cameFrom, map))
                {
                    validDirs.Add(Direction.Right);
                }
                if ((current.dir == null || current.dir != Direction.Right) && IsValidDirection(current.pos, Direction.Left, cameFrom, map))
                {
                    validDirs.Add(Direction.Left);
                }
                if ((current.dir == null || current.dir != Direction.Up) && IsValidDirection(current.pos, Direction.Down, cameFrom, map))
                {
                    validDirs.Add(Direction.Down);
                }
                if ((current.dir == null || current.dir != Direction.Down) && IsValidDirection(current.pos, Direction.Up, cameFrom, map))
                {
                    validDirs.Add(Direction.Up);
                }

                //openSet.Remove(current)

                foreach (var dir in validDirs)
                {
                    //for each neighbor of current
                    // d(current,neighbor) is the weight of the edge from current to neighbor
                    // tentative_gScore is the distance from start to the neighbor through current
                    // tentative_gScore := gScore[current] + d(current, neighbor)
                    var neighbor = FollowDir(current.pos, dir);
                    var tentative_gScore = gScore.GetValueOrDefault(current.pos, int.MaxValue) + map[neighbor.y][neighbor.x];

                    //if tentative_gScore < gScore[neighbor]
                    if (tentative_gScore < gScore.GetValueOrDefault(neighbor, int.MaxValue))
                    {
                        // This path to neighbor is better than any previous one. Record it!
                        //cameFrom[neighbor] := current
                        cameFrom[neighbor] = (current.pos, dir);
                        //gScore[neighbor] := tentative_gScore
                        gScore[neighbor] = tentative_gScore;
                        //fScore[neighbor] := tentative_gScore + h(neighbor)
                        fScore[neighbor] = tentative_gScore + Heuristic(neighbor, g, cameFrom, map);
                        if ( !openSet.UnorderedItems.Any(i => i.Element == (neighbor, (Direction?)dir)))
                        {
                            openSet.Enqueue((neighbor, dir), fScore[neighbor]);
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

        public enum Direction
        {
            Up, Left, Down, Right

        }
        class Step
        {
            public (int x, int y) pos;
            public Direction dir;
            public List<Direction> steps = new List<Direction>();
            public HashSet<(int x, int y)> seen = new HashSet<(int x, int y)>();
            public int score = 0;
        }

        public void Part1()
        {
            var map = File.ReadAllLines("Inputs/Day17Example.txt").Select(l => l.ToCharArray().Select(c=>c - '0').ToArray()).ToList();
            //var mins = File.ReadAllLines("Inputs/Day17.txt").Select(l => Enumerable.Repeat(0, l.Length)).ToList();


            var result = AStar((0, 0), (map[0].Length - 1, map.Count - 1), map);

            var score = result.Skip(1).Select(pos => map[pos.y][pos.x]).Sum();

            int g = 56;
        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day17.txt");
        }
    }
}
