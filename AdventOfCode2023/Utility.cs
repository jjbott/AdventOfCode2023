using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal static class Utility
    {
        // swiped from https://codereview.stackexchange.com/q/92366
        public static IEnumerable<int> Primes(int upperLimit)
        {
            if (upperLimit < 2)
            {
                throw new ArgumentException("Upper Limit be must greater than or equal to 2.");
            }
            yield return 2;
            if (upperLimit == 2)
            {
                yield break;
            }
            // Check odd numbers for primality
            const int offset = 3;
            Func<int, int> ToNumber = delegate (int index) { return (2 * index) + offset; };
            Func<int, int> ToIndex = delegate (int number) { return (number - offset) / 2; };
            var bits = new BitArray(ToIndex(upperLimit) + 1, defaultValue: true);
            var upperSqrtIndex = ToIndex((int)Math.Sqrt(upperLimit));
            for (var i = 0; i <= upperSqrtIndex; i++)
            {
                // If this bit has already been turned off, then its associated number is composite. 
                if (!bits[i]) continue;
                var number = ToNumber(i);
                // The instant we have a known prime, immediately yield its value.
                yield return number;
                // Any multiples of number are composite and their respective bits should be turned off.
                for (var j = ToIndex(number * number); (j > i) && (j < bits.Length); j += number)
                {
                    bits[j] = false;
                }
            }
            // Output remaining primes once bit array is fully resolved:
            for (var i = upperSqrtIndex + 1; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    yield return ToNumber(i);
                }
            }
        }

        public static bool IsPrime(int n)
        {
            if (n < 0)
            {
                return false;
            }

            if (n == 1 || n == 2)
            {
                return true;
            }

            for(int i = 2; i < Math.Sqrt(n); i++) 
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        public static IEnumerable<long> PrimeFactors(long n, IEnumerable<int>? primes = null)
        {
            var limit = Math.Sqrt(n);
            foreach (var p in primes ?? Primes(1000))
            {
                if (n % p == 0)
                {
                    yield return p;
                    do
                    {
                        n /= p;
                    } while (n % p == 0);
                }

                if (p > limit)
                {
                    break;
                }
            }

            if ( n > 1)
            {
                yield return n;
            }
        }

        public static long LCM(IEnumerable<long> values, IEnumerable<int>? primes = null)
        {
            primes ??= Primes(1000);
            var factors = values.Select(v => PrimeFactors(v, primes).ToList()).ToList();
            
            return factors.SelectMany(v => v).Distinct().Aggregate((p, v) => p * v);
        }
    }
}
