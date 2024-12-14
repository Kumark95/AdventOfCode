using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day10.Model;

internal static class InputParser
{
    public static (int[,], List<Position>) ParseInput(string[] inputLines)
    {
        var trailheadPositions = new List<Position>();

        var rowLength = inputLines.Length;
        var colLength = inputLines[0].Length;
        var map = new int[rowLength, colLength];

        for (var row = 0; row < rowLength; row++)
        {
            for (var col = 0; col < colLength; col++)
            {
                map[row, col] = inputLines[row][col] - '0';

                if (map[row, col] == 0)
                {
                    trailheadPositions.Add(new Position(row, col));
                }
            }
        }

        return (map, trailheadPositions);
    }
}
