using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day4
{
    class Program
    {
        private static readonly IReadOnlyCollection<char> HexDigit = "0123456789abcdef".ToArray();

        private static readonly IReadOnlyCollection<string> EyeColors = new[]
        {
            "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
        };

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var passports = ReadPassports(lines);

            Console.WriteLine("There are {0} well-formed passports", passports.Count(RequiredFieldsPresent));
            Console.WriteLine("There are {0} valid passports", passports.Count(ValuesValid));
        }

        private static bool RequiredFieldsPresent(Passport p)
        {
            return !(p is null
                || p.BirthYear is null
                || p.IssueYear is null
                || p.ExpirationYear is null
                || p.Height is null
                || p.HairColor is null
                || p.EyeColor is null
                || p.PassportId is null);
        }

        private static bool HeightValid(string? height)
        {
            if (height is null || height.Length < 4)
            {
                return false;
            }

            int.TryParse(height.AsSpan().Slice(0, height.Length - 2), out var value);
            var end = height.Substring(height.Length - 2);
            if (end == "cm" && 150 <= value && value <= 193)
            {
                return true;
            }

            if (end == "in" && 59 <= value && value <= 76)
            {
                return true;
            }

            return false;
        }

        private static bool ValuesValid(Passport p)
        {
            return p is not null
                && int.TryParse(p.BirthYear, out var by) && 1920 <= by && by <= 2002
                && int.TryParse(p.IssueYear, out var iy) && 2010 <= iy && iy <= 2020
                && int.TryParse(p.ExpirationYear, out  var ey) && 2020 <= ey && ey <= 2030
                && HeightValid(p.Height)
                && p.HairColor?.Length == 7 && p.HairColor[0] == '#' && p.HairColor.Skip(1).All(HexDigit.Contains)
                && EyeColors.Contains(p.EyeColor)
                && p.PassportId is { Length: 9 } && p.PassportId.All(char.IsDigit);
        }

        private static IEnumerable<Passport> ReadPassports(IEnumerable<string> lines)
        {
            var passport = new Passport();
            var yielded = false;
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    yield return passport;
                    passport = new Passport();
                    yielded = true;
                    continue;
                }

                yielded = false;
                foreach (var chunk in line.Split(' '))
                {
                    var chunkChunks = chunk.Split(':', 2);
                    var type = chunkChunks[0];
                    var value = chunkChunks[1];
                    passport = (type) switch
                    {
                        "byr" => passport with { BirthYear = value },
                        "iyr" => passport with { IssueYear = value },
                        "eyr" => passport with { ExpirationYear = value },
                        "hgt" => passport with { Height = value },
                        "hcl" => passport with { HairColor = value },
                        "ecl" => passport with { EyeColor = value },
                        "pid" => passport with { PassportId = value },
                        "cid" => passport with { CountryId = value },
                        _ => throw new Exception($"Unknown type: {type}")
                    };
                }
            }

            if (!yielded)
            {
                yield return passport;
            }
        }

        internal record Passport(string? BirthYear = null, string? IssueYear = null, string? ExpirationYear = null, string? Height = null, string? HairColor = null, string? EyeColor = null, string? PassportId = null, string? CountryId = null);
    }
}
