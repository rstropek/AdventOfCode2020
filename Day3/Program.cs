using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var data = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(data)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(data)}");

public static class Solution
{
    public static long SolvePuzzle1(IReadOnlyList<string> lines) =>
        Solve(lines, 3, 1);

    public static long Solve(IReadOnlyList<string> lines, int offsetX, int offsetY)
    {
        int row = 0, lineLength = lines[0].Length;
        return lines
            .Select((l, index) => index > 0 && index % offsetY == 0 && l[(row += offsetX) % lineLength] == '#')
            .Count(r => r);
    }

    public static long SolvePuzzle2(IReadOnlyList<string> lines)
    {
        return new (int x, int y)[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) }
            .Select(slope => Solve(lines, slope.x, slope.y))
            .Aggregate(1L, (agg, n) => agg * n);
    }
}