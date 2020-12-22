using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day9
{
    class Program
    {
        private const int WindowSize = 25;

        static void Main(string[] args)
        {
            var lines = File
                .ReadAllLines("input.txt")
                .SelectMany(s => long.TryParse(s, out var l) ? new[] { l } : new long[0])
                .ToList();

            var targetNumber = 0L;
            var i = WindowSize;
            while (true)
            {
                var found = false;
                targetNumber = lines[i];
                for (var j = i - WindowSize; j < i ; j++)
                {
                    for (var k = j + 1; k < i; k++)
                    {
                        var fst = lines[j];
                        var snd = lines[k];
                        found |= fst != snd & fst + snd == targetNumber;
                    }
                }

                if (!found)
                {
                    break;
                }

                i++;
            }

            Console.WriteLine("First failure: {0}", targetNumber);

            var windowStart = 0;
            var windowEnd = 0;
            var windowSum = 0L;
            while (true)
            {
                if (windowSum < targetNumber)
                {
                    windowSum += lines[windowEnd++];
                }
                else if (windowSum > targetNumber)
                {
                    windowSum -= lines[windowStart++];
                }
                else
                {
                    var min = long.MaxValue;
                    var max = long.MinValue;
                    i = windowStart;
                    while (i < windowEnd)
                    {
                        min = Math.Min(min, lines[i]);
                        max = Math.Max(max, lines[i]);
                        i++;
                    }

                    Console.WriteLine(
                        "Found range {0} to {1}, sum of min and max: {2}",
                        windowStart,
                        windowEnd,
                        min + max);
                    break;
                }
            }
        }
    }
}
