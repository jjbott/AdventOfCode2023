using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal class Day19
    {
        class Rule
        {
            public char variable;
            public string? comparison;
            public int value;
            public string Result;
            public override string ToString()
            {
                if (comparison != null)
                {
                    return $"{variable}{comparison}{value}:{Result}";
                }
                return Result;
            }

            public string GetResult(Dictionary<char, int> item)
            {
                if (comparison == null)
                {
                    return Result;
                }
                else if (comparison == "<")
                {
                    if (item[variable] < value)
                    {
                        return Result;
                    }
                }
                else if (comparison == ">")
                {
                    if (item[variable] > value)
                    {
                        return Result;
                    }
                }

                return null;
            }
        }

        class Workflow
        {
            public string Name;
            public List<Rule> Rules = new List<Rule>();

            public string GetResult(Dictionary<char, int> item)
            {
                foreach (var rule in Rules)
                {
                    var result = rule.GetResult(item);
                    if (result != null)
                    {
                        return result;
                    }
                }

                throw new Exception("Dang");
            }
        }

        public void Part1()
        {
            var lines = File.ReadAllLines("Inputs/Day19.txt");

            var workflows = new Dictionary<string, Workflow>();
            var items = new List<Dictionary<char, int>>();
            var parsingWorkflows = true;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    parsingWorkflows = false;
                    continue;
                }

                if (parsingWorkflows)
                {
                    // vns{m>1190:xxl,x>1874:R,a>2111:dfq,A}
                    var workflow = new Workflow { Name = line.Split("{")[0] };
                    var rules = line.Split("{")[1].Split("}")[0].Split(',');
                    foreach (var r in rules)
                    {
                        var s = Regex.Match(r, @"([xmas])([<>])(\d+):(\w+)");
                        if (s.Success)
                        {
                            workflow.Rules.Add(
                                new Rule
                                {
                                    variable = s.Groups[1].Value[0],
                                    comparison = s.Groups[2].Value[0].ToString(),
                                    value = Int32.Parse(s.Groups[3].Value),
                                    Result = s.Groups[4].Value
                                }
                            );
                        }
                        else
                        {
                            workflow.Rules.Add(new Rule { Result = r });
                        }
                    }
                    workflows[workflow.Name] = workflow;
                }
                else
                {
                    var split = line.Split("{")[1].Split("}")[0].Split(',');
                    var item = new Dictionary<char, int>();
                    foreach (var i in split)
                    {
                        item[i[0]] = Int32.Parse(i.Substring(2));
                    }
                    items.Add(item);
                }
            }

            long sum = 0;

            foreach (var item in items)
            {
                var workflow = workflows["in"];
                while (workflow != null)
                {
                    var result = workflow.GetResult(item);
                    if (result == "A")
                    {
                        sum += item.Values.Sum();
                        workflow = null;
                    }
                    else if (result == "R")
                    {
                        workflow = null;
                    }
                    else
                    {
                        workflow = workflows[result];
                    }
                }
            }

            Console.WriteLine(sum);
        }

        public void Part2(string filename)
        {
            var lines = File.ReadAllLines(filename);

            var workflows = new Dictionary<string, Workflow>();
            var parsingWorkflows = true;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    parsingWorkflows = false;
                    continue;
                }

                if (parsingWorkflows)
                {
                    // vns{m>1190:xxl,x>1874:R,a>2111:dfq,A}
                    var workflow = new Workflow { Name = line.Split("{")[0] };
                    var rules = line.Split("{")[1].Split("}")[0].Split(',');
                    foreach (var r in rules)
                    {
                        var s = Regex.Match(r, @"([xmas])([<>])(\d+):(\w+)");
                        if (s.Success)
                        {
                            workflow.Rules.Add(
                                new Rule
                                {
                                    variable = s.Groups[1].Value[0],
                                    comparison = s.Groups[2].Value[0].ToString(),
                                    value = Int32.Parse(s.Groups[3].Value),
                                    Result = s.Groups[4].Value
                                }
                            );
                        }
                        else
                        {
                            workflow.Rules.Add(new Rule { Result = r });
                        }
                    }
                    workflows[workflow.Name] = workflow;
                }
            }

            bool ApplyRuleToMinmax(
                Rule r,
                Dictionary<char, (int min, int max)> minmax,
                bool opposite = false
            )
            {
                if (r.comparison == "<" && !opposite)
                {
                    minmax[r.variable] = (
                        minmax[r.variable].min,
                        (int)Math.Min(minmax[r.variable].max, r.value - 1)
                    );
                }
                else if (r.comparison == ">" && opposite)
                {
                    minmax[r.variable] = (
                        minmax[r.variable].min,
                        (int)Math.Min(minmax[r.variable].max, r.value)
                    );
                }
                else if (r.comparison == ">" && !opposite)
                {
                    minmax[r.variable] = (
                        (int)Math.Max(minmax[r.variable].min, r.value + 1),
                        minmax[r.variable].max
                    );
                }
                else if (r.comparison == "<" && opposite)
                {
                    minmax[r.variable] = (
                        (int)Math.Max(minmax[r.variable].min, r.value),
                        minmax[r.variable].max
                    );
                }

                if (minmax[r.variable].min <= minmax[r.variable].max)
                {
                    return true;
                }
                return false;
            }

            // enumarate failure paths

            var rulePaths = new List<List<Rule>>();
            var queue =
                new List<(
                    Workflow wf,
                    int ruleIndex,
                    List<Rule> r,
                    List<string> r2,
                    Dictionary<char, (int min, int max)> minmax
                )>();
            var minmax = new Dictionary<char, (int min, int max)>();
            minmax['x'] = (min: 1, max: 4000);
            minmax['m'] = (min: 1, max: 4000);
            minmax['a'] = (min: 1, max: 4000);
            minmax['s'] = (min: 1, max: 4000);

            long validCount = 0;

            var validMinMaxes = new List<Dictionary<char, (int min, int max)>>();

            queue.Add((workflows["in"], 0, new List<Rule>(), new List<string>(), minmax));
            while (queue.Any())
            {
                var current = queue[0];
                queue.RemoveAt(0);

                var rules = new List<Rule>(current.r);

                //foreach (var rule in current.wf.Rules)
                //{

                var rule = current.wf.Rules[current.ruleIndex];

                var nextMinMax = new Dictionary<char, (int min, int max)>(current.minmax);
                if (rule.Result == "A")
                {


                    if (rule.comparison != null)
                    {
                        // on success, apply the rule and count
                        if (ApplyRuleToMinmax(rule, nextMinMax))
                        {
                            validMinMaxes.Add(nextMinMax);
                            validCount += nextMinMax.Values.Aggregate((long)1, (a, v) => a * ((long)v.max - (long)v.min + 1));
                        }


                        rules.Add(rule);


                        // failure. Stay in this workflow, but go to next rule
                        nextMinMax = new Dictionary<char, (int min, int max)>(current.minmax);
                        if (ApplyRuleToMinmax(rule, nextMinMax, true))
                        {
                            var newR2 = new List<string>(current.r2);
                            newR2.Add($"{current.wf.Name} {rule} fail");

                            queue.Add(
                                (current.wf, current.ruleIndex + 1, new List<Rule>(rules), newR2, nextMinMax)
                            );
                        }
                    }
                    else
                    {
                        validMinMaxes.Add(nextMinMax);
                        validCount += current.minmax.Values.Aggregate((long)1, (a, v) => a * ((long)v.max - (long)v.min + 1));
                    }
                    rulePaths.Add(new List<Rule>(rules));
                }
                else if (rule.Result == "R")
                {
                    if (rule.comparison != null)
                    {
                        // success case will fail, so don't bother

                        // failure. Stay in this workflow, but go to next rule
                        nextMinMax = new Dictionary<char, (int min, int max)>(current.minmax);
                        if (ApplyRuleToMinmax(rule, nextMinMax, true))
                        {
                            var newR2 = new List<string>(current.r2);
                            newR2.Add($"{current.wf.Name} {rule} failure");

                            queue.Add(
                                (current.wf, current.ruleIndex + 1, new List<Rule>(rules), newR2, nextMinMax)
                            );
                        }
                    }
                }
                else
                {
                    // We're going to the next rule, so this one failed. Flip the sign on it
                    /*rules.Add(new Rule
                    {
                        variable = rule.variable,
                        comparison = rule.comparison == "<" ? ">=" : "<='",
                        value = rule.value,
                        Result = rule.Result
                    });*/

                    if (rule.comparison == null)
                    {
                        rules.Add(rule);

                        var newR2 = new List<string>(current.r2);
                        newR2.Add($"{current.wf.Name} {rule} succeed");

                        queue.Add(
                            (workflows[rule.Result], 0, new List<Rule>(rules), newR2, current.minmax)
                        );
                    }
                    else
                    {

                        // success
                        if (ApplyRuleToMinmax(rule, nextMinMax))
                        {
                            var newR2 = new List<string>(current.r2);
                            newR2.Add($"{current.wf.Name} {rule} succeed");

                            rules.Add(rule);
                            queue.Add((workflows[rule.Result], 0, new List<Rule>(rules), newR2, nextMinMax));
                        }

                        // failure. Stay in this workflow, but go to next rule
                        nextMinMax = new Dictionary<char, (int min, int max)>(current.minmax);
                        if (ApplyRuleToMinmax(rule, nextMinMax, true))
                        {
                            var newR2 = new List<string>(current.r2);
                            newR2.Add($"{current.wf.Name} {rule} failure");

                            queue.Add(
                                (current.wf, current.ruleIndex + 1, new List<Rule>(rules), newR2, nextMinMax)
                            );
                        }
                    }
                }
                //}
            }

            
            /*
            foreach (var rulePath in rulePaths)
            {
                var minmax = new Dictionary<char, (int min, int max)>();
                minmax['x'] = (min: 1, max: 4000);
                minmax['m'] = (min: 1, max: 4000);
                minmax['a'] = (min: 1, max: 4000);
                minmax['s'] = (min: 1, max: 4000);

                foreach (var rule in rulePath)
                {
                    if (rule.comparison == "<")
                    {
                        minmax[rule.variable] = (minmax[rule.variable].min, (int)Math.Min(minmax[rule.variable].max, rule.value - 1));
                    }
                    else if (rule.comparison == "<=")
                    {
                        minmax[rule.variable] = (minmax[rule.variable].min, (int)Math.Min(minmax[rule.variable].max, rule.value));
                    }
                    else if (rule.comparison == ">")
                    {
                        minmax[rule.variable] = ((int)Math.Max(minmax[rule.variable].min, rule.value + 1), minmax[rule.variable].max);
                    }
                    else if (rule.comparison == ">=")
                    {
                        minmax[rule.variable] = ((int)Math.Max(minmax[rule.variable].min, rule.value), minmax[rule.variable].max);
                    }


                }

                validCount += minmax.Values.Aggregate((long)1, (a, v) => a * (long)v.max - (long)v.min + 1);

                Console.WriteLine($"X: {minmax['x'].min} {minmax['x'].max}");
                Console.WriteLine($"M: {minmax['m'].min} {minmax['m'].max}");
                Console.WriteLine($"A: {minmax['a'].min} {minmax['a'].max}");
                Console.WriteLine($"S: {minmax['s'].min} {minmax['s'].max}");
                Console.WriteLine();



            }
            */
            
            bool Contains(Dictionary<char, (int min, int max)> a, Dictionary<char, (int min, int max)> b)
            {
                foreach (var kv in a)
                {
                    if (kv.Value.min > b[kv.Key].min || kv.Value.max < b[kv.Key].max)
                    {
                        return false;
                    }
                }

                return true;
            }

            long validCount2 = 0;


            foreach (var m in validMinMaxes)
            {
                Console.WriteLine($"X: {m['x'].min} {m['x'].max}");
                Console.WriteLine($"M: {m['m'].min} {m['m'].max}");
                Console.WriteLine($"A: {m['a'].min} {m['a'].max}");
                Console.WriteLine($"S: {m['s'].min} {m['s'].max}");
                Console.WriteLine(m.Values.Aggregate((long)1, (a, v) => a * ((long)v.max - (long)v.min + 1)));
                

                if (validMinMaxes.Any(m2 => m != m2 && Contains(m2, m)))
                {
                    Console.WriteLine("contained in another");
                }
                else
                {
                    validCount2 += m.Values.Aggregate((long)1, (a, v) => a * ((long)v.max - (long)v.min + 1));
                }
                Console.WriteLine();
            }

            Console.WriteLine(validCount);
            int l = 56;

            // 484078879986150
            // 1802577951901556
            // 878154863249802
            // 455492475000000

            // 167409079868000
            // 
            //  15320205000000
            //  20576430000000
            //  35328000000000
            //  85632000000000
            //  42635840000000
            // 156928000000000
            //  99072000000000
        }
    }
}
