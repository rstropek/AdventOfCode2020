using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private static readonly string puzzle = "389125467";

        [Fact]
        public void Puzzle1_10() => Assert.Equal("92658374", Solution.SolvePuzzle1(puzzle, 10));

        [Fact]
        public void Puzzle1_100() => Assert.Equal("67384529", Solution.SolvePuzzle1(puzzle, 100));

        [Fact]
        public void Puzzle2() => Assert.Equal(149245887792L, Solution.SolvePuzzle2(puzzle));
    }

    public class LinkedList
    {
        [Fact]
        public void Create()
        {
            var index = new Item[3];
            var items = new Item(new [] { 1, 2, 3 }, index);
            Assert.Equal(1, items.Value);
            Assert.Equal(2, items.Next.Value);
            Assert.Equal(3, items.Next.Next.Value);
            Assert.Equal(1, items.Next.Next.Next.Value);

            Assert.Equal(1, index[0].Value);
            Assert.Equal(2, index[1].Value);
            Assert.Equal(3, index[2].Value);
        }

        [Fact]
        public void Slice()
        {
            var items = new Item(new[] { 1, 2, 3, 4, 5 }, new Item[5]);
            var slice = items.SliceThree();
            Assert.Equal(1, items.Value);
            Assert.Equal(5, items.Next.Value);
            Assert.Equal(2, slice.Value);
            Assert.Equal(3, slice.Next.Value);
            Assert.Equal(4, slice.Next.Next.Value);
            Assert.Null(slice.Next.Next.Next);
        }

        [Fact]
        public void Insert()
        {
            var items = new Item(new[] { 1, 2, 3, 4, 5 }, new Item[5]);
            var slice = items.SliceThree();
            items.InsertThree(slice);
            Assert.Equal(1, items.Value);
            Assert.Equal(2, items.Next.Value);
            Assert.Equal(3, items.Next.Next.Value);
            Assert.Equal(4, items.Next.Next.Next.Value);
            Assert.Equal(5, items.Next.Next.Next.Next.Value);
            Assert.Equal(items, items.Next.Next.Next.Next.Next);
        }

        [Fact]
        public void Find()
        {
            var originalItems = new Item(new[] { 1, 2, 3, 4, 5 }, new Item[5]);
            var items = originalItems.Find(5);
            Assert.Equal(5, items.Value);
            Assert.Equal(1, items.Next.Value);
            Assert.Equal(2, items.Next.Next.Value);
            Assert.Equal(3, items.Next.Next.Next.Value);
            Assert.Equal(4, items.Next.Next.Next.Next.Value);
            Assert.Equal(items, items.Next.Next.Next.Next.Next);
        }

        [Fact]
        public void FindWithNull()
        {
            var items = new Item(new[] { 1, 2, 3 }, new Item[3]);
            items.Next.Next.Next = null;
            Assert.Null(items.Find(4));
            Assert.Equal(3, items.Find(3).Value);
        }
    }
}
