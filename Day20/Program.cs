using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Day20.Tests")]

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines)}");

internal record Tile(int ID, string[] Lines)
{
    public Tile RotateClockwise()
    {
        var matrix = Enumerable.Range(0, Lines.Length).Select(l => new char[Lines.Length]).ToArray();
        for (var x = 0; x < Lines.Length; x++)
        {
            for (var y = 0; y < Lines.Length; y++)
            {
                matrix[x][Lines.Length - y - 1] = Lines[y][x];
            }
        }

        return new(ID, matrix.Select(l => new string(l)).ToArray());
    }

    public Tile Flip() => new(ID, Lines.Select(l => new string(l.Reverse().ToArray())).ToArray());

    public Tile Trim() => new(ID, Lines.Skip(1).Take(Lines.Length - 2)
        .Select(l => new string(l.AsSpan()[1..^1])).ToArray());

    public IEnumerable<string> Sides
    {
        get
        {
            yield return Top;
            yield return Bottom;
            yield return Left;
            yield return Right;
            yield return new(Lines[0].Reverse().ToArray());
            yield return new(Lines[^1].Reverse().ToArray());
            yield return new(Lines.Select(l => l[0]).Reverse().ToArray());
            yield return new(Lines.Select(l => l[^1]).Reverse().ToArray());
        }
    }

    public string Top => Lines[0];
    public string Bottom => Lines[^1];
    public string Left => new(Lines.Select(l => l[0]).ToArray());
    public string Right => new(Lines.Select(l => l[^1]).ToArray());
}

public static class Solution
{
    private static IEnumerable<Tile> FindCornerTiles(this IEnumerable<Tile> tiles) =>
        tiles.Select(t1 => new
        {
            Tile = t1,
            MatchingTiles = tiles.Count(t2 => t1.ID != t2.ID && t1.Sides.Count(s1 => t2.Sides.Any(s2 => s1 == s2)) == 2)
        })
        .Where(t => t.MatchingTiles == 2)
        .Select(t => t.Tile);

    public enum Position { Right, Bottom };

    internal static bool TryFlipUntilFit(Tile anchor, Tile tile, Position position, out Tile result)
    {
        var anchorSide = position == Position.Right ? anchor.Right : anchor.Bottom;

        var found = false;
        for (var variant = 0; !found && variant < 8; variant++)
        {
            var tileSide = position == Position.Right ? tile.Left : tile.Top;
            if (anchorSide != tileSide)
            {
                tile = tile.RotateClockwise();
                if (variant == 3) tile = tile.Flip();
            }
            else found = true;
        }

        result = tile;
        return found;
    }

    private static IEnumerable<Tile> ParseTiles(string[] lines)
    {
        for (var index = 0; index < lines.Length; index += 12)
        {
            yield return new(int.Parse(lines[index][5..^1]), lines[(index + 1)..(index + 11)]);
        }
    }

    public static long SolvePuzzle1(string[] lines) =>
        ParseTiles(lines).FindCornerTiles().Aggregate(1L, (acc, item) => acc *= item.ID);

    static IEnumerable<Tile> GetPossibleNeighbours(this IEnumerable<Tile> tiles, IEnumerable<int> positionedTiles, int ID) =>
        tiles.Where(nt => nt.ID != ID && !positionedTiles.Any(pt => nt.ID == pt)
            && nt.Sides.Any(nts => tiles.First(t => t.ID == ID).Sides.Any(luct => luct == nts)));

    internal static IEnumerable<string> Stitch(Tile[] tiles)
    {
        var tilesSideLength = (int)Math.Sqrt(tiles.Length);

        for (var i = 0; i < tilesSideLength; i++)
        {
            for (var k = 0; k < tiles[0].Lines[0].Length - 2; k++)
            {
                yield return string.Create((tiles[0].Lines.Length - 2) * tilesSideLength, tiles, (buf, t) =>
                {
                    for (var j = 0; j < tilesSideLength; j++)
                    {
                        tiles[i].Lines[1..^1][k].AsSpan()[1..^1].CopyTo(buf);
                        buf = buf[(tiles[0].Lines.Length - 2)..];
                    }
                });
            }
        }
    }

    public static int SolvePuzzle2(string[] lines)
    {
        var tiles = ParseTiles(lines).ToList();
        var sideLength = (int)Math.Sqrt(tiles.Count);
        var tileIDs = new int[tiles.Count];

        var leftUpperCornerTile = tiles.FindCornerTiles().First();
        tileIDs[0] = leftUpperCornerTile.ID;
        var leftUpperNeighbours = tiles.GetPossibleNeighbours(tileIDs, leftUpperCornerTile.ID);
        tileIDs[1] = leftUpperNeighbours.First().ID;
        tileIDs[sideLength] = leftUpperNeighbours.Last().ID;

        for (var ix = 1; ix < tiles.Count - sideLength; ix++)
        {
            var possibleNeighbours = tiles.GetPossibleNeighbours(tileIDs, tileIDs[ix]).ToArray();
            if (possibleNeighbours.Length > 1)
            {
                var below = possibleNeighbours.Intersect(tiles.GetPossibleNeighbours(tileIDs, tileIDs[ix + sideLength - 1]));
                tileIDs[ix + sideLength] = below.First().ID;
                tileIDs[ix + 1] = possibleNeighbours.First(n => n.ID != tileIDs[ix + sideLength]).ID;
            }
            else tileIDs[ix + sideLength] = possibleNeighbours.First().ID;
        }

        var arrangedTiles = tileIDs.Select(id => tiles.First(t => id == t.ID)).ToArray();

        var found = false;
        for (var variant = 0; !found && variant < 8; variant++)
        {
            if (TryFlipUntilFit(arrangedTiles[0], arrangedTiles[1], Position.Right, out var right)
                 && TryFlipUntilFit(arrangedTiles[0], arrangedTiles[sideLength], Position.Bottom, out var bottom))
            {
                arrangedTiles[1] = right;
                arrangedTiles[sideLength] = bottom;
                found = true;
            }
            else
            {
                arrangedTiles[0] = arrangedTiles[0].RotateClockwise();
                if (variant == 3) arrangedTiles[0] = arrangedTiles[0].Flip();
            }
        }

        for (var ix = 1; ix < arrangedTiles.Length; ix++)
        {
            var compareTop = ix % sideLength == 0;
            var anchorTile = arrangedTiles[compareTop ? ix - sideLength : ix - 1];
            var anchorSide = anchorTile.Sides.Skip(compareTop ? 1 : 3).First();

            Debug.Assert(TryFlipUntilFit(anchorTile, arrangedTiles[ix],
                compareTop ? Position.Bottom : Position.Right, out var result));
            arrangedTiles[ix] = result;
        }

        throw new NotImplementedException();
    }
}