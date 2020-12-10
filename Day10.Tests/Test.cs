using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string[] puzzleNumbersShort = new[]
        {
            "16", "10", "15", "5", "1", "11", "7", "19", "6", "12", "4",
        };

        private readonly string[] puzzleNumbersLong = new[]
        {
            "28", "33", "18", "42", "31", "14", "46", "20", "48", "47", "24", "23", "49", "45", "19",
            "38", "39", "11", "1", "32", "25", "35", "8", "17", "7", "9", "4", "2", "34", "10", "3",
        };

        [Fact]
        public void Puzzle1Short() => Assert.Equal(35, Solution.SolvePuzzle1(puzzleNumbersShort));

        [Fact]
        public void Puzzle1Long() => Assert.Equal(220, Solution.SolvePuzzle1(puzzleNumbersLong));

        [Fact]
        public void Puzzle2Short() => Assert.Equal(8, Solution.SolvePuzzle2(puzzleNumbersShort));

        [Fact]
        public void Puzzle2() => Assert.Equal(19208, Solution.SolvePuzzle2(puzzleNumbersLong));
    }
}
