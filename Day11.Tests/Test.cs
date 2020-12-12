using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string[] seatPlan = new[]
        {
            "L.LL.LL.LL",
            "LLLLLLL.LL",
            "L.L.L..L..",
            "LLLL.LL.LL",
            "L.LL.LL.LL",
            "L.LLLLL.LL",
            "..L.L.....",
            "LLLLLLLLLL",
            "L.LLLLLL.L",
            "L.LLLLL.LL",
        };

        [Fact]
        public void Puzzle1() => Assert.Equal(37, Solution.SolvePuzzle1(seatPlan));

        [Fact]
        public void Puzzle2() => Assert.Equal(26, Solution.SolvePuzzle2(seatPlan));
    }
}
