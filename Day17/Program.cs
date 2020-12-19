using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Day17.Tests")]

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines)}");

public static class Solution
{
    public static int SolvePuzzle1(string[] lines)
    {
        var sideLength = lines.Length;
        var space = new bool[12 + sideLength, 12 + sideLength, 13];

        var startY = (space.GetLength(1) - lines.Length) / 2;
        for (var y = startY; y < startY + lines.Length; y++)
        {
            var startX = (space.GetLength(0) - lines[y - startY].Length) / 2;
            for (var x = startX; x < startX + lines[y - startY].Length; x++)
            {
                if (lines[y - startY][x - startX] == '#')
                {
                    Debug.WriteLine($"Setting {x}/{y}/6 = true");
                    space[x, y, 6] = true;
                }
            }
        }

        for (var cycles = 0; cycles < 6; cycles++)
        {
            var newSpace = (bool[,,])space.Clone();
            for (var x = 0; x < space.GetLength(0); x++)
            {
                for (var y = 0; y < space.GetLength(1); y++)
                {
                    for (var z = 0; z < space.GetLength(2); z++)
                    {
                        var xMin = x == 0 ? x : x - 1;
                        var yMin = y == 0 ? y : y - 1;
                        var zMin = z == 0 ? z : z - 1;
                        var xMax = x == space.GetLength(0) - 1 ? x : x + 1;
                        var yMax = y == space.GetLength(1) - 1 ? y : y + 1;
                        var zMax = z == space.GetLength(2) - 1 ? z : z + 1;

                        var activeNeighbours = 0;
                        for (var dx = xMin; dx < xMax; dx++)
                        {
                            for (var dy = yMin; dy < yMax; dy++)
                            {
                                for (var dz = zMin; dz < zMax; dz++)
                                {
                                    if ((dx != x || dy != y || dz != z) && space[dx, dy, dz])
                                    {
                                        activeNeighbours++;
                                    }
                                }
                            }
                        }

                        if (activeNeighbours > 1) Debug.WriteLine($"{x}/{y}/{y}: {activeNeighbours}");

                        if (space[x, y, z] && activeNeighbours is not 2 and not 3) newSpace[x, y, z] = false;
                        else if (!space[x, y, z] && activeNeighbours == 3) newSpace[x, y, z] = true;
                    }
                }
            }

            space = newSpace;
        }

        var activeCount = 0;
        for (var x = 0; x < space.GetLength(0); x++)
        {
            for (var y = 0; y < space.GetLength(1); y++)
            {
                for (var z = 0; z < space.GetLength(2); z++)
                {
                    if (space[x, y, z]) activeCount++;
                }
            }
        }

        return activeCount;
    }

    public static int SolvePuzzle2(string[] lines)
    {
        throw new NotImplementedException();
    }
}