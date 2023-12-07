using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace AdventOfCode2023
{
    internal class Day7
    {
        private static char[] Cards = new char[]
        {
            'A',
            'K',
            'Q',
            'J',
            'T',
            '9',
            '8',
            '7',
            '6',
            '5',
            '4',
            '3',
            '2'
        };
        private static char[] Cards2 = new char[]
        {
            'A',
            'K',
            'Q',
            'T',
            '9',
            '8',
            '7',
            '6',
            '5',
            '4',
            '3',
            '2',
            'J'
        };

        private static int Score(string hand)
        {
            var groups = hand.GroupBy(c => c).OrderByDescending(g => g.Count()).ToList();
            if (groups[0].Count() == 5)
                return 6;
            if (groups[0].Count() == 4)
                return 5;
            if (groups[0].Count() == 3 && groups[1].Count() == 2)
                return 4;
            if (groups[0].Count() == 3)
                return 3;
            if (groups[0].Count() == 2 && groups[1].Count() == 2)
                return 2;
            if (groups[0].Count() == 2)
                return 1;
            return 0;
        }

        private static int Score2(string hand)
        {
            if (hand.IndexOf('J') < 0)
                return Score(hand);

            var jCount = hand.Count(c => c == 'J');
            var remainingCards = hand.Replace("J", "");

            if (remainingCards.Length == 0)
                return Score("AAAAA");

            var groups = remainingCards.GroupBy(c => c).OrderByDescending(g => g.Count()).ToList();
            var bestCard = groups[0].Key;

            return Score(remainingCards + new string(bestCard, jCount));
        }

        public class HandComparer : IComparer<string>
        {
            public int Compare(string hand1, string hand2)
            {
                var score1 = Score(hand1);
                var score2 = Score(hand2);
                if (score1 < score2)
                    return -1;
                if (score1 > score2)
                    return 1;

                for (int i = 0; i < hand1.Length; i++)
                {
                    var card1Rank = Array.IndexOf(Cards, hand1[i]);
                    var card2Rank = Array.IndexOf(Cards, hand2[i]);
                    if (card1Rank != card2Rank)
                    {
                        if (card1Rank < card2Rank)
                            return 1;
                        if (card1Rank > card2Rank)
                            return -1;
                    }
                }

                return 0;
            }
        }

        public class HandComparer2 : IComparer<string>
        {
            public int Compare(string hand1, string hand2)
            {
                var score1 = Score2(hand1);
                var score2 = Score2(hand2);
                if (score1 < score2)
                    return -1;
                if (score1 > score2)
                    return 1;

                for (int i = 0; i < hand1.Length; i++)
                {
                    var card1Rank = Array.IndexOf(Cards2, hand1[i]);
                    var card2Rank = Array.IndexOf(Cards2, hand2[i]);
                    if (card1Rank != card2Rank)
                    {
                        if (card1Rank < card2Rank)
                            return 1;
                        if (card1Rank > card2Rank)
                            return -1;
                    }
                }

                return 0;
            }
        }

        public void Part1()
        {
            var lines = File.ReadAllLines("Inputs/Day7.txt");
            var games = lines
                .Select(l => (hand: l.Split(" ")[0], bind: int.Parse(l.Split(" ")[1])))
                .ToList();

            var sorted = games.OrderBy(g => g.hand, new HandComparer()).ToList();

            var result = sorted.Select((g, i) => g.bind * (i + 1)).ToList();

            Console.WriteLine(result.Sum());
        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Inputs/Day7.txt");
            var games = lines
                .Select(l => (hand: l.Split(" ")[0], bind: int.Parse(l.Split(" ")[1])))
                .ToList();

            var sorted = games.OrderBy(g => g.hand, new HandComparer2()).ToList();

            var result = sorted.Select((g, i) => g.bind * (i + 1)).ToList();

            Console.WriteLine(result.Sum());
        }
    }
}
