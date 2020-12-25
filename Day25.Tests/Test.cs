using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        [Fact]
        public void Puzzle1() => Assert.Equal(14897079L, Solution.SolvePuzzle1(5764801L, 17807724L));

        //[Fact]
        //public void Puzzle2() => Assert.Equal(10, Solution.SolvePuzzle2(lines));
    }
}
