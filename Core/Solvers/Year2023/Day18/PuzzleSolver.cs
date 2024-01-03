using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day18.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day18;

[PuzzleName("Lavaduct Lagoon")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 18;

    [PuzzleInput(filename: "example.txt", expectedResult: 62)]
    [PuzzleInput(filename: "input.txt", expectedResult: 34329)]
    public object SolvePartOne(string[] inputLines)
    {
        var instructions = InputParser.ParseInput(inputLines);

        var digPlan = new DigPlan(instructions);
        return digPlan.CalculateLagoonCapacity();
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 952408144115)]
    [PuzzleInput(filename: "input.txt", expectedResult: 42617947302920)]
    public object SolvePartTwo(string[] inputLines)
    {
        var instructions = InputParser.ParseInput(inputLines, fixInstructions: true);

        var digPlan = new DigPlan(instructions);
        return digPlan.CalculateLagoonCapacity();
    }
}
