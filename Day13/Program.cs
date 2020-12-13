using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var travel = await File.ReadAllTextAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(travel)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(travel)}");

public static class Solution
{
    private record Travel(int departure, List<long> schedule);

    private static Travel Parse(string travel)
    {
        var parts = travel.Split('\n');
        return new Travel(
            int.Parse(parts.First()),
            parts.Last().Split(',').Select(n => n switch { "x" => -1, _ => long.Parse(n) }).ToList());
    }

    public static long SolvePuzzle1(string travelString)
    {
        var (departure, schedule) = Parse(travelString);
        var waitings = schedule.Where(t => t != -1).Select(t => (t, t - departure % t));

        var minWait = long.MaxValue;
        var minBus = 0L;
        var i = 0;
        foreach(var wait in waitings)
        {
            if (wait.Item2 < minWait)
            {
                minWait = wait.Item2;
                minBus = wait.t;
            }

            i++;
        }

        return minBus * minWait;
    }

    public static long SolvePuzzle2(string travelString)
    {
        var (_, scheduleRaw) = Parse(travelString);
        var schedule = scheduleRaw.Select((s, ix) => (s, ix))
            .Where(s => s.s != (-1))
            .ToList();

        var increment = schedule[0].s;
        var busIndex = 1;
        long i;
        for (i = schedule[0].s; busIndex < schedule.Count; i += increment)
        {
            if ((i + schedule[busIndex].ix) % schedule[busIndex].s == 0)
            {
                increment *= schedule[busIndex].s;
                busIndex++;
            }
        }

        return i - increment;
    }
}