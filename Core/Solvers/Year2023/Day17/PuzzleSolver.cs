using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day17.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day17;

[PuzzleName("Clumsy Crucible")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 17;

    [PuzzleInput(filename: "example.txt", expectedResult: 102)]
    [PuzzleInput(filename: "input.txt", expectedResult: -1)]
    public long? SolvePartOne(string[] inputLines)
    {
        var crucibleMap = InputParser.ParseInput(inputLines);

        return crucibleMap.CalculateMinimumHeatLoss();
    }

    [PuzzleInput(filename: "example.txt", expectedResult: -1)]
    [PuzzleInput(filename: "input.txt", expectedResult: -1)]
    public long? SolvePartTwo(string[] inputLines)
    {
        return null;
    }
}