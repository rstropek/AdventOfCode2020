using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string[] puzzle1Ops = new[]
        {
            "nop +0", "acc +1", "jmp +4", "acc +3", "jmp -3",
            "acc -99", "acc +1", "jmp -4", "acc +6",        
        };

        [Fact]
        public void Puzzle1() => Assert.Equal(5, Solution.SolvePuzzle1(puzzle1Ops));

        [Fact]
        public void Puzzle2() => Assert.Equal(8, Solution.SolvePuzzle2(puzzle1Ops));
    }
}
