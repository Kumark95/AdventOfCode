using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day19.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day19;

[PuzzleName("Aplenty")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 19;

    [PuzzleInput(filename: "example.txt", expectedResult: 19114)]
    [PuzzleInput(filename: "input.txt", expectedResult: 319295)]
    public long? SolvePartOne(string[] inputLines)
    {
        var (workflows, ratings) = InputParser.ParseInput(inputLines);

        var organizer = new PartOrganizer(workflows);

        return organizer.CalculateTotalRating(ratings);
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 167409079868000)]
    [PuzzleInput(filename: "input.txt", expectedResult: -1)]
    public long? SolvePartTwo(string[] inputLines)
    {
        return null;
    }
}
