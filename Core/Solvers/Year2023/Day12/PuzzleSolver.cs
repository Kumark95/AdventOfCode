using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day12.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day12;

[PuzzleName("Hot Springs")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 12;

    [PuzzleInput(filename: "example.txt", expectedResult: 21)]
    [PuzzleInput(filename: "input.txt", expectedResult: 6935)]
    public long? SolvePartOne(string[] inputLines)
    {
        return InputParser.ParseInput(inputLines)
            .Select(springs => springs.CalculatePossibleArrangements())
            .Sum();
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 525152)]
    [PuzzleInput(filename: "input.txt", expectedResult: 3920437278260)]
    public long? SolvePartTwo(string[] inputLines)
    {
        return InputParser.ParseInputAndExpand(inputLines)
            .Select(springs => springs.CalculatePossibleArrangements())
            .Sum();
    }
}
