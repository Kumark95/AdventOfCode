namespace AdventOfCode.Core.Solvers.Year2024.Day05.Model;

internal static class InputParser
{
    public static (List<List<int>> Pages, Dictionary<int, HashSet<int>> Instructions) ParseInput(string[] inputLines)
    {
        var pages = new List<List<int>>();
        var instructions = new Dictionary<int, HashSet<int>>();

        foreach (var line in inputLines)
        {
            if (line.Contains('|'))
            {
                var pageNumbers = line.Split('|').Select(int.Parse).ToList();
                var left = pageNumbers[0];
                var right = pageNumbers[1];

                if (instructions.TryGetValue(left, out HashSet<int>? set))
                {
                    set.Add(right);
                }
                else
                {
                    instructions[left] = [right];
                }
            }
            else if (line.Contains(','))
            {
                var pageNumbers = line.Split(',').Select(int.Parse).ToList();
                pages.Add(pageNumbers);
            }
        }

        return (pages, instructions);
    }
}
