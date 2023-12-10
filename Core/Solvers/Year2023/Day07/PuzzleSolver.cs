using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day07.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day07;

[PuzzleName("Camel Cards")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 7;

    public long? SolvePartOne(string[] inputLines)
    {
        var cardHands = InputParser.ParseHands(inputLines, jAsJokers: false);

        Array.Sort(cardHands, new CardHandComparer(jAsJokers: false));

        return cardHands
            .Select((hand, index) => hand.BidAmount * (index + 1))
            .Sum();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var cardHands = InputParser.ParseHands(inputLines, jAsJokers: true);

        Array.Sort(cardHands, new CardHandComparer(jAsJokers: true));

        return cardHands
            .Select((hand, index) => hand.BidAmount * (index + 1))
            .Sum();
    }
}
