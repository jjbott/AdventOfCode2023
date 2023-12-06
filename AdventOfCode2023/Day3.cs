using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Day3
    {
        private bool IsPartNumber(char[][] yxGrid, int y, int startX, int endX)
        {
            for(int x = startX - 1; x <= endX + 1; x++)
            {
                if ( x >= 0 && x < yxGrid[0].Length )
                {
                    if (yxGrid[y][x] != '.' && !char.IsDigit(yxGrid[y][x]))
                    {
                        return true;
                    }

                    if (y > 0 && yxGrid[y-1][x] != '.' && !char.IsDigit(yxGrid[y-1][x]))
                    {
                        return true;
                    }

                    if (y < yxGrid.Length - 1 && yxGrid[y + 1][x] != '.' && !char.IsDigit(yxGrid[y + 1][x]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private (int y,int x)? IsGear(char[][] yxGrid, int y, int startX, int endX)
        {
            for (int x = startX - 1; x <= endX + 1; x++)
            {
                if (x >= 0 && x < yxGrid[0].Length)
                {
                    if (yxGrid[y][x] == '*')
                    {
                        return (y,x);
                    }

                    if (y > 0 && yxGrid[y - 1][x] == '*')
                    {
                        return (y-1, x);
                    }

                    if (y < yxGrid.Length - 1 && yxGrid[y + 1][x] == '*')
                    {
                        return (y+1, x);
                    }
                }
            }

            return null;
        }

        public void RunPart1()
        {
            var lines = File.ReadAllLines(@".\Inputs\Day3.txt");
            var yxGrid = lines.Select(l => l.ToArray()).ToArray();
            
            var partNumbers = new List<int>();



            for(int y = 0; y < yxGrid.Count(); ++y)
            {
                var yArray = yxGrid[y];
                int currentDigit = 0;
                int digitStartX = -1;
                int digitEndtX = -1;
                for(int x = 0; x < yArray.Length; ++x)
                {
                    var yx = yArray[x];
                    if ( char.IsDigit(yx))
                    {
                        currentDigit *= 10;
                        currentDigit += yx - '0';

                        if ( digitStartX < 0)
                        {
                            digitStartX = x;
                        }
                    }
                    else
                    {
                        if (  digitStartX >= 0 ) 
                        {
                            digitEndtX = x - 1;

                            if (IsPartNumber(yxGrid, y, digitStartX, digitEndtX)){
                                partNumbers.Add(currentDigit);
                            }
                        }

                        digitStartX = -1;
                        digitEndtX = -1;
                        currentDigit = 0;
                    }
                }

                if (digitStartX >= 0)
                {
                    digitEndtX = yxGrid.Length - 1;

                    if (IsPartNumber(yxGrid, y, digitStartX, digitEndtX))
                    {
                        partNumbers.Add(currentDigit);
                    }
                }
            }

            Console.WriteLine(partNumbers.Sum());
        }

        public void RunPart2()
        {
            var lines = File.ReadAllLines(@".\Inputs\Day3.txt");
            var yxGrid = lines.Select(l => l.ToArray()).ToArray();

            var gears = new Dictionary<(int y, int x), List<int>>();

            for (int y = 0; y < yxGrid.Count(); ++y)
            {
                var yArray = yxGrid[y];
                int currentDigit = 0;
                int digitStartX = -1;
                int digitEndtX = -1;
                for (int x = 0; x < yArray.Length; ++x)
                {
                    var yx = yArray[x];
                    if (char.IsDigit(yx))
                    {
                        currentDigit *= 10;
                        currentDigit += yx - '0';

                        if (digitStartX < 0)
                        {
                            digitStartX = x;
                        }
                    }
                    else
                    {
                        if (digitStartX >= 0)
                        {
                            digitEndtX = x - 1;

                            var gear = IsGear(yxGrid, y, digitStartX, digitEndtX);
                            if (gear != null) 
                            {
                                if (!gears.ContainsKey(gear.Value))
                                {
                                    gears[gear.Value] = new List<int>();
                                }
                                gears[gear.Value].Add(currentDigit);
                            }
                        }

                        digitStartX = -1;
                        digitEndtX = -1;
                        currentDigit = 0;
                    }
                }

                if (digitStartX >= 0)
                {
                    digitEndtX = yxGrid.Length - 1;

                    var gear = IsGear(yxGrid, y, digitStartX, digitEndtX);
                    if (gear != null)
                    {
                        if (!gears.ContainsKey(gear.Value))
                        {
                            gears[gear.Value] = new List<int>();
                        }
                        gears[gear.Value].Add(currentDigit);
                    }
                }
            }

            Console.WriteLine(gears.Where(kv => kv.Value.Count() == 2).Select(kv => kv.Value[0] * kv.Value[1]).Sum());
        }
    }
}
