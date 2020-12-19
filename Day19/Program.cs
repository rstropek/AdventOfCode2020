using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Day19.Tests")]

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines)}");

public static class Solution
{
    private abstract record Rule
    {
        /// <summary>
        /// Verifies whether message fulfills the rule
        /// </summary>
        /// <param name="rules">Dictionary of all rules</param>
        /// <param name="message">Message to verify</param>
        /// <param name="result"><code>true</code> if message fulfilles rule, otherwise <code>false</code></c></param>
        /// <returns>
        /// If message fulfills the rule, the remaining message is returned, otherwise the original message.
        /// </returns>
        public abstract ReadOnlySpan<char> Verify(Dictionary<int, Rule> rules, ReadOnlySpan<char> message, out bool result);
    }

    private record Sequence(int ID, IReadOnlyList<int> ChildrenIDs) : Rule
    {
        public override ReadOnlySpan<char> Verify(Dictionary<int, Rule> rules, ReadOnlySpan<char> message, out bool result)
        {
            result = true;
            var remainingMessage = message;
            for (var i = 0; i < ChildrenIDs.Count; i++)
            {
                remainingMessage = rules[ChildrenIDs[i]].Verify(rules, remainingMessage, out var verifyResult);
                if (remainingMessage.Length == 0 && i != ChildrenIDs.Count - 1)
                {
                    // Reached end of message but not end of list of child rule IDs -> cannot be a match.
                    result = false;
                    return message;
                }

                result &= verifyResult;
            }

            return result ? remainingMessage : message;
        }
    }

    private record Or(int ID, Sequence Left, Sequence Right) : Rule
    {
        public override ReadOnlySpan<char> Verify(Dictionary<int, Rule> rules, ReadOnlySpan<char> message, out bool result)
        {
            var nextMessage = Left.Verify(rules, message, out result);
            if (result) return nextMessage;

            nextMessage = Right.Verify(rules, message, out result);
            if (result) return nextMessage;

            return message;
        }
    }

    private record Zero() : Rule
    {
        public override ReadOnlySpan<char> Verify(Dictionary<int, Rule> rules, ReadOnlySpan<char> message, out bool result)
        {
            ReadOnlySpan<char> nextMessage = message;

            // Count the number of 42 fulfillments
            var fourtytwo = rules[42];
            var countFourtytwo = 0;
            do
            {
                nextMessage = fourtytwo.Verify(rules, nextMessage, out result);
                if (result) countFourtytwo++;
            }
            while (result && nextMessage.Length > 0);

            if (nextMessage.Length == 0)
            {
                // Reached end of message, but here must be at least on 31 fulfillment -> rule not fulfilled
                result = false;
                return message;
            }

            // Count the number of 31 fulfillments
            var thirtyone = rules[31];
            var countThirtyone = 0;
            do
            {
                nextMessage = thirtyone.Verify(rules, nextMessage, out result);
                if (result) countThirtyone++;
            }
            while (result && nextMessage.Length > 0);

            // At least one 31 and 42 fulfillments must be at least one more than 31s.
            result = countThirtyone > 0 && countFourtytwo > countThirtyone;
            return result ? nextMessage : message;
        }
    }

    private record Letter(int ID, char ExpectedLetter) : Rule
    {
        public override ReadOnlySpan<char> Verify(Dictionary<int, Rule> _, ReadOnlySpan<char> message, out bool result)
        {
            result = message[0] == ExpectedLetter;
            return result ? message[1..] : message;
        }
    }

    internal static IReadOnlyList<int> BuildSequence(ReadOnlySpan<char> l)
    {
        var result = new List<int>();
        while (l.Length > 0)
        {
            var nextBlank = l.IndexOf(' ');
            if (nextBlank == -1)
            {
                result.Add(int.Parse(l));
                return result;
            }
            else
            {
                result.Add(int.Parse(l[..nextBlank]));
                l = l[(nextBlank + 1)..];
            }
        }

        return result;
    }

    public static int SolvePuzzle1(string[] lines, bool replaceZero = false)
    {
        var i = ParseRules(lines, replaceZero, out var rules);
        var ruleZero = rules[0];

        var correct = 0;
        for (i++; i < lines.Length; i++)
        {
            var remaining = ruleZero.Verify(rules, lines[i], out var result);
            if (result && remaining.Length == 0) correct++;
        }

        return correct;
    }

    private static int ParseRules(string[] lines, bool replaceZero, out Dictionary<int, Rule> rules)
    {
        var i = 0;
        rules = new Dictionary<int, Rule>();
        for (; lines[i].Length > 0; i++)
        {
            var l = lines[i].AsSpan();
            var colonIx = l.IndexOf(':');
            var id = int.Parse(l[..colonIx]);

            // Handle replacement of zero rule for second puzzle
            if (replaceZero)
            {
                if (id is 8 or 11) continue;
                if (id == 0) { rules[id] = new Zero(); continue; }
            }

            l = l[(colonIx + 2)..];
            if (l[0] == '"') rules[id] = new Letter(id, l[1]);
            else
            {
                var pipeIx = l.IndexOf('|');
                if (pipeIx != -1)
                {
                    var left = new Sequence(-1, BuildSequence(l[..(pipeIx - 1)].ToString()));
                    var right = new Sequence(-1, BuildSequence(l[(pipeIx + 2)..].ToString()));
                    rules[id] = new Or(id, left, right);
                }
                else
                {
                    rules[id] = new Sequence(id, BuildSequence(l.ToString()));
                }
            }
        }

        return i;
    }

    public static int SolvePuzzle2(string[] expressionStrings) =>
        SolvePuzzle1(expressionStrings, true);
}