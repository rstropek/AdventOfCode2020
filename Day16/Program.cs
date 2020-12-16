using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines)}");

public static class Solution
{
    static readonly Regex ruleRegex = new(@"([\w ]+): (\d+)-(\d+) or (\d+)-(\d+)");

    public static long SolvePuzzle1(string[] input)
    {
        var minMax = input.TakeWhile(s => !string.IsNullOrEmpty(s))
            .SelectMany(rule =>
            {
                var ruleMatches = ruleRegex.Matches(rule);
                return new (int min, int max)[]
                {
                    (int.Parse(ruleMatches[0].Groups[2].Value), int.Parse(ruleMatches[0].Groups[3].Value)),
                    (int.Parse(ruleMatches[0].Groups[4].Value), int.Parse(ruleMatches[0].Groups[5].Value)),
                };
            })
            .ToArray();

        return input.Skip(minMax.Length / 2 + 5)
            .Select(ticket =>
                ticket.Split(',')
                    .Select(s => int.Parse(s))
                    .Where(n => !minMax.Any(mm => n >= mm.min && n <= mm.max))
                    .Sum())
            .Sum();
    }

    private record FieldDef(string Field, int Min1, int Max1, int Min2, int Max2);

    public static long SolvePuzzle2(string[] input)
    {
        var minMax = input.TakeWhile(s => !string.IsNullOrEmpty(s))
            .Select(rule =>
            {
                var ruleMatches = ruleRegex.Matches(rule);
                return new FieldDef(ruleMatches[0].Groups[1].Value,
                    int.Parse(ruleMatches[0].Groups[2].Value), int.Parse(ruleMatches[0].Groups[3].Value),
                    int.Parse(ruleMatches[0].Groups[4].Value), int.Parse(ruleMatches[0].Groups[5].Value));
            })
            .ToArray();

        var potentialFields = Enumerable.Range(0, minMax.Length)
            .Select(_ => minMax.Select(v => v.Field).ToArray())
            .ToArray();

        var nearby = input[(minMax.Length + 5)..];
        var identifiedNames = new HashSet<string>();
        foreach (var ticket in nearby)
        {
            var numbers = ticket.Split(',').Select(s => int.Parse(s)).ToArray();

            var potentialNames = numbers.Select(n =>
                minMax.Where(mm => (n >= mm.Min1 && n <= mm.Max1) || (n >= mm.Min2 && n <= mm.Max2))
                    .Select(mm => mm.Field)
                    .ToArray()
                ).ToArray();
            if (potentialNames.Any(pn => pn.Length == 0)) continue;

            for (var i = 0; i < potentialFields.Length; i++)
            {
                if (potentialFields[i].Length == 1) continue;
                potentialFields[i] = potentialFields[i].Intersect(potentialNames[i]).ToArray();
                if (potentialFields[i].Length == 1) identifiedNames.Add(potentialFields[i][0]);
            }

            bool found;
            do
            {
                found = false;
                for (var i = 0; i < potentialFields.Length; i++)
                {
                    if (potentialFields[i].Length == 1) continue;
                    potentialFields[i] = potentialFields[i]
                        .Where(pf => !identifiedNames.Any(ident => pf == ident))
                        .ToArray();
                    if (potentialFields[i].Length == 1)
                    {
                        found = true;
                        identifiedNames.Add(potentialFields[i][0]);
                    }
                }
            }
            while (found);
        }

        if (potentialFields.Any(pf => pf.Length != 1)) throw new InvalidOperationException("Invalid State");

        var myTicket = input[minMax.Length + 2]
            .Split(',')
            .Select(s => int.Parse(s))
            .ToArray();

        var product = 1L;
        for (var i = 0; i < potentialFields.Length; i++)
        {
            if (potentialFields[i].First().StartsWith("departure")) product *= myTicket[i];
        }

        return product;
    }
}