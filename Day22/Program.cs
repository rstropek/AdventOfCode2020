using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Day22.Tests")]

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle(lines, true)}");

public static class Solution
{
    internal static Queue<int>[] Parse(string[] lines) =>
        new[] {
            new Queue<int>(lines.Skip(1).TakeWhile(l => l.Length > 0).Select(l => int.Parse(l))),
            new Queue<int>(lines.SkipWhile(l => l.Length > 0).Skip(2).Select(l => int.Parse(l)))
        };

    internal static (int winner, int loser) Solve(Queue<int>[] decks, bool recurse = false)
    {
        var history = new HashSet<string>[] { new(), new() };
        while (decks.All(d => d.Count > 0))
        {
            var historyDecks = decks.Select(d => string.Join(',', d)).ToArray();
            if (recurse && history.Any(h => historyDecks.Any(hd => h.Contains(hd)))) return (0, 1);
            history[0].Add(historyDecks[0]);
            history[1].Add(historyDecks[1]);

            (int winner, int loser) result;
            var drawnCards = decks.Select(d => d.Dequeue()).ToArray();
            if (recurse && decks[0].Count >= drawnCards[0] && decks[1].Count >= drawnCards[1])
            {
                result = Solve(decks.Select((d, ix) => new Queue<int>(d.Take(drawnCards[ix]))).ToArray(), true);
            }
            else result = drawnCards[0] > drawnCards[1] ? (0, 1) : (1, 0);
            decks[result.winner].Enqueue(drawnCards[result.winner]);
            decks[result.winner].Enqueue(drawnCards[result.loser]);
        }

        return decks[0].Count == 0 ? (1, 0) : (0, 1);
    }

    public static long SolvePuzzle(string[] lines, bool recurse = false)
    {
        var decks = Parse(lines);
        return decks[Solve(decks, recurse).winner].Reverse<int>().Select((c, ix) => c * (ix + 1)).Sum();
    }
}