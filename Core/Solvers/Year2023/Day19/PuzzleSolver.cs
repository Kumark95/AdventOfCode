using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Solvers.Year2023.Day19.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day19;

[PuzzleName("Aplenty")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 19;

    [PuzzleInput(filename: "example.txt", expectedResult: 19114)]
    [PuzzleInput(filename: "input.txt", expectedResult: 319295)]
    public object SolvePartOne(string[] inputLines)
    {
        var (workflows, ratings) = InputParser.ParseInput(inputLines, parseRatings: true);

        var organizer = new PartOrganizer(workflows);
        return organizer.CalculateTotalRating(ratings);
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 167409079868000)]
    [PuzzleInput(filename: "input.txt", expectedResult: 110807725108076)]
    public object SolvePartTwo(string[] inputLines)
    {
        var (workflows, _) = InputParser.ParseInput(inputLines, parseRatings: false);

        var organizer = new PartOrganizer(workflows);

        // Explore all the accepted combinations from 1 to 4000 for each part category
        var initialRange = new Range<int>(1, 4000);
        var initialRatingRange = new Rating(x: initialRange, m: initialRange, a: initialRange, s: initialRange);

        return organizer.AcceptedCombinations(initialRatingRange);
    }
}
