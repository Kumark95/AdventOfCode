using AdventOfCode.Common.Algorithms;
using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Solvers.Year2021.Day09;


[PuzzleName("Smoke Basin")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 9;

    private static int[][] Input(string[] inputLines) => inputLines
            .Select(line => line.Select(c => c - '0').ToArray())
            .ToArray();

    /// <summary>
    /// Find the adjacent items in a grid
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    private List<int> Adjacent(int[][] grid, int row, int col)
    {
        var adjacentLocations = new List<int>();

        if (row - 1 >= 0)
            adjacentLocations.Add(grid[row - 1][col]); // Top
        if (row + 1 < grid.Length)
            adjacentLocations.Add(grid[row + 1][col]); // Bottom
        if (col - 1 >= 0)
            adjacentLocations.Add(grid[row][col - 1]); // Left
        if (col + 1 < grid[0].Length)
            adjacentLocations.Add(grid[row][col + 1]); // Right

        return adjacentLocations;
    }

    public object SolvePartOne(string[] inputLines)
    {
        var input = Input(inputLines);

        var result = 0;
        for (int row = 0; row < input.Length; row++)
        {
            for (int col = 0; col < input[0].Length; col++)
            {
                int current = input[row][col];

                var adjacentLocations = Adjacent(input, row, col);

                var isLowPoint = adjacentLocations.All(l => current < l);
                if (isLowPoint)
                {
                    result += 1 + current;
                }
            }
        }

        return result;
    }

    public object SolvePartTwo(string[] inputLines)
    {
        var input = Input(inputLines);

        var basins = new List<List<int>>();
        for (int row = 0; row < input.Length; row++)
        {
            for (int col = 0; col < input[0].Length; col++)
            {
                int current = input[row][col];

                var adjacentLocations = Adjacent(input, row, col);

                var isLowPoint = adjacentLocations.All(l => current < l);
                if (isLowPoint)
                {
                    // Traverse grid to search
                    var bfs = new BreadthFirstGrid(input);

                    var basinLocations = bfs.Traverse(row, col, exitCondition: i => i == 9);

                    // The lowPoint also form part of the basin
                    basinLocations.Add(current);

                    basins.Add(basinLocations);
                }
            }
        }

        return basins
            .OrderByDescending(b => b.Count)
            .Take(3)
            .Aggregate(seed: 1, (total, next) => total * next.Count);
    }
}
