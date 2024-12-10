
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace AdventOfCode2023
{
    public struct Vector2
    {
        public long X;
        public long Y;

        public Vector2 (long x, long y) 
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator+(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X+v2.X, v1.Y+v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator*(Vector2 v, long t)
        {
            return new Vector2(v.X * t, v.Y * t);
        }

        public static bool operator == (Vector2 v1, Vector2 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !(v1 == v2);
        }
    }

    public class Projectile2
    {
        public Vector2 Initial;
        public Vector2 Delta;

        
    }

    internal class Day24
    {
        public void Part1()
        {
            var lines = File.ReadAllLines("Inputs/Day24.txt");
            //var min = 7;
            //var max = 27;

            var min = 200000000000000;
            var max = 400000000000000;

            var projectiles = new List<Projectile2>();
            foreach(var l in lines)
            {
                // 19, 13, 30 @ -2,  1, -2
                var s = l.Split(new char[] { ' ', ',', '@' }, StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList();
                projectiles.Add(new Projectile2 { Initial = new Vector2(s[0], s[1]), Delta = new Vector2(s[3], s[4]) });
            }

            var collisionCount = 0;

            for(int i = 0; i < projectiles.Count; i++)
            {
                var p1 = projectiles[i];

                for (int j = i + 1; j < projectiles.Count; j++)
                {
                    var p2 = projectiles[j];

                    if (p1.Initial == p2.Initial)
                    {
                        // collide at t = 0
                        // I'm assuming we wont have any of these, but just in case.
                        collisionCount++;
                    }
                    else if ( (p1.Delta.X == 0 && p2.Delta.X == 0) || (p1.Delta.Y / (double)p1.Delta.X) == (p2.Delta.Y / (double)p2.Delta.X))
                    {
                        // Parallel, wont collide
                        continue;
                    }
                    else
                    {
                        // calculate collision time

                        var c1 = p1.Initial;
                        var c2 = p1.Initial + p1.Delta * 1;

                        var c3 = p2.Initial;
                        var c4 = p2.Initial + p2.Delta * 1;


                        var denominator = checked((c1.X - c2.X) * (c3.Y - c4.Y) - (c1.Y - c2.Y) * (c3.X - c4.X));
                        if ( denominator == 0)
                        {
                            // coincident. Dangit.

                            int g = 56;
                        }
                        else
                        {
                            var intX =checked( (double)(((BigInteger)c1.X * c2.Y - (BigInteger)c1.Y * c2.X) * (c3.X - c4.X) - (c1.X - c2.X) * ((BigInteger)c3.X * c4.Y - (BigInteger)c3.Y * c4.X)) / (double)denominator);
                            var intY =checked( (double)(((BigInteger)c1.X * c2.Y - (BigInteger)c1.Y * c2.X) * (c3.Y - c4.Y) - (c1.Y - c2.Y) * ((BigInteger)c3.X * c4.Y - (BigInteger)c3.Y * c4.X)) / (double)denominator);

                            var t1 = checked((intX - p1.Initial.X) / (double)p1.Delta.X);
                            var t2 = checked((intX - p2.Initial.X) / (double)p2.Delta.X);

                            var good = intX <= max && intX >= min && intY <= max && intY >= min && t1 > 0 && t2 > 0;

                            if (good)
                            {
                                int g = 56;
                                collisionCount++;
                            }
                            else
                            {
                                int g = 56;
                            }
                        }
                    }

                }
            }

            Console.WriteLine(collisionCount);

        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day24.txt");
            //var min = 7;
            //var max = 27;

            var min = 200000000000000;
            var max = 400000000000000;

            var projectiles = new List<Projectile2>();
            foreach (var l in lines)
            {
                // 19, 13, 30 @ -2,  1, -2
                var s = l.Split(new char[] { ' ', ',', '@' }, StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList();

                var t = 1000;
                Console.WriteLine($"[{s[0]},{s[1]},{s[2]},{checked(s[3]*t+ s[0])},{checked(s[4] * t + s[1])},{checked(s[5] * t + s[2])}],");
            }
        }
    }
}
