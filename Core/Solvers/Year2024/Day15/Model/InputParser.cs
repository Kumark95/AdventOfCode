using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day15.Model;

internal static class InputParser
{
    public static (char[,], Position, char[]) ParseInput(string[] inputLines)
    {
        var rowLength = inputLines.Where(l => l.StartsWith('#')).Count();
        var colLength = inputLines[0].Length;
        var map = new char[rowLength, colLength];

        Position? robotPosition = null;

        for (var row = 0; row < rowLength; row++)
        {
            for (var col = 0; col < colLength; col++)
            {
                map[row, col] = inputLines[row][col];

                if (inputLines[row][col] == '@')
                {
                    robotPosition = new Position(row, col);
                }
            }
        }

        if (robotPosition is null)
        {
            throw new InvalidOperationException("Could not find robot initial position");
        }

        var instructions = inputLines.Skip(rowLength + 1).SelectMany(l => l.ToCharArray()).ToArray();

        return (map, robotPosition.Value, instructions);
    }

    public static (char[,], Position, char[]) ParseWideInput(string[] inputLines)
    {
        var rowLength = inputLines.Where(l => l.StartsWith('#')).Count();

        // The map is now twice as wide
        var originalLength = inputLines[0].Length;
        var colLength = originalLength * 2;

        var map = new char[rowLength, colLength];

        Position? robotPosition = null;

        for (var row = 0; row < rowLength; row++)
        {
            for (var col = 0; col < originalLength; col++)
            {
                var character = inputLines[row][col];

                if (character == '@')
                {
                    robotPosition = new Position(row, col * 2);
                    map[row, col * 2] = character;
                    map[row, col * 2 + 1] = '.';
                }
                else if (character == 'O')
                {
                    map[row, col * 2] = '[';
                    map[row, col * 2 + 1] = ']';
                }
                else
                {
                    map[row, col * 2] = character;
                    map[row, col * 2 + 1] = character;
                }
            }
        }

        if (robotPosition is null)
        {
            throw new InvalidOperationException("Could not find robot initial position");
        }

        var instructions = inputLines.Skip(rowLength + 1).SelectMany(l => l.ToCharArray()).ToArray();

        return (map, robotPosition.Value, instructions);
    }
}
