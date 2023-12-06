using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Day5
    {
        public class Day5Map
        {
            private UInt64 _destStart;
            public readonly UInt64 SourceStart;
            public readonly UInt64 Length;
            public UInt64 SourceEnd => SourceStart + Length - 1;

            public Day5Map(UInt64 destStart, UInt64 sourceStart, UInt64 length)
            {
                _destStart = destStart;
                SourceStart = sourceStart;
                Length = length;
            }
            public UInt64 Lookup(UInt64 sourceValue)
            {
                if (sourceValue >= SourceStart && sourceValue < (SourceStart + Length))
                {
                    return _destStart + (sourceValue - SourceStart);
                }
                return sourceValue;
            }
        }

        public class Day5Map2
        {
            public string SourceName { get; private set; }
            public string DestinationName { get; private set; }
            private List<Day5Map> _maps = new List<Day5Map>();

            public Day5Map2(List<string> lines)
            {
                // "seed-to-soil map:"
                SourceName = lines[0].Split(' ')[0].Split('-')[0];
                DestinationName = lines[0].Split(' ')[0].Split('-')[2];

                foreach (var line in lines.Skip(1))
                {
                    var split = line.Split(' ');
                    _maps.Add(new Day5Map(UInt64.Parse(split[0]), UInt64.Parse(split[1]), UInt64.Parse(split[2])));
                }
            }
            public Day5Map2(string sourceName, string destinationName, List<Day5Map> maps)
            {
                SourceName = sourceName;
                DestinationName = destinationName;
                _maps = maps;
            }

            public UInt64 Lookup(UInt64 sourceValue)
            {
                foreach (var map in _maps)
                {
                    var destinationValue = map.Lookup(sourceValue);
                    if (destinationValue != sourceValue)
                    {
                        return destinationValue;
                    }
                }

                return sourceValue;
            }

            public List<(UInt64, UInt64)> Lookup2(UInt64 sourceValue, UInt64 count)
            {
                var destValues = new List<(UInt64, UInt64)>();
                var sourceLastValue = sourceValue + count - 1;

                var value = sourceValue;
                var validMaps = _maps.Where(m => sourceValue <= m.SourceEnd && m.SourceStart <= (sourceValue + count - 1)).OrderBy(m => m.SourceStart).ToList();

                if(!validMaps.Any())
                {
                    return new List<(UInt64, UInt64)> { ( sourceValue, count ) };
                }

                UInt64 lastValue = sourceValue - 1;
                foreach(var map in validMaps)
                {
                    if ((lastValue + 1) < map.SourceStart)
                    {
                        destValues.Add((lastValue + 1, (map.SourceStart - 1) - (lastValue + 1) + 1));
                        lastValue = map.SourceStart -1;
                    }

                    var end = (UInt64)Math.Min(sourceLastValue, map.SourceEnd);
                    destValues.Add((map.Lookup(lastValue + 1), map.Lookup(end) - map.Lookup(lastValue + 1) + 1));

                    lastValue = end;
                }

                if ((lastValue + 1) < sourceLastValue)
                {
                    destValues.Add((lastValue + 1, sourceLastValue - (lastValue + 1) + 1));
                }

                return destValues;
            }

        }
        public void RunPart1()
        {
            var lines = File.ReadAllLines("Inputs/Day5.txt");
            var mapInputs = new List<List<string>>();
            List<string> mapInput = new List<string>();
            foreach (var mapInputLine in lines.Skip(2))
            {
                if (string.IsNullOrWhiteSpace(mapInputLine))
                {
                    mapInputs.Add(mapInput);
                    mapInput = new List<string>();
                }
                else
                {
                    mapInput.Add(mapInputLine);
                }
            }
            mapInputs.Add(mapInput);

            var maps = mapInputs.Select(mi => new Day5Map2(mi)).ToDictionary(m => m.SourceName, m => m);

            var seeds = lines[0].Split(' ').Skip(1).Select(s => UInt64.Parse(s)).ToList();

            List<UInt64> locations = new List<UInt64>();

            foreach (var seed in seeds)
            {
                var map = maps["seed"];
                UInt64 value = seed;
                while (map != null)
                {
                    value = map.Lookup(value);
                    Console.WriteLine($"{map.SourceName} -> {map.DestinationName} = {value}");

                    if (maps.ContainsKey(map.DestinationName))
                    {
                        map = maps[map.DestinationName];
                    }
                    else
                    {
                        map = null;
                    }
                }

                locations.Add(value);
            }

            Console.WriteLine(locations.Min());
        }

        public void RunPart2()
        {
            var lines = File.ReadAllLines("Inputs/Day5.txt");
            var mapInputs = new List<List<string>>();
            List<string> mapInput = new List<string>();
            foreach (var mapInputLine in lines.Skip(2))
            {
                if (string.IsNullOrWhiteSpace(mapInputLine))
                {
                    mapInputs.Add(mapInput);
                    mapInput = new List<string>();
                }
                else
                {
                    mapInput.Add(mapInputLine);
                }
            }
            mapInputs.Add(mapInput);

            var maps = mapInputs.Select(mi => new Day5Map2(mi)).ToDictionary(m => m.SourceName, m => m);

            var seeds = lines[0].Split(' ').Skip(1).Select(s => UInt64.Parse(s)).ToList();

            List<UInt64> locations = new List<UInt64>();

            for (int i = 0; i < seeds.Count; i += 2)
            {
                var seed = seeds[i];
                var count = seeds[i + 1];

                var map = maps["seed"];
                List<(UInt64, UInt64)> values = new() { (seed, count) };
                while (map != null)
                {
                    var newValues = new List<(UInt64, UInt64)>();

                    foreach(var value in values)
                    {
                        newValues.AddRange(map.Lookup2(value.Item1, value.Item2));
                    }

                    //Console.WriteLine($"{map.SourceName} -> {map.DestinationName} = {value}");

                    if (maps.ContainsKey(map.DestinationName))
                    {
                        map = maps[map.DestinationName];
                    }
                    else
                    {
                        map = null;
                    }

                    values = Condense(newValues);
                }

                locations.Add(values.Select(v => v.Item1).Min());
            }

            Console.WriteLine(locations.Min());
        }

        private List<(UInt64, UInt64)> Condense(List<(UInt64, UInt64)> values)
        {
            values = values.OrderBy(v => v.Item1).ToList();

            var newValues = new List<(UInt64, UInt64)>();
            var newValue = values.First();
            foreach(var value in values.Skip(1))
            {
                if(value.Item1 == newValue.Item1)
                {
                    newValue.Item2 = Math.Max(newValue.Item2, value.Item2);
                    continue;
                }

                if (value.Item1 == newValue.Item1 + newValue.Item2 + 1)
                {
                    newValue.Item2 += value.Item2;
                }
                else
                {
                    newValues.Add(newValue);
                    newValue = value;
                }
            }

            newValues.Add(newValue);
            return newValues;
        }
    }
}
