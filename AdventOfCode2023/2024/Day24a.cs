using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2024
{
    internal class Day24a
    {


        public void Run()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day24.txt");

            Dictionary<string, bool> values = new();
            List<(string a, string op, string b, string output)> operations = new();

            var sepFound = false;
            foreach(var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    sepFound = true;
                    continue;
                }

                if ( !sepFound )
                {
                    values[line.Substring(0, 3)] = line[5] == '1';
                }
                else
                {
                    // btc OR rns -> dtb
                    var split = line.Split(' ');
                    operations.Add((split[0], split[1], split[2], split[4]));
                }
            }

            var done = false;

            while(!done)
            {
                done = true;

                foreach(var operation in operations)
                {
                    if ( values.ContainsKey(operation.a) && values.ContainsKey(operation.b) && !values.ContainsKey(operation.output) ) 
                    {
                        done = false;
                        switch(operation.op)
                        {
                            case "XOR":
                                values[operation.output] = values[operation.a] ^ values[operation.b]; 
                                break;
                            case "OR":
                                values[operation.output] = values[operation.a] || values[operation.b];
                                break;
                            case "AND":
                                values[operation.output] = values[operation.a] && values[operation.b];
                                break;
                        }
                    }
                }
            }

            ulong output = 0;
            foreach(var kv in values.Where(kv => kv.Key.StartsWith("z") && kv.Value))
            {
                var index = int.Parse(kv.Key.Substring(1));

                output = output | ((ulong)1 << index);
            }

            Console.WriteLine(output);
        }
    }
}
