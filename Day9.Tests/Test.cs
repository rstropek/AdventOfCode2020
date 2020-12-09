using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string[] puzzle1Numbers = new[]
        {
            "35", "20", "15", "25", "47", "40", "62", "55", "65", "95", "102", "117",
            "150", "182", "127", "219", "299", "277", "309", "576",        
        };

        [Fact]
        public void Puzzle1() => Assert.Equal(127L, Solution.SolvePuzzle1(puzzle1Numbers, 5));

        [Fact]
        public void Puzzle2() => Assert.Equal(62, Solution.SolvePuzzle2(puzzle1Numbers, 5));
    }
}
