using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

var input = await File.ReadAllTextAsync("data.txt");

for (var i = 0; i < 5; i++)
{
    var sw = Stopwatch.StartNew();
    Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(input)}");
    Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(input)}");
    Console.WriteLine(sw.Elapsed.TotalMilliseconds);
    Console.WriteLine();
}

public static class Solution
{
    public static long SolvePuzzle1(string input) => SolvePuzzle(input, 2020);

    public static long SolvePuzzle2(string input) => SolvePuzzle(input, 30000000);

    private static long SolvePuzzle(string input, long nTh)
    {
        var lastSaidCollection = new Dictionary<long, (long last, long secondLast)>();
        var index = 0;
        var lastSaid = -1L;
        foreach (var n in input.Split(',').Select(s => int.Parse(s)))
        {
            lastSaidCollection[n] = (index++, -1);
            lastSaid = n;
        }

        while (index < nTh)
        {
            var (last, secondLast) = lastSaidCollection[lastSaid];
            lastSaid = secondLast == -1 ? 0 : last - secondLast;
            if (lastSaidCollection.TryGetValue(lastSaid, out var nextSaidTuple))
            {
                lastSaidCollection[lastSaid] = (index, nextSaidTuple.last);
            }
            else
            {
                lastSaidCollection[lastSaid] = (index, -1);
            }

            index++;
        }

        return lastSaid;
    }
}