namespace AdventOfCode.Core.Solvers.Year2024.Day07.Model;

internal static class InputParser
{
    public static List<(long, List<long>)> ParseInput(string[] inputLines)
    {
        var groups = new List<(long, List<long>)>();

        foreach (var line in inputLines)
        {
            var lineParts = line.Split(": ");
            var testValue = long.Parse(lineParts[0]);

            var numbers = lineParts[1].Split(" ").Select(long.Parse).ToList();

            groups.Add((testValue, numbers));
        }

        return groups;
    }
}
