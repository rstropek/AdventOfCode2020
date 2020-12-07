using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string[] seatStrings = new string[] { "BFFFBBFRRR", "FFFBBBFRRR", "BBFFBBFRLL", };

        [Fact]
        public void Puzzle1() => Assert.Equal(820, Solution.SolvePuzzle1(seatStrings));
    }
}
