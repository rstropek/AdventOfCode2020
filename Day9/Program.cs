using System;
using System.IO;
using System.Linq;

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines, 25)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines, 25)}");

public static class Solution
{
    private static long[] GetNumbers(string[] numbersStrings) =>
        numbersStrings.Select(nString => long.Parse(nString)).ToArray();

    private static long FindSecretNumber(long[] numbers, int preambleLength)
    {
        for (var i = preambleLength; i < numbers.Length; i++)
        {
            var found = false;
            for (var j = i - preambleLength; j < i && !found; j++)
            {
                for (var k = j + 1; k < i && !found; k++)
                {
                    found = numbers[j] + numbers[k] == numbers[i];
                }
            }

            if (!found)
            {
                return numbers[i];
            }
        }

        throw new InvalidOperationException("Should not happen");
    }

    public static long SolvePuzzle1(string[] numbersStrings, int preambleLength) =>
        FindSecretNumber(GetNumbers(numbersStrings), preambleLength);

    public static long SolvePuzzle2(string[] numbersStrings, int preambleLength)
    {
        var numbers = GetNumbers(numbersStrings);
        var secretNumber = FindSecretNumber(numbers, preambleLength);

        for (var i = 0; i < numbers.Length; i++)
        {
            long sum = numbers[i];
            var j = i + 1;
            for (; j < numbers.Length && (sum += numbers[j]) < secretNumber; j++) ;
            if (sum == secretNumber)
            {
                var range = numbers.Skip(i).Take(j - i);
                return range.Min() + range.Max();
            }
        }

        throw new InvalidOperationException("Should not happen");
    }
}