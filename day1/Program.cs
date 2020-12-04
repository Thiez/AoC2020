using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = File
                .ReadAllLines("input.txt")
                .SelectMany(s => uint.TryParse(s, out var n) ? new[] { n } : Array.Empty<uint>());
            var numberSet = new HashSet<uint>(numbers);

            var firstResult = 0u;
            var secondResult = 0u;
            foreach (var a in numberSet)
            {
                foreach (var b in numberSet)
                {
                    var sum = a + b;
                    if (sum == 2020)
                    {
                        firstResult = a * b;
                    }
                    else if (sum < 2020 && numberSet.Contains(2020 - sum))
                    {
                        secondResult = a * b * (2020 - sum);
                    }
                }
            }

            Console.WriteLine("First result: {0}\nSecond result: {1}", firstResult, secondResult);
        }
    }
}
