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
    internal class Day24b
    {
        public void Run()
        {
            var lines = File.ReadAllLines("./2024/Inputs/Day24.txt");

            Dictionary<string, bool> values = new();
            List<(string a, string op, string b, string output)> operations = new();

            var sepFound = false;
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    sepFound = true;
                    continue;
                }

                if (!sepFound)
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

            Dictionary<string, string> aliases = new Dictionary<string, string>();

            List<(string actual, string expected)> badOutputs = new();

            foreach (var operation in operations)
            {
                var operands = new List<string> { operation.a, operation.b }.OrderBy(x => x).ToList();

                if (operands[0].StartsWith("x") && operands[1].StartsWith("y") && operation.op == "XOR") 
                {
                    // _Should_ be the bit value;
                    aliases[operation.output] = "bitValue" + operands[0].Substring(1);
                }
                else if (operands[0].StartsWith("x") && operands[1].StartsWith("y") && operation.op == "AND")
                {
                    // _Should_ be the bit carry;
                    aliases[operation.output] = "bitCarry" + operands[0].Substring(1);
                }
            }

            // fix first "carriedCarry"
            var firstCarriedCarry = operations.Where(o => (o.a == "x00" || o.a == "y00") && (o.b == "x00" || o.b == "y00") && o.op == "AND").Single();
            aliases[firstCarriedCarry.output] = "carriedCarry00";

            var found = true;
            while (found)
            {
                found = false;
                for(int i = 0; i < operations.Count; ++i)
                {
                    var operation = operations[i];
                    
                    var a1 = aliases.ContainsKey(operation.a) ? aliases[operation.a] : "";
                    var a2 = aliases.ContainsKey(operation.b) ? aliases[operation.b] : "";

                    if (!aliases.ContainsKey(operation.output))
                    {
                        if (operation.op == "AND"
                            && (a1.StartsWith("bitValue") || a2.StartsWith("bitValue"))
                            && (a1.StartsWith("carriedCarry") || a2.StartsWith("carriedCarry")))
                        {
                            found = true;

                            var bitValue = a1.StartsWith("bitValue") ? a1 : a2;
                            var carriedCarry = a1.StartsWith("carriedCarry") ? a1 : a2;

                            var bitValueBit = int.Parse(bitValue.Substring(8));
                            var carriedCarryBit = int.Parse(carriedCarry.Substring(12));
                            if (bitValueBit != (carriedCarryBit + 1))
                            {
                                // output that produced either bitValueBit or carriedCarryBit is wrong
                                int h = 56;
                            }

                            aliases[operation.output] = "needToCarry" + bitValueBit.ToString().PadLeft(2, '0');
                        }
                        else if (operation.op == "XOR"
                            && (a1.StartsWith("bitValue") || a2.StartsWith("bitValue"))
                            && (a1.StartsWith("carriedCarry") || a2.StartsWith("carriedCarry")))
                        {
                            //found = true;

                            // final output
                            // bitValueX ^ carriedCarry(x-1) = bit

                            var bitValue = a1.StartsWith("bitValue") ? a1 : a2;
                            var carriedCarry = a1.StartsWith("carriedCarry") ? a1 : a2;

                            var bitValueBit = int.Parse(bitValue.Substring(8));
                            var carriedCarryBit = int.Parse(carriedCarry.Substring(12));
                            if (bitValueBit != (carriedCarryBit + 1))
                            {
                                // output that produced either bitValueBit or carriedCarryBit is wrong
                                int h = 56;
                            }

                            var expectedOutput = "z" + bitValueBit.ToString().PadLeft(2, '0');

                            if (operation.output != expectedOutput && !badOutputs.Any(bo => bo.actual == operation.output))
                            {
                                // oooh we got one

                                badOutputs.Add((operation.output, expectedOutput));

                                // fix it!
                                var otherOperation = operations.FindIndex(o => o.output == expectedOutput);
                                operations[otherOperation] = (operations[otherOperation].a, operations[otherOperation].op, operations[otherOperation].b, operation.output);
                                operations[i] = (operation.a, operation.op, operation.b, expectedOutput);
                            }
                        }
                        else if (operation.op == "OR"
                            && (a1.StartsWith("bitCarry") || a2.StartsWith("bitCarry"))
                            && (a1.StartsWith("needToCarry") || a2.StartsWith("needToCarry")))
                        {

                            var bitCarry = a1.StartsWith("bitCarry") ? a1 : a2;
                            var needToCarry = a1.StartsWith("needToCarry") ? a1 : a2;

                            var bitCarryBit = int.Parse(bitCarry.Substring(8));
                            var needToCarryBit = int.Parse(needToCarry.Substring(11));
                            if (bitCarryBit != needToCarryBit)
                            {
                                // output that produced either bitCarry or needToCarry is wrong
                                int h = 56;
                            }

                            var outputAlias = "carriedCarry" + bitCarryBit.ToString().PadLeft(2, '0');

                            if (operation.output.StartsWith("z") && operation.output != "z45")
                            {

                                // oh snap there is a `z` output. That cant be right for an `OR`
                                // And, we cant alias a z!
                                // See if we can swap it...

                                var opIndex = operations.FindIndex(o => aliases.ContainsKey(o.output) && aliases[o.output] == outputAlias);
                                if (opIndex >= 0)
                                {
                                    found = true;

                                    // swap
                                    int h = 56;
                                    badOutputs.Add((operation.output, operations[opIndex].output));

                                    var otherOutputName = operations[opIndex].output;
                                    operations[opIndex] = (operations[opIndex].a, operations[opIndex].op, operations[opIndex].b, operation.output);
                                    operations[i] = (operation.a, operation.op, operation.b, otherOutputName);

                                    // remove any alias. I think it's wrong
                                    if ( aliases.ContainsKey(otherOutputName))
                                    {
                                        aliases.Remove(otherOutputName);
                                    }
                                }
                                else
                                {
                                    // Do nothing for now. Hopefully next pass we'll catch it.
                                }
                            }
                            else
                            {
                                found = true;
                                aliases[operation.output] = "carriedCarry" + bitCarryBit.ToString().PadLeft(2, '0');
                            }
                        }
                        else if (operation.op == "XOR"
                            && (a1.StartsWith("bitCarry") || a2.StartsWith("bitCarry"))
                            && (a1.StartsWith("carriedCarry") || a2.StartsWith("carriedCarry"))
                            && operation.output.StartsWith("z"))
                        {
                            found = true;

                            // carriedCarry14 XOR bitCarry15 => z15 : dck XOR ctg => z15

                            // Bad op!
                            // 2 out of 3 are correct for a "carriedCarry XOR bitValue => z".
                            // So I'm going to assume the `bitCarry` is wrong.

                            // Lets check the bit values just to make sure

                            var bitCarry = a1.StartsWith("bitCarry") ? a1 : a2;
                            var carriedCarry = a1.StartsWith("carriedCarry") ? a1 : a2;

                            var bitCarryBit = int.Parse(bitCarry.Substring(8));
                            var carriedCarryBit = int.Parse(carriedCarry.Substring(12));

                            var expectedOutput = "z" + (carriedCarryBit + 1).ToString().PadLeft(2, '0');

                            if (expectedOutput != operation.output)
                            {
                                throw new Exception();
                            }

                            // Get the actual bitCarry name back. I could get it from the operation, but reversing the alias is easier.
                            var bitCarryName = aliases.First(a => a.Value == bitCarry).Key;

                            var expectedBitValueAlias = "bitValue" + (carriedCarryBit + 1).ToString().PadLeft(2, '0');
                            var expectedBitValueName = aliases.First(a => a.Value == expectedBitValueAlias).Key;

                            var bitValueOut = operations.FindIndex(o => o.output == expectedBitValueName);
                            var bitCarryOut = operations.FindIndex(o => o.output == bitCarryName);

                            operations[bitValueOut] = (operations[bitValueOut].a, operations[bitValueOut].op, operations[bitValueOut].b, bitCarryName);
                            operations[bitCarryOut] = (operations[bitCarryOut].a, operations[bitCarryOut].op, operations[bitCarryOut].b, expectedBitValueName);

                            // swap aliases
                            aliases[bitCarryName] = expectedBitValueAlias;
                            aliases[expectedBitValueName] = bitCarry;

                            // These variable names are all awful and I'm confusing myself.


                            badOutputs.Add((bitCarryName, expectedBitValueName));
                        }
                        else if (operation.op == "XOR"
                            && (a1.StartsWith("bitValue") || a2.StartsWith("bitValue"))
                            && operation.output.StartsWith("z"))
                        {
                            found = true;

                            // missing alias for the "carriedCarry"
                            // dtf XOR bitValue32 => z32 : dtf XOR vjq => z32

                            var bitValueAlias = a1.StartsWith("bitValue") ? a1 : a2;
                            var bitValueName = a1.StartsWith("bitValue") ? operation.a : operation.b;
                            var carriedCarryName = a1.StartsWith("bitValue") ? operation.b : operation.a;

                            // Make sure bit numbers are correct
                            var bitValueBit = int.Parse(bitValueAlias.Substring(8));
                            var expectedOutput = "z" + bitValueBit.ToString().PadLeft(2, '0');

                            if (expectedOutput != operation.output)
                            {
                                throw new Exception();
                            }

                            aliases[carriedCarryName] = "carriedCarry" + (bitValueBit - 1).ToString().PadLeft(2, '0');
                        }
                        else if (operation.op == "XOR"
                            && ((operation.a.StartsWith('x') || operation.b.StartsWith('x')) && (operation.a.StartsWith('y') || operation.b.StartsWith('y')) )
                            && !aliases.ContainsKey(operation.output))
                        {
                            found = true;

                            // new output got swapped in for a bitValue. Set the alias

                            aliases[operation.output] = "bitValue" + int.Parse(operation.a.Substring(1)).ToString().PadLeft(2, '0');


                        }
                        else if (operation.op == "AND"
                            && ((operation.a.StartsWith('x') || operation.b.StartsWith('x')) && (operation.a.StartsWith('y') || operation.b.StartsWith('y')))
                            && !aliases.ContainsKey(operation.output))
                        {
                            found = true;

                            // new output got swapped in for a bitCarry. Set the alias

                            aliases[operation.output] = "bitCarry" + int.Parse(operation.a.Substring(1)).ToString().PadLeft(2, '0');


                        }
                        else
                        {
                            

                            int g = 56;
                        }
                    }
                }
            }


            foreach(var operation in operations)
            {
                string a1 = aliases.ContainsKey(operation.a) ? aliases[operation.a] : operation.a;
                string a2 = aliases.ContainsKey(operation.b) ? aliases[operation.b] : operation.b;
                string a3 = aliases.ContainsKey(operation.output) ? aliases[operation.output] : operation.output;

                var o1 = operation.a;
                var o2 = operation.b;
                var o3 = operation.output;

                if ( a2.CompareTo(a1) > 0 )
                {
                    var s = a1;
                    a1 = a2;
                    a2 = s;

                    s = o1;
                    o1 = o2;
                    o2 = s;
                }

                Console.WriteLine($"{a1} {operation.op} {a2} => {a3} : {o1} {operation.op} {o2} => {operation.output}");
            }

            // Swap z38 and z39?
            /*var actualZ38 = operations.FindIndex(o => o.output == "z39");
            var actualZ39 = operations.FindIndex(o => o.output == "z38");

            operations[actualZ38] = (operations[actualZ38].a, operations[actualZ38].op, operations[actualZ38].b, "z38");
            operations[actualZ39] = (operations[actualZ39].a, operations[actualZ39].op, operations[actualZ39].b, "z39");

            badOutputs.Add(("z38", "z39"));
            */

            // Some test code to find the first problem bit            
            for (var i = 0; i < 45; ++i)
            {
                Dictionary<string, bool> testValues = new();
                for (int j = 0; j < 45; ++j)
                {
                    testValues["x" + j.ToString().PadLeft(2, '0')] = false;
                    testValues["y" + j.ToString().PadLeft(2, '0')] = false;
                }

                testValues["x" + i.ToString().PadLeft(2, '0')] = true;
                testValues["y" + i.ToString().PadLeft(2, '0')] = true;

                ulong testOutput = Run(testValues, operations);

                if ( testOutput != ((ulong)1 << (i + 1)) )
                {
                    Console.WriteLine("Bad bit: " + i.ToString());
                    PrintValues(aliases, testValues);
                    int g = 56;
                }
            }

            /*
            for (var i = 0; i < 45; ++i)
            {
                Dictionary<string, bool> testValues = new();

                var testVal = ((ulong)1 << i) - 1;

                for (int j = 0; j < 45; ++j)
                {
                    testValues["x" + j.ToString().PadLeft(2, '0')] = (testVal & ((ulong)1 << j)) > 0;
                    testValues["y" + j.ToString().PadLeft(2, '0')] = (testVal & ((ulong)1 << j)) > 0;
                }

                ulong testOutput = Run(testValues, operations);

                if (testOutput != (ulong)(testVal * 2))
                {
                    int g = 56;


                    foreach(var a in aliases.OrderBy(kv => kv.Value))
                    {
                        Console.WriteLine($"{a.Value}:{a.Key} = {testValues[a.Key]}");
                    }
                    foreach (var kv in testValues.OrderBy(kv => kv.Key).Where(kv => kv.Key.StartsWith("x") || kv.Key.StartsWith("y")))
                    {
                        Console.WriteLine($"{kv.Key} = {testValues[kv.Key]}");
                    }
                }
            }
            */

            /*
            var r = new Random();
            while(true)
            {
                Dictionary<string, bool> testValues = new();

                var testVal1 = (((ulong)r.Next() << 32) + (ulong)r.Next()) & 0b1111111111111111111111111111111111111111111;
                var testVal2 = (((ulong)r.Next() << 32) + (ulong)r.Next()) & 0b1111111111111111111111111111111111111111111;

                for (int j = 0; j < 45; ++j)
                {
                    testValues["x" + j.ToString().PadLeft(2, '0')] = (testVal1 & ((ulong)1 << j)) > 0;
                    testValues["y" + j.ToString().PadLeft(2, '0')] = (testVal2 & ((ulong)1 << j)) > 0;
                }

                ulong testOutput = Run(testValues, operations);

                if (testOutput != testVal1 + testVal2)
                {
                    int g = 56;
                    PrintValues(aliases, testValues);
                }
            }
            */

            Console.WriteLine(string.Join(",",badOutputs.Select(bo => bo.actual).Union(badOutputs.Select(bo => bo.expected)).OrderBy(x => x)));
        }

        private static void PrintValues(Dictionary<string, string> aliases, Dictionary<string, bool> testValues)
        {
            var log = new Dictionary<int, List<string>>();
            for(int i = 0; i < 46; ++i)
            {
                log[i] = new List<string>();
            }

            foreach (var a in aliases.OrderBy(kv => kv.Value))
            {
                var bit = int.Parse(a.Value.Substring(a.Value.Length - 2, 2));

                log[bit].Add($"{a.Value}:{a.Key} = {testValues[a.Key]}");
            }
            foreach (var kv in testValues.Where(kv => kv.Key.StartsWith("x") || kv.Key.StartsWith("y") || kv.Key.StartsWith("z")).OrderBy(kv => kv.Key.Substring(1)).ThenBy(kv => kv.Key[0]))
            {
                var bit = int.Parse(kv.Key.Substring(1));

                log[bit].Add($"{kv.Key} = {testValues[kv.Key]}");
            }

            for (int i = 0; i < 45; ++i)
            {
                foreach(var l in log[i].OrderBy(x => x))
                {
                    Console.WriteLine(l);
                }
            }
        }

        private static ulong Run(Dictionary<string, bool> values, List<(string a, string op, string b, string output)> operations)
        {
            var done = false;

            while (!done)
            {
                done = true;

                foreach (var operation in operations)
                {
                    if (values.ContainsKey(operation.a) && values.ContainsKey(operation.b) && !values.ContainsKey(operation.output))
                    {
                        done = false;
                        switch (operation.op)
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
            foreach (var kv in values.Where(kv => kv.Key.StartsWith("z") && kv.Value))
            {
                var index = int.Parse(kv.Key.Substring(1));

                output = output | ((ulong)1 << index);
            }

            return output;
        }
    }
}
