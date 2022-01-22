using AdventOfCode2021.Lib;

namespace AdventOfCode2021.Day1;

/// <summary>
/// Puzzle for Day 1.
/// Src: https://adventofcode.com/2021/day/1
/// </summary>
[PuzzleName("Sonar Sweep")]
public class Day1Solver : IPuzzleSolver
{

    public int CountPreviousLargerItem(int[] input)
    {
        var counter = 0;

        for (var i = 0; i < input.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }

            if (input[i] > input[i - 1])
            {
                counter++;
            }
        }

        return counter;
    }

    public int[] SlidingWindow(int[] input, int window)
    {
        var result = new int[input.Length - window + 1];
        int stopIndex = input.Length - window;
        int? currentSum = null;
        for (int i = 0; i < input.Length; i++)
        {
            // Exit if the window sum cannot be computed
            if (i > stopIndex)
            {
                break;
            }

            if (currentSum.HasValue)
            {
                currentSum += input[i + window - 1]; // Add next
                currentSum -= input[i - 1]; // Remove previous
            }
            else
            {
                currentSum = input[i] + input[i + 1] + input[i + 2];
            }

            result[i] = currentSum.Value;
        }

        return result;
    }

    private int[] Input(string[] inputLines) => inputLines
            .Select(l => int.Parse(l))
            .ToArray();

    public int SolvePartOne(string[] inputLines)
    {
        return CountPreviousLargerItem(Input(inputLines));
    }

    public int? SolvePartTwo(string[] inputLines)
    {
        var slidingWindowInput = SlidingWindow(Input(inputLines), window: 3);
        return CountPreviousLargerItem(slidingWindowInput);
    }
}
