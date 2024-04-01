using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day21.Model;

internal static class InputParser
{
    public static (char[,] map, Position startPosition, List<Position> rockPositions) ParseInput(string[] inputLines)
    {
        var rowLength = inputLines.Length;
        var colLength = inputLines[0].Length;
        var map = new char[rowLength, colLength];

        Position startPosition = new();
        List<Position> rockPositions = [];
        for (var row = 0; row < rowLength; row++)
        {
            for (var col = 0; col < colLength; col++)
            {
                var character = inputLines[row][col];
                if (character == 'S')
                {
                    startPosition = new Position(row, col);

                    // The start position acts as a garden plot
                    character = '.';
                }
                else if (character == '#')
                {
                    rockPositions.Add(new Position(row, col));
                }

                map[row, col] = character;
            }
        }

        return (map, startPosition, rockPositions);
    }
}
