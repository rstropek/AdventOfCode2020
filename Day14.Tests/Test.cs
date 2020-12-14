using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string[] programPuzzle1 = new[] {
            "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
            "mem[8] = 11",
            "mem[7] = 101",
            "mem[8] = 0",
        };

        private readonly string[] programPuzzle2 = new[]
        {
            "mask = 000000000000000000000000000000X1001X",
            "mem[42] = 100",
            "mask = 00000000000000000000000000000000X0XX",
            "mem[26] = 1",
        };

        [Fact]
        public void Puzzle1() => Assert.Equal(165, Solution.SolvePuzzle1(programPuzzle1));

        [Fact]
        public void ParseMasksXorMask1()
        {
            var masks = Solution.ParseMask2("00000000000000000000000000000000X0XX");
            Assert.Equal(8, masks.XorMasks.Length);
            Assert.Contains(masks.XorMasks, v => v == 0b0000L);
            Assert.Contains(masks.XorMasks, v => v == 0b0001L);
            Assert.Contains(masks.XorMasks, v => v == 0b0010L);
            Assert.Contains(masks.XorMasks, v => v == 0b0011L);
            Assert.Contains(masks.XorMasks, v => v == 0b1000L);
            Assert.Contains(masks.XorMasks, v => v == 0b1001L);
            Assert.Contains(masks.XorMasks, v => v == 0b1010L);
            Assert.Contains(masks.XorMasks, v => v == 0b1011L);
        }

        [Fact]
        public void ParseMasksXorMask2()
        {
            var masks = Solution.ParseMask2("000000000000000000000000000000X1001X");
            Assert.Equal(4, masks.XorMasks.Length);
            Assert.Contains(masks.XorMasks, v => v == 0b000000L);
            Assert.Contains(masks.XorMasks, v => v == 0b000001L);
            Assert.Contains(masks.XorMasks, v => v == 0b100000L);
            Assert.Contains(masks.XorMasks, v => v == 0b100001L);
        }

        [Fact]
        public void ParseMasksXorMask3()
        {
            var masks = Solution.ParseMask2("000000000000000000000000000000010010");
            Assert.Single(masks.XorMasks);
            Assert.Equal(0L, masks.XorMasks[0]);
        }

        [Fact]
        public void ParseMasksXorMask4()
        {
            var masks = Solution.ParseMask2("00000000000000000000000000000001001X");
            Assert.Equal(2, masks.XorMasks.Length);
            Assert.Contains(masks.XorMasks, v => v == 0b0L);
            Assert.Contains(masks.XorMasks, v => v == 0b1L);
        }

        [Fact]
        public void Puzzle2() => Assert.Equal(208, Solution.SolvePuzzle2(programPuzzle2));
    }
}
