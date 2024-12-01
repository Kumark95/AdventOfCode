namespace AdventOfCode.Core.Solvers.Year2024.Day01.Model;

internal static class InputParser
{
    public static (int[] firstList, int[] secondList) ParseLists(string[] inputLines)
    {
        var firstList = new int[inputLines.Length];
        var secondList = new int[inputLines.Length];

        for (int i = 0; i < inputLines.Length; i++)
        {
            var parts = inputLines[i].Split("   ");
            firstList[i] = int.Parse(parts[0]);
            secondList[i] = int.Parse(parts[1]);
        }

        return (firstList, secondList);
    }
}
