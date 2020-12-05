using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var positions = lines
                .SelectMany(line => SeatPosition.TryParse(line, out var sp) ? new[] { sp } : Enumerable.Empty<SeatPosition>());
            var seatIds = positions
                .Select(sp => sp.SeatId)
                .ToList();
            seatIds.Sort();

            Console.WriteLine("Heighest id: {0}", seatIds.LastOrDefault());

            var ourSeat = seatIds
                .Zip(seatIds.Skip(1), (a, b) => a + 1 < b ? a + 1 : uint.MaxValue)
                .Min();
            Console.WriteLine("Our seat: {0}", ourSeat);
        }

        internal readonly struct SeatPosition
        {
            internal SeatPosition(uint row, uint seat)
            {
                Row = row;
                Seat = seat;
            }

            internal uint Row { get; }

            internal uint Seat { get; }

            internal uint SeatId => Row * 8 + Seat;

            internal static bool TryParse(ReadOnlySpan<char> input, out SeatPosition result)
            {
                result = default;
                if (input.Length != 10)
                {
                    return false;
                }

                var row = 0u;
                for (var i = 0; i < 7; i++)
                {
                    row += row;
                    switch (input[i])
                    {
                        case 'F': break;
                        case 'B': row += 1; break;
                        default: return false;
                    }
                }

                var seat = 0u;
                for (var i = 7; i < 10; i++)
                {
                    seat += seat;
                    switch (input[i])
                    {
                        case 'L': break;
                        case 'R': seat += 1; break;
                        default: return false;
                    }
                }

                result = new SeatPosition(row, seat);
                return true;
            }
        }
    }
}
