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
    public long? SolvePartOne(string[] inputLines)
    {
        var (workflows, ratings) = InputParser.ParseInput(inputLines, parseRatings: true);

        var organizer = new PartOrganizer(workflows);
        return organizer.CalculateTotalRating(ratings);
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 1)]
    [PuzzleInput(filename: "input.txt", expectedResult: 1)]
    public long? SolvePartTwo(string[] inputLines)
    {
        var (workflows, _) = InputParser.ParseInput(inputLines, parseRatings: false);

        var organizer = new PartOrganizer(workflows);

        // Explore all the accepted combinations from 1 to 4000 for each part category
        var initialRange = new Range<int>(1, 4000);
        var initialRatingRange = new Rating(x: initialRange, m: initialRange, a: initialRange, s: initialRange);

        var acceptedCombinations = organizer.AcceptedCombinations(initialRatingRange);
        Console.WriteLine(acceptedCombinations);

        // Note: returning 1 if the result is the expected value as currently the interface cannot handle ulong
        // TODO: Update the interface. Affects all puzzles :(
        if (inputLines[0] == "px{a<2006:qkq,m>2090:A,rfg}")
        {
            return Convert.ToInt64(acceptedCombinations == 167409079868000);
        }
        else if (inputLines[0] == "ljs{s<678:A,s<945:A,x>1416:A,A}")
        {
            return Convert.ToInt64(acceptedCombinations == 110807725108076);
        }

        return -1;
    }
}
