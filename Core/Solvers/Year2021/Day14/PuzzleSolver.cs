using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Solvers.Year2021.Day14;

[PuzzleName("Extended Polymerization")]
internal class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 14;

    private static (string polymerTemplate, Dictionary<string, string> insertionElements) Input(string[] inputLines)
    {
        var insertionElements = inputLines
            .Skip(2)
            .ToDictionary(line => line.Split(" -> ")[0], line => line.Split(" -> ")[1]);

        return (inputLines[0], insertionElements);
    }

    private static long PairInsertion(string polymerTemplate, Dictionary<string, string> insertionElements, int maxSteps)
    {
        Dictionary<string, long> basePairFrequency = polymerTemplate.Zip(
                    polymerTemplate.Skip(1),
                    (first, second) => new string(new[] { first, second })
                )
                .GroupBy(p => p)
                .ToDictionary(p => p.Key, p => Convert.ToInt64(p.Count()));

        for (int step = 1; step <= maxSteps; step++)
        {
            var newPairFrequency = new Dictionary<string, long>();
            foreach (var (basePair, basePairCount) in basePairFrequency)
            {
                if (insertionElements.ContainsKey(basePair))
                {
                    var pairFirst = basePair[0].ToString();
                    var pairLast = basePair[1].ToString();

                    var elementToInsert = insertionElements[basePair];

                    var newPairs = new List<string> { pairFirst + elementToInsert, elementToInsert + pairLast };
                    foreach (var newPair in newPairs)
                    {
                        if (newPairFrequency.ContainsKey(newPair))
                        {
                            newPairFrequency[newPair] += basePairCount;
                        }
                        else
                        {
                            newPairFrequency.Add(newPair, basePairCount);
                        }
                    }
                }
            }

            basePairFrequency = newPairFrequency;
        }

        // Generate the frequency of each element
        var elementFrequency = new Dictionary<char, long>();
        foreach (var (pair, count) in basePairFrequency)
        {
            // Only sum the first element of the pair as the second is always duplicated
            if (!elementFrequency.ContainsKey(pair[0]))
            {
                elementFrequency[pair[0]] = count;
            }
            else
            {
                elementFrequency[pair[0]] += count;
            }
        }
        // The first and last element of the polymer does not change. But needs to add the last to the count
        elementFrequency[polymerTemplate.Last()]++;

        var frequencyCounts = elementFrequency.Values.ToList();
        return frequencyCounts.Max() - frequencyCounts.Min();
    }

    public long SolvePartOne(string[] inputLines)
    {
        var (polymerTemplate, insertionElements) = Input(inputLines);

        return PairInsertion(polymerTemplate, insertionElements, 10);
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var (polymerTemplate, insertionElements) = Input(inputLines);

        return PairInsertion(polymerTemplate, insertionElements, 40);
    }
}
