using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day12
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var start = new State(new(0, 0), new(1,0));
            var lines = File.ReadAllLines("input.txt");
            var endPosition = lines
                .Aggregate(start, Advance);

            Console.WriteLine(
                "Position: {0}",
                Math.Abs(endPosition.Pos.East) + Math.Abs(endPosition.Pos.North));

            start = new State(new(0, 0), new(10,1));
            endPosition = lines
                .Aggregate(start, AdvanceWaypoint);

            Console.WriteLine(
                "Position: {0}",
                Math.Abs(endPosition.Pos.East) + Math.Abs(endPosition.Pos.North));
        }

        private static State Advance(State state, string instruction)
        {
            if (string.IsNullOrWhiteSpace(instruction))
            {
                return state;
            }

            var newState = state;
            var firstChar = instruction[0];
            var value = int.Parse(instruction.AsSpan(1));
            var east = state.Pos.East;
            var north = state.Pos.North;
            var de = state.Dir.East;
            var dn = state.Dir.North;
            switch (firstChar)
            {
                case 'N':
                    newState = state with { Pos = new(east, north + value) };
                    break;
                case 'S':
                    newState = state with { Pos = new(east, north - value) };
                    break;
                case 'E':
                    newState = state with { Pos = new(east + value, north) };
                    break;
                case 'W':
                    newState = state with { Pos = new(east - value, north) };
                    break;
                case 'L':
                    value = value switch
                    {
                        90 => 270,
                        270 => 90,
                        _ => value
                    };
                    goto case 'R';
                case 'R':
                    var newDir = state.Dir;
                    for (var i = 0; i < value; i += 90)
                    {
                        newDir = newDir with { North = -newDir.East, East = newDir.North };
                    }

                    newState = state with { Dir = newDir };
                    break;
                case 'F':
                    newState = state with { Pos = new(east + value * de, north + value * dn) };
                    break;
            }

            return newState;
        }

        private static State AdvanceWaypoint(State state, string instruction)
        {
            if (string.IsNullOrWhiteSpace(instruction))
            {
                return state;
            }

            var newState = state;
            var firstChar = instruction[0];
            var value = int.Parse(instruction.AsSpan(1));
            var east = state.Pos.East;
            var north = state.Pos.North;
            var de = state.Dir.East;
            var dn = state.Dir.North;
            switch (firstChar)
            {
                case 'N':
                    newState = state with { Dir = new(de, dn + value) };
                    break;
                case 'S':
                    newState = state with { Dir = new(de, dn - value) };
                    break;
                case 'E':
                    newState = state with { Dir = new(de + value, dn) };
                    break;
                case 'W':
                    newState = state with { Dir = new(de - value, dn) };
                    break;
                case 'L':
                    value = value switch
                    {
                        90 => 270,
                        270 => 90,
                        _ => value
                    };
                    goto case 'R';
                case 'R':
                    var newDir = state.Dir;
                    for (var i = 0; i < value; i += 90)
                    {
                        newDir = newDir with { North = -newDir.East, East = newDir.North };
                    }

                    newState = state with { Dir = newDir };
                    break;
                case 'F':
                    newState = state with { Pos = new(east + value * de, north + value * dn) };
                    break;
            }

            return newState;
        }
    }

    internal record State(Position Pos, Direction Dir);

    internal record Position(int East, int North);

    internal record Direction(int East, int North);
}
