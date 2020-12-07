using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string puzzleAnswers = @"abc

a
b
c

ab
ac

a
a
a
a

b".Replace("\r\n", "\n");

        [Fact]
        public void Puzzle1() => Assert.Equal(11, Solution.SolvePuzzle1(puzzleAnswers));

        [Fact]
        public void Puzzle2() => Assert.Equal(6, Solution.SolvePuzzle2(puzzleAnswers));
    }
}
