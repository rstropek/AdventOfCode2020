using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("Day14.Tests")]

var program = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(program)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(program)}");

public static class Solution
{
    private static (long orMask, long andMask) ParseMask(string mask)
    {
        long orMask = 0, andMask = 0;
        foreach(var digit in mask)
        {
            orMask <<= 1;
            andMask <<= 1;
            switch (digit)
            {
                case '1':
                    orMask |= 1;
                    andMask |= 1;
                    break;
                case '0':
                    orMask |= 1;
                    break;
                default:
                    andMask |= 1;
                    break;
            }
        }

        return (orMask, andMask);
    }

    public static long SolvePuzzle1(string[] programLines)
    {
        long orMask = 0, andMask = 0;
        var memory = new Dictionary<long, long>();
        var maskRegex = new Regex(@"mask = ([X01]+)");
        var memoryRegex = new Regex(@"mem\[(\d+)\] = (\d+)");

        foreach(var line in programLines)
        {
            var maskParsed = maskRegex.Matches(line);
            if (maskParsed.Count != 0)
            {
                (orMask, andMask) = ParseMask(maskParsed[0].Groups[1].Value);
            }
            else
            {
                var lineParsed = memoryRegex.Matches(line);
                memory[long.Parse(lineParsed[0].Groups[1].Value)] = (long.Parse(lineParsed[0].Groups[2].Value) | orMask) & andMask;
            }
        }

        return memory.Values.Sum();
    }

    internal record Masks(long OrMask, long AndMask, long[] XorMasks);

    private static long CalculatePowerOf2(int powerOf) => 1L << powerOf;

    internal static Masks ParseMask2(string mask)
    {
        long orMask = 0, andMask = 0;
        var xorMasks = new long[CalculatePowerOf2(mask.Count(x => x == 'X'))];
        var nextXorIndex = 0;

        for(var i = 0; i < mask.Length; i++)
        {
            var digit = mask[i];
            orMask <<= 1;
            andMask <<= 1;
            switch (digit)
            {
                case '1':
                    orMask |= 1;
                    andMask |= 1;
                    break;
                case '0':
                    andMask |= 1;
                    break;
                default:
                    andMask |= 1;
                    var xOrMask = CalculatePowerOf2(mask.Length - 1 - i);
                    if (nextXorIndex > 0)
                    {
                        Array.Copy(xorMasks, 0, xorMasks, nextXorIndex, nextXorIndex);
                        for (var xix = nextXorIndex; xix < nextXorIndex * 2; xix++)
                        {
                            xorMasks[xix] |= xOrMask;
                        }

                        nextXorIndex *= 2;
                    }
                    else
                    {
                        xorMasks[1] = xOrMask;
                        nextXorIndex = 2;
                    }

                    break;
            }
        }

        return new Masks(orMask, andMask, xorMasks);
    }

    public static long SolvePuzzle2(string[] programLines)
    {
        Masks? masks = null;
        var memory = new Dictionary<long, long>();
        var maskRegex = new Regex(@"mask = ([X01]+)");
        var memoryRegex = new Regex(@"mem\[(\d+)\] = (\d+)");

        foreach (var line in programLines)
        {
            var maskParsed = maskRegex.Matches(line);
            if (maskParsed.Count != 0)
            {
                masks = ParseMask2(maskParsed[0].Groups[1].Value);
            }
            else
            {
                var lineParsed = memoryRegex.Matches(line);
                var addressOriginal = (long.Parse(lineParsed[0].Groups[1].Value) | masks!.OrMask) & masks.AndMask;
                var value = long.Parse(lineParsed[0].Groups[2].Value);
                foreach (var xorMask in masks.XorMasks)
                {
                    var address = addressOriginal ^ xorMask;
                    memory[address] = value;
                }
            }
        }

        return memory.Values.Sum();
    }
}