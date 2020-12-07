using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var answersText = await File.ReadAllTextAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(answersText)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(answersText)}");

public static class Solution
{
    public static int SolvePuzzle1(string answersText) =>
        answersText.Split("\n\n")
            .Select(group => group.Replace("\n", "").Distinct().Count())
            .Sum();

    public static int SolvePuzzle2(string answersText) =>
        answersText.Split("\n\n")
            .Select(group => 
                group.Split('\n')
                    .Aggregate<IEnumerable<char>, IEnumerable<char>>(null!,
                        (acc, person) => acc = acc == null ? person.AsEnumerable() : acc.Intersect(person))
                    .Distinct()
                    .Count())
        .Sum();
}