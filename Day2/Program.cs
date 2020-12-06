using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var data = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(data)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(data)}");

public static class Solution
{
    public static int SolvePuzzle1(IEnumerable<string> passwords)
    {
        int indexOfDash = 0, indexOfSpace = 0, letterCount = 0;
        return passwords.Select(p => new
        {
            from = int.Parse(p[..(indexOfDash = p.IndexOf('-'))]),
            to = int.Parse(p[(indexOfDash + 1)..(indexOfSpace = p.IndexOf(' ', indexOfDash + 2))]),
            letter = p[indexOfSpace + 1],
            password = p[(indexOfSpace + 4)..]
        }).Count(p => (letterCount = p.password.Count(c => c == p.letter)) >= p.from && letterCount <= p.to);
    }

    public static int SolvePuzzle2(IEnumerable<string> passwords)
    {
        int indexOfDash = 0, indexOfSpace = 0;
        return passwords.Select(p => new
        {
            first = int.Parse(p[..(indexOfDash = p.IndexOf('-'))]),
            second = int.Parse(p[(indexOfDash + 1)..(indexOfSpace = p.IndexOf(' ', indexOfDash + 2))]),
            letter = p[indexOfSpace + 1],
            password = p[(indexOfSpace + 4)..]
        }).Count(p => p.password[p.first - 1] == p.letter ^ p.password[p.second - 1] == p.letter);
    }
}