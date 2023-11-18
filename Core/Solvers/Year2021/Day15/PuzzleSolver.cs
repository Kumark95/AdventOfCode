using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2021.Day15;

[PuzzleName("Chiton")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 15;

    /// <summary>
    /// Generates a grid from the input
    /// </summary>
    /// <param name="inputLines"></param>
    /// <returns></returns>
    private static int[,] Input(string[] inputLines)
    {
        var length = inputLines.Length;
        var map = new int[length, length];

        for (int i = 0; i < length; i++)
        {
            var line = inputLines[i];
            for (int j = 0; j < length; j++)
            {
                map[i, j] = line[j] - '0';
            }
        }

        return map;
    }

    /// <summary>
    /// Generates a grid, "rounds" times as large than the original input, increasing the risk level in each round
    /// </summary>
    /// <param name="inputLines"></param>
    /// <param name="rounds"></param>
    /// <returns></returns>
    private static int[,] InputExpanded(string[] inputLines, int rounds)
    {
        var baseLength = inputLines.Length;
        var expandedLength = baseLength * rounds;
        var map = new int[expandedLength, expandedLength];

        for (int x = 0; x < baseLength; x++)
        {
            var line = inputLines[x];
            for (int y = 0; y < baseLength; y++)
            {
                var baseValue = line[y] - '0';

                // Iterate over the rounds * rounds total positions in the final grid
                for (int i = 0; i < rounds; i++)
                {
                    for (int j = 0; j < rounds; j++)
                    {
                        var newValue = (baseValue + i + j - 1) % 9 + 1;
                        map[x + i * baseLength, y + j * baseLength] = newValue;
                    }
                }
            }
        }

        return map;
    }

    public long SolvePartOne(string[] inputLines)
    {
        var map = Input(inputLines);

        var dijkstra = new Dijkstra(map);
        return dijkstra.MinimumCost(new Point(0, 0), new Point(map.GetLength(0) - 1, map.GetLength(1) - 1));
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var map = InputExpanded(inputLines, rounds: 5);

        var dijkstra = new Dijkstra(map);
        return dijkstra.MinimumCost(new Point(0, 0), new Point(map.GetLength(0) - 1, map.GetLength(1) - 1));
    }
}
