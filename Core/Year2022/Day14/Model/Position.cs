namespace AdventOfCode.Core.Year2022.Day14.Model;

internal record struct Position(int X, int Y)
{
    public Position MoveSouth() => new(X, Y + 1);
    public Position MoveSouthWest() => new(X - 1, Y + 1);
    public Position MoveSouthEast() => new(X + 1, Y + 1);

    public IEnumerable<Position> PossibleMoves()
    {
        yield return MoveSouth();
        yield return MoveSouthWest();
        yield return MoveSouthEast();
    }
}
