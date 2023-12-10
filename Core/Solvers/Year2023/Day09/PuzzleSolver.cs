using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day09.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day09;

[PuzzleName("Mirage Maintenance")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 9;

    [PuzzleInput(filename: "example.txt", expectedResult: 114)]
    [PuzzleInput(filename: "input.txt", expectedResult: 1479011877)]
    public long? SolvePartOne(string[] inputLines)
    {
        return inputLines
            .Select(InputParser.ParseNumbers)
            .Select(SequencePrediction.PredictNextValue)
            .Sum();
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 2)]
    [PuzzleInput(filename: "input.txt", expectedResult: 973)]
    public long? SolvePartTwo(string[] inputLines)
    {
        return inputLines
            .Select(InputParser.ParseNumbers)
            .Select(SequencePrediction.PredictPreviousValue)
            .Sum();
    }
}
