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

    public PracticeGameResult Play(List<Player> players)
    {
        while (true)
        {
            foreach (var player in players)
            {
                // Roll the dice 3 times
                var positionIncrease = Enumerable.Range(0, 3)
                    .Select(i => Die.Roll())
                    .Sum();

                player.MovePosition(positionIncrease);

                if (player.Score >= MaxScore)
                {
                    var losingPlayer = players.Single(p => p.Id != player.Id);
                    return new PracticeGameResult(losingPlayer.Score, Die.RollCounter);
                }
            }
        }
    }
}

internal record struct PracticeGameResult(int LosingPlayerScore, int DieRollCount);

internal class Player
{
    public int Id { get; init; }
    public int Position { get; private set; }
    public int Score { get; private set; }

    public Player(int id, int initialPosition)
    {
        Id = id;
        Position = initialPosition;
        Score = 0;
    }

    public void MovePosition(int increment)
    {
        if (increment < 0)
        {
            throw new Exception("Position increment cannot be negative");
        }

        // Subtract 1 before modulo. Add 1 after as the numbers start from 1 not 0
        Position = (Position + increment - 1) % 10 + 1;

        // The score is updated based on the landing position
        Score += Position;
    }
}

internal class DeterministicDie
{
    public int RollCounter { get; set; }
    private int Sides { get; init; }

    public DeterministicDie(int sides)
    {
        RollCounter = 0;
        Sides = sides;
    }

    public int Roll()
    {
        RollCounter++;

        // Subtract 1 before modulo. Add 1 after as the numbers start from 1 not 0
        return (RollCounter - 1) % Sides + 1;
    }
}
