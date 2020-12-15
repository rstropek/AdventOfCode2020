using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = await File.ReadAllTextAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(input)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(input)}");

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
            var lastSaidTuple = lastSaidCollection[lastSaid];
            if (lastSaidTuple.secondLast == -1)
            {
                if (lastSaidCollection.TryGetValue(0, out var lastSaidZero))
                {
                    lastSaidCollection[0] = (index, lastSaidZero.last);
                }
                else
                {
                    lastSaidCollection[0] = (index, -1);
                }

                lastSaid = 0;
            }
            else
            {
                var nextSaid = lastSaidTuple.last - lastSaidTuple.secondLast;
                if (lastSaidCollection.TryGetValue(nextSaid, out var nextSaidTuple))
                {
                    lastSaidCollection[nextSaid] = (index, nextSaidTuple.last);
                }
                else
                {
                    lastSaidCollection[nextSaid] = (index, -1);
                }

                lastSaid = nextSaid;
            }

            index++;
        }

        return lastSaid;
    }
}