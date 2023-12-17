using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Solvers.Year2023.Day16.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day16;

[PuzzleName("The Floor Will Be Lava")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 16;

    [PuzzleInput(filename: "example.txt", expectedResult: 46)]
    [PuzzleInput(filename: "input.txt", expectedResult: 8034)]
    public long? SolvePartOne(string[] inputLines)
    {
        var lightContraption = InputParser.ParseInput(inputLines);

        return lightContraption.CalculateEnergizedPositions(new Position(0, 0), Direction.Right);
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 51)]
    [PuzzleInput(filename: "input.txt", expectedResult: 8225)]
    public long? SolvePartTwo(string[] inputLines)
    {
        var lightContraption = InputParser.ParseInput(inputLines);

        return lightContraption.CalculateMaxEnergizedPositions();
    }
}
