using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Solvers.Year2024.Day11;

[PuzzleName("Plutonian Pebbles")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 11;

    [PuzzleInput(filename: "example.txt", expectedResult: 55312)]
    [PuzzleInput(filename: "input.txt", expectedResult: 183620)]
    public object SolvePartOne(string[] inputLines)
    {
        var stones = inputLines[0].Split(" ").Select(long.Parse).ToList();

        return SplitStones(stones, iterations: 25);
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 65601038650482)]
    [PuzzleInput(filename: "input.txt", expectedResult: 220377651399268)]
    public object SolvePartTwo(string[] inputLines)
    {
        var stones = inputLines[0].Split(" ").Select(long.Parse).ToList();

        return SplitStones(stones, iterations: 75);
    }

    private static long SplitStones(List<long> stones, int iterations)
    {
        // Calculate the baseline frequency of each stone number
        var stoneNumberFrequency = stones
            .GroupBy(s => s)
            .ToDictionary(g => g.Key, l => Convert.ToInt64(l.Count()));

        for (var iter = 1; iter <= iterations; iter++)
        {
            // Recalculate the frequency with the new numbers
            stoneNumberFrequency = RecalculateFrequency(stoneNumberFrequency);
        }

        return stoneNumberFrequency.Select(s => s.Value).Sum();
    }

    private static Dictionary<long, long> RecalculateFrequency(Dictionary<long, long> stoneValueFrequency)
    {
        var iterationFrequency = new Dictionary<long, long>(stoneValueFrequency);

        foreach ((long stoneValue, long stoneFrequency) in stoneValueFrequency)
        {
            if (stoneFrequency == 0)
            {
                continue;
            }

            foreach (var newStoneValue in GenerateNewStones(stoneValue))
            {
                if (iterationFrequency.TryGetValue(newStoneValue, out long value))
                {
                    iterationFrequency[newStoneValue] = value + stoneFrequency;
                }
                else
                {
                    iterationFrequency[newStoneValue] = stoneFrequency;
                }
            }

            iterationFrequency[stoneValue] -= stoneFrequency;
        }

        return iterationFrequency;
    }

    private static IEnumerable<long> GenerateNewStones(long stoneValue)
    {
        if (stoneValue == 0)
        {
            yield return 1;
        }
        else
        {
            var stoneValueStr = stoneValue.ToString();
            var numberOfDigits = stoneValueStr.Length;

            var halfPos = numberOfDigits / 2;

            if (numberOfDigits % 2 == 0)
            {
                yield return long.Parse(stoneValueStr[0..halfPos]);
                yield return long.Parse(stoneValueStr[halfPos..]);
            }
            else
            {
                yield return stoneValue * 2024;
            }
        }
    }
}
