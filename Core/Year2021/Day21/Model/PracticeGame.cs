using System.Numerics;

namespace AdventOfCode.Core.Year2021.Day21.Model;

internal class PracticeGame
{
    private DeterministicDie Die { get; init; }
    private int MaxScore { get; init; }

    public PracticeGame(int maxScore)
    {
        MaxScore = maxScore;
        Die = new DeterministicDie(sides: 100);
    }

    public PracticeGameResult Play(GameState state)
    {
        var workingState = state;
        while (true)
        {
            // Roll the dice 3 times
            var positionIncrease = Enumerable.Range(0, 3)
                .Select(i => Die.Roll())
                .Sum();

            var newActiveState = workingState.ActivePlayerState.MovePosition(positionIncrease);
            if (newActiveState.Score >= MaxScore)
            {
                // The result contains the losing player score
                return new PracticeGameResult(workingState.WaitingPlayerState.Score, Die.RollCounter);
            }
            else
            {
                // The roles are switched for the next turn
                workingState = new GameState
                {
                    ActivePlayerState = workingState.WaitingPlayerState,
                    WaitingPlayerState = newActiveState
                };
            }
        }
    }
}

internal record struct PracticeGameResult(int LosingPlayerScore, int DieRollCount);
