using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day08.Model;

internal static class InputParser
{
    public static (char[,], Dictionary<char, List<Position>>) ParseInput(string[] inputLines)
    {
        var rowLength = inputLines.Length;
        var colLength = inputLines[0].Length;
        var map = new char[rowLength, colLength];

        var antennaPositions = new Dictionary<char, List<Position>>();

        for (var row = 0; row < rowLength; row++)
        {
            for (var col = 0; col < colLength; col++)
            {
                var character = inputLines[row][col];
                map[row, col] = character;

                if (character != '.')
                {
                    if (antennaPositions.TryGetValue(character, out List<Position>? positions))
                    {
                        positions.Add(new Position(row, col));
                    }
                    else
                    {
                        antennaPositions[character] = [new Position(row, col)];
                    }
                }
            }
        }

        return (map, antennaPositions);
    }
}
