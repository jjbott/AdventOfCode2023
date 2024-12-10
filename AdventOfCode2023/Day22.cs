
namespace AdventOfCode2023
{
    internal class Day22
    {
        public class Block
        {
            public (int x, int y, int z) Pos1;
            public (int x, int y, int z) Pos2;

            public int MinZ => Math.Min(Pos1.z, Pos2.z);
            public int MaxZ => Math.Max(Pos1.z, Pos2.z);

            public List<Block> BlocksBelow = new List<Block>();
            public List<Block> BlocksAbove = new List<Block>();

            public bool Settled = false;

            public int Id;

            public static Block Deserialize(string line, int i = 0)
            {
                // 6,8,103~7,8,103

                var coords = line.Split(',', '~').Select(s => int.Parse(s)).ToList();
                var block = new Block();
                block.Id = i;
                block.Pos1 = (coords[0], coords[1], coords[2]);
                block.Pos2 = (coords[3], coords[4], coords[5]);

                return block;
            }

            public bool IsBelow(Block block)
            {
                if (Math.Min(Pos1.z, Pos2.z) > Math.Max(block.Pos1.z, block.Pos2.z))
                {
                    return false;
                }

                if (Math.Min(Pos1.x, Pos2.x) <= Math.Max(block.Pos1.x, block.Pos2.x)
                    && Math.Max(Pos1.x, Pos2.x) >= Math.Min(block.Pos1.x, block.Pos2.x)
                    && Math.Min(Pos1.y, Pos2.y) <= Math.Max(block.Pos1.y, block.Pos2.y)
                    && Math.Max(Pos1.y, Pos2.y) >= Math.Min(block.Pos1.y, block.Pos2.y))
                {
                    return true;
                }

                return false;
            }

            public bool WouldFall(List<Block> ignoredBlocks)
            {
                var bb = BlocksBelow.Where(b => !ignoredBlocks.Contains(b));

                if ( !bb.Any())
                {
                    return MinZ > 1;
                }

                var maxZBelow = bb.Max(b => Math.Max(b.Pos1.z, b.Pos2.z));
                var minZ = Math.Min(Pos1.z, Pos2.z);
                return minZ - maxZBelow > 1;
            }

            public bool IsSupporting(Block block)
            {
                // Assuming we already know they occupy the same x/y space
                return MaxZ == block.MinZ - 1;
            }

            public void RemoveNotTouching()
            {
                BlocksBelow = BlocksBelow.Where(bb => bb.IsSupporting(this)).ToList();
                BlocksAbove = BlocksAbove.Where(ba => IsSupporting(ba)).ToList();
            }

