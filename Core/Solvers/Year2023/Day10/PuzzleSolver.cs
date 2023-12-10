using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day10.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day10;

[PuzzleName("Pipe Maze")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 10;

    public long? SolvePartOne(string[] inputLines)
    {
        var maze = InputParser.ParseInput(inputLines);

        return maze.CalculateMaxLoopDistance();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var maze = InputParser.ParseInput(inputLines);

        return maze.CalculateTotalEnclosedTiles();
    }
}
