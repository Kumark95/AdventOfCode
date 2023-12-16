namespace AdventOfCode.Core.Solvers.Year2023.Day14.Model;

internal static class InputParser
{
    public static ReflectorDish ParseInput(string[] inputLines)
    {
        var rowLength = inputLines.Length;
        var colLength = inputLines[0].Length;
        var map = new char[rowLength, colLength];

        for (int row = 0; row < rowLength; row++)
        {
            for (int col = 0; col < colLength; col++)
            {
                map[row, col] = inputLines[row][col];
            }
        }

        return new ReflectorDish(map);
    }
}
