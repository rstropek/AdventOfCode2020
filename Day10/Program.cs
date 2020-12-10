using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines)}");

public static class Solution
{
    private static int[] ConvertToNumbers(string[] numbersStrings) =>
        numbersStrings.Select(ns => int.Parse(ns))
            .Concat(new[] { 0 })
            .OrderBy(n => n)
            .ToArray();

    public static int SolvePuzzle1(string[] numbersStrings)
    {
        int[] numbers = ConvertToNumbers(numbersStrings);

        var count = new int[3];
        for (var i = 1; i < numbers.Length; i++) count[numbers[i] - numbers[i - 1] - 1]++;
        return count[0] * (count[2] + 1);
    }

    private record Node(int Jolt, Node? Parent = null)
    {
        public List<Node> Children { get; } = new();

        private long? pathsCached;
        public long Paths => pathsCached ??= Children.Count == 0 ? 1 : Children.Select(c => c.Paths).Sum();

        public Node? Find(int jolt)
        {
            if (Jolt == jolt) return this;
            foreach (var child in Children)
            {
                var found = child.Find(jolt);
                if (found != null) return found;
            }

            return null;
        }
    }

    private static void Fill(Span<int> numbers, Node root, Node parent)
    {
        if (parent.Children.Count > 0) return;
        while (numbers.Length > 0 && numbers[0] <= parent.Jolt + 3)
        {
            var child = root.Find(numbers[0]) ?? new Node(numbers[0], parent);
            parent.Children.Add(child);
            numbers = numbers[1..];
            if (numbers.Length > 0) Fill(numbers, root, child);
        }
    }

    public static long SolvePuzzle2(string[] numbersStrings)
    {
        var numbers = ConvertToNumbers(numbersStrings);
        
        var root = new Node(0);
        Fill(numbers[1..], root, root);
        return root.Paths;
    }
}