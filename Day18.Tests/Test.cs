using System;
using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        [Theory]
        [InlineData("(1)", 2)]
        [InlineData("(1 + 2)", 6)]
        [InlineData("(1 + (2 + 3))", 12)]
        public void FindNextMatchingClosingBrace(string expression, int index) =>
            Assert.Equal(index, Solution.FindNextMatchingClosingBrace(expression.AsSpan()));

        [Theory]
        [InlineData("1 * 2", 2)]
        [InlineData("1 + 2 * 3", 6)]
        [InlineData("1 + 2 + (3 * 4) * 3", 16)]
        [InlineData("(1 * 2) * (2 * 3)", 8)]
        [InlineData("(1 * 2)", -1)]
        [InlineData("2", -1)]
        [InlineData("1 + 2", -1)]
        public void FindNext(string expression, int index) =>
            Assert.Equal(index, Solution.FindNext(expression.AsSpan(), '*'));

        [Theory]
        [InlineData("1 + 2 * 3 + 4 * 5 + 6", 71)]
        [InlineData("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [InlineData("2 * 3 + (4 * 5)", 26)]
        [InlineData("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [InlineData("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [InlineData("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void Puzzle1(string expressionString, long result) =>
            Assert.Equal(result, Solution.SolvePuzzle1(new[] { expressionString }));

        [Theory]
        [InlineData("1 + 2 * 3 + 4 * 5 + 6", 231)]
        [InlineData("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [InlineData("2 * 3 + (4 * 5)", 46)]
        [InlineData("5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
        [InlineData("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
        [InlineData("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
        public void Puzzle2(string expressionString, long result) =>
            Assert.Equal(result, Solution.SolvePuzzle2(new[] { expressionString }));
    }
}
