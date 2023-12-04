using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2023.Day04.Model;

internal partial record struct Card(int Id, int MatchingNumbers)
{
    [GeneratedRegex("Card +(?<CardNumber>\\d*): (?<OwnNumbers>.*) \\| (?<WinningNumbers>.*)")]
    private static partial Regex CardRegex();

    public static Card Parse(string line)
    {
        var matches = CardRegex().Match(line);
        if (!matches.Success)
        {
            throw new ArgumentException("Invalid card contents");
        }

        var cardId = int.Parse(matches.Groups["CardNumber"].Value);
        var ownNumbers = matches.Groups["OwnNumbers"].Value
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .ToHashSet();
        var winningNumbers = matches.Groups["WinningNumbers"].Value
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .ToHashSet();

        var matchingNumbers = ownNumbers
            .Intersect(winningNumbers).Count();

        return new Card(cardId, matchingNumbers);
    }
};
