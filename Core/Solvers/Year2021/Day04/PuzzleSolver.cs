using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Solvers.Year2021.Day04;


[PuzzleName("Giant Squid")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 4;

    private (List<int> numbers, List<BingoCard> cards) Input(string[] inputLines)
    {
        var numbers = inputLines[0].Split(',').Select(i => int.Parse(i)).ToList();

        var cards = new List<BingoCard>();
        var boardLines = inputLines.Skip(1).Where(l => !string.IsNullOrEmpty(l)).ToList();

        var maxIters = boardLines.Count / 5;
        var iter = 0;
        while (iter < maxIters)
        {
            var cardNumbers = boardLines
                .Skip(iter * 5)
                .Take(5)
                .Select(
                    row => row
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(i => int.Parse(i))
                        .ToArray()
                )
                .ToArray();

            cards.Add(new BingoCard(cardNumbers));

            iter++;
        }

        return (numbers, cards);
    }

    public long? SolvePartOne(string[] inputLines)
    {
        var (numbers, cards) = Input(inputLines);

        foreach (var number in numbers)
        {
            foreach (var card in cards)
            {
                if (card.MarkNumber(number))
                {
                    return number * card.SumNotMarked();
                }
            }
        }

        throw new Exception("No winning card found");
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var (numbers, cards) = Input(inputLines);

        int? lastWinningNumber = null;
        var winningCards = new HashSet<BingoCard>();

        foreach (var number in numbers)
        {
            foreach (var card in cards)
            {
                if (card.MarkNumber(number))
                {
                    lastWinningNumber = number;
                    winningCards.Add(card);
                }

                if (winningCards.Count == cards.Count)
                {
                    goto End;
                }
            }
        }

    End:

        if (!lastWinningNumber.HasValue || winningCards.Count == 0)
        {
            throw new Exception("No winning card found");
        }

        return lastWinningNumber * winningCards.Last().SumNotMarked();
    }
}
