using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines)}");

public static class Solution
{
    private static IEnumerable<int> GetIDs(string[] lines) =>
        lines.Select(l => l.Take(7).Aggregate(0, (row, c) => (row << 1) | (c == 'B' ? 1 : 0)) * 8
            + l.Skip(7).Aggregate(0, (seat, c) => (seat << 1) | (c == 'R' ? 1 : 0)));

    public static int SolvePuzzle1(string[] lines) => GetIDs(lines).Max();

    public static int SolvePuzzle2(string[] lines)
    {
        var ids = GetIDs(lines).ToArray();
        return Enumerable.Range(ids.Min(), ids.Length).Single(val => !ids.Any(id => id == val));
    }
}