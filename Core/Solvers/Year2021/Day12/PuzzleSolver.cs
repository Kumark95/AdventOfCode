using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Solvers.Year2021.Day12;


[PuzzleName("Passage Pathing")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 12;


    /// <summary>
    /// Generates the adjacency list for each vertex 
    /// </summary>
    /// <param name="inputLines"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Generate all the possible paths from startVertex to endVertex using different exploration modes
    /// </summary>
    /// <param name="adjacencyLists">Adjacency lists for each vertex</param>
    /// <param name="startVertex"></param>
    /// <param name="endVertex"></param>
    /// <param name="explorationMode">Conditions to apply to the path finding</param>
    /// <returns></returns>
    private static List<List<string>> FindPaths(Dictionary<string, HashSet<string>> adjacencyLists, string startVertex, string endVertex, ExplorationMode explorationMode)
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

                // Apply a different condition based on the exploration mode
                if (explorationMode == ExplorationMode.SmallCaveOnlyOnce)
                {
                    // Lowercase nodes can only appear once
                    if (adjacentVertex == adjacentVertex.ToLowerInvariant() && currentPath.Contains(adjacentVertex))
                    {
                        continue;
                    }
                }
                else
                {
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
                }

                var adjacentVerticePath = currentPath.Append(adjacentVertex).ToList();
                stack.Push((adjacentVertex, adjacentVerticePath));
            }
        }

        return paths;
    }

    public object SolvePartOne(string[] inputLines)
    {
        var adjacencyLists = Input(inputLines);

        var paths = FindPaths(adjacencyLists, "start", "end", ExplorationMode.SmallCaveOnlyOnce);
        return paths.Count;
    }

    public object SolvePartTwo(string[] inputLines)
    {
        var adjacencyLists = Input(inputLines);

        var paths = FindPaths(adjacencyLists, "start", "end", ExplorationMode.SingleSmallCaveMaxTwice);
        return paths.Count;
    }
}
