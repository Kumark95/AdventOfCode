using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day13.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day13;

[PuzzleName("Point of Incidence")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 13;

    [PuzzleInput(filename: "example.txt", expectedResult: 405)]
    [PuzzleInput(filename: "input.txt", expectedResult: 40006)]
    public object SolvePartOne(string[] inputLines)
    {
        var valleys = InputParser.ParseInput(inputLines);

        return valleys
            .Select(v => v.CalculateScore())
            .Sum();
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 400)]
    [PuzzleInput(filename: "input.txt", expectedResult: 28627)]
    public object SolvePartTwo(string[] inputLines)
    {
        var valleys = InputParser.ParseInput(inputLines);

        return valleys
            .Select(v => v.FixSmudgeAndCalculateScore())
            .Sum();
    }
}
