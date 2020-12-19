using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly static string[] puzzle1 = new[] { ".#.", "..#", "###", };

        [Fact]
        public void Puzzle1() => Assert.Equal(112, Solution.SolvePuzzle1(puzzle1));
    }
}
