namespace AdventOfCode.Core.Solvers.Year2023.Day06.Model;

internal static class InputParser
{
    public static IEnumerable<long> ExtractNumbers(string line)
    {
        return line
            .Substring(line.IndexOf(':') + 1)
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse);
    }

    public static long ExtractSingleNumber(string line)
    {
        var strValue = line
            .Substring(line.IndexOf(':') + 1)
            .Replace(" ", "");

        return long.Parse(strValue);
    }
}
