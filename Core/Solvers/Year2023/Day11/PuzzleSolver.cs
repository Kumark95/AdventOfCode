using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day11.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day11;

[PuzzleName("Cosmic Expansion")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 11;

    [PuzzleInput(filename: "example.txt", expectedResult: 374)]
    [PuzzleInput(filename: "input.txt", expectedResult: 10289334)]
    public long? SolvePartOne(string[] inputLines)
    {
        var universe = InputParser.ParseInput(inputLines);

        return universe.CalculateTotalShortestPathBetweenGalaxies(expansionConstant: 1);
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 82000210)]
    [PuzzleInput(filename: "input.txt", expectedResult: 649862989626)]
    public long? SolvePartTwo(string[] inputLines)
    {
        var universe = InputParser.ParseInput(inputLines);

        return universe.CalculateTotalShortestPathBetweenGalaxies(expansionConstant: 1_000_000 - 1);
    }
}
