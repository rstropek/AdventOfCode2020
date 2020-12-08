using System;
using System.IO;
using System.Linq;

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines)}");

public static class Solution
{
    private record Op(string OpCode, int Param);

    private static Op[] ParseOps(string[] opsStrings) =>
        opsStrings.Select(o => o.Split(' '))
            .Select(o => new Op(o[0], int.Parse(o[1])))
            .ToArray();

    private static (int acc, bool loop) Run(Op[] ops)
    {
        var visited = new bool[ops.Length];
        int line = 0, acc = 0;
        while (line < ops.Length && !visited[line])
        {
            visited[line] = true;
            switch (ops[line].OpCode)
            {
                case "nop":
                    line++;
                    break;
                case "acc":
                    acc += ops[line].Param;
                    line++;
                    break;
                case "jmp":
                    line += ops[line].Param;
                    break;
                default:
                    throw new InvalidOperationException("Unknown op code");
            }
        }

        return (acc, line < ops.Length);
    }

    public static int SolvePuzzle1(string[] opsStrings)
    {
        var (acc, _) = Run(ParseOps(opsStrings));
        return acc;
    }

    public static int SolvePuzzle2(string[] opsStrings)
    {
        var ops = ParseOps(opsStrings);

        for (var line = 0; line < ops.Length; line++)
        {
            if (ops[line].OpCode is not "nop" and not "jmp") continue;

            var originalLine = ops[line];
            ops[line] = ops[line] with { OpCode = (ops[line].OpCode == "nop") ? "jmp" : "nop" };
            var (acc, loop) = Run(ops);

            if (!loop) return acc;

            ops[line] = originalLine;
        }

        throw new InvalidOperationException("No solution found");
    }
}