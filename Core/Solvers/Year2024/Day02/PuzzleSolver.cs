using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Solvers.Year2024.Day02;

[PuzzleName("Red-Nosed Reports")]
public sealed partial class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 2;

    [PuzzleInput(filename: "example.txt", expectedResult: 2)]
    [PuzzleInput(filename: "input.txt", expectedResult: 534)]
    public object SolvePartOne(string[] inputLines)
    {
        var safeCount = 0;

        foreach (var line in inputLines)
        {
            var numbers = InputParser.ParseNumbers(line);

            if (Report.IsSafe(numbers))
            {
                safeCount++;
            }
        }

        return safeCount;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 4)]
    [PuzzleInput(filename: "input.txt", expectedResult: 577)]
    public object SolvePartTwo(string[] inputLines)
    {
        var safeCount = 0;

        foreach (var line in inputLines)
        {
            var numbers = InputParser.ParseNumbers(line);

            var isSafe = Report.IsSafe(numbers);
            if (isSafe)
            {
                safeCount++;
                continue;
            }

            // Test removing one item at a time
            for (int posToRemove = 0; posToRemove < numbers.Count; posToRemove++)
            {
                var candidateNumbers = numbers.Where((_, index) => index != posToRemove).ToList();
                var isCandidateSafe = Report.IsSafe(candidateNumbers);
                if (isCandidateSafe)
                {
                    safeCount++;
                    break;
                }
            }
        }

        return safeCount;
    }

    private class InputParser
    {
        internal static List<int> ParseNumbers(string line)
        {
            return line
                .Split(' ')
                .Select(int.Parse)
                .ToList();
        }
    }

    private enum State
    {
        Ascending,
        Descending
    }

    private class Report
    {
        internal static bool IsSafe(List<int> numbers)
        {
            if (numbers.Count < 2)
            {
                return false;
            }

            var prev = numbers[0];
            State? prevState = null;

            foreach (var current in numbers.Skip(1))
            {
                var diff = current - prev;
                if (Math.Abs(diff) > 3 || diff == 0)
                {
                    return false;
                }

                var curState = diff > 0 ? State.Ascending : State.Descending;
                if (prevState is not null && prevState != curState)
                {
                    return false;
                }

                prev = current;
                prevState = curState;
            }

            return true;
        }
    }
}
