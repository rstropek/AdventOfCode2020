using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly int[] numbers = new[] { 1721, 979, 366, 299, 675, 1456, };

        [Fact]
        public void Puzzle1() => Assert.Equal(514579, Solution.SolvePuzzle1(numbers));

        [Fact]
        public void Puzzle2() => Assert.Equal(241861950, Solution.SolvePuzzle2(numbers));
    }
}
