using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var data = (await File.ReadAllLinesAsync("data.txt")).Select(line => int.Parse(line));

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(data)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(data)}");

public static class Solution
{
    public static int SolvePuzzle1(IEnumerable<int> numbers)
    {
        int secondNumber = 0;
        return numbers.First(n => numbers.Any(n2 => n + (secondNumber = n2) == 2020)) * secondNumber;
    }

    public static int SolvePuzzle2(IEnumerable<int> numbers)
    {
        int secondNumber = 0, thirdNumber = 0;
        return numbers.First(n => numbers.Any(n2 => numbers.Any(n3 => n + (secondNumber = n2) + (thirdNumber = n3) == 2020))) * secondNumber * thirdNumber;
    }
}