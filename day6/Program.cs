using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var answers = DetermineAnswers(lines);

            var firstResult = 0;
            var secondResult = 0;
            foreach (var answer in answers)
            {
                firstResult += answer.DifferentAnswers();
                secondResult += answer.SameAnswers();
            }

            Console.WriteLine("First answer: {0}", firstResult);
            Console.WriteLine("Second answer: {0}", secondResult);
        }

        private static IEnumerable<AnswerGroup> DetermineAnswers(IEnumerable<string> lines)
        {
            var count = 0;
            var answers = new int[26];
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    yield return new AnswerGroup(count, answers);
                    answers = new int[26];
                    count = 0;
                    continue;
                }

                count++;
                foreach (var c in line)
                {
                    answers[(int)(c - 'a')]++;
                }
            }

            if (count > 0)
            {
                yield return new AnswerGroup(count, answers);
            }
        }

        internal readonly struct AnswerGroup
        {
            private readonly ReadOnlyMemory<int> _answers;

            internal AnswerGroup(int count, ReadOnlyMemory<int> answers)
            {
                Count = count;
                _answers = answers;
            }

            internal int Count { get; }

            internal int SameAnswers()
            {
                var result = 0;
                var span = _answers.Span;
                for (var i = 0; i < span.Length; i++)
                {
                    result += span[i] == Count ? 1 : 0;
                }

                return result;
            }

            internal int DifferentAnswers()
            {
                var result = 0;
                var span = _answers.Span;
                for (var i = 0; i < span.Length; i++)
                {
                    result += span[i] > 0 ? 1 : 0;
                }

                return result;
            }

            public override string ToString()
            {
                var result = new StringBuilder("{ Count: ")
                    .Append(Count)
                    .Append(", Answers: [");
                for (var i = 0; i < _answers.Length; i++)
                {
                    if (i != 0)
                    {
                        result.Append(", ");
                    }

                    result.Append(_answers.Span[i]);
                }

                return result.Append("] }").ToString();
            }
        }
    }
}
