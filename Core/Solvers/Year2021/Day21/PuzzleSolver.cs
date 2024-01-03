using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2021.Day21.Model;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2021.Day21;

[PuzzleName("Dirac Dice")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 21;

    private static List<int> Input(string[] inputLines)
    {
        var playerPositions = new List<int>();
        foreach (var line in inputLines)
        {
            var matches = Regex.Match(line, @"Player (?<PlayerId>\d+) starting position: (?<InitialPosition>\d+)");
            var initialPosition = int.Parse(matches.Groups["InitialPosition"].Value);

            playerPositions.Add(initialPosition);
        }

        return playerPositions;
    }

    public object SolvePartOne(string[] inputLines)
    {
        var playerPositions = Input(inputLines);

        var game = new PracticeGame(maxScore: 1000);
        var result = game.Play(new GameState
        {
            ActivePlayerState = new PlayerState(playerPositions[0], 0),
            WaitingPlayerState = new PlayerState(playerPositions[1], 0),
        });

        return result.LosingPlayerScore * result.DieRollCount;
    }

    public object SolvePartTwo(string[] inputLines)
    {
        var playerPositions = Input(inputLines);

        var quantumGame = new QuantumGame(maxScore: 21);
        var result = quantumGame.Play(new GameState
        {
            ActivePlayerState = new PlayerState(playerPositions[0], 0),
            WaitingPlayerState = new PlayerState(playerPositions[1], 0),
        });

        return result.ActivePlayerVictories > result.WaitingPlayerVictories ? result.ActivePlayerVictories : result.WaitingPlayerVictories;
    }
}
