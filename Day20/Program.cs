using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
        sides.Add(string.Join(string.Empty, lines[1..].Select(l => l[0])));
        sides.Add(string.Join(string.Empty, lines[1..].Select(l => l[^1])));
        sides.AddRange(sides.Select(s => new string(s.Reverse().ToArray())).ToArray());

        return result;
    }

    private static IEnumerable<Tile> FindCornerTiles(this IEnumerable<Tile> tiles) =>
        tiles.Select(t1 => new
        {
            Tile = t1,
            MatchingTiles = tiles.Count(t2 => t1.ID != t2.ID && t1.Sides.Count(s1 => t2.Sides.Any(s2 => s1 == s2)) == 2)
        })
        .Where(t => t.MatchingTiles == 2)
        .Select(t => t.Tile);

    private static List<Tile> ParseTiles(string[] lines)
    {
        var tiles = new List<Tile>();
        for (var index = 0; index < lines.Length; index += 12)
        {
            var tile = ParseTile(lines[index..(index + 11)]);
            tiles.Add(tile);
        }

        return tiles;
    }

    public static long SolvePuzzle1(string[] lines) =>
        ParseTiles(lines).FindCornerTiles().Aggregate(1L, (acc, item) => acc *= item.ID);

    public static int SolvePuzzle2(string[] lines)
    {
        var tiles = ParseTiles(lines);
        var sideLength = (int)Math.Sqrt(tiles.Count);
        var tileIDs = new int[tiles.Count];

        static IEnumerable<Tile> GetPossibleNeighbours(IEnumerable<Tile> tiles, IEnumerable<int> positionedTiles, int ID) =>
            tiles.Where(nt => nt.ID != ID && !positionedTiles.Any(pt => nt.ID == pt) 
                && nt.Sides.Any(nts => tiles.First(t => t.ID == ID).Sides.Any(luct => luct == nts)));

        var leftUpperCornerTile = tiles.FindCornerTiles().First();
        tileIDs[0] = leftUpperCornerTile.ID;
        var leftUpperNeighbours = GetPossibleNeighbours(tiles, tileIDs, leftUpperCornerTile.ID);
        tileIDs[1] = leftUpperNeighbours.First().ID;
        tileIDs[sideLength] = leftUpperNeighbours.Last().ID;

        for (var ix = 1; ix < tiles.Count - sideLength; ix++)
        {
            var possibleNeighbours = GetPossibleNeighbours(tiles, tileIDs, tileIDs[ix]).ToArray();
            if (possibleNeighbours.Length > 1)
            {
                var below = possibleNeighbours.Intersect(GetPossibleNeighbours(tiles, tileIDs, tileIDs[ix + sideLength - 1]));
                tileIDs[ix + sideLength] = below.First().ID;
                tileIDs[ix + 1] = possibleNeighbours.First(n => n.ID != tileIDs[ix + sideLength]).ID;
            }
            else tileIDs[ix + sideLength] = possibleNeighbours.First().ID;
        }




        throw new NotImplementedException();
    }
}