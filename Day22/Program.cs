using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Day22.Tests")]

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines)}");

public static class Solution
{
    internal static List<int>[] Parse(string[] lines) =>
        new[] {
            lines.Skip(1).TakeWhile(l => l.Length > 0).Select(l => int.Parse(l)).ToList(),
            lines.SkipWhile(l => l.Length > 0).Skip(2).Select(l => int.Parse(l)).ToList()
        };

    internal static (int winner, int loser) Solve(List<int>[] decks, bool recurse = false)
    {
        var history = new HashSet<string>[] { new(), new() };
        //List<List<int>[]> history = new();
        while (!decks.Any(d => d.Count == 0))
        {
            //if (recurse && history.Any(h => h.Select((hd, ix) => hd.SequenceEqual(decks[ix])).Any(r => r))) return (1, 0);
            //if (recurse && history.Any(h => h[0].SequenceEqual(decks[0]) || h[1].SequenceEqual(decks[1]))) return (1, 0);
            //history.Add(new[] { new List<int>(decks[0]), new List<int>(decks[1]) });
            var d0s = string.Join(',', decks[0]);
            var d1s = string.Join(',', decks[1]);
            if (recurse && history[0].Contains(d0s)) return (1, 0);
            if (recurse && history[1].Contains(d1s)) return (1, 0);
            history[0].Add(d0s);
            history[1].Add(d1s);

            (int winner, int loser) result;
            if (recurse && decks.All(d => d.Count > d[0])) result = Solve(decks.Select(d => d.Skip(1).Take(d[0]).ToList()).ToArray(), true);
            else result = decks[0][0] > decks[1][0] ? (0, 1) : (1, 0);
            decks[result.winner].AddRange(new[] { decks[result.winner][0], decks[result.loser][0] });
            decks[result.loser].RemoveAt(0);
            decks[result.winner].RemoveAt(0);
        }

        return decks[0].Count == 0 ? (1, 0) : (0, 1);
    }

    public static long SolvePuzzle1(string[] lines)
    {
        var decks = Parse(lines);
        return decks[Solve(decks).winner].Reverse<int>().Select((c, ix) => c * (ix + 1)).Sum();
    }

    public static long SolvePuzzle2(string[] lines)
    {
        var decks = Parse(lines);
        return decks[Solve(decks, true).winner].Reverse<int>().Select((c, ix) => c * (ix + 1)).Sum();
    }
}