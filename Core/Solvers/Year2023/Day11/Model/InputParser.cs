namespace AdventOfCode.Core.Solvers.Year2023.Day11.Model;

internal static class InputParser
{
    public static Universe ParseInput(string[] inputLines)
    {
        var rowLength = inputLines.Length;
        var colLength = inputLines[0].Length;

        var map = new char[rowLength, colLength];
        for (var row = 0; row < rowLength; row++)
        {
            for (var col = 0; col < colLength; col++)
            {
                map[row, col] = inputLines[row][col];
            }
        }

        return new Universe(map);
    }
}
