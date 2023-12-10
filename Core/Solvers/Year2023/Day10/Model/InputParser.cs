using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day10.Model;

internal static class InputParser
{
    public static PipeMaze ParseInput(string[] inputLines)
    {
        var map = new char[inputLines.Length, inputLines[0].Length];
        Position? startPosition = null;

        for (var row = 0; row < inputLines.Length; row++)
        {
            for (var col = 0; col < inputLines[0].Length; col++)
            {
                var character = inputLines[row][col];
                if (character == 'S')
                {
                    startPosition = new Position(row, col);
                }

                map[row, col] = character;
            }
        }

        if (startPosition is null)
        {
            throw new ArgumentException("Invalid start position");
        }

        return new PipeMaze(map, (Position)startPosition);
    }
}
