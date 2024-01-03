using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day04.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day04;

[PuzzleName("Scratchcards")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 4;

    [PuzzleInput(filename: "example.txt", expectedResult: 13)]
    [PuzzleInput(filename: "input.txt", expectedResult: 24848)]
    public object SolvePartOne(string[] inputLines)
    {
        var result = 0;

        foreach (var line in inputLines)
        {
            var card = Card.Parse(line);
            result += (int)Math.Pow(2, card.MatchingNumbers - 1);
        }

        return result;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 30)]
    [PuzzleInput(filename: "input.txt", expectedResult: 7258152)]
    public object SolvePartTwo(string[] inputLines)
    {
        // Initialize the dictionary 
        var cardsProcessedByNumber = Enumerable.Range(1, inputLines.Length)
            .ToDictionary(key => key, value => 1);

        foreach (var line in inputLines)
        {
            var card = Card.Parse(line);

            for (int i = 1; i <= card.MatchingNumbers; i++)
            {
                var copyCardId = card.Id + i;
                cardsProcessedByNumber[copyCardId] = cardsProcessedByNumber[copyCardId] + cardsProcessedByNumber[card.Id];
            }
        }

        return cardsProcessedByNumber
            .Values
            .Sum();
    }
}
