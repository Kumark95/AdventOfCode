using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Year2022.Day09.Model;

namespace AdventOfCode.Core.Year2022.Day09;

[PuzzleName("Rope Bridge")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 9;

    public long SolvePartOne(string[] inputLines)
    {
        var instructions = inputLines
            .Select(MovementInstruction.Parse)
            .ToList();

        var rope = new Rope(knots: 2);
        foreach (var instruction in instructions)
        {
            rope.Move(instruction);
        }

        return rope.UniqueTailPositionCount();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var instructions = inputLines
            .Select(MovementInstruction.Parse)
            .ToList();

        var rope = new Rope(knots: 10);
        foreach (var instruction in instructions)
        {
            rope.Move(instruction);
        }

        return rope.UniqueTailPositionCount();
    }
}
