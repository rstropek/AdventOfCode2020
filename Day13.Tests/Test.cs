using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string travelPuzzle1 = "939\n7,13,x,x,59,x,31,19";

        [Fact]
        public void Puzzle1() => Assert.Equal(295, Solution.SolvePuzzle1(travelPuzzle1));

        [Fact]
        public void Puzzle2_1() => Assert.Equal(3417, Solution.SolvePuzzle2("0\n17,x,13,19"));

        [Fact]
        public void Puzzle2_2() => Assert.Equal(754018, Solution.SolvePuzzle2("0\n67,7,59,61"));

        [Fact]
        public void Puzzle2_3() => Assert.Equal(779210, Solution.SolvePuzzle2("0\n67,x,7,59,61"));

        [Fact]
        public void Puzzle2_4() => Assert.Equal(1261476, Solution.SolvePuzzle2("0\n67,7,x,59,61"));

        [Fact]
        public void Puzzle2_5() => Assert.Equal(1202161486, Solution.SolvePuzzle2("0\n1789,37,47,1889"));

        [Fact]
        public void Puzzle2_6() => Assert.Equal(1068781, Solution.SolvePuzzle2(travelPuzzle1));
    }
}
