using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2023
{
    internal class Day18
    {

        public void Part1()
        {
            var lines = File.ReadAllLines("Inputs/Day18Example5.txt");

            var steps = lines.Select(l => l.Split(' ')).Select(s => (dir: s[0][0], dist: int.Parse(s[1]), Color: s[2])).ToList();

            var nodes = new List<(int x, int y, string color)>();
            nodes.Add((0, 0, steps.First().Color));


            //-102, -11
            //-101, -10
            var minX2 = -101;
            var minY2 = -11;
            bool[,] map2 = new bool[287, 251];

            foreach (var step in steps)
            {
                var last = nodes.Last();
                switch (step.dir)
                {
                    case 'U':
                        for (int i = 1; i <= step.dist; i++)
                        {
                            nodes.Add((last.x, last.y - i, step.Color));
                            //if ( map2[nodes.Last().x - minX2, nodes.Last().y - minY2]) throw new Exception("Dang");
                            //map2[nodes.Last().x - minX2, nodes.Last().y - minY2] = true;
                        }
                        break;
                    case 'D':
                        for (int i = 1; i <= step.dist; i++)
                        {
                            nodes.Add((last.x, last.y + i, step.Color));
                            //if (map2[nodes.Last().x - minX2, nodes.Last().y - minY2]) throw new Exception("Dang");
                            //map2[nodes.Last().x - minX2, nodes.Last().y - minY2] = true;
                        }
                        break;
                    case 'R':
                        for (int i = 1; i <= step.dist; i++)
                        {
                            nodes.Add((last.x + i, last.y, step.Color));
                            //if (map2[nodes.Last().x - minX2, nodes.Last().y - minY2]) throw new Exception("Dang");
                            //map2[nodes.Last().x - minX2, nodes.Last().y - minY2] = true;
                        }
                        break;
                    case 'L':
                        for (int i = 1; i <= step.dist; i++)
                        {
                            nodes.Add((last.x - i, last.y, step.Color));
                            //if (map2[nodes.Last().x - minX2, nodes.Last().y - minY2]) throw new Exception("Dang");
                            //map2[nodes.Last().x - minX2, nodes.Last().y - minY2] = true;
                        }
                        break;
                }
            }

            var minX = nodes.Min(n => n.x);
            var maxX = nodes.Max(n => n.x);

            var minY = nodes.Min(n => n.y);
            var maxY = nodes.Max(n => n.y);
            //287, 251
            bool[,] map = new bool[maxX - minX + 1, maxY - minY + 1];

            foreach (var node in nodes)
            {
                map[node.x - minX, node.y - minY] = true;
            }


            for (int y = 0; y <= maxY - minY; ++y)
            {
                var s = "";
                for (int x = 0; x <= maxX - minX; ++x)
                {
                    if (map[x, y]) s += "#";
                    else s += ".";
                }
                Console.WriteLine(s);
            }

            Console.WriteLine();



            for (int y = 0; y <= maxY - minY; ++y)
            {
                bool inside = false;
                var wallStart = '.';
                for (int x = 0; x <= maxX - minX; ++x)
                {


                    if (y == 200 && x == 261)
                    {
                        int h = 56;
                    }


                    if (map[x, y])
                    {
                        bool topwall = y > 0 && map[x, y - 1];
                        bool bottomwall = y < maxY - minY && map[x, y + 1];

                        var wall = '-';
                        var wallEnd = '.';
                        if (topwall && bottomwall) wall = '|';
                        else if (wallStart != '.')
                        {
                            if (topwall) wallEnd = 'J';
                            else if (bottomwall) wallEnd = '7';
                        }
                        else
                        {
                            if (topwall) wallStart = 'L';
                            else if (bottomwall) wallStart = 'F';
                        }

                        if (wall == '|')
                        {
                            inside = !inside;
                        }
                        else if (wallStart == 'L' && wallEnd == '7')
                        {
                            inside = !inside;
                        }
                        else if (wallStart == 'F' && wallEnd == 'J')
                        {
                            inside = !inside;
                        }

                        if (wall == '|' || wallEnd != '.')
                        {
                            wallStart = '.';
                        }
                    }
                    else if (inside)
                    {
                        map[x, y] = true;
                    }

                    //if ( map[x,y] ) Console.Write("#");
                    //else Console.Write(".");
                }

                //Console.WriteLine();
            }


            Console.WriteLine();

            for (int y = 0; y <= maxY - minY; ++y)
            {
                var s = "";
                for (int x = 0; x <= maxX - minX; ++x)
                {
                    if (map[x, y]) s += "#";
                    else s += ".";
                }
                Console.WriteLine(s);
            }

            Console.WriteLine(map.Cast<bool>().Count(b => b));

            int g = 56;

        }


        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day18.txt");

            var steps = lines.Select(l => l.Split(' ')[2]).Select(s => (dir: s.Substring(7, 1), dist: Convert.ToInt32(s.Substring(2, 5), 16))).ToList();

            var nodes = new List<(int x, int y)>();
            nodes.Add((0, 0));


            foreach (var step in steps)
            {
                var last = nodes.Last();

                switch (step.dir)
                {
                    case "3": // U
                        nodes.Add((last.x, last.y - step.dist));
                        break;
                    case "1": // D
                        nodes.Add((last.x, last.y + step.dist));
                        break;
                    case "0": // R
                        nodes.Add((last.x + step.dist, last.y));
                        break;
                    case "2": // L
                        nodes.Add((last.x - step.dist, last.y));
                        break;
                }
            }

            long count = Count2(nodes);

            Console.WriteLine(count);

            // ex:    952408144115
            // mine: 1169415959064
        }

        public void Part2b()
        {
            // Same algorthm as Part2, but uses the easier input format

            var lines = File.ReadAllLines("Inputs/Day18.txt");

            var steps = lines.Select(l => l.Split(' ')).Select(s => (dir: s[0], dist: int.Parse(s[1]))).ToList();

            var nodes = new List<(int x, int y)>();
            nodes.Add((0, 0));


            foreach (var step in steps)
            {
                var last = nodes.Last();

                switch (step.dir)
                {
                    case "U": // U
                        nodes.Add((last.x, last.y - step.dist));
                        break;
                    case "D": // D
                        nodes.Add((last.x, last.y + step.dist));
                        break;
                    case "R": // R
                        nodes.Add((last.x + step.dist, last.y));
                        break;
                    case "L": // L
                        nodes.Add((last.x - step.dist, last.y));
                        break;
                }
            }

            long count = Count2(nodes);

            Console.WriteLine(count);
        }

        private static long Count2(List<(int x, int y)> nodes)
        {
            var sorted = nodes.Select((n, i) => (n, i)).OrderBy(nodes => nodes.n.y).ThenBy(nodes => nodes.n.x).ToList();
            var grouped = nodes.Select((n, i) => (n, i)).Skip(1).GroupBy(nodes => nodes.n.y);

            int y = sorted.First().n.y;

            long count2 = 0;

            var nextNodes = new List<((int x, int y) n, int i)>();

            // How many filled points are on any Y with no nodes
            long noNodeCount = 0;

            long lastY = 0;

            foreach (var group in grouped.OrderBy(g => g.Key))
            {
                long rowCount = 0;

                rowCount += noNodeCount * (group.First().n.y - lastY - 1);

                var g = group.ToList();

                char wallStart = '.';
                char wallEnd = '.';
                var nodeS = sorted[0];
                var inside = false;

                foreach (var n in nextNodes)
                {
                    if (!group.Any(gn => gn.n.x == n.n.x))
                    {
                        g.Add(n);
                    }
                }
                nextNodes = new List<((int x, int y) n, int i)>();

                var prev2 = g.OrderBy(n => n.n.x).First();

                int? insideStartX = null;
                foreach (var node in g.OrderBy(n => n.n.x))
                {
                    var prevI = node.i - 1;
                    if (prevI < 0) prevI = sorted.Count - 2; // last one is a copy of the first, skip that one too.

                    var nextI = node.i + 1;
                    if (nextI >= sorted.Count) nextI = 1; // first one is a copy of the last, skip that one too.

                    var prev = nodes[prevI];
                    var next = nodes[nextI];

                    if (node.i < 0 || (node.n.y == prev.y && node.n.y == next.y))
                    {
                        nextNodes.Add((p: (node.n.x, node.n.y + 1), i: -1));
                        inside = !inside;

                        if (inside)
                        {
                            insideStartX = node.n.x;
                        }
                        else
                        {
                            rowCount += node.n.x - insideStartX.Value + 1;
                            insideStartX = null;
                        }
                    }
                    else if (wallStart == '.')
                    {
                        nodeS = node;

                        if (!insideStartX.HasValue)
                        {
                            insideStartX = node.n.x;
                        }

                        if ((node.n.y > prev.y && node.n.x < next.x) || (node.n.y > next.y && node.n.x < prev.x))
                        {
                            wallStart = 'L';
                        }
                        else if ((node.n.y < prev.y && node.n.x < next.x) || (node.n.y < next.y && node.n.x < prev.x))
                        {
                            wallStart = 'F';
                            nextNodes.Add((p: (node.n.x, node.n.y + 1), i: -1));
                        }
                        else
                        {
                            throw new Exception("Dang");
                        }
                    }
                    else if (wallStart != '.')
                    {

                        var prevInside = inside;

                        if ((prev.y < node.n.y && next.x < node.n.x) || (next.y < node.n.y && prev.x < node.n.x))
                        {
                            wallEnd = 'J';

                            if (wallStart == 'F')
                            {
                                inside = !inside;
                            }
                        }
                        else if ((node.n.y < prev.y && node.n.x > next.x) || (node.n.y < next.y && node.n.x > prev.x))
                        {
                            wallEnd = '7';
                            nextNodes.Add((p: (node.n.x, node.n.y + 1), i: -1));

                            if (wallStart == 'L')
                            {
                                inside = !inside;
                            }
                        }
                        else
                        {
                            throw new Exception("Dang");
                        }

                        if (!prevInside && !inside)
                        {
                            // add the wall edge
                            rowCount += node.n.x - nodeS.n.x + 1;
                            insideStartX = null;
                        }

                        if (prevInside && !inside)
                        {
                            rowCount += node.n.x - insideStartX.Value + 1;
                            insideStartX = null;
                        }

                        if (!prevInside && inside && !insideStartX.HasValue)
                        {
                            insideStartX = node.n.x + 1;
                        }
                    }

                    if (wallEnd != '.')
                    {
                        wallStart = '.';
                        wallEnd = '.';
                    }

                    prev2 = node;
                }

                Console.WriteLine(rowCount);
                count2 += rowCount;

                lastY = group.First().n.y;

                if (nextNodes.Count % 2 != 0)
                {
                    throw new Exception("Dang");
                }

                noNodeCount = 0;
                for (int i = 0; i < nextNodes.Count; i += 2)
                {
                    noNodeCount += nextNodes[i + 1].n.x - nextNodes[i].n.x + 1;
                }
            }
            Console.WriteLine(count2);

            return count2;
        }
    }
}