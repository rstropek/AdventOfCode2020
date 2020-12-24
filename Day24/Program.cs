using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Day24.Tests")]

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines, 100)}");

public enum Direction { se, sw, nw, ne, e, w };

public class Tile
{
    private readonly Direction[] OppositeDirections = new [] { Direction.nw, Direction.ne, Direction.se, Direction.sw, Direction.w, Direction.e };

    private readonly HashSet<Tile> globalTileList;

    public Tile(HashSet<Tile> globalTileList)
    {
        this.globalTileList = globalTileList;
        globalTileList.Add(this);
    }

    public Tile[] Neighbours { get; } = new Tile[6];

    public bool IsWhite { get; private set; } = true;
    public bool IsBlack => !IsWhite;

    public void Flip() => IsWhite = !IsWhite;

    public bool TempIsWhite { get; private set; } = true;

    public void Transform()
    {
        var blackNeighbours = Neighbours.Where(n => n != null).Count(n => n.IsBlack);
        TempIsWhite = IsWhite;
        if (IsBlack) TempIsWhite = blackNeighbours is 0 or > 2;
        else TempIsWhite = blackNeighbours != 2;
    }

    public void Commit() => IsWhite = TempIsWhite;

    public Tile Go(Direction direction, Tile existingNeighbour = null)
    {
        Touch(direction, existingNeighbour);
        Neighbours[(int)direction].CreateAllNeighbours();
        return Neighbours[(int)direction];
    }

    public Tile Touch(Direction direction, Tile existingNeighbour = null)
    {
        var newTile= Neighbours[(int)direction] ??= existingNeighbour ?? new Tile(globalTileList);
        newTile.Neighbours[(int)OppositeDirections[(int)direction]] = this;
        return newTile;
    }

    public void CreateAllNeighbours()
    {
        var ne = Neighbours[(int)Direction.ne] = Touch(Direction.ne);

        var nw = Neighbours[(int)Direction.nw] = Touch(Direction.nw);
        nw.Touch(Direction.e, ne);

        var w = Neighbours[(int)Direction.w] = Touch(Direction.w);
        w.Touch(Direction.ne, nw);

        var sw = Neighbours[(int)Direction.sw] = Touch(Direction.sw);
        sw.Touch(Direction.nw, w);

        var se = Neighbours[(int)Direction.se] = Touch(Direction.se);
        se.Touch(Direction.w, sw);

        var e = Neighbours[(int)Direction.e] = Touch(Direction.e);
        e.Touch(Direction.sw, se);

        ne.Touch(Direction.se, e);
    }
}

public static class Solution
{
    public static readonly List<string> DirectionStrings = new() { "se", "sw", "nw", "ne", "e", "w", };

    public static Tile FillArea(HashSet<Tile> tiles)
    {
        var tile = new Tile(tiles);
        tile.CreateAllNeighbours();

        var startTile = tile;
        for (var ring = 0; ring < 70; ring++)
        {
            startTile = startTile.Go(Direction.ne);
            var newTile = startTile;
            for (var steps = 0; steps < ring + 1; steps++) newTile = newTile.Go(Direction.se);
            for (var steps = 0; steps < ring + 1; steps++) newTile = newTile.Go(Direction.sw);
            for (var steps = 0; steps < ring + 1; steps++) newTile = newTile.Go(Direction.w);
            for (var steps = 0; steps < ring + 1; steps++) newTile = newTile.Go(Direction.nw);
            for (var steps = 0; steps < ring + 1; steps++) newTile = newTile.Go(Direction.ne);
            for (var steps = 0; steps < ring + 1; steps++) newTile = newTile.Go(Direction.e);
            Debug.Assert(startTile == newTile);
        }

        return tile;
    }

    public static IEnumerable<Direction> Parse(string line)
    {
        var index = 0;
        while (index < line.Length)
        {
            for(var i = 0; i < DirectionStrings.Count; i++)
            {
                if (line[index] == DirectionStrings[i][0] 
                    && (DirectionStrings[i].Length == 1 || line[index + 1] == DirectionStrings[i][1]))
                {
                    yield return (Direction)i;
                    index += DirectionStrings[i].Length;
                    break;
                }
            }
        }
    }

    public static int SolvePuzzle1(string[] lines) => CalculateTiles(lines).Count(t => t.IsBlack);

    private static HashSet<Tile> CalculateTiles(string[] lines)
    {
        var tiles = new HashSet<Tile>();
        var tile = FillArea(tiles);

        foreach (var line in lines)
        {
            var currentTile = tile;
            foreach (var move in Parse(line))
            {
                currentTile = currentTile.Go(move);
            }

            currentTile.Flip();
        }

        return tiles;
    }

    public static int SolvePuzzle2(string[] lines, int days)
    {
        var tiles = CalculateTiles(lines);
        for (var i = 0; i < days; i++)
        {
            foreach (var tile in tiles) tile.Transform();
            foreach (var tile in tiles) tile.Commit();
        }

        return tiles.Count(t => t.IsBlack);
    }
}