using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Day25.Tests")]

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(15335876L, 15086442L)}");
//Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines)}");

public static class Solution
{
    private const int initialSubjectNumber = 7;

    public static long SolvePuzzle1(long pubKeyCard, long pubKeyDoor)
    {
        static int FindLoopSize(long pubKey)
        {
            int loopSize, value;
            for (loopSize = 0, value = 1; value != pubKey; value = (value * initialSubjectNumber) % 20201227, loopSize++) ;
            return loopSize;
        }

        static long Transform(long subject, int loopSize)
        {
            int loop;
            long value;
            for (loop = 0, value = 1; loop < loopSize; value = (value * subject) % 20201227, loop++) ;
            return value;
        }

        var loopSizeDoor = FindLoopSize(pubKeyDoor);

        var result = Transform(pubKeyCard, loopSizeDoor);
        return result;
    }

    public static int SolvePuzzle2(string[] lines)
    {
        throw new NotImplementedException();
    }
}