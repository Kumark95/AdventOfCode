using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Solvers.Year2023.Day18.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day18;

[PuzzleName("Lavaduct Lagoon")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 18;

    [PuzzleInput(filename: "example.txt", expectedResult: 62)]
    [PuzzleInput(filename: "input.txt", expectedResult: 34329)]
    public long? SolvePartOne(string[] inputLines)
    {
        var instructions = InputParser.ParseInput(inputLines);

        var prevPosition = new Position(0, 0);
        var positions = new List<Position> { prevPosition };
        var trenchLength = 0;
        foreach (var instruction in instructions)
        {
            var newPosition = prevPosition + instruction.Direction.DirectionIncrement() * instruction.Steps;
            positions.Add(newPosition);

            prevPosition = newPosition;
            trenchLength += instruction.Steps;
        }

        // We need to add the border of the trench
        var polygon = new Polygon(positions);
        return (int)polygon.CalculateArea() + trenchLength / 2 + 1;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: -1)]
    [PuzzleInput(filename: "input.txt", expectedResult: -1)]
    public long? SolvePartTwo(string[] inputLines)
    {
        return null;
    }
}
