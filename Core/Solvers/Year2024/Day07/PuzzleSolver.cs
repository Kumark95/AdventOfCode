using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2024.Day07.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day07;

[PuzzleName("Bridge Repair")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 7;

    [PuzzleInput(filename: "example.txt", expectedResult: 3749)]
    [PuzzleInput(filename: "input.txt", expectedResult: 21572148763543)]
    public object SolvePartOne(string[] inputLines)
    {
        var groups = InputParser.ParseInput(inputLines);

        long result = 0;
        foreach ((long testValue, List<long> numbers) in groups)
        {
            if (IsValid(testValue, numbers, useConcatenation: false))
            {
                result += testValue;
            }
        }

        return result;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 11387)]
    [PuzzleInput(filename: "input.txt", expectedResult: 581941094529163)]
    public object SolvePartTwo(string[] inputLines)
    {
        var groups = InputParser.ParseInput(inputLines);

        long result = 0;
        foreach ((long testValue, List<long> numbers) in groups)
        {
            if (IsValid(testValue, numbers, useConcatenation: true))
            {
                result += testValue;
            }
        }

        return result;
    }

    private static bool IsValid(long testValue, List<long> numbers, bool useConcatenation)
    {
        var queue = new Queue<List<long>>();
        queue.Enqueue(numbers);

        while (queue.Count > 0)
        {
            var tempNumbers = queue.Dequeue();

            var sumResult = tempNumbers[0] + tempNumbers[1];
            var multiplyResult = tempNumbers[0] * tempNumbers[1];
            var concatenatedResult = long.Parse(tempNumbers[0].ToString() + tempNumbers[1].ToString());

            if (tempNumbers.Count == 2)
            {
                // Test all scenarios
                if (sumResult == testValue
                    || multiplyResult == testValue
                    || useConcatenation && concatenatedResult == testValue)
                {
                    return true;
                }
            }

            // Enqueue all options
            tempNumbers.RemoveAt(0);
            if (tempNumbers.Count == 1)
            {
                continue;
            }

            // Break early when the partial result exceeds the test value
            if (sumResult <= testValue)
            {
                var newSumNumbers = new List<long>(tempNumbers);
                newSumNumbers[0] = sumResult;
                queue.Enqueue(newSumNumbers);
            }

            if (multiplyResult <= testValue)
            {
                var newMultNumbers = new List<long>(tempNumbers);
                newMultNumbers[0] = multiplyResult;
                queue.Enqueue(newMultNumbers);
            }

            if (useConcatenation && concatenatedResult <= testValue)
            {
                var newConcatNumbers = new List<long>(tempNumbers);
                newConcatNumbers[0] = concatenatedResult;
                queue.Enqueue(newConcatNumbers);
            }
        }

        return false;
    }
}
