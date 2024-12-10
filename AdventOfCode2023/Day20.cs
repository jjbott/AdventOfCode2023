using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2023
{
    internal class Day20
    {
        public static Dictionary<bool, int> PulseCount = new Dictionary<bool, int> { { false, 0 }, { true, 0 } };

        abstract class Node
        {
            public List<string> OutputNodesNames = new List<string>();
            public List<Node> OutputNodes = new List<Node>();

            public string Name = "";

            abstract public IEnumerable<(bool state, Node to, Node from)> ReceivePulse(bool high, Node from);

            protected IEnumerable<(bool state, Node to, Node from)> SendOutput(bool high)
            {
                foreach (var node in OutputNodes)
                {
                    PulseCount[high]++;
                    yield return (high, node, this);                    
                }
            }
        }

        class Broadcaster : Node
        {
            public Broadcaster()
            {
                Name = "broadcaster";
            }

            public override IEnumerable<(bool state, Node to, Node from)> ReceivePulse(bool high, Node from)
            {
                return SendOutput(high);           
            }
        }

        class FlipFlop : Node
        {
            public bool IsOn = false;

            public FlipFlop(string name)
            {
                Name = name;
            }

            public override IEnumerable<(bool state, Node to, Node from)> ReceivePulse(bool high, Node from)
            {
                if (Name == "bp")
                {
                    int g = 56;
                }

                if (!high)
                {
                    IsOn = !IsOn;

                    return SendOutput(IsOn);
                }

                return Enumerable.Empty<(bool state, Node to, Node from)>();
            }
        }

        class Conjunction : Node
        {
            public Conjunction(string name)
            {
                Name = name;
            }

            public List<Node> InputNodes = new List<Node>();
            public readonly Dictionary<Node, bool> _inputNodeState = new Dictionary<Node, bool>();

            public override IEnumerable<(bool state, Node to, Node from)> ReceivePulse(bool high, Node from)
            {
                if ( OutputNodes.FirstOrDefault()?.Name == "rx" && high)
                {
                    int g = 56;
                }

                // fill out _inputNodeState on first pulse
                if (!_inputNodeState.Any())
                {
                    foreach (var n in InputNodes)
                    {
                        _inputNodeState[n] = false;
                    }
                }

                _inputNodeState[from] = high;

                var allHigh = _inputNodeState.Values.All(v => v);
                return SendOutput(!allHigh);
            }
        }

        class Output : Node
        {
            public Output(string name)
            {
                Name = name;
            }

            public override IEnumerable<(bool state, Node to, Node from)> ReceivePulse(bool high, Node from)
            {
                return Enumerable.Empty<(bool state, Node to, Node from)>();
            }
        }

        public void Part1()
        {
            PulseCount = new Dictionary<bool, int> { { false, 0 }, { true, 0 } };

            var lines = File.ReadAllLines("Inputs/Day20.txt");

            Broadcaster broadcaster = null;
            var nodes = new Dictionary<string, Node>();
            var conjunctions = new Dictionary<string, Conjunction>();

            foreach (var l in lines)
            {
                var split = l.Split(" -> ");
                if (split[0] == "broadcaster")
                {
                    broadcaster = new Broadcaster();
                    nodes[broadcaster.Name] = broadcaster;
                    broadcaster.OutputNodesNames = split[1].Split(", ").ToList();
                }
                else
                {
                    Node node = null;
                    if (split[0][0] == '%')
                    {
                        node = new FlipFlop(split[0].Substring(1));
                    }
                    else if (split[0][0] == '&')
                    {
                        node = new Conjunction(split[0].Substring(1));
                        conjunctions[node.Name] = (Conjunction)node;
                    }
                    nodes[node.Name] = node;
                    node.OutputNodesNames = split[1].Split(", ").ToList();
                }
            }

            // resolve output references
            foreach (var n in nodes)
            {
                n.Value.OutputNodes = n.Value.OutputNodesNames.Select(name => nodes.ContainsKey(name) ? nodes[name] : new Output(name)).ToList();
                foreach(var o in n.Value.OutputNodes)
                {
                    if (o is Conjunction)
                    {
                        ((Conjunction)o).InputNodes.Add(n.Value);
                    }
                }
            }            

            for (int i = 0; i < 1000; i++)
            {
                var pulseQueue = new Queue<(bool state, Node to, Node from)>();
                pulseQueue.Enqueue((false, broadcaster, null));
                int j = 0;
                while(pulseQueue.Count > 0)
                {
                    ++j;
                    if ( pulseQueue.Count == 1)
                    {
                        int g = 56;
                    }
                    var pulse = pulseQueue.Dequeue();

                    foreach (var r in pulse.to.ReceivePulse(pulse.state, pulse.from))
                    {
                        pulseQueue.Enqueue(r);
                    }
                }
                Console.WriteLine(j);
            }

            Console.WriteLine(PulseCount[false] + 1000);
            Console.WriteLine(PulseCount[true]);

            Console.WriteLine();
            Console.WriteLine((PulseCount[false] + 1000) * PulseCount[true]);

        }

        public void Part2()
        {
            PulseCount = new Dictionary<bool, int> { { false, 0 }, { true, 0 } };

            var lines = File.ReadAllLines("Inputs/Day20.txt");

            Broadcaster broadcaster = null;
            var nodes = new Dictionary<string, Node>();
            var conjunctions = new Dictionary<string, Conjunction>();

            foreach (var l in lines)
            {
                var split = l.Split(" -> ");
                if (split[0] == "broadcaster")
                {
                    broadcaster = new Broadcaster();
                    nodes[broadcaster.Name] = broadcaster;
                    broadcaster.OutputNodesNames = split[1].Split(", ").ToList();
                }
                else
                {
                    Node node = null;
                    if (split[0][0] == '%')
                    {
                        node = new FlipFlop(split[0].Substring(1));
                    }
                    else if (split[0][0] == '&')
                    {
                        node = new Conjunction(split[0].Substring(1));
                        conjunctions[node.Name] = (Conjunction)node;
                    }
                    nodes[node.Name] = node;
                    node.OutputNodesNames = split[1].Split(", ").ToList();
                }
            }

            string BuildStateString()
            {
                var sb = new StringBuilder();
                foreach (var n in nodes.Values.OrderBy(n => n.Name))
                {
                    if (n is FlipFlop)
                    {
                        sb.Append(n.Name);
                        sb.Append(':');
                        sb.Append(((FlipFlop)n).IsOn ? "1" : "0");
                        sb.Append(',');
                    }
                    else if (n is Conjunction)
                    {
                        sb.Append(n.Name);
                        sb.Append(':');
                        sb.Append(string.Join('|', ((Conjunction)n)._inputNodeState.OrderBy(kv => kv.Key.Name).Select(kv => kv.Value ? "1" : "0")));
                        sb.Append(',');
                    }
                }

                return sb.ToString();
            }

            void ResetStateFromString(string state)
            {
                var split = state.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var s in split)
                {
                    var split2 = s.Split(':');
                    var node = nodes[split2[0]];

                    if (node is FlipFlop)
                    {
                        ((FlipFlop)node).IsOn = split2[1] == "1";
                    }
                    else if (node is Conjunction)
                    {
                        var conjunction = (Conjunction)node;
                        var split3 = split2[1].Split('|', StringSplitOptions.RemoveEmptyEntries);

                        var inputs = conjunction._inputNodeState.OrderBy(kv => kv.Key.Name).ToList();
                        for (int i = 0; i < split3.Length; i++)
                        {
                            conjunction._inputNodeState[inputs[i].Key] = split3[i] == "1";
                        }
                    }
                }
            }

            string FilterState(string state, List<string> affectedNodes)
            {
                var newState = new List<string>();

                foreach (var s in state.Split(','))
                { 
                    var split2 = s.Split(':');
                    var nodeName = split2[0];
                    if (affectedNodes.Contains(nodeName))
                    {
                        if (nodes[nodeName] is FlipFlop || nodes[nodeName] is Conjunction)
                        {
                            newState.Add(s);
                        }
                    }
                }

                return string.Join(':', newState);
            }

            

            // resolve output references
            foreach (var n in nodes)
            {
                n.Value.OutputNodes = n.Value.OutputNodesNames.Select(name => nodes.ContainsKey(name) ? nodes[name] : new Output(name)).ToList();
                foreach (var o in n.Value.OutputNodes)
                {
                    if (o is Conjunction)
                    {
                        ((Conjunction)o).InputNodes.Add(n.Value);
                    }
                }
            }

            var stateMap = new Dictionary<(bool pulse, Node to, Node from, string state, string queueState), (string state, List<string> nodes)>();

            string QueueState(List<(bool pulse, Node to, Node from)> queue)
            {
                var sb = new StringBuilder();
                foreach (var q in queue)
                {
                    sb.Append(q.pulse ? "1" : "0");
                    sb.Append(q.from?.Name ?? "");
                    sb.Append('>');
                    sb.Append(q.to?.Name ?? "");
                    sb.Append(',');
                    //sb.Append(q.State);
                }

                return sb.ToString();
            }

            (string state, List<String> nodes) GetNextState((bool pulse, Node to, Node from) current, List<(bool pulse, Node to, Node from)> queue)
            {
                var state = BuildStateString();
                var queueState = QueueState(queue);

                if (!current.pulse && current.to is Output)
                {
                    throw new Exception("yay");
                }

                
                if (stateMap.TryGetValue((current.pulse, current.to, current.from, state, queueState), out var newState))
                {
                    return newState;
                }
                else
                {
                    var affectedNodes = new List<string> { current.to.Name };
                    if (current.to is Conjunction)
                    {
                        affectedNodes.AddRange(((Conjunction)current.to).InputNodes.Select(n => n.Name));
                    }

                    List<(bool pulse, Node to, Node from)> newQueue = new List<(bool pulse, Node to, Node from)>(queue);

                    newQueue.AddRange(current.to.ReceivePulse(current.pulse, current.from));

                    (string state, List<String> nodes) nextState;

                    if (newQueue.Any())
                    {
                        var newCurrent = newQueue[0];
                        newQueue.RemoveAt(0);

                        nextState = GetNextState(newCurrent, newQueue);

                    }
                    else
                    {
                        nextState = (BuildStateString(), affectedNodes);
                    }

                    affectedNodes.AddRange(nextState.nodes);
                    affectedNodes = affectedNodes.Distinct().ToList();

                    nextState = (FilterState(nextState.state, affectedNodes), affectedNodes.Distinct().ToList());

                    stateMap[(current.pulse, current.to, current.from, FilterState(state, affectedNodes), queueState)] = nextState;


                    return nextState;
                }
            }

            int i = 0;
            while(true)
            {
                // new plan:
                // Change to recursive, passing queue to next step
                // At each step
                // * If new queue is empty, return state + node name
                // * Else
                //     * If current state and queue is in cache, return new state
                //         ******** If I filter states below, I may never find a match here. Maybe try without filtering? ********
                //         * don't think I can prefilter here, because I don't know all involved nodes yet
                //         * I could compare all cached filtered states to current state (ignoring any nodes not in the cached state) but that could be gross if there are a trillion states
                //     * Add new pulses to queue, call next step
                //     * Add current state + queue to cache, resolving to new state
                //         * Filter old/state to only include involved nodes (from return value + current node). 
                //     * return filtered new state + involved nodes (from return value + current node)
                //

                ++i;
                var pulseQueue = new List<(bool pulse, Node to, Node from)>();

                try
                {
                    GetNextState((false, broadcaster, null), pulseQueue);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(i);
                }
            }
        }

        public void Part2b()
        {
            PulseCount = new Dictionary<bool, int> { { false, 0 }, { true, 0 } };

            var lines = File.ReadAllLines("Inputs/Day20.txt");

            Broadcaster broadcaster = null;
            var nodes = new Dictionary<string, Node>();
            var conjunctions = new Dictionary<string, Conjunction>();

            foreach (var l in lines)
            {
                var split = l.Split(" -> ");
                if (split[0] == "broadcaster")
                {
                    broadcaster = new Broadcaster();
                    nodes[broadcaster.Name] = broadcaster;
                    broadcaster.OutputNodesNames = split[1].Split(", ").ToList();
                }
                else
                {
                    Node node = null;
                    if (split[0][0] == '%')
                    {
                        node = new FlipFlop(split[0].Substring(1));
                    }
                    else if (split[0][0] == '&')
                    {
                        node = new Conjunction(split[0].Substring(1));
                        conjunctions[node.Name] = (Conjunction)node;
                    }
                    nodes[node.Name] = node;
                    node.OutputNodesNames = split[1].Split(", ").ToList();
                }
            }

            nodes["rx"] = new Output("rx");

            // resolve output references
            foreach (var n in nodes)
            {
                n.Value.OutputNodes = n.Value.OutputNodesNames.Select(name => nodes.ContainsKey(name) ? nodes[name] : new Output(name)).ToList();
                foreach (var o in n.Value.OutputNodes)
                {
                    if (o is Conjunction)
                    {
                        ((Conjunction)o).InputNodes.Add(n.Value);
                    }
                }
            }

            //List<bool> rxBits = new List<bool>();
            Dictionary<int, List<int>> bitIndexes = new Dictionary<int, List<int>>();
            Dictionary<int, int> bitPeriods = new Dictionary<int, int>();

            var lastNode = (Conjunction)(nodes.Where(kv => kv.Value.OutputNodes.FirstOrDefault() == nodes["rx"]).Single().Value);

            void UpdateBits(int index)
            {
                var inputNodes = lastNode.InputNodes.OrderBy(n => n.Name).ToList();
                for(int i = 0; i < inputNodes.Count; ++i)
                {
                    var bitState = lastNode._inputNodeState[inputNodes[i]];
                    if (bitState)
                    {
                        if ( !bitIndexes.ContainsKey(i))
                        {
                            bitIndexes[i] = new List<int>();
                        }

                        if (bitIndexes[i].LastOrDefault() == index )
                        {
                            continue;
                        }

                        bitIndexes[i].Add(index);

                        if (!bitPeriods.ContainsKey(i) && bitIndexes[i].Count >=3 )
                        {
                            var diff1 = bitIndexes[i].Last() - bitIndexes[i][bitIndexes[i].Count - 2];
                            var diff2 = bitIndexes[i][bitIndexes[i].Count - 2] - bitIndexes[i][bitIndexes[i].Count - 3];
                            if ( diff2 == diff1)
                            {
                                bitPeriods[i] = diff1;
                            }
                        }
                    }
                }

                if ( bitPeriods.Count == inputNodes.Count)
                {
                    Console.WriteLine(Utility.LCM(bitPeriods.Values.Select(v => (long)v)));
                }
                /*
                var newRxBits = lastNode.InputNodes.OrderBy(n => n.Name).Select(n => lastNode._inputNodeState[n]).ToList();
                var oldBits = string.Join(" ", rxBits.Select(b => b ? "1" : "0"));
                var newBits = string.Join(" ", newRxBits.Select(b => b ? "1" : "0"));
                rxBits = newRxBits;
                if (oldBits != newBits)
                {
                    Console.WriteLine($"{i}: {newBits}");
                }
                */
            }            

            int i = 0;
            while(true)
            {
                i++;
                var pulseQueue = new Queue<(bool state, Node to, Node from)>();
                pulseQueue.Enqueue((false, broadcaster, null));
                int j = 0;
                while (pulseQueue.Count > 0)
                {
                    ++j;
                    if (pulseQueue.Count == 1)
                    {
                        int g = 56;
                    }
                    var pulse = pulseQueue.Dequeue();

                    foreach (var r in pulse.to.ReceivePulse(pulse.state, pulse.from))
                    {
                        pulseQueue.Enqueue(r);
                    }

                    if ( pulse.to == lastNode)
                    {
                        UpdateBits(i);
                    }
                }

                if (i == 1000)
                {
                    Console.WriteLine(PulseCount[false] + 1000);
                    Console.WriteLine(PulseCount[true]);

                    Console.WriteLine();
                    Console.WriteLine((PulseCount[false] + 1000) * PulseCount[true]);
                }
            }

            Console.WriteLine(PulseCount[false] + 1000);
            Console.WriteLine(PulseCount[true]);

            Console.WriteLine();
            Console.WriteLine((PulseCount[false] + 1000) * PulseCount[true]);
        }
    }
}
