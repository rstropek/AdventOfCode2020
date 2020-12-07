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
    private const string sourceDestSplitter = " bags contain ";
    private static readonly Regex bagRegex = new(@"(\d+) ([\w ]+?(?= bag))");

    private static Dictionary<string, Dictionary<string, int>> GetRules(IEnumerable<string> lines) =>
        new(lines.Select(line => {
            var splitterIndex = line.IndexOf(sourceDestSplitter);
            return new KeyValuePair<string, Dictionary<string, int>>(
                line[..splitterIndex],
                new(bagRegex.Matches(line[(splitterIndex + sourceDestSplitter.Length)..])
                    .Select(match => new KeyValuePair<string, int>(
                         match.Groups[2].Value,
                         int.Parse(match.Groups[1].Value))))
            );
        }));

    public static int SolvePuzzle1(string[] lines)
    {
        static bool CanContainShinyGold(Dictionary<string, Dictionary<string, int>> rules, string color) =>
            rules[color].ContainsKey("shiny gold") || rules[color].Keys.Any(c => CanContainShinyGold(rules, c));

        var rules = GetRules(lines);
        return rules.Keys.Where(c => CanContainShinyGold(rules, c)).Count();
    }

    public static int SolvePuzzle2(string[] lines)
    {
        static int NumberOfContainedBags(Dictionary<string, Dictionary<string, int>> rules, string color) =>
            rules[color].Sum(c => c.Value)
                + rules[color].Select(c => NumberOfContainedBags(rules, c.Key) * c.Value).Sum();

        return NumberOfContainedBags(GetRules(lines), "shiny gold");
    }
}