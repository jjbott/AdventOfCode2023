using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day12b
    {
        public void Part2()
        {
            var expandCount = 4;

            var lines = File.ReadAllLines("Inputs/Day12.txt");
            long count = 0;
            foreach (var l in lines)
            {
                //Console.WriteLine(l);

                var c = Count(l, 4);
                count += c;

            }

            Console.WriteLine(count);
        }

        public long Count(string line, int expandCount = 0)
        {
            var map = line.Split(" ")[0];
            map = string.Join("?", Enumerable.Repeat(map, expandCount + 1));

            var values = line.Split(" ")[1].Split(",").Select(n => int.Parse(n)).ToList();
            values = Enumerable.Repeat(values, expandCount + 1).SelectMany(v => v).ToList();
            return Count(values, map);
        }

        // Dictionary<(string,string), (long, List<string>)> _cache = new Dictionary<(string, string), (long, List<string>)>();
        Dictionary<(string, string), long> _cache = new Dictionary<(string, string), long>();

        public long Count(List<int> values, string map, int depth = 0)
        {
            var valStr = string.Join(',', values);
            if (_cache.TryGetValue((map, valStr), out var cached))
            {
                if (cached > 0)
                {
                    int g = 56;
                }
                return cached;
            }

            if (values.Sum() + values.Count() - 1 > map.Length)
            {
                _cache[(map, valStr)] = 0;
                return _cache[(map, valStr)];
            }


            var current = values[0];
            var s = 0;
            bool found = false;
            long count = 0;
            //var strings = new List<string>();
            for (int i = 0; i < map.Length - current + 1; i++)
            {
                if (map[i] == '.')
                {
                    continue;
                }

                if (!map.Substring(i, current).Contains('.') && ((i + current >= map.Length) || (map[i + current] != '#')) && ((i == 0) || (map[i - 1] != '#')))
                {
                    found = true;

                    if (values.Count > 1)
                    {
                        if (i + current + 1 >= map.Length)
                        {
                            //Console.WriteLine($"{new string(' ', depth * 2)}{map.Substring(i)} {string.Join(",",values)} 0");
                            break;
                        }
                        var c = Count(values.Skip(1).ToList(), map.Substring(i + current + 1), depth + 1);
                        if (c != 0)
                        {
                            count += c;
                            //strings.AddRange(c.Item2.Select(s => new string('.', i) + new string((char)('0' + current), current) + '.' + s + new string('.', map.Length - i - current -s.Length-1)));
                        }
                    }
                    else if ((i + current + 1) < map.Length && map.Substring(i + current + 1).Contains('#'))
                    {
                        // Unused '#'s
                    }
                    else
                    {
                        count++;
                        //strings.Add(new string('.', i) + new string((char)('0' + current), current) + new string('.', map.Length - i - current));
                    }
                }

                if (map[i] == '#')
                {
                    // Cant just skip '#'s if we didn't find anything
                    break;
                }
            }

            if (!found)
            {
                _cache[(map, valStr)] = 0;// (0, new List<string>());
                return _cache[(map, valStr)];
            }

            if (map.Length < 20)
            {
                var c2 = new Day12a().Part1Count(map, values);
                if (c2 != count)
                {
                    Console.WriteLine("Dang");
                }
            }

            _cache[(map, valStr)] = count;// (count, strings);
            //Console.WriteLine($"{new string(' ', depth * 2)}{map} {string.Join(",", values)} {count}");
            return count; //(count, strings);


        }
    }
}
