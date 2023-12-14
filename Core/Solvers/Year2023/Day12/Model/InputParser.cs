namespace AdventOfCode.Core.Solvers.Year2023.Day12.Model;

internal static class InputParser
{
    public static HotSpringsRecords[] ParseInput(string[] inputLines)
    {
        return inputLines
            .Select(l =>
            {
                var parts = l.Split(' ');
                var records = parts[0];
                var damagedSprings = parts[1]
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();
                return new HotSpringsRecords(records, damagedSprings);
            })
            .ToArray();
    }

    public static HotSpringsRecords[] ParseInputAndExpand(string[] inputLines)
    {
        return inputLines
            .Select(l =>
            {
                var parts = l.Split(' ');

                // This time the input is repeated 5 times
                var records = Repeat(parts[0], repetitions: 5, separator: '?');
                var damagedSprings = Repeat(parts[1], repetitions: 5, separator: ',')
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();
                return new HotSpringsRecords(records, damagedSprings);
            })
            .ToArray();
    }

    private static string Repeat(string value, int repetitions, char separator)
    {
        return string.Join(separator, Enumerable.Repeat(value, repetitions));
    }
}
