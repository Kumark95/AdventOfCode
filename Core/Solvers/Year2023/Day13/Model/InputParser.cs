namespace AdventOfCode.Core.Solvers.Year2023.Day13.Model;
internal static class InputParser
{
    public static Valley[] ParseInput(string[] inputLines)
    {
        var list = new List<Valley>();

        var group = new List<string>();
        foreach (var line in inputLines)
        {
            if (line == "")
            {
                list.Add(ParseGroup(group));

                // Reset group
                group = new List<string>();
            }
            else
            {
                group.Add(line);
            }
        }

        // Parse the last group
        list.Add(ParseGroup(group));

        return list.ToArray();
    }

    private static Valley ParseGroup(List<string> group)
    {
        var map = new char[group.Count, group[0].Length];
        for (var row = 0; row < group.Count; row++)
        {
            for (var col = 0; col < group[0].Length; col++)
            {
                map[row, col] = group[row][col];
            }
        }

        return new Valley(map);
    }
}
