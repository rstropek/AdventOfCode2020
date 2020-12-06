using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string[] passwords = new[]
        {
            "1-3 a: abcde",
            "1-3 b: cdefg",
            "2-9 c: ccccccccc",
        };

        [Fact]
        public void Puzzle1() => Assert.Equal(2, Solution.SolvePuzzle1(passwords));

        [Fact]
        public void Puzzle2() => Assert.Equal(1, Solution.SolvePuzzle2(passwords));
    }
}
