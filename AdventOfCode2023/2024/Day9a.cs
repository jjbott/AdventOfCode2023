using AOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{

    internal class Day9a
    {

        class Node
        {
            public int size;
        }

        class FileNode : Node
        {
            public int id;
        }

        public void Run()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day9.txt");

            var nodes = new List<Node>();

            var id = 0;
            bool isFile = true;

            foreach(var c in lines[0])
            {
                var size = c - '0';
                if ( isFile )
                {
                    nodes.Add(new FileNode { size = size, id = id });
                    id++;
                }
                else
                {
                    nodes.Add(new Node { size = size });
                }
                isFile = !isFile;
            }

            PrintDisk(nodes);

            var fsp = 0;

            var nextFsp = () =>
            {
                while (++fsp < nodes.Count && ((nodes[fsp] is FileNode) || (nodes[fsp].size ==0))) { }
            };

            nextFsp();

            for (int i = nodes.Count - 1; i > fsp; i--)
            {
                var node = nodes[i];

                if (node is FileNode fn)
                {
                    if (fn.size == nodes[fsp].size)
                    {
                        nodes[fsp] = fn;
                        nodes.RemoveAt(i);
                        nextFsp();
                    }
                    else
                    {
                        while (fn.size > nodes[fsp].size)
                        {
                            fn.size -= nodes[fsp].size;
                            nodes[fsp] = new FileNode { size = nodes[fsp].size, id = fn.id };

                            nextFsp();
                        }

                        if (fn.size >= 0)
                        {
                            nodes.Insert(fsp, fn);
                            // bump indexes so we're pointing at the same one
                            fsp++;
                            i++;
                            nodes[fsp].size -= fn.size;
                            nodes.RemoveAt(i);

                            if (nodes[fsp].size == 0)
                            {
                                //todo: remove empty node?
                                nodes.RemoveAt(fsp);

                                nextFsp();
                            }
                        }
                    }
                }

                PrintDisk(nodes);

            }

            nodes = nodes.Where(n => n.size > 0).ToList();

            long sum = 0;
            var diskIndex = 0;
            for(int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] is FileNode fn)
                {
                    for(int j = 0; j < nodes[i].size; j++)
                    {
                        sum += diskIndex * fn.id;
                        diskIndex++;
                    }
                }
            }
            PrintDisk(nodes);
            int g = 56;
        }

        void PrintDisk(List<Node> nodes)
        {
            return;
            foreach(Node n in nodes.Where(n => n.size > 0))
            {
                if (n is FileNode fn)
                {
                    Console.Write(new string((char)('0' + fn.id), fn.size));
                }
                else
                {
                    Console.Write(new string('.', n.size));
                }
            }
            Console.WriteLine();
        }
    }
}

