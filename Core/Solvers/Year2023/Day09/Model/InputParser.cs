namespace AdventOfCode.Core.Solvers.Year2023.Day09.Model;

internal static class InputParser
{
    public static long[] ParseNumbers(string line)
    {
        return line.Split(' ')
            .Select(long.Parse)
            .ToArray();
    }
}
