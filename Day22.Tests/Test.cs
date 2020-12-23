using System.Linq;
using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private static readonly string[] lines = new[]
        {
            "Player 1:",
            "9",
            "2",
            "6",
            "3",
            "1",
            "",
            "Player 2:",
            "5",
            "8",
            "4",
            "7",
            "10",
        };

        [Fact]
        public void Puzzle1() => Assert.Equal(306, Solution.SolvePuzzle(lines));

        [Fact]
        public void Puzzle2() => Assert.Equal(291, Solution.SolvePuzzle(lines, true));

        private static readonly string[] linesInfiniteRecursion = new[]
        {
            "Player 1:",
            "43",
            "19",
            "",
            "Player 2:",
            "2",
            "29",
            "14",
        };

        [Fact]
        public void SolveInfinite() => Assert.Equal((0, 1), Solution.Solve(Solution.Parse(linesInfiniteRecursion), true));
    }
}
