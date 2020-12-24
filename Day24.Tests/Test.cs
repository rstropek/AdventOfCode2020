using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AoC.Tests
{
    public class Test
    {
        private readonly string[] lines =
        {
            "sesenwnenenewseeswwswswwnenewsewsw",
            "neeenesenwnwwswnenewnwwsewnenwseswesw",
            "seswneswswsenwwnwse",
            "nwnwneseeswswnenewneswwnewseswneseene",
            "swweswneswnenwsewnwneneseenw",
            "eesenwseswswnenwswnwnwsewwnwsene",
            "sewnenenenesenwsewnenwwwse",
            "wenwwweseeeweswwwnwwe",
            "wsweesenenewnwwnwsenewsenwwsesesenwne",
            "neeswseenwwswnwswswnw",
            "nenwswwsewswnenenewsenwsenwnesesenew",
            "enewnwewneswsewnwswenweswnenwsenwsw",
            "sweneswneswneneenwnewenewwneswswnese",
            "swwesenesewenwneswnwwneseswwne",
            "enesenwswwswneneswsenwnewswseenwsese",
            "wnwnesenesenenwwnenwsewesewsesesew",
            "nenewswnwewswnenesenwnesewesw",
            "eneswnwswnwsenenwnwnwwseeswneewsenese",
            "neswnwewnwnwseenwseesewsenwsweewe",
            "wseweeenwnesenwwwswnew",
        };

        [Fact]
        public void Puzzle1() => Assert.Equal(10, Solution.SolvePuzzle1(lines));

        [Theory]
        [InlineData(1, 15)]
        [InlineData(2, 12)]
        [InlineData(3, 25)]
        [InlineData(4, 14)]
        [InlineData(5, 23)]
        [InlineData(6, 28)]
        [InlineData(7, 41)]
        [InlineData(8, 37)]
        [InlineData(9, 49)]
        [InlineData(10, 37)]
        [InlineData(20, 132)]
        [InlineData(30, 259)]
        [InlineData(40, 406)]
        [InlineData(50, 566)]
        [InlineData(60, 788)]
        [InlineData(70, 1106)]
        [InlineData(80, 1373)]
        [InlineData(90, 1844)]
        [InlineData(100, 2208)]
        public void Puzzle2(int days, int expected) => Assert.Equal(expected, Solution.SolvePuzzle2(lines, days));

        [Fact]
        public void TileNeighbours()
        {
            var tiles = new HashSet<Tile>();
            var tile = new Tile(tiles);
            Assert.Single(tiles);

            tile.Go(Direction.ne);
            Assert.Equal(7, tiles.Count);
        }

        [Fact]
        public void TileRoundTrip()
        {
            var tiles = new HashSet<Tile>();
            var tile = new Tile(tiles);
            Assert.Single(tiles);

            var newTile = tile.Go(Direction.ne).Go(Direction.sw);
            newTile.Flip();
            Assert.Equal(tile, newTile);
            Assert.False(tile.IsWhite);
        }

        [Fact]
        public void TileLargeRoundTrip()
        {
            var tiles = new HashSet<Tile>();
            var tile = new Tile(tiles);
            tile.CreateAllNeighbours();
            Assert.Equal(7, tiles.Count);

            var newTile = tile.Go(Direction.ne);
            Assert.Equal(10, tiles.Count);
            newTile = newTile.Go(Direction.se);
            Assert.Equal(12, tiles.Count);
            newTile = newTile.Go(Direction.sw);
            Assert.Equal(14, tiles.Count);
            newTile = newTile.Go(Direction.w);
            Assert.Equal(16, tiles.Count);
            newTile = newTile.Go(Direction.nw);
            Assert.Equal(18, tiles.Count);
            newTile = newTile.Go(Direction.ne);
            Assert.Equal(19, tiles.Count);
            newTile = newTile.Go(Direction.e);
            newTile = newTile.Go(Direction.sw);
            Assert.Equal(tile, newTile);
            Assert.Equal(19, tiles.Count);
        }

        [Fact]
        public void Esww()
        {
            var tiles = new HashSet<Tile>();
            var tile = new Tile(tiles);
            tile.CreateAllNeighbours();

            var id = tile.Go(Direction.e).Go(Direction.sw).Go(Direction.w);
            id.Flip();

            Assert.False(tile.Neighbours[(int)Direction.sw].IsWhite);
        }

        [Fact]
        public void Nwwswee()
        {
            var tiles = new HashSet<Tile>();
            var tile = new Tile(tiles);
            tile.CreateAllNeighbours();

            var id = tile.Go(Direction.nw).Go(Direction.w).Go(Direction.sw).Go(Direction.e).Go(Direction.e);
            id.Flip();

            Assert.False(tile.IsWhite);
        }

        [Fact]
        public void TileMediumRoundTrip()
        {
            var tiles = new HashSet<Tile>();
            var tile = new Tile(tiles);
            Assert.Single(tiles);

            var newTile = tile.Go(Direction.ne).Go(Direction.ne).Go(Direction.sw).Go(Direction.sw);
            Assert.Equal(tile, newTile);
        }

        [Fact]
        public void Parse()
        {
            var parse = Solution.Parse("nwwswee");
            Assert.True(new[] { Direction.nw, Direction.w, Direction.sw, Direction.e, Direction.e }.SequenceEqual(parse));
        }
    }
}
