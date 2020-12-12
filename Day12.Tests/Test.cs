using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string[] route = new[] { "F10", "N3", "F7", "R90", "F11", };

        [Fact]
        public void Puzzle1() => Assert.Equal(25, Solution.SolvePuzzle1(route));

        [Fact]
        public void Puzzle2() => Assert.Equal(286, Solution.SolvePuzzle2(route));
    }
}
