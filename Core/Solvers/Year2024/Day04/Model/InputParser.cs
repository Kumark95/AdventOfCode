namespace AdventOfCode.Core.Solvers.Year2024.Day04.Model;

internal static class InputParser
{
    public static char[,] ParseInput(string[] inputLines)
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

        return map;
    }
}
