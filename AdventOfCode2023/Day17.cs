using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace AdventOfCode2023
{
    internal class Day17
    {
        public enum Direction
        {
            Up, Left, Down, Right

        }
        class Step
        {
            public (int x, int y) pos;
            public Direction dir;
            public List<Direction> steps = new List<Direction>();
            public List<(int x, int y)> seen = new List<(int x, int y)>();
            public int score = 0;
        }

        /*
        public IEnumerable<((int x, int y) pos, Direction dir)> NextMin(int x, int y, Direction dir, List<List<List<(Direction dir, int score)>>> mins)
        {
            while (true)
            {
                switch (dir)
                {
                    case Direction.Left: x -= 1; break;
                    case Direction.Right: x += 1; break;
                    case Direction.Up: y -= 1; break;
                    case Direction.Down: x += 1; break;
                }

                if (x < 0 || x >= mins[0].Count || y < 0 || y >= mins.Count)
                {
                    yield break;
                }

                var min = mins[y][x];
                if (min == null || !min.Any())
                {
                    yield break;
                }

                yield return ((x,y), min.)
            }
        }
        */

        public bool IsValid(IEnumerable<Direction> path)
        {
            var count = 1;
            Direction dir = path.First();
            var prev = dir;
            foreach (var s in path.Skip(1))
            {
                if (s == Direction.Left && prev == Direction.Right)
                {
                    return false;
                }

                if (s == Direction.Right && prev == Direction.Left)
                {
                    return false;
                }

                if (s == Direction.Up && prev == Direction.Down)
                {
                    return false;
                }

                if (s == Direction.Down && prev == Direction.Up)
                {
                    return false;
                }


                if (dir == s)
                {
                    count++;
                    if (count > 3)
                    {
                        return false;
                    }
                }
                else
                {
                    count = 1;
                    dir = s;
                }

                prev = s;
            }
            return true;
        }

        public bool InMap(int x, int y, List<char[]> map)
        {
            if (x < 0 || x >= map[0].Length || y < 0 || y >= map.Count)
            {
                return false;
            }
            return true;
        }

        public bool DoesntTouch(List<(int x, int y)> path)
        {
            // Path should never loop around and touch itself. That'd indicate shortcut that we missed

            // GOsh I hope this is true. The 3 consecutive rule could potentially mess that up.

            // Assuming the last position is the only one we need to check

            // decided loops _might_ be possible, so disabling for now

            return true;

            if (path.Count > 2)
            {
                int x = path.Last().x;
                int y = path.Last().y;

                foreach (var s in path.Take(path.Count - 2))
                {
                    if (s.x == x && Math.Abs(s.y - y) <= 1)
                    {
                        return false;
                    }
                    if (s.y == y && Math.Abs(s.x - x) <= 1)
                    {
                        return false;
                    }
                }
            }

            return true;

        }

        public bool IsLooping(List<(int x, int y)> path)
        {
            var lastPos = path.Last();
            for (int i = 3; i < path.Count - 1; i++)
            {
                if (path[i] == lastPos
                    && path[i - 1] == path[path.Count - 2]
                    && path[i - 2] == path[path.Count - 3]
                    && path[i - 3] == path[path.Count - 4])
                {
                    return true;
                }
            }
            return false;
        }

        private List<Direction> testPath = new List<Direction>
        { 
            Direction.Right,
            Direction.Right,
            Direction.Down,/*
            Direction.Right,
            Direction.Right,
            Direction.Right,
            Direction.Up,
            Direction.Right,
            Direction.Right,
            Direction.Right,
            Direction.Down,
            Direction.Down,
            Direction.Right,
            Direction.Right,
            Direction.Down,
            Direction.Down,
            Direction.Right,
            Direction.Down,
            Direction.Down,
            Direction.Down,
            Direction.Right,
            Direction.Down,
            Direction.Down,
            Direction.Down,
            Direction.Left,
            Direction.Down,
            Direction.Down,
            Direction.Right*/
        };


        public (bool success, int score, List<Direction> steps/*, List<(int x, int y)> path*/) CalcMinimum(int x, int y, List<Direction> previousSteps, List<(int x, int y)> path, List<char[]> map, List<List<List<(List<Direction> path, int score, int limit)>>> mins, int limit = int.MaxValue)
        {
            if ( previousSteps.Count == testPath.Count )
            {
                bool found = true;
                for(int i = 0; i < previousSteps.Count; i++)
                {
                    if (previousSteps[i] != testPath[i])
                    {
                        found = false;
                        break;
                    }
                }
                if ( found)
                {
                    int g = 56;
                }
            }

            if (limit <= 0)
            {
                return (false, 0, null);
            }

            var min = mins[y][x] ?? new List<(List<Direction> path, int score, int limit)>();
            
            if ( x == map[0].Length -1 && y == map.Count -1)
            {
                return (true, 0, new List<Direction>());
                //return (true, map[y][x] - '0', new List<Direction>());//, new List<(int x, int y)> { (x,y) });
            }

            // find valid existing path
            if (!min.Any(m => m.limit <= limit && m.score >= 0) && min.Any(m => m.limit >= limit && m.score < 0))
            {
                // impossible path, I hope
                return (false, 0, null);
            }
            foreach(var m in min.Where(m => m.limit <= limit && m.score >= 0))
            {
                var fullPath = new List<Direction>(previousSteps);
                fullPath.AddRange(m.path.Take(3));
                if ( IsValid(fullPath) )
                {
                    return (true, m.score, m.path);
                }
            }       

            // no known good path yet

            var newMins = new List<(List<Direction> path, int score)>();

            var bestScore = limit;

            void FollowPath(int newX, int newY, Direction dir)
            {
                var newSteps = new List<Direction>(previousSteps);
                newSteps.Add(dir);
                // Removed `!path.Contains((newX, newY))` because loops are possible. I hope that doesnt screw up everything 
                // Changed my mind. I dont think loops are possible anymore
                if (InMap(newX, newY, map) && (newSteps.Count < 4 || IsValid(newSteps.Skip(newSteps.Count - 4))) && !path.Contains((newX, newY)))
                {
                    var newPath = new List<(int x, int y)>(path);
                    newPath.Add((newX, newY));

                    if (!IsLooping(newPath))
                    {
                        var newPosScore = map[newY][newX] - '0';
                        var newMin = CalcMinimum(newX, newY, newSteps, newPath, map, mins, limit - newPosScore);
                        if (newMin.success)
                        {
                            //var newScore = newMin.score + (map[newY][newX] - '0'); // this is wrong I think. No it wasnt.
                            var newScore = newMin.score + newPosScore;
                            if (newScore < bestScore)
                            {
                                bestScore = newScore;
                                var minPath = new List<Direction> { dir };
                                minPath.AddRange(newMin.steps);
                                newMins.Add((minPath, newScore));
                            }
                        }
                    }
                }
            }

            if (!previousSteps.Any() || previousSteps.Last() != Direction.Left)
            {
                FollowPath(x + 1, y, Direction.Right);
            }

            if (!previousSteps.Any() || previousSteps.Last() != Direction.Up)
            {
                FollowPath(x, y + 1, Direction.Down);
            }

            if (!previousSteps.Any() || previousSteps.Last() != Direction.Right)
            {
                FollowPath(x - 1, y, Direction.Left);
            }

            if (!previousSteps.Any() || previousSteps.Last() != Direction.Down)
            {
                FollowPath(x, y - 1, Direction.Up);
            }

            if (newMins.Count == 0)
            {
                /*
                if (!previousSteps.Any() || previousSteps.Last() != Direction.Right)
                {
                    FollowPath(x - 1, y, Direction.Left);
                }

                if (!previousSteps.Any() || previousSteps.Last() != Direction.Left)
                {
                    FollowPath(x + 1, y, Direction.Right);
                }

                if (!previousSteps.Any() || previousSteps.Last() != Direction.Up)
                {
                    FollowPath(x, y + 1, Direction.Down);
                }

                if (!previousSteps.Any() || previousSteps.Last() != Direction.Down)
                {
                    FollowPath(x, y - 1, Direction.Up);
                }
                */
                min.Add((new List<Direction>(), -1, limit));
                return (false, 0, null);
            }

            var winner = newMins.OrderBy(m => m.score).First();

            min.Add((winner.path, winner.score, limit));
            min = min.OrderBy(m => m.score).ToList();
            mins[y][x] = min;
            
            return (true, winner.score, winner.path); 
        }

        public void Part1()
        {
            var map = File.ReadAllLines("Inputs/Day17.txt").Select(l => l.ToCharArray()).ToList();

            var seen = new HashSet<(int x, int y)> { (0, 0) };
            var queue = new List<Step> { new Step() { pos = (1, 0), dir = Direction.Right }, new Step() { pos = (0, 1), dir = Direction.Down } };

            // var mins = new List<List<(Direction dir, int score)>>();

            var mins = map.Select(l => l.Select(c => new List<(List<Direction> path, int score, int limit)>()).ToList()).ToList();

            var baselineScore = 0;
            var x = 0;
            for(int y = 0; y < map.Count; y++)
            {
                if (y != 0)
                {
                    baselineScore += map[y][x] - '0';
                }
                if(x < map[y].Length - 1)
                {
                    ++x;
                    baselineScore += map[y][x] - '0';
                }
            }

            var min1 = CalcMinimum(0, 0, new List<Direction>(), new List<(int x, int y)> { (0, 0) }, map, mins, baselineScore);
            int g = 56;
            /*
            Step bestPath = null;

            while(queue.Any())
            {
                var current = queue[0];
                queue.RemoveAt(0);

                if (current.pos.x < 0 || current.pos.x >= map[0].Length || current.pos.y < 0 || current.pos.y >= map.Count)
                {
                    continue;
                }

                

                if ( current.seen.Contains(current.pos) )
                {
                    continue;
                }

                if ( current.steps.Count >= 3 && current.steps.Skip(current.steps.Count - 3).Distinct().Count() == 1 && current.steps.Last() == current.dir)
                {
                    // more than 3 in a straight line
                    continue;
                }

                current.steps.Add(current.dir);
                current.seen.Add(current.pos);
                current.score += (map[current.pos.y][current.pos.x] - '0');

                if (current.pos.x == map[0].Length - 1 && current.pos.y == map.Count - 1)
                {

                    if (bestPath == null || current.score < bestPath.score)
                    {
                        bestPath = current;

                        var score = 0;
                        for(int i = current.seen.Count - 1; i >= 0; --i)
                        {
                            score += (map[current.seen[i].y][current.seen[i].x] - '0');
                            mins[current.steps[i]][current.seen[i].y][current.seen[i].x] = score;
                        }
                    }

                    continue;
                }

                if (current.score >= (bestPath?.score ?? int.MaxValue))
                {
                    continue;
                }

                int? min = mins[current.dir][current.pos.y][current.pos.x];
                if (min != null)
                {
                    if (min + current.score >= bestPath.score)
                    {
                        // Not done, but already on a bad path
                        continue;
                    }
                    // if it's less, wait until the path gets to the finish to recalc
                }



                

                if (current.dir != Direction.Right)
                {
                    var s = new Step() {seen = new List<(int x, int y)>(current.seen), pos = (current.pos.x - 1, current.pos.y), dir = Direction.Left, steps = new List<Direction>(current.steps), score = current.score};
                    queue.Insert(0, s);
                }

                if (current.dir != Direction.Down)
                {
                    var s = new Step() { seen = new List<(int x, int y)>(current.seen), pos = (current.pos.x, current.pos.y - 1), dir = Direction.Up, steps = new List<Direction>(current.steps), score = current.score };

                    queue.Insert(0, s);
                }

                if (current.dir != Direction.Left)
                {
                    var s  = new Step() { seen = new List<(int x, int y)>(current.seen), pos = (current.pos.x + 1, current.pos.y), dir = Direction.Right, steps = new List<Direction>(current.steps), score = current.score};
                
                    queue.Insert(0,s);
                }

                if (current.dir != Direction.Up)
                {
                    var s = new Step() { seen = new List<(int x, int y)>(current.seen), pos = (current.pos.x, current.pos.y+1), dir = Direction.Down, steps = new List<Direction>(current.steps), score = current.score};

                    queue.Insert(0, s);
                }

                
            }
            */

            g = 56;
        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day17.txt");
        }
    }
}
