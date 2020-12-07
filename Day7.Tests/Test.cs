using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string[] puzzle1bagRules = new[]
        {
            "light red bags contain 1 bright white bag, 2 muted yellow bags.",
            "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
            "bright white bags contain 1 shiny gold bag.",
            "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
            "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags.",
        };

        private readonly string[] puzzle2bagRules = new[]
        {
            "shiny gold bags contain 2 dark red bags.",
            "dark red bags contain 2 dark orange bags.",
            "dark orange bags contain 2 dark yellow bags.",
            "dark yellow bags contain 2 dark green bags.",
            "dark green bags contain 2 dark blue bags.",
            "dark blue bags contain 2 dark violet bags.",
            "dark violet bags contain no other bags.",
        };

        [Fact]
        public void Puzzle1() => Assert.Equal(4, Solution.SolvePuzzle1(puzzle1bagRules));

        [Fact]
        public void Puzzle2a() => Assert.Equal(32, Solution.SolvePuzzle2(puzzle1bagRules));

        [Fact]
        public void Puzzle2b() => Assert.Equal(126, Solution.SolvePuzzle2(puzzle2bagRules));
    }
}
