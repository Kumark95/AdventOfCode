using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Year2021.Day21.Model;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Year2021.Day21;

[PuzzleName("Dirac Dice")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 21;

    private static List<Player> Input(string[] inputLines)
    {
        var playerPositions = new List<Player>();
        foreach (var line in inputLines)
        {
            var matches = Regex.Match(line, @"Player (?<PlayerId>\d+) starting position: (?<InitialPosition>\d+)");
            var playerId = int.Parse(matches.Groups["PlayerId"].Value);
            var initialPosition = int.Parse(matches.Groups["InitialPosition"].Value);

            playerPositions.Add(new Player(playerId, initialPosition));
        }

        return playerPositions;
    }

    public long SolvePartOne(string[] inputLines)
    {
        var players = Input(inputLines);
        var game = new PracticeGame(maxScore: 1000);

        var result = game.Play(players);
        return result.LosingPlayerScore * result.DieRollCount;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var players = Input(inputLines);

        var quantumGame = new QuantumGame(maxScore: 21);

        var initialGameState = new GameState
        {
            PlayerAPosition = players[0].Position,
            PlayerAScore = 0,
            PlayerBPosition = players[1].Position,
            PlayerBScore = 0,
            IsPlayer1Turn = true
        };
        var result = quantumGame.Play(initialGameState);
        return result.PlayerAVictories > result.PlayerBVictories ? result.PlayerAVictories : result.PlayerBVictories;
    }
}
