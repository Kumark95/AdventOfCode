using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day15.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day15;

[PuzzleName("Lens Library")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 15;

    [PuzzleInput(filename: "example.txt", expectedResult: 1320)]
    [PuzzleInput(filename: "input.txt", expectedResult: 519041)]
    public long? SolvePartOne(string[] inputLines)
    {
        return inputLines[0]
            .Split(',')
            .Select(AsciiStringHelper.Hash)
            .Sum();
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 145)]
    [PuzzleInput(filename: "input.txt", expectedResult: 260530)]
    public long? SolvePartTwo(string[] inputLines)
    {
        var instructions = InputParser.ParseInstructions(inputLines[0]);

        var boxes = new BoxGroup();
        boxes.RunInstructions(instructions);

        return boxes.CalculateFocusingPower();
    }
}
