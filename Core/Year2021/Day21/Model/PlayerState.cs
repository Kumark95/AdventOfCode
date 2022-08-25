namespace AdventOfCode.Core.Year2021.Day21.Model;

internal record struct PlayerState
{
    public int Position { get; init; }
    public int Score { get; init; }

    public PlayerState(int position, int score)
    {
        Position = position;
        Score = score;
    }

    public PlayerState MovePosition(int increment)
    {
        if (increment < 0)
        {
            throw new Exception("Position increment cannot be negative");
        }

        // Subtract 1 before modulo. Add 1 after as the numbers start from 1 not 0
        var newPosition = (Position + increment - 1) % QuantumGame.BOARD_POSITIONS + 1;
        return new PlayerState
        {
            Position = newPosition,
            Score = Score + newPosition
        };
    }
}