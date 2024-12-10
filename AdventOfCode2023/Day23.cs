
namespace AdventOfCode2023
{
    internal class Day23
    {
        public List<(int x, int y)> ValidDirections((int x, int y) pos, string[] map, bool ignoreSlopes = false)
        {
            var validDirections = new List<(int x, int y)>();
            if (pos.x > 0 && map[pos.y][pos.x -1] != '#' && (ignoreSlopes || map[pos.y][pos.x - 1] != '>')) validDirections.Add((pos.x - 1, pos.y));
            if (pos.y > 0 && map[pos.y - 1][pos.x] != '#' && (ignoreSlopes || map[pos.y - 1][pos.x] != 'v')) validDirections.Add((pos.x, pos.y -1));

            if (pos.x < map[0].Length - 1 && map[pos.y][pos.x + 1] != '#' && (ignoreSlopes || map[pos.y][pos.x + 1] != '<')) validDirections.Add((pos.x + 1, pos.y));
            if (pos.y < map.Length - 1 && map[pos.y + 1][pos.x] != '#' && (ignoreSlopes || map[pos.y + 1][pos.x] != '^')) validDirections.Add((pos.x, pos.y + 1));

            return validDirections;
        }

        (HashSet<(int x, int y)> path, bool valid) MaxPathLengthFrom((int x, int y) pos, HashSet<(int x, int y)> prevPath, string[] map, bool ignoreSlopes = false)
        {
            var dirs = ValidDirections(pos, map, ignoreSlopes).Where(d => !prevPath.Contains(d)).ToList();
            prevPath.Add(pos);

            if ( pos.y == map.Length - 1)
            {
                return (prevPath, true);
            }

            while (dirs.Count() == 1)
            {
                prevPath.Add(dirs.First());
                
                if (dirs.First().y == map.Length - 1)
                {
                    return (prevPath, true);
                }

                dirs = ValidDirections(dirs.First(), map, ignoreSlopes).Where(d => !prevPath.Contains(d)).ToList();
            }

            if ( dirs.Any())
            {
                var maxLengths = dirs.Select(d => MaxPathLengthFrom(d, new HashSet<(int x, int y)>(prevPath), map, ignoreSlopes)).ToList();
                var max = maxLengths.Where(ml => ml.valid).OrderByDescending(ml => ml.path.Count).FirstOrDefault();

                if (max.valid)
                {
                    var newPath = new HashSet<(int x, int y)>(prevPath);
                    foreach (var p in max.path)
                    {
                        newPath.Add(p);
                    }

                    return (newPath, true);
                }
            }
            
            return (new HashSet<(int x, int y)>(prevPath), false);

        }
        
        public void Part1()
        {
            var map = File.ReadAllLines("Inputs/Day23.txt");

            var startPos = (x: map[0].IndexOf('.'), y: 0);

            var maxLength = MaxPathLengthFrom(startPos, new HashSet<(int x, int y)> { startPos }, map);


            for(int y = 0; y < map.Length; y++)
            {
                for(int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] != '.')
                    {
                        Console.Write(map[y][x]);
                    }
                    else if ( maxLength.path.Contains((x,y)))
                    {
                        Console.Write('O');
                    }
                    else { Console.Write(map[y][x]); }
                }
                Console.WriteLine();
            }
            Console.WriteLine();


            Console.WriteLine(maxLength.path.Count - 1);

        }

        public void Part2()
        {
            // This could be _much_ more efficient. Shouldnt be a problem to find the nodes and do a normal djikstra.
            // But I was too tired and left it run overnight for fun, and it finished eventually. Good enough!

            var map = File.ReadAllLines("Inputs/Day23.txt");

            var startPos = (x: map[0].IndexOf('.'), y: 0);

            var maxLength = MaxPathLengthFrom(startPos, new HashSet<(int x, int y)> { startPos }, map, true);


            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] != '.')
                    {
                        Console.Write(map[y][x]);
                    }
                    else if (maxLength.path.Contains((x, y)))
                    {
                        Console.Write('O');
                    }
                    else { Console.Write(map[y][x]); }
                }
                Console.WriteLine();
            }
            Console.WriteLine();


            Console.WriteLine(maxLength.path.Count - 1);
        }
    }
}
