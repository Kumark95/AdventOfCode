using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day03.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day03;

[PuzzleName("Gear Ratios")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 3;

    [PuzzleInput(filename: "example.txt", expectedResult: 4361)]
    [PuzzleInput(filename: "input.txt", expectedResult: 527446)]
    public long? SolvePartOne(string[] inputLines)
    {
        return EngineSchematic.Parse(inputLines)
            .CalculateTotalPartNumber();
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 467835)]
    [PuzzleInput(filename: "input.txt", expectedResult: 73201705)]
    public long? SolvePartTwo(string[] inputLines)
    {
        return EngineSchematic.Parse(inputLines)
            .CalculateTotalGearRatio();
    }
}
