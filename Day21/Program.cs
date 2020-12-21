using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Day21.Tests")]

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines)}");

public static class Solution
{
    private record Product(string[] Ingredients, string[] Allergenes);

    private static Product[] Parse(string[] lines) =>
        lines.Select(l =>
        {
            var parts = l.Split(" (");
            return new Product(parts[0].Split(' '), parts[1]["contains ".Length..][..^1].Split(", "));
        }).ToArray();

    public static long SolvePuzzle1(string[] lines)
    {
        var products = Parse(lines);
        var possible = CalculateIngredientsWithAllergenes(products);

        var result = 0;
        foreach (var p in products)
        {
            result += p.Ingredients.Count(i => !possible.Values.Any(ia => ia.Contains(i)));
        }

        return result;
    }

    private static Dictionary<string, HashSet<string>> CalculateIngredientsWithAllergenes(Product[] products)
    {
        var possible = new Dictionary<string, HashSet<string>>();
        foreach (var prod in products)
        {
            foreach (var all in prod.Allergenes)
            {
                if (possible.TryGetValue(all, out var ingredients))
                {
                    possible[all] = possible[all].Intersect(prod.Ingredients).ToHashSet();
                }
                else possible[all] = prod.Ingredients.ToHashSet();
            }
        }

        for (var foundItemToRemove = true; foundItemToRemove;)
        {
            foundItemToRemove = false;
            foreach (var all in possible.Where(kvp => kvp.Value.Count == 1))
            {
                var allToRemove = all.Value.First();
                foreach (var allToFix in possible.Where(kvp => kvp.Value.Count > 1))
                {
                    foundItemToRemove = true;
                    allToFix.Value.Remove(allToRemove);
                }
            }
        }

        return possible;
    }

    public static string SolvePuzzle2(string[] lines)
    {
        var products = Parse(lines);
        var possible = CalculateIngredientsWithAllergenes(products);

        return string.Join(',', possible.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value.First()));
    }
}