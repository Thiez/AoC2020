using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using QuickGraph;


namespace day7
{
    using RuleGraph = QuickGraph.BidirectionalGraph<Bag, Edge<Bag>>;

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var graph = lines
                .Where(s => !string.IsNullOrEmpty(s))
                .Aggregate(new RuleGraph(), AddToGraph);

            var shinyGold = new Bag("shiny", "gold");

            {
                var seen = new HashSet<Bag>();
                var toVisit = new List<Bag>{ shinyGold };
                for (var i = 0; i < toVisit.Count; i++)
                {
                    var target = toVisit[i];
                    toVisit.AddRange(graph.InEdges(target).Select(e => e.Source).Where(seen.Add));
                }

                Console.WriteLine("Bags that may contain a shiny gold bag: {0}", toVisit.Count - 1);
            }

            {
                var seenEdges = graph.OutEdges(shinyGold).ToList();
                for (var i = 0; i < seenEdges.Count; i++)
                {
                    var newSource = seenEdges[i].Target;
                    seenEdges.AddRange(graph.OutEdges(newSource));
                }

                Console.WriteLine("Bags contained by a shiny gold bag: {0}", seenEdges.Count);
            }
        }

        private static RuleGraph AddToGraph(RuleGraph graph, string line)
        {
            var chunks = line.Split(' ');
            var source = new Bag(chunks[0], chunks[1]);
            graph.AddVertex(source);
            if (chunks[4] != "no")
            {
                for (var i = 4; i < chunks.Length; i += 4)
                {
                    var target = new Bag(chunks[i+1], chunks[i+2]);
                    graph.AddVertex(target);
                    var count = uint.Parse(chunks[i]);
                    for (var j = 0u; j < count; j++)
                    {
                        graph.AddEdge(new Edge<Bag>(source, target));
                    }
                }
            }

            return graph;
        }
    }

    internal record Bag(string Style, string Color);
}
