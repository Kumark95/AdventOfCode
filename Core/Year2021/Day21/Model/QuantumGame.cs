namespace AdventOfCode.Core.Year2021.Day21.Model;

internal class QuantumGame
{
    public const int BOARD_POSITIONS = 10;
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

    public QuantumGameResult Play(GameState state)
    {
        if (state.WaitingPlayerState.Score >= MaxScore)
        {
            return new QuantumGameResult(0, 1);
        }

        if (ResultCache.ContainsKey(state))
        {
            return ResultCache[state];
        }

        long activePLayerVictories = 0;
        long waitingPlayerVictories = 0;

        foreach (var (positionIncrease, resultFrequency) in DiracDieResultFrequency)
        {
            // The roles are switched for the next turn
            var newResult = Play(new GameState
            {
                ActivePlayerState = state.WaitingPlayerState,
                WaitingPlayerState = state.ActivePlayerState.MovePosition(positionIncrease),
            });

            // The returning victories also need to be swapped
            activePLayerVictories += (newResult.WaitingPlayerVictories * resultFrequency);
            waitingPlayerVictories += (newResult.ActivePlayerVictories * resultFrequency);
        }

        var result = new QuantumGameResult
        {
            ActivePlayerVictories = activePLayerVictories,
            WaitingPlayerVictories = waitingPlayerVictories
        };

        ResultCache.Add(state, result);
        return result;
    }
}

internal record struct QuantumGameResult(long ActivePlayerVictories, long WaitingPlayerVictories);

