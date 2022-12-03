using System.Diagnostics;

namespace AdventOfCode.Core.Year2022.Day02.Model;

internal class HandGameRound
{
    private HandShape UserShape { get; init; }
    private HandShape OponentShape { get; init; }

    internal HandGameRound(HandShape userShape, HandShape oponentShape)
    {
        UserShape = userShape;
        OponentShape = oponentShape;
    }

    internal int CalculateScore()
    {
        return SelectionScore() + WinScore();
    }

    private int SelectionScore()
    {
        return UserShape switch
        {
            HandShape.Rock => 1,
            HandShape.Paper => 2,
            HandShape.Scissors => 3,
            _ => throw new UnreachableException("Invalid option")
        };
    }

    private int WinScore()
    {
        // Draw
        if (UserShape == OponentShape)
        {
            return 3;
        }

        // Win
        if (UserShape == HandShape.Rock && OponentShape == HandShape.Scissors
            || UserShape == HandShape.Paper && OponentShape == HandShape.Rock
            || UserShape == HandShape.Scissors && OponentShape == HandShape.Paper)
        {
            return 6;
        }

        // Lose
        return 0;
    }
}
