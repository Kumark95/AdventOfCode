using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day08.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day08;

[PuzzleName("Haunted Wasteland")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 8;

    [PuzzleInput(filename: "example.txt", expectedResult: 2)]
    [PuzzleInput(filename: "input.txt", expectedResult: 19951)]
    public object SolvePartOne(string[] inputLines)
    {
        NavigationMap map = InputParser.ParseInput(inputLines);

        return map.CalculateStepsForHumans();
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 2)]
    [PuzzleInput(filename: "input.txt", expectedResult: 16342438708751)]
    public object SolvePartTwo(string[] inputLines)
    {
        NavigationMap map = InputParser.ParseInput(inputLines);

        return map.CalculateStepsForGhosts();
    }
}
