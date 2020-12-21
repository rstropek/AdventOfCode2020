using System.Linq;
using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private static readonly string[] lines = new[]
        {
            "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
            "trh fvjkl sbzzf mxmxvkd (contains dairy)",
            "sqjhc fvjkl (contains soy)",
            "sqjhc mxmxvkd sbzzf (contains fish)",
        };

        [Fact]
        public void Puzzle1() => Assert.Equal(5, Solution.SolvePuzzle1(lines));

        [Fact]
        public void Puzzle2() => Assert.Equal("mxmxvkd,sqjhc,fvjkl", Solution.SolvePuzzle2(lines));
    }
}
