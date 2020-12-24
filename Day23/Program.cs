using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Day23.Tests")]

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1("716892543", 100)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2("716892543")}");

internal class Item
{
    public int Value { get; set; }

    public Item Next { get; set; }

    public Item(int value, Item next) => (Value, Next) = (value, next);

    public Item(IEnumerable<int> source, Item[] lookup)
    {
        bool first = true;
        Item prev = null;
        foreach(var item in source)
        {
            if (first)
            {
                lookup[item - 1] = this;
                Value = item;
                prev = this;
                first = false;
            }
            else
            {
                var newItem = new Item(item, null);
                lookup[item - 1] = newItem;
                prev.Next = newItem;
                prev = newItem;
            }
        }

        prev.Next = this;
    }

    public Item SliceThree()
    {
        var result = Next;
        Next = Next.Next.Next.Next;
        result.Next.Next.Next = null;
        return result;
    }

    public void InsertThree(Item item)
    {
        item.Next.Next.Next = Next;
        Next = item;
    }

    public Item Find(int value)
    {
        var current = this;
        while (current != null && !current.Value.Equals(value)) current = current.Next;
        return current;
    }
}

public static class Solution
{
    public static string SolvePuzzle1(string puzzle, int iterations)
    {
        var currentCup = SolvePuzzleImpl(puzzle.Select(c => c - '0'), 9, iterations);

        var current = currentCup.Find(1).Next;
        var sb = new StringBuilder();
        for (var i = 0; i < 8; i++)
        {
            sb.Append(current.Value.ToString());
            current = current.Next;
        }

        return sb.ToString();
    }

    public static long SolvePuzzle2(string puzzle)
    {
        var puzzleComplete = puzzle.Select(c => c - '0')
            .Concat(Enumerable.Range(10, 1_000_000 - 9));
        var currentCup = SolvePuzzleImpl(puzzleComplete, 1_000_000, 10_000_000);

        var current = currentCup.Find(1).Next;
        return (long)current.Value * (long)current.Next.Value;
    }

    internal static Item SolvePuzzleImpl(IEnumerable<int> values, int maxValue, int iterations)
    {
        var index = new Item[values.Count()];
        var cups = new Item(values, index);
        var currentCup = cups;

        for (var round = 0; round < iterations; round++)
        {
            var takenCups = currentCup.SliceThree();

            var destinationCupValue = currentCup.Value > 1 ? currentCup.Value - 1 : maxValue;
            while (takenCups.Find(destinationCupValue) != null)
            {
                if (--destinationCupValue < 1) destinationCupValue = maxValue;
            }

            var destinationCup = index[destinationCupValue - 1];
            destinationCup.InsertThree(takenCups);

            currentCup = currentCup.Next;
        }

        return currentCup;
    }
}