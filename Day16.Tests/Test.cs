using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string[] linesPuzzle1 = new[]
        {
            "class: 1-3 or 5-7",
            "row: 6-11 or 33-44",
            "seat: 13-40 or 45-50",
            "",
            "your ticket:",
            "7,1,14",
            "",
            "nearby tickets:",
            "7,3,47",
            "40,4,50",
            "55,2,20",
            "38,6,12",
        };

        [Fact]
        public void Puzzle1() => Assert.Equal(71, Solution.SolvePuzzle1(linesPuzzle1));

        private readonly string[] linesPuzzle2 = new[]
        {
            "departure class: 0-1 or 4-19",
            "departure row: 0-5 or 8-19",
            "seat: 0-13 or 16-19",
            "",
            "your ticket:",
            "11,12,13",
            "",
            "nearby tickets:",
            "99,0,0",
            "3,9,18",
            "15,1,5",
            "5,14,9",
        };

        [Fact]
        public void Puzzle2() => Assert.Equal(11 * 12, Solution.SolvePuzzle2(linesPuzzle2));
    }
}
