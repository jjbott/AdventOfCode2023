using AOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{

    internal class Day9b
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

            for (int i = nodes.Count - 1; i > 0; i--)
            {
                var node = nodes[i];

                if (node is FileNode fn)
                {
                    var freeSpaceNode = nodes.Take(i).Select((n, i) => (n, i)).Where((t) => !(t.Item1 is FileNode) && t.Item1.size >= fn.size).FirstOrDefault();
                    if (freeSpaceNode.Item1 != null)
                    {
                        var fs = freeSpaceNode.Item1;
                        if ( fs.size > fn.size )
                        {

                            nodes[i] = new Node { size = fn.size };
                            nodes.Insert(freeSpaceNode.Item2, fn);
                            fs.size -= fn.size;
                        }
                        else
                        {
                            nodes[freeSpaceNode.Item2] = fn;
                            nodes[i] = freeSpaceNode.Item1;
                        }

                    }
                }

                PrintDisk(nodes);

            }

            nodes = nodes.Where(n => n.size > 0).ToList();
            //00992111777.44.333....5555.6666.....8888..
            long sum = 0;
            var diskIndex = 0;
            for(int i = 0; i < nodes.Count; i++)
            {

                for (int j = 0; j < nodes[i].size; j++)
                {
                    if (nodes[i] is FileNode fn)
                    {
                        sum += diskIndex * fn.id;
                    }
                    diskIndex++;

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

