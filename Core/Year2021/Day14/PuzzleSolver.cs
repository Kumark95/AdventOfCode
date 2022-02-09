using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Year2021.Day14;

[PuzzleName("Extended Polymerization")]
internal class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 14;

    private static (List<string> polymerTemplate, Dictionary<string, string> insertionElements) Input(string[] inputLines)
    {
        var polymerTemplate = inputLines[0]
            .ToCharArray()
            .Select(c => c.ToString())
            .ToList();

        var insertionElements = inputLines
            .Skip(2)
            .ToDictionary(line => line.Split(" -> ")[0], line => line.Split(" -> ")[1]);

        return (polymerTemplate, insertionElements);
    }

    private static long PairInsertion(List<string> polymerTemplate, Dictionary<string, string> insertionElements, int maxSteps)
    {
        for (int step = 1; step <= maxSteps; step++)
        {
            var pairs = polymerTemplate.Zip(
                    polymerTemplate.Skip(1),
                    (first, second) => first + second
                )
                .ToList();

            var insertionCount = 0;
            for (int idx = 0; idx < pairs.Count; idx++)
            {
                var pair = pairs[idx];

                if (insertionElements.ContainsKey(pair))
                {
                    // Insert between the pair taking into account the previous insertions
                    polymerTemplate.Insert(idx + insertionCount + 1, insertionElements[pair]);

                    insertionCount++;
                }
            }
        }

        // Generate the frequency of each
        var frequency = polymerTemplate
            .GroupBy(p => p)
            .ToList()
            .ToDictionary(p => p.Key, p => p.Count());

        var frequencyCounts = frequency.Values.ToList();
        return frequencyCounts.Max() - frequencyCounts.Min();
    }

    public long SolvePartOne(string[] inputLines)
    {
        var (polymerTemplate, insertionElements) = Input(inputLines);

        return PairInsertion(polymerTemplate, insertionElements, 10);
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        return null;
    }
}
