using System;
using System.IO;
using System.Linq;

namespace day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File
                .ReadAllLines("input.txt")
                .SelectMany(s => Line.TryParse(s, out var line) ? new[] { line } : Array.Empty<Line>())
                .ToList();

            var validPasswordsOne = lines.Count(IsValidOne);
            Console.WriteLine("There are {0} valid passwords by the first standard", validPasswordsOne);

            var validPasswordsTwo = lines.Count(IsValidTwo);
            Console.WriteLine("There are {0} valid passwords by the second standard", validPasswordsTwo);
        }

        private static bool IsValidOne(Line line)
        {
            var occurs = line.Password.Count(c => c == line.C);
            return line.MinOccurs <= occurs && occurs <= line.MaxOccurs;
        }

        private static bool IsValidTwo(Line line)
        {
            return line.MaxOccurs <= line.Password.Length
                && (line.Password[line.MinOccurs - 1] == line.C
                ^ line.Password[line.MaxOccurs - 1] == line.C);
        }

        private readonly struct Line
        {
            private Line(int minOccurs, int maxOccurs, char c, string password)
            {
                MinOccurs = minOccurs;
                MaxOccurs = maxOccurs;
                C = c;
                Password = password;
            }

            internal int MinOccurs { get; }

            internal int MaxOccurs { get; }

            internal char C { get; }

            internal string Password { get; }

            public override string ToString()
            {
                return $"{MinOccurs}-{MaxOccurs} {C}: {Password}";
            }

            internal static bool TryParse(string s, out Line result)
            {
                result = default;
                var span = s.AsSpan();
                var splitIndex = span.IndexOf('-');
                if (splitIndex < 0 || !int.TryParse(span.Slice(0, splitIndex), out var minOccurs))
                {
                    return false;
                }

                span = span.Slice(splitIndex + 1);
                splitIndex = span.IndexOf(' ');
                if (splitIndex < 0 || !int.TryParse(span.Slice(0, splitIndex), out var maxOccurs) && maxOccurs < minOccurs)
                {
                    return false;
                }

                span = span.Slice(splitIndex + 1);
                if (span.Length > 2 && span[1] == ':' && span[2] == ' ')
                {
                    var c = span[0];
                    var password = new string(span.Slice(3));
                    result = new Line(minOccurs, maxOccurs, c, password);
                    return true;
                }

                return false;
            }
        }
    }
}
