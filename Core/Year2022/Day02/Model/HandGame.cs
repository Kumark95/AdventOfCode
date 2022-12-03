using System.Diagnostics;

namespace AdventOfCode.Core.Year2022.Day02.Model;

internal class HandGame
{
    private List<HandGameRound> Rounds { get; init; }

    internal HandGame(string[] strategyGuide, DecryptionStrategy decryptionStrategy)
    {
        Rounds = strategyGuide
            .Select(line => DecryptRound(line, decryptionStrategy))
            .ToList();
    }

    internal int CalculateTotalScore()
    {
        return Rounds
            .Select(r => r.CalculateScore())
            .Sum();
    }

    private HandGameRound DecryptRound(string line, DecryptionStrategy decryptionStrategy)
    {
        var selections = line.Split(" ");

        var oponentShape = (HandShape)char.Parse(selections[0]);
        var userShape = DecryptShape(oponentShape, message: char.Parse(selections[1]), decryptionStrategy);

        return new HandGameRound(userShape, oponentShape);
    }

    private HandShape DecryptShape(HandShape oponentShape, char message, DecryptionStrategy decryptionStrategy)
    {
        return decryptionStrategy switch
        {
            DecryptionStrategy.UserSelection => DecryptExpectedUserShape(oponentShape, message),
            DecryptionStrategy.RoundResult => DecryptExpectedRoundResult(oponentShape, message),
            _ => throw new UnreachableException("Invalid strategy")
        };
    }

    private static HandShape DecryptExpectedUserShape(HandShape oponentShape, char message)
    {
        return message switch
        {
            'X' => HandShape.Rock,
            'Y' => HandShape.Paper,
            'Z' => HandShape.Scissors,
            _ => throw new UnreachableException("Invalid option")
        };
    }

    private static HandShape DecryptExpectedRoundResult(HandShape oponentShape, char message)
    {
        return message switch
        {
            'X' => PickLosingShape(oponentShape),
            'Y' => PickDrawingShape(oponentShape),
            'Z' => PickWinningShape(oponentShape),
            _ => throw new UnreachableException("Invalid option")
        };
    }

    private static HandShape PickLosingShape(HandShape oponentShape)
    {
        return oponentShape switch
        {
            HandShape.Rock => HandShape.Scissors,
            HandShape.Paper => HandShape.Rock,
            HandShape.Scissors => HandShape.Paper,
            _ => throw new UnreachableException("Invalid option")
        };
    }

    private static HandShape PickDrawingShape(HandShape oponentShape)
    {
        return oponentShape;
    }

    private static HandShape PickWinningShape(HandShape oponentShape)
    {
        return oponentShape switch
        {
            HandShape.Rock => HandShape.Paper,
            HandShape.Paper => HandShape.Scissors,
            HandShape.Scissors => HandShape.Rock,
            _ => throw new UnreachableException("Invalid option")
        };
    }
}
