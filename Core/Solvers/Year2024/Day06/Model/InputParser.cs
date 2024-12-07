using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day06.Model;

internal static class InputParser
{
    public static (char[,], Position) ParseInput(string[] inputLines)
    {
        var rowLength = inputLines.Length;
        var colLength = inputLines[0].Length;
        var map = new char[rowLength, colLength];

        Position? startingPos = null;

        for (var row = 0; row < rowLength; row++)
        {
            for (var col = 0; col < colLength; col++)
            {
                map[row, col] = inputLines[row][col];

                if (inputLines[row][col] == '^')
                {
                    startingPos = new Position(row, col);
                }
            }
        }

        if (startingPos is null)
        {
            throw new Exception("Could not find initial position");
        }

        return (map, startingPos.Value);
    }
}
