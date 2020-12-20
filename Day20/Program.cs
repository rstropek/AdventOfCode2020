using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Day20.Tests")]

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines)}");

public static class Solution
{
    private record Tile(int ID, List<string> Sides);

    private static Tile ParseTile(string[] lines)
    {
        var sides = new List<string>();
        var result = new Tile(int.Parse(lines[0][5..^1]), sides);

        sides.Add(lines[1]);
        sides.Add(lines[^1]);
        sides.Add(string.Join("", lines[1..].Select(l => l[0])));
        sides.Add(string.Join("", lines[1..].Select(l => l[^1])));
        sides.AddRange(sides.Select(s => new string(s.Reverse().ToArray())).ToArray());

        return result;
    }

    public static long SolvePuzzle1(string[] lines)
    {
        var tiles = new List<Tile>();
        for (var index = 0; index < lines.Length; index += 12)
        {
            var tile = ParseTile(lines[index..(index + 11)]);
            tiles.Add(tile);
        }

        return tiles.Select(t1 => new
        {
            Tile = t1,
            MatchingTiles = tiles.Count(t2 => t1.ID != t2.ID && t1.Sides.Count(s1 => t2.Sides.Any(s2 => s1 == s2)) == 2)
        })
        .Where(t => t.MatchingTiles == 2)
        .Aggregate(1L, (acc, item) => acc *= item.Tile.ID);
    }

    public static int SolvePuzzle2(string[] lines)
    {
        throw new NotImplementedException();
    }
}