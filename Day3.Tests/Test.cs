using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string[] passwords = new[]
        {
            "..##.......",
            "#...#...#..",
            ".#....#..#.",
            "..#.#...#.#",
            ".#...##..#.",
            "..#.##.....",
            ".#.#.#....#",
            ".#........#",
            "#.##...#...",
            "#...##....#",
            ".#..#...#.#",
        };

        [Fact]
        public void Puzzle1() => Assert.Equal(7, Solution.SolvePuzzle1(passwords));

        [Fact]
        public void Puzzle2() => Assert.Equal(336, Solution.SolvePuzzle2(passwords));
    }
}
