using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day08.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day08;

[PuzzleName("Haunted Wasteland")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 8;

    public long? SolvePartOne(string[] inputLines)
    {
        NavigationMap map = InputParser.ParseInput(inputLines);

        return map.CalculateStepsForHumans();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        NavigationMap map = InputParser.ParseInput(inputLines);

        return map.CalculateStepsForGhosts();
    }
}
