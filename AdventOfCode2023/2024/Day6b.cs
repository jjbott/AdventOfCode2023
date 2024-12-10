using AOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{
    internal class Day6b
    {
        class Node
        {
            public int x;
            public int y;
            public char type;
            public Node? next;
        }
        public void Test1()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day6Example5.txt");

            var mine = Run(lines);

            //var reference = new AoC_2024.Days.Day6();
            //reference.Input = input;
            //var theirs = reference.Run();
            var theirs = day6ref2.Part2(lines.ToList());

            int g = 56;
        }

        public void Test()
        {
            var r = new Random();
            int w = 20;
            int h = 20;
            int o = 20;
            while (true)
            {
                char[][] chars = Enumerable.Range(0, h).Select(l => new String('.', w).ToCharArray()).ToArray();

                for (int i = 0; i < o; ++i)
                {
                    var ox = r.Next(w);
                    var oy = r.Next(h);
                    chars[oy][ox] = '#';
                }

                var ix = r.Next(w);
                var iy = r.Next(h - 1) + 1;
                chars[iy][ix] = '^';




                string[] input = chars.Select(l => new string(l)).ToArray()!;

                var mine = Run(input);

                //var reference = new AoC_2024.Days.Day6();
                //reference.Input = input;
                //var theirs = reference.Run();
                var theirs = day6ref2.Part2(input.ToList());

                if (mine != theirs)
                {
                    int g = 56;
                }


            }
        }


        public int Run(string[]? input)
        {
            var lines = input ?? File.ReadAllLines("./2024/Inputs/Day6.txt");

            var nodes = new List<Node>();
            int sx = 0, sy = 0;

            for(int y = 0; y < lines.Length; y++)
            {
                for(int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#' )
                    {
                        nodes.Add(new Node { x = x, y = y- 1, type = 'U' });
                        nodes.Add(new Node { x = x+1, y = y, type = 'R' });
                        nodes.Add(new Node { x = x, y = y + 1, type = 'D' });
                        nodes.Add(new Node { x = x-1, y = y, type = 'L' });
                    }
                    else if (lines[y][x] == '^' )
                    {
                        sx = x;
                        sy = y;
                    }
                }
            }

            nodes = nodes
                .Where(n => n.x >=0 && n.x < lines[0].Length && n.y >= 0 && n.y < lines.Length).ToList();

            var nextR = (int x, int y, List<Node>? n = null) => (n ?? nodes).Where(n => n.x <= x && n.y == y && n.type == 'R').OrderByDescending(n => n.x).FirstOrDefault();
            var nextL = (int x, int y, List<Node>? n = null) => (n ?? nodes).Where(n => n.x >= x && n.y == y && n.type == 'L').OrderBy(n => n.x).FirstOrDefault();
            var nextU = (int x, int y, List<Node>? n = null) => (n ?? nodes).Where(n => n.x == x && n.y >= y && n.type == 'U').OrderBy(n => n.y).FirstOrDefault();
            var nextD = (int x, int y, List<Node>? n = null) => (n ?? nodes).Where(n => n.x == x && n.y <= y && n.type == 'D').OrderByDescending(n => n.y).FirstOrDefault();

            foreach (Node node in nodes)
            {
                Node? next = null;
                if (node.type == 'U')
                {
                    // Find first 'R' node to the left
                    next = nodes.Where(n => n.x <= node.x && n.y == node.y && n.type == 'R').OrderByDescending(n => n.x).FirstOrDefault();
                }
                else if (node.type == 'D')
                {
                    // Find first 'L' node to the right
                    next = nodes.Where(n => n.x >= node.x && n.y == node.y && n.type == 'L').OrderBy(n => n.x).FirstOrDefault();
                }
                else if (node.type == 'L')
                {
                    // Find first 'U' node below
                    next = nodes.Where(n => n.x == node.x && n.y >= node.y && n.type == 'U').OrderBy(n => n.y).FirstOrDefault();
                }
                else if (node.type == 'R')
                {
                    // Find first 'D' node above
                    next = nodes.Where(n => n.x == node.x && n.y <= node.y && n.type == 'D').OrderByDescending(n => n.y).FirstOrDefault();
                }

                node.next = next;
            }

            // Find first node target

            var next2 = nodes.Where(n => n.x == sx && n.y <= sy && n.type == 'D').OrderByDescending(n => n.y).FirstOrDefault() ?? new Node { x = sx, y = -1 };
            Node? prev = new Node { x = sx, y = sy, next = next2 };
            char dir = 'U';
            int cx = sx;
            int cy = sy;
            // Used to exit the map, when we get there
            Node? lastNode = null;

            var obstructions = new HashSet<(int x, int y)>();
            var visited = new HashSet<(int x, int y)>();

            while (next2 != null)
            {
                visited.Add((cx, cy));

                if (cx != next2.x || cy != next2.y)
                {
                    if (prev != null)
                    {
                        // pretend we're at a node. Whats the next node?
                        int ox = cx;
                        int oy = cy;
                        Node fakeNode = new Node { x = cx, y = cy };
                        Node? fakeNext = null;
                        switch (dir)
                        {
                            case 'U': fakeNext = nextL(cx, cy); fakeNode.type = 'D'; oy--; break;
                            case 'R': fakeNext = nextU(cx, cy); fakeNode.type = 'L'; ox++; break;
                            case 'D': fakeNext = nextR(cx, cy); fakeNode.type = 'U'; oy++; break;
                            case 'L': fakeNext = nextD(cx, cy); fakeNode.type = 'R'; ox--; break;
                        }

                        if (ox >= 0 && ox < lines[0].Length && oy >= 0 && oy < lines.Length && !visited.Contains((ox, oy)) && fakeNext != null)
                        {
                            // Dont use our graph at all!
                            // Adding a node means that potentially several other nodes change to point to this one.
                            // Thats too fiddly. Just traverse the hard way.

                            // This node system was probably a bad idea.
                            var fakeNodes = new List<Node>()
                            {
                                new Node { x = ox - 1, y = oy, type = 'L' },
                                new Node { x = ox + 1, y = oy, type = 'R' },
                                new Node { x = ox, y = oy - 1, type = 'U' },
                                new Node { x = ox, y = oy + 1, type = 'D' },
                            };


                            int cx2 = cx;
                            int cy2 = cy;
                            var n2 = fakeNode;
                            var newNodes = new List<Node>(nodes);
                            newNodes.AddRange(fakeNodes);
                            var v2 = new HashSet<Node>();
                            var d2 = dir;
                            v2.Add(fakeNode);
                            while(n2 != null)
                            {
                                Node? n3 = null;
                                switch (d2)
                                {
                                    case 'U': n3 = nextL(n2.x, n2.y, newNodes); d2 = 'R'; break;
                                    case 'R': n3 = nextU(n2.x, n2.y, newNodes); d2 = 'D'; break;
                                    case 'D': n3 = nextR(n2.x, n2.y, newNodes); d2 = 'L'; break;
                                    case 'L': n3 = nextD(n2.x, n2.y, newNodes); d2 = 'U'; break;
                                }
                                if (n3 != null)
                                {
                                    if (v2.Contains(n3))
                                    {
                                        obstructions.Add((ox, oy));
                                        break;
                                    }
                                    v2.Add(n3);
                                }
                                n2 = n3;
                            }
                            /*
                            // temporarily inject the node into the graph
                            prev.next = fakeNode;
                            fakeNode.next = fakeNext;                            

                            var n1 = fakeNode.next;
                            var n2 = fakeNode.next.next;
                            while(n1 != null && n2 != null)
                            {
                                if (n1 == n2)
                                {
                                    obstructions.Add((ox, oy));
                                    break;
                                }
                                n1 = n1.next;
                                n2 = n2.next?.next;
                            }
                            */
                            /*
                            for (int i = 0; i < 10000; ++i)
                            {
                                if (test == null)
                                {
                                    // not a loop
                                    break;
                                }
                                else if (test == fakeNode)
                                {
                                    // is a loop!

                                    obstructions.Add((ox, oy));
                                    break;
                                }
                                else
                                {
                                    test = test.next;
                                }

                                if (i == 9999)
                                {
                                    int h = 56;
                                }
                            }
                            */

                            prev.next = next2;

                        }
                    }

                    // finally, move a step
                    switch (dir)
                    {
                        case 'U': cy--; break;
                        case 'R': cx++; break;
                        case 'D': cy++; break;
                        case 'L': cx--; break;
                    }
                }
                else
                {
                    if ( lastNode != null && next2 == lastNode)
                    {
                        break;
                    }

                    prev = next2;
                    next2 = next2.next;
                    switch(dir)
                    {
                        case 'U': dir = 'R'; break;
                        case 'R': dir = 'D'; break;
                        case 'D': dir = 'L'; break;
                        case 'L': dir = 'U'; break;
                    }

                    if (next2 == null)
                    {
                        lastNode = new Node { x = cx, y = cy };

                        switch (dir)
                        {
                            case 'U': lastNode.y = 0; break;
                            case 'R': lastNode.x = lines[0].Length - 1; break;
                            case 'D': lastNode.y = lines.Length - 1; break;
                            case 'L': lastNode.x = 0; break;
                        }
                        next2 = lastNode;
                    }
                }
            }

            Console.WriteLine(visited.Count);
            Console.WriteLine(obstructions.Count);

            PrintMap(lines, visited, obstructions);
            int g = 56;

            return obstructions.Count;
        }

        void PrintMap(string[] lines, HashSet<(int x, int y)> visited, HashSet<(int x, int y)> obstructions)
        {
            for(int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    bool v = visited.Contains((x, y));
                    bool o = obstructions.Contains((x, y));
                    if (v && o)
                    {
                        Console.Write('B');
                    }
                    else if (v)
                    {
                        Console.Write('X');
                    }
                    else if (o)
                    {
                        Console.Write('O');
                    }
                    else
                    {
                        Console.Write(lines[y][x]);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}

