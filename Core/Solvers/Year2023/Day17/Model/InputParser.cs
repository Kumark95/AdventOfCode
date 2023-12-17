namespace AdventOfCode.Core.Solvers.Year2023.Day17.Model;

internal static class InputParser
{
    public static CrucibleMap ParseInput(string[] inputLines)
    {
        var rowLength = inputLines.Length;
        var colLength = inputLines[0].Length;
        var map = new int[rowLength, colLength];

        for (var row = 0; row < rowLength; row++)
        {
            for (var col = 0; col < colLength; col++)
            {
                map[row, col] = inputLines[row][col] - '0';
            }
        }

        return new CrucibleMap(map);
    }
}
