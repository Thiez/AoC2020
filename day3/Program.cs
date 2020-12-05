using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").Where(s => !string.IsNullOrWhiteSpace(s));
            var board = new Board(lines);

            var routes = new (Position Position, Step Step, ulong Count)[]
            {
                (default, new Step(1, 1), 0),
                (default, new Step(3, 1), 0),
                (default, new Step(5, 1), 0),
                (default, new Step(7, 1), 0),
                (default, new Step(1, 2), 0),
            };

            for (var i = 0; i < routes.Length; i++)
            {
                var (position, step, count) = routes[i];
                char? c;
                do
                {
                    c = board.CharAt(position);
                    count += c == '#' ? 1ul : 0ul;
                    position = position.MakeStep(step);
                }
                while (c is not null);

                routes[i] = (position, step, count);
            }

            Console.WriteLine("Trees for 3,1 step route: {0}", routes[1].Count);
            Console.WriteLine("Product of trees for all routes: {0}", routes.Aggregate(1ul, (sum, result) => sum * result.Count));
        }

        internal readonly struct Position
        {
            internal Position(uint right, uint down)
            {
                Right = right;
                Down = down;
            }

            internal uint Right { get; }

            internal uint Down { get; }

            internal Position MakeStep(Step step)
            {
                return new Position(Right + step.Right, Down + step.Down);
            }
        }

        internal readonly struct Step
        {
            internal Step(uint right, uint down)
            {
                Right = right;
                Down = down;
            }

            internal uint Right { get; }

            internal uint Down { get; }
        }

        private class Board
        {
            private readonly string[] _template;

            internal Board(IEnumerable<string> template)
            {
                _template = template?.ToArray() ?? throw new ArgumentNullException(nameof(template));
            }

            internal char? CharAt(Position position)
            {
                if (position.Down >= _template.Length)
                {
                    return null;
                }

                var line = _template[(int)position.Down];
                var index = (int)( position.Right % line.Length );
                return line[index];
            }
        }
    }
}
