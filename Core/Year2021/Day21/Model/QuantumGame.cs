namespace AdventOfCode.Core.Year2021.Day21.Model;

internal class QuantumGame
{
    private int MaxScore { get; init; }
    private Dictionary<GameState, QuantumGameResult> ResultCache { get; set; }
    private readonly Dictionary<int, int> DiracDieResultFrequency = new()
    {
        { 3, 1 },
        { 4, 3 },
        { 5, 6 },
        { 6, 7 },
        { 7, 6 },
        { 8, 3 },
        { 9, 1 }
    };

    public QuantumGame(int maxScore)
    {
        MaxScore = maxScore;
        ResultCache = new Dictionary<GameState, QuantumGameResult>();
    }

    private static int ChangePlayerPosition(int initialPosition, int increment)
    {
        // Subtract 1 before modulo. Add 1 after as the numbers start from 1 not 0
        return (initialPosition + increment - 1) % 10 + 1;
    }

    /// <summary>
    /// Play the game with a given state
    /// </summary>
    /// <param name="state"></param>
    /// <returns>The number of times each player has won</returns>
    public QuantumGameResult Play(GameState state)
    {
        if (ResultCache.ContainsKey(state))
        {
            return ResultCache[state];
        }

        long player1Victories = 0;
        long player2Victories = 0;

        // TODO: Refactor
        if (state.IsPlayer1Turn)
        {
            foreach (var (positionIncrease, dieResultFrequency) in DiracDieResultFrequency)
            {
                var landingPosition = ChangePlayerPosition(state.PlayerAPosition, positionIncrease);
                var playerNewScore = state.PlayerAScore + landingPosition;

                var newGameState = state with
                {
                    PlayerAPosition = landingPosition,
                    PlayerAScore = playerNewScore,
                    IsPlayer1Turn = false,
                };

                QuantumGameResult newStateResults;
                if (ResultCache.ContainsKey(newGameState))
                {
                    var intermediateResults = ResultCache[newGameState];
                    newStateResults = intermediateResults with
                    {
                        PlayerAVictories = intermediateResults.PlayerAVictories * dieResultFrequency,
                        PlayerBVictories = intermediateResults.PlayerBVictories * dieResultFrequency
                    };
                }
                else if (playerNewScore >= MaxScore)
                {
                    newStateResults = new QuantumGameResult(PlayerAVictories: dieResultFrequency, PlayerBVictories: 0);
                }
                else
                {
                    var intermediateResults = Play(newGameState);
                    newStateResults = intermediateResults with
                    {
                        PlayerAVictories = intermediateResults.PlayerAVictories * dieResultFrequency,
                        PlayerBVictories = intermediateResults.PlayerBVictories * dieResultFrequency
                    };
                }

                player1Victories += newStateResults.PlayerAVictories;
                player2Victories += newStateResults.PlayerBVictories;
            }
        }
        else
        {
            foreach (var (positionIncrease, dieResultFrequency) in DiracDieResultFrequency)
            {
                var landingPosition = ChangePlayerPosition(state.PlayerBPosition, positionIncrease);
                var playerNewScore = state.PlayerBScore + landingPosition;

                var newGameState = state with
                {
                    PlayerBPosition = landingPosition,
                    PlayerBScore = playerNewScore,
                    IsPlayer1Turn = true,
                };

                QuantumGameResult newStateResults;
                if (ResultCache.ContainsKey(newGameState))
                {
                    var intermediateResults = ResultCache[newGameState];
                    newStateResults = intermediateResults with
                    {
                        PlayerAVictories = intermediateResults.PlayerAVictories * dieResultFrequency,
                        PlayerBVictories = intermediateResults.PlayerBVictories * dieResultFrequency
                    };
                }
                else if (playerNewScore >= MaxScore)
                {
                    newStateResults = new QuantumGameResult(PlayerAVictories: 0, PlayerBVictories: dieResultFrequency);
                }
                else
                {
                    var intermediateResults = Play(newGameState);
                    newStateResults = intermediateResults with
                    {
                        PlayerAVictories = intermediateResults.PlayerAVictories * dieResultFrequency,
                        PlayerBVictories = intermediateResults.PlayerBVictories * dieResultFrequency
                    };
                }

                player1Victories += newStateResults.PlayerAVictories;
                player2Victories += newStateResults.PlayerBVictories;
            }
        }

        var result = new QuantumGameResult
        {
            PlayerAVictories = player1Victories,
            PlayerBVictories = player2Victories
        };

        ResultCache.Add(state, result);
        return result;
    }
}


internal record struct GameState
{
    public int PlayerAPosition { get; init; }
    public int PlayerAScore { get; init; }
    public int PlayerBPosition { get; init; }
    public int PlayerBScore { get; init; }
    public bool IsPlayer1Turn { get; init; }
}

internal record struct QuantumGameResult(long PlayerAVictories, long PlayerBVictories);
