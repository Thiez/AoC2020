using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var adapters = File
                .ReadAllLines("input.txt")
                .SelectMany(s => int.TryParse(s, out var n) ? new[] { n } : new int[0])
                .ToList();
            adapters.Sort();

            {
                var prevJolts = 0;
                var diffs = new int[4];
                foreach (var adapter in adapters)
                {
                    diffs[adapter - prevJolts]++;
                    prevJolts = adapter;
                }

                diffs[3]++;
                Console.WriteLine("First answer: {0}", diffs[1] * diffs[3]);
            }

            {
                var prevJolts = new List<(int Jolts, long Paths)> { (0, 1) };
                for (var i = 0; i < adapters.Count; i++)
                {
                    var adapter = adapters[i];
                    prevJolts.RemoveAll(tup => adapter - tup.Jolts > 3);
                    var paths = prevJolts.Select(tup => tup.Paths).Sum();
                    prevJolts.Add((adapter, paths));
                    //Console.WriteLine(string.Join(", ", prevJolts));
                }

                var combinations = prevJolts.Last().Paths;
                Console.WriteLine("Second answer: {0}", combinations);
            }
        }
    }
}
