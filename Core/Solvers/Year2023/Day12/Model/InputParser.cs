using System.Text;

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
}

internal readonly record struct HotSpringsRecords(string Records, int[] DamagedSprings)
{
    public int CalculatePossibleArrangements()
    {
        var result = 0;

        var stack = new Stack<string>();
        stack.Push(Records);

        while (stack.Count > 0)
        {
            var testStr = stack.Pop();

            var firstUnknownIdx = testStr.IndexOf('?');
            if (firstUnknownIdx == -1)
            {
                if (IsValid(testStr))
                {
                    result++;
                }

                continue;
            }

            stack.Push(ReplaceCharAtIndex(testStr, firstUnknownIdx, '#'));
            stack.Push(ReplaceCharAtIndex(testStr, firstUnknownIdx, '.'));
        }

        return result;
    }

    private bool IsValid(string testRecords)
    {
        var testDamagedSprings = testRecords
            .Split('.', StringSplitOptions.RemoveEmptyEntries)
            .Select(segment => segment.Length)
            .ToArray();

        return DamagedSprings.Length == testDamagedSprings.Length
            && DamagedSprings.SequenceEqual(testDamagedSprings);
    }

    private static string ReplaceCharAtIndex(string content, int index, char replacement)
    {
        // TODO: Use spans
        var stringBuilder = new StringBuilder(content);
        stringBuilder[index] = replacement;

        return stringBuilder.ToString();
    }
};
