using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day03.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day03;

[PuzzleName("Gear Ratios")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 3;

    public long SolvePartOne(string[] inputLines)
    {
        return EngineSchematic.Parse(inputLines)
            .CalculateTotalPartNumber();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        return EngineSchematic.Parse(inputLines)
            .CalculateTotalGearRatio();
    }
}
