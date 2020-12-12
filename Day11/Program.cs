using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

var lines = await File.ReadAllLinesAsync("data.txt");

Console.WriteLine($"Puzzle 1: {Solution.SolvePuzzle1(lines)}");
Console.WriteLine($"Puzzle 2: {Solution.SolvePuzzle2(lines)}");

public static class Solution
{
    private readonly struct Place
    {
        public bool IsSeat { get; }
        public bool IsOccupied { get; }
        public readonly bool IsEmpty => !IsOccupied;

        public Place(char seat) => (IsSeat, IsOccupied) = (seat == 'L', false);
        public Place(bool isSeat, bool isOccupied) => (IsSeat, IsOccupied) = (isSeat, isOccupied);
    }

    private class Plan : IEquatable<Plan>
    {
        public Place[] Places;
        public int Width;
        public int Height;

        public Plan(string[] lines)
        {
            Width = lines[0].Length;
            Height = lines.Length;
            Places = new Place[Width * Height];

            for (var row = 0; row < Height; row++)
            {
                for (var col = 0; col < Width; col++)
                {
                    Places[GetIndex(row, col)] = new Place(lines[row][col]);
                }
            }
        }

        public Plan(Plan source) =>
            (Width, Height, Places) = (source.Width, source.Height, source.Places.ToArray());

        protected int GetIndex(int row, int col) => row * Width + col;

        public bool Equals(Plan? other) =>
            other != null && Width == other.Width && Height == other.Height
                && Enumerable.SequenceEqual(Places, other.Places);

        public int NumberOfOccupiedPlaces => Places.Count(p => p.IsOccupied);

        protected bool IsTaken(int row, int col, int currentRow, int currentCol) =>
                Places[GetIndex(row, col)] is { IsSeat: true, IsOccupied: true };

        protected virtual int GetOccupiedNeighbours(int row, int col)
        {
            var result = 0;
            var maxRow = (row == Height - 1) ? row : row + 1;
            for (var r = row == 0 ? 0 : row - 1; r <= maxRow; r++)
            {
                var maxCol = ((col == Width - 1) ? col : col + 1);
                for (var c = col == 0 ? 0 : col - 1; c <= maxCol; c++)
                {
                    if ((r != row || c != col) && IsTaken(r, c, row, col)) result++;
                }
            }

            return result;
        }

        protected virtual int MinimumOccupiedNeighboursForEmpty => 4;

        public virtual Plan Clone() => new Plan(this);

        public Plan Run()
        {
            var result = Clone();

            for (var row = 0; row < Height; row++)
            {
                for (var col = 0; col < Width; col++)
                {
                    var ix = GetIndex(row, col);
                    var p = Places[ix];
                    if (p.IsSeat)
                    {
                        var occupiedNeighbours = GetOccupiedNeighbours(row, col);
                        if (p.IsEmpty && occupiedNeighbours == 0)
                        {
                            result.Places[ix] = new(p.IsSeat, true);
                        }
                        else if (p.IsOccupied && occupiedNeighbours >= MinimumOccupiedNeighboursForEmpty)
                        {
                            result.Places[ix] = new(p.IsSeat, false);
                        }
                        else
                        {
                            result.Places[ix] = p;
                        }
                    }
                }
            }

            return result;
        }

        // Just a helper method to print status
        public string AsString() =>
            string.Create((Width + 1) * Height, this, (bs, p) =>
            {
                for (var row = 0; row < p.Height; row++)
                {
                    for (var col = 0; col < p.Width; col++)
                    {
                        bs[0] = p.Places[row * p.Width + col] switch
                        {
                            { IsSeat: false } => '.',
                            { IsOccupied: false } => 'L',
                            _ => '#'
                        };

                        bs = bs[1..];
                    };

                    bs[0] = '\n';
                    bs = bs[1..];
                }
            });
    }

    private class Plan2 : Plan
    {
        public Plan2(string[] lines) : base(lines) { }

        public Plan2(Plan source) : base(source) { }

        public override Plan Clone() => new Plan2(this);

        protected override int GetOccupiedNeighbours(int row, int col)
        {
            var result = 0;

            bool Check(int row, int col, int currentRow, int currentCol, ref int result)
            {
                if (currentRow == row && currentCol == col) return false;
                if (IsTaken(row, col, currentRow, currentCol))
                { 
                    result++; 
                }

                return Places[GetIndex(row, col)].IsSeat;
            }

            int c, r;
            for (c = col; c >= 0; c--) if (Check(row, c, row, col, ref result)) break;
            for (c = col; c < Width; c++) if (Check(row, c, row, col, ref result)) break;

            for (r = row; r >= 0; r--) if (Check(r, col, row, col, ref result)) break;
            for (r = row; r < Height; r++) if (Check(r, col, row, col, ref result)) break;

            for (c = col, r = row; c >= 0 && r >= 0; c--, r--) if (Check(r, c, row, col, ref result)) break;
            for (c = col, r = row; c < Width && r < Height; c++, r++) if (Check(r, c, row, col, ref result)) break;

            for (c = col, r = row; c < Width && r >= 0; c++, r--) if (Check(r, c, row, col, ref result)) break;
            for (c = col, r = row; c >= 0 && r < Height; c--, r++) if (Check(r, c, row, col, ref result)) break;

            return result;
        }

        protected override int MinimumOccupiedNeighboursForEmpty => 5;
    }

    public static int SolvePuzzle1(string[] lines) => SolvePuzzle(new Plan(lines));

    public static long SolvePuzzle2(string[] lines) => SolvePuzzle(new Plan2(lines));

    private static int SolvePuzzle(Plan plan)
    {
        for (var newPlan = plan.Run(); !newPlan.Equals(plan); plan = newPlan, newPlan = plan.Run()) ;
        return plan.NumberOfOccupiedPlaces;
    }
}