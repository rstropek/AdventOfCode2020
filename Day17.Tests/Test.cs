using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly static string[] puzzle = new[] { ".#.", "..#", "###", };

        [Fact]
        public void Puzzle1() => Assert.Equal(112, Solution.SolvePuzzle1(puzzle));

        [Fact]
        public void Puzzle2() => Assert.Equal(848, Solution.SolvePuzzle2(puzzle));
    }
}
