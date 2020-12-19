using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Day18.Tests")]

var expressionStrings = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(expressionStrings)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(expressionStrings)}");

public static class Solution
{
    internal static int FindNextMatchingClosingBrace(ReadOnlySpan<char> expressionString)
    {
        int index = 0;
        int braceCount = 1;
        do
        {
            braceCount += expressionString[++index] switch
            {
                '(' => 1,
                ')' => -1,
                _ => 0,
            };
        }
        while (braceCount > 0);
        return index;
    }

    internal static int FindNext(ReadOnlySpan<char> expressionString, char c)
    {
        var index = 0;
        while (expressionString.Length > 0 && expressionString[0] != c)
        {
            if (expressionString[0] == '(')
            {
                var nextBrace = FindNextMatchingClosingBrace(expressionString) + 1;
                index += nextBrace;
                expressionString = expressionString[nextBrace..];
            }
            else
            {
                expressionString = expressionString[1..];
                index++;
            }
        }

        return expressionString.Length > 0 ? index : -1;
    }

    private static long EvaluateSimple(ReadOnlySpan<char> expressionString)
    {
        long result = 0;
        var op = '+';
        do
        {
            var number = 0L;
            switch (expressionString[0])
            {
                case char c when c is >= '0' and <= '9':
                    var nextBlank = expressionString.IndexOf(' ');
                    if (nextBlank == -1) nextBlank = expressionString.Length;
                    number = long.Parse(expressionString[..nextBlank]);
                    expressionString = expressionString[nextBlank..];
                    break;
                case ' ':
                    op = expressionString[1];
                    expressionString = expressionString[3..];
                    continue;
                case '(':
                    var closingBrace = FindNextMatchingClosingBrace(expressionString);
                    number = EvaluateSimple(expressionString[1..closingBrace]);
                    expressionString = expressionString[(closingBrace + 1)..];
                    break;
            }

            result = op switch
            {
                '+' => result + number,
                '*' => result * number,
                _ => throw new NotImplementedException("Should never happen"),
            };
        }
        while (expressionString.Length > 0);

        return result;
    }

    private static long EvaluateComplex(ReadOnlySpan<char> expressionString)
    {
        static (long left, long right) GetLeftRight(ReadOnlySpan<char> expressionString, int opIndex) =>
            (EvaluateComplex(expressionString[0..(opIndex - 1)]),
            EvaluateComplex(expressionString[(opIndex + 2)..]));

        int nextOp;
        if ((nextOp = FindNext(expressionString, '*')) != -1)
        {
            var (left, right) = GetLeftRight(expressionString, nextOp);
            return left * right;
        }
        else if ((nextOp = FindNext(expressionString, '+')) != -1)
        {
            var (left, right) = GetLeftRight(expressionString, nextOp);
            return left + right;
        }

        return expressionString[0] switch
        {
            >= '0' and <= '9' => long.Parse(expressionString),
            '(' => EvaluateComplex(expressionString[1..^1]),
            _ => throw new InvalidOperationException("Should never happen")
        };
    }

    public static long SolvePuzzle1(string[] expressionStrings) =>
        expressionStrings.Select(es => EvaluateSimple(es.AsSpan())).Sum();

    public static long SolvePuzzle2(string[] expressionStrings) =>
        expressionStrings.Select(es => EvaluateComplex(es.AsSpan())).Sum();
}