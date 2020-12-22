using System.Linq;
using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly static string[] puzzle1 = new[]
        {
            "Tile 2311:",
            "..##.#..#.",
            "##..#.....",
            "#...##..#.",
            "####.#...#",
            "##.##.###.",
            "##...#.###",
            ".#.#.#..##",
            "..#....#..",
            "###...#.#.",
            "..###..###",
            "",
            "Tile 1951:",
            "#.##...##.",
            "#.####...#",
            ".....#..##",
            "#...######",
            ".##.#....#",
            ".###.#####",
            "###.##.##.",
            ".###....#.",
            "..#.#..#.#",
            "#...##.#..",
            "",
            "Tile 1171:",
            "####...##.",
            "#..##.#..#",
            "##.#..#.#.",
            ".###.####.",
            "..###.####",
            ".##....##.",
            ".#...####.",
            "#.##.####.",
            "####..#...",
            ".....##...",
            "",
            "Tile 1427:",
            "###.##.#..",
            ".#..#.##..",
            ".#.##.#..#",
            "#.#.#.##.#",
            "....#...##",
            "...##..##.",
            "...#.#####",
            ".#.####.#.",
            "..#..###.#",
            "..##.#..#.",
            "",
            "Tile 1489:",
            "##.#.#....",
            "..##...#..",
            ".##..##...",
            "..#...#...",
            "#####...#.",
            "#..#.#.#.#",
            "...#.#.#..",
            "##.#...##.",
            "..##.##.##",
            "###.##.#..",
            "",
            "Tile 2473:",
            "#....####.",
            "#..#.##...",
            "#.##..#...",
            "######.#.#",
            ".#...#.#.#",
            ".#########",
            ".###.#..#.",
            "########.#",
            "##...##.#.",
            "..###.#.#.",
            "",
            "Tile 2971:",
            "..#.#....#",
            "#...###...",
            "#.#.###...",
            "##.##..#..",
            ".#####..##",
            ".#..####.#",
            "#..#.#..#.",
            "..####.###",
            "..#.#.###.",
            "...#.#.#.#",
            "",
            "Tile 2729:",
            "...#.#.#.#",
            "####.#....",
            "..#.#.....",
            "....#..#.#",
            ".##..##.#.",
            ".#.####...",
            "####.#.#..",
            "##.####...",
            "##..#.##..",
            "#.##...##.",
            "",
            "Tile 3079:",
            "#.#.#####.",
            ".#..######",
            "..#.......",
            "######....",
            "####.#..#.",
            ".#...#.##.",
            "#.#####.##",
            "..#.###...",
            "..#.......",
            "..#.###...",
        };

        [Fact]
        public void Puzzle1() => Assert.Equal(20899048083289L, Solution.SolvePuzzle1(puzzle1));

        [Fact]
        public void Puzzle2() => Assert.Equal(0L, Solution.SolvePuzzle2(puzzle1));

        [Fact]
        public void RotateClockwise()
        {
            var source = new Tile(0, new[] {
                "*   ",
                " * *",
                " ** ",
                "*  *",
            });
            var expected = new Tile(0, new [] {
                "*  *",
                " ** ",
                " *  ",
                "* * ",
            });
            var rotated = source.RotateClockwise();
            Assert.True(rotated.Lines.SequenceEqual(expected.Lines));

            rotated = rotated.RotateClockwise();
            rotated = rotated.RotateClockwise();
            rotated = rotated.RotateClockwise();
            Assert.True(rotated.Lines.SequenceEqual(source.Lines));
        }

        [Fact]
        public void Flip()
        {
            var source = new Tile(0, new[] {
                "*   ",
                " * *",
                " ** ",
                "*  *",
            });
            var expected = new Tile(0, new[] {
                "   *",
                "* * ",
                " ** ",
                "*  *",
            });
            var rotated = source.Flip();
            Assert.True(rotated.Lines.SequenceEqual(expected.Lines));

            rotated = rotated.Flip();
            Assert.True(rotated.Lines.SequenceEqual(source.Lines));
        }

        [Fact]
        public void Trim()
        {
            var source = new Tile(0, new[] {
                "****",
                "*  *",
                "*  *",
                "****",
            });
            var trimmed = source.Trim();
            Assert.Empty(trimmed.Lines.Where(t => t.Trim().Length > 0));
        }
    }
}
