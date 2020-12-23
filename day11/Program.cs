using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day11
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var originalBoard = ParseBoard(File
                .ReadAllLines("input.txt")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray());

            var seen = new HashSet<Board>();
            var currentBoard = originalBoard;
            while (seen.Add(currentBoard))
            {
                currentBoard = currentBoard.Next(4, 1);
            }

            Console.WriteLine("Steady state occupied: {0}", currentBoard.ToString().Count(c => c == '#'));

            seen.Clear();
            currentBoard = originalBoard;
            while (seen.Add(currentBoard))
            {
                currentBoard = currentBoard.Next(5, Math.Max(currentBoard.Rows, currentBoard.Cols));
            }

            Console.WriteLine("Steady state occupied: {0}", currentBoard.ToString().Count(c => c == '#'));
        }

        internal static Board ParseBoard(string[] lines)
        {
            var rows = lines.Length;
            var cols = lines[0].Length;
            var positions = new Position[rows,cols];
            for (var row = 0; row < rows; row++)
            for (var col = 0; col < cols; col++)
            {
                positions[row,col] = lines[row][col] switch
                {
                    'L' => Position.Empty,
                    '#' => Position.Occupied,
                    _ => Position.Floor
                };
            }

            return new Board(positions);
        }
    }

    internal enum Position
    {
        Empty,
        Occupied,
        Floor
    }

    internal readonly struct Board : IEquatable<Board>
    {
        private static readonly (int,int)[] Directions = new[]
        {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1), (0, 1),
            (1, -1), (1, 0), (1, 1)
        };

        private readonly Position[,] Positions;

        internal Board(Position[,] positions)
        {
            Positions = positions;
        }

        internal int Rows => Positions.GetLength(0);

        internal int Cols => Positions.GetLength(1);

        public override bool Equals(object obj) => obj is Board other && Equals(other);

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            for (var row = 0; row < Rows; row++)
            for (var col = 0; col < Cols; col++)
            {
                hashCode.Add(Positions[row,col]);
            }

            return hashCode.ToHashCode();
        }

        public bool Equals(Board other)
        {
            if (Rows != other.Rows || Cols != other.Cols)
            {
                return false;
            }

            for (var row = 0; row < Rows; row++)
            for (var col = 0; col < Cols; col++)
            {
                if (Positions[row,col] != other.Positions[row,col])
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var row = 0; row < Rows; row++)
            {
                if (row != 0)
                {
                    sb.Append('\n');
                }

                for (var col = 0; col < Cols; col++)
                {
                    sb.Append(Positions[row,col] switch { Position.Empty => 'L', Position.Occupied => '#', _ => '.' });
                }
            }

            return sb.ToString();
        }

        internal Board Next(int limit, int distance)
        {
            var newPositions = new Position[Rows,Cols];
            for (var row = 0; row < Rows; row++)
            for (var col = 0; col < Cols; col++)
            {
                newPositions[row,col] = (Positions[row,col], AdjacentOccupied(row, col, distance)) switch
                {
                    (Position.Empty, 0) => Position.Occupied,
                    (Position.Occupied, var n) when n >= limit => Position.Empty,
                    var (position, _) => position
                };
            }

            return new Board(newPositions);
        }

        private int AdjacentOccupied(int row, int col, int distance)
        {
            var count = 0;
            foreach (var (dr, dc) in Directions)
            for (var i = 1; i <= distance; i++)
            {
                var position = GetPosition(row + i * dr, col + i * dc);
                if (position == Position.Occupied)
                {
                    count++;
                    break;
                }

                if (position == Position.Empty)
                {
                    break;
                }
            }

            return count;
        }

        private Position GetPosition(int row, int col)
        {
            return 0 <= row && row < Rows && 0 <= col && col < Cols
                ? Positions[row,col]
                : Position.Floor;
        }
    }
}
