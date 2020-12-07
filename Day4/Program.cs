using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var data = await File.ReadAllTextAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(data)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(data)}");

public static class Solution
{
    private static readonly string[] requiredProperties = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", };

    public static int SolvePuzzle1(string data) =>
        data.Split("\n\n")
            .Select(r => r.Replace('\n', ' '))
            .Select(r => new Dictionary<string, string>(r.Split(' ')
                    .Select(p => p.Split(':'))
                    .Select(p => KeyValuePair.Create(p[0], p[1]))))
        .Count(r => requiredProperties.All(m => r.ContainsKey(m)));

    public static int SolvePuzzle2(string data)
    {
        var hclRegex = new Regex("^#[0-9a-f]{6}$");
        var pidRegex = new Regex("^[0-9]{9}$");
        return data.Split("\n\n")
            .Select(r => r.Replace('\n', ' '))
            .Select(r => new Dictionary<string, string>(r.Split(' ')
                    .Select(p => p.Split(':'))
                    .Select(p => KeyValuePair.Create(p[0], p[1]))
                    .Where(kvp => kvp.Key switch
                    {
                        "byr" => int.TryParse(kvp.Value, out var byr) && byr is >= 1920 and <= 2002,
                        "iyr" => int.TryParse(kvp.Value, out var iyr) && iyr is >= 2010 and <= 2020,
                        "eyr" => int.TryParse(kvp.Value, out var iyr) && iyr is >= 2020 and <= 2030,
                        "hgt" => (kvp.Value.EndsWith("cm")
                                    && int.TryParse(kvp.Value[..^2], out var hgtcm) && hgtcm is >= 150 and <= 193)
                                || (kvp.Value.EndsWith("in")
                                    && int.TryParse(kvp.Value[..^2], out var hgtin) && hgtin is >= 59 and <= 76),
                        "hcl" => hclRegex.IsMatch(kvp.Value),
                        "ecl" => new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Any(v => v == kvp.Value),
                        "pid" => pidRegex.IsMatch(kvp.Value),
                        _ => false
                    })))
            .Count(r => requiredProperties.All(m => r.ContainsKey(m)));
    }
}