            public List<Block> FallingBlocks(List<Block> ignored, Dictionary<string, List<Block>> cache, bool forceFall = false)
            {
                var fallingSupports = ignored.Where(i => BlocksBelow.Contains(i)).ToList();
                fallingSupports.AddRange(ignored.Where(i => i.MaxZ >= MinZ)); // Make sure to consider any other ignoreds that could cause the above to fall

                var cacheKey = $"{Id}:" + string.Join(",", fallingSupports.Distinct().Select(s => s.Id).OrderBy(i => i));

                if ( cache.ContainsKey(cacheKey) )
                {
                    return cache[cacheKey];
                }

                var fallingBlocks = new List<Block>() { this };

                var newFallingBlocks = BlocksAbove.Where(ba => ba.WouldFall(fallingBlocks.Union(ignored).ToList()));

                if (newFallingBlocks.Any())
                {
                    //fallingBlocks.AddRange(newFallingBlocks);
                    fallingBlocks.AddRange(newFallingBlocks.SelectMany(fb => fb.FallingBlocks(fallingBlocks.Union(ignored).ToList(), cache)));
                }

                cache[cacheKey] = fallingBlocks.Distinct().ToList();

                return cache[cacheKey];
            }
        }
        public void Part1()
        {

            var lines = File.ReadAllLines("Inputs/Day22.txt");

            var blocks = lines.Select(l => Block.Deserialize(l)).ToList();

            for(int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                for (int j = i + 1; j < blocks.Count; j++)
                {
                    if (block.IsBelow(blocks[j]))
                    {
                        block.BlocksAbove.Add(blocks[j]);
                        blocks[j].BlocksBelow.Add(blocks[i]);
                    }
                    else if (blocks[j].IsBelow(blocks[i]))
                    {
                        block.BlocksBelow.Add(blocks[j]);
                        blocks[j].BlocksAbove.Add(blocks[i]);
                    }
                }
            }

            // drop each block

            foreach(var block in blocks.OrderBy(b => Math.Min(b.Pos1.z, b.Pos2.z)).ToList())
            {
                if (block.BlocksBelow.Any(b => !b.Settled))
                {
                    throw new Exception("Dang");
                }

                var maxZBelow = block.BlocksBelow.Any() ? block.BlocksBelow.Max(b => Math.Max(b.Pos1.z, b.Pos2.z)) : 0;
                var minZ = Math.Min(block.Pos1.z, block.Pos2.z);
                var zDiff = minZ - maxZBelow;
                block.Pos1 = (block.Pos1.x, block.Pos1.y, block.Pos1.z - zDiff + 1);
                block.Pos2 = (block.Pos2.x, block.Pos2.y, block.Pos2.z - zDiff + 1);
                block.Settled = true;
            }


            var count = 0;

            foreach (var block in blocks)
            {
                var supportingBlocks = block.BlocksAbove.Where(ba =>
                {
                    var thisMaxZ = Math.Max(block.Pos1.z, block.Pos2.z);
                    var theirMinZ = Math.Min(ba.Pos1.z, ba.Pos2.z);

                    return (thisMaxZ == theirMinZ - 1);
                }).ToList();

                var badBlocks = supportingBlocks.Where(sb =>

                    !sb.BlocksBelow.Where(bb => bb != block).Any(bb =>
                    {
                        var thisMaxZ = Math.Max(bb.Pos1.z, bb.Pos2.z);
                        var theirMinZ = Math.Min(sb.Pos1.z, sb.Pos2.z);

                        return (thisMaxZ == theirMinZ - 1);
                    }
                ));

                if (!badBlocks.Any())
                {
                    count++;
                }


            }

            Console.WriteLine(count);

        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day22.txt");

            var blocks = lines.Select((l,i) => Block.Deserialize(l,i)).ToList();

            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                for (int j = i + 1; j < blocks.Count; j++)
                {
                    if (block.IsBelow(blocks[j]))
                    {
                        block.BlocksAbove.Add(blocks[j]);
                        blocks[j].BlocksBelow.Add(blocks[i]);
                    }
                    else if (blocks[j].IsBelow(blocks[i]))
                    {
                        block.BlocksBelow.Add(blocks[j]);
                        blocks[j].BlocksAbove.Add(blocks[i]);
                    }
                }
            }

            // drop each block

            foreach (var block in blocks.OrderBy(b => Math.Min(b.Pos1.z, b.Pos2.z)).ToList())
            {
                if (block.BlocksBelow.Any(b => !b.Settled))
                {
                    throw new Exception("Dang");
                }

                var maxZBelow = block.BlocksBelow.Any() ? block.BlocksBelow.Max(b => Math.Max(b.Pos1.z, b.Pos2.z)) : 0;
                var minZ = Math.Min(block.Pos1.z, block.Pos2.z);
                var zDiff = minZ - maxZBelow;
                block.Pos1 = (block.Pos1.x, block.Pos1.y, block.Pos1.z - zDiff + 1);
                block.Pos2 = (block.Pos2.x, block.Pos2.y, block.Pos2.z - zDiff + 1);
                block.Settled = true;
            }

            foreach (var block in blocks)
            {
                block.RemoveNotTouching();
            }

            var count = 0;

            Dictionary<string, List<Block>> cache = new Dictionary<string, List<Block>>();

            foreach (var block in blocks)
            {
                count += block.FallingBlocks(new List<Block>(), cache).Count() - 1;
            }

            Console.WriteLine(count);
        }
    }
}
