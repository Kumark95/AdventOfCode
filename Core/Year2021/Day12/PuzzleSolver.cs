using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Year2021.Day12;


[PuzzleName("Passage Pathing")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 12;


    private static Dictionary<string, HashSet<string>> Input(string[] inputLines)
    {
        var graph = new Graph();
        foreach (var line in inputLines)
        {
            var lineComponents = line.Split('-');
            var u = lineComponents[0];
            var v = lineComponents[1];

            graph.AddEdge(u, v);
        }

        return graph.Edges;
    }

    private List<List<string>> FindPathsMaxOnceSmallCaves(Dictionary<string, HashSet<string>> adjacencyLists, string startVertex, string endVertex)
    {
        var paths = new List<List<string>>();

        var stack = new Stack<(string vertice, List<string> path)>();
        stack.Push((startVertex, new List<string> { startVertex }));

        while (stack.Count != 0)
        {
            (string vertex, List<string> currentPath) = stack.Pop();
            if (currentPath.Last() == endVertex)
            {
                paths.Add(currentPath);
                continue;
            }

            foreach (var adjacentVertex in adjacencyLists[vertex])
            {
                if (adjacentVertex == startVertex) continue;

                // Lowercase nodes can only appear once
                if (adjacentVertex == adjacentVertex.ToLowerInvariant() && currentPath.Contains(adjacentVertex)) continue;

                var adjacentVerticePath = currentPath.Append(adjacentVertex).ToList();
                stack.Push((adjacentVertex, adjacentVerticePath));
            }
        }

        return paths;
    }

    private List<List<string>> FindPathsMaxTwiceSingleSmallCave(Dictionary<string, HashSet<string>> adjacencyLists, string startVertex, string endVertex)
    {
        var paths = new List<List<string>>();

        var stack = new Stack<(string vertice, List<string> path)>();
        stack.Push((startVertex, new List<string> { startVertex }));

        while (stack.Count != 0)
        {
            (string vertex, List<string> currentPath) = stack.Pop();
            if (currentPath.Last() == endVertex)
            {
                paths.Add(currentPath);
                continue;
            }

            foreach (var adjacentVertex in adjacencyLists[vertex])
            {
                if (adjacentVertex == startVertex) continue;

                // Lowercase nodes can only appear once
                if (adjacentVertex == adjacentVertex.ToLowerInvariant())
                {
                    var smallCaveVisitedMoreThanOnce = currentPath
                        .Where(v => v != startVertex && v != endVertex && v == v.ToLowerInvariant())
                        .GroupBy(v => v)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key)
                        .FirstOrDefault();

                    if (smallCaveVisitedMoreThanOnce != null)
                    {
                        if (smallCaveVisitedMoreThanOnce == adjacentVertex || currentPath.Contains(adjacentVertex))
                        {
                            continue;
                        }
                    }
                }

                var adjacentVerticePath = currentPath.Append(adjacentVertex).ToList();
                stack.Push((adjacentVertex, adjacentVerticePath));
            }
        }

        return paths;
    }

    public long SolvePartOne(string[] inputLines)
    {
        var edges = Input(inputLines);

        var paths = FindPathsMaxOnceSmallCaves(edges, "start", "end");
        return paths.Count;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var edges = Input(inputLines);


        var paths = FindPathsMaxTwiceSingleSmallCave(edges, "start", "end");
        return paths.Count;
    }
}
