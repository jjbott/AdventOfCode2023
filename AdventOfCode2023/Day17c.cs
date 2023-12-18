using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023
{
    internal class Day17c
    {
        public class Node
        {
            public int Cost;
            public (int x, int y) Pos;
            public (int x, int y) DPos;
            public int S;

        }

        public int Ugh(int minSteps, int maxSteps, List<int[]> map)
        {
            var end = (map[0].Length - 1, map.Count - 1);

            var queue = new PriorityQueue<Node, int>();
            queue.Enqueue(new Node { Cost = map[1][0], Pos = (0, 1), DPos = (0, 1), S = 0 }, map[1][0]);
            queue.Enqueue(new Node { Cost = map[0][1], Pos = (1, 0), DPos = (1, 0), S = 0 }, map[0][1]);

            var visited = new HashSet<((int x, int y) pos, (int x, int y) dpos, int steps)>();

            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                if ( c.Pos ==  end)
                {
                    return c.Cost;
                }

                if ( visited.Contains((c.Pos, c.DPos, c.S)) )
                {
                    continue;
                }
                visited.Add((c.Pos, c.DPos, c.S));

                if (c.S < (maxSteps - 1) && InMap(c.Pos.x + c.DPos.x, c.Pos.y + c.DPos.y, map))
                {
                    var next = (x: c.Pos.x + c.DPos.x, y: c.Pos.y + c.DPos.y);
                    var nextCost = c.Cost + map[next.y][next.x];
                    queue.Enqueue(new Node { Cost = nextCost, Pos = next, DPos = c.DPos, S = c.S + 1 }, nextCost);
                }

                if ( minSteps <= c.S)
                {
                    var lx = c.DPos.y;
                    var ly = -c.DPos.x;
                    var rx = -c.DPos.y;
                    var ry = c.DPos.x;
                    var lPos = (x: c.Pos.x + lx, y: c.Pos.y + ly);
                    var rPos = (x: c.Pos.x + rx, y: c.Pos.y + ry);
                    
                    if ( InMap(lPos.x, lPos.y, map))
                    {
                        var nextCost = c.Cost + map[lPos.y][lPos.x];
                        queue.Enqueue(new Node { Cost = nextCost, Pos = lPos, DPos = (lx, ly), S = 0 }, nextCost);
                    }

                    if (InMap(rPos.x, rPos.y, map))
                    {
                        var nextCost = c.Cost + map[rPos.y][rPos.x];
                        queue.Enqueue(new Node { Cost = nextCost, Pos = rPos, DPos = (rx, ry), S = 0 }, nextCost);
                    }
                }
            }

            return -1;
        }

        // Definitely not swiped from Reddit, thats for sure.
        // Certainly not from https://github.com/zivnadel/advent-of-code/blob/eb3626dff4019b25fa3dba82718360dc643dbe79/2023/python/day17.py
        /*
        def sol(min_steps, max_steps):
    q = [(grid[(1, 0)], (1, 0), (1, 0), 0), (grid[(0, 1)], (0, 1), (0, 1), 0)]
    visited, end = set(), max(grid)
    while q:
        heat, (x, y), (dx, dy), steps = heapq.heappop(q)
        if (x, y) == end and min_steps <= steps:
            return heat
        if ((x, y), (dx, dy), steps) in visited:
            continue
        visited.add(((x, y), (dx, dy), steps))
        if steps < (max_steps - 1) and (x + dx, y + dy) in grid:
            s_pos = (x + dx, y + dy)
            heapq.heappush(q, (heat + grid[s_pos], s_pos, (dx, dy), steps + 1))
        if min_steps <= steps:
            lx, ly, rx, ry = dy, -dx, -dy, dx
            l_pos, r_pos = (x + lx, y + ly), (x + rx, y + ry)
            for xx, yy, pos in zip((lx, rx), (ly, ry), (l_pos, r_pos)):
                if pos in grid:
                    heapq.heappush(q, (heat + grid[pos], pos, (xx, yy), 0))
        */




        public bool InMap(int x, int y, List<int[]> map)
        {
            if (x < 0 || x >= map[0].Length || y < 0 || y >= map.Count)
            {
                return false;
            }
            return true;
        }

        public void Part1()
        {
            var map = File.ReadAllLines("Inputs/Day17.txt").Select(l => l.ToCharArray().Select(c=>c - '0').ToArray()).ToList();


            var result = Ugh(0, 3, map);

            int g = 56;
        }

        public void Part2()
        {
            var map = File.ReadAllLines("Inputs/Day17.txt").Select(l => l.ToCharArray().Select(c => c - '0').ToArray()).ToList();


            var result = Ugh(3, 10, map);

            int g = 56;
        }
    }
}
