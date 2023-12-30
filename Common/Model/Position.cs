namespace AdventOfCode.Common.Model;

public readonly record struct Position(int Row, int Col)
{
    public static Position operator +(Position a, Position b) => new(a.Row + b.Row, a.Col + b.Col);
    public static Position operator -(Position a, Position b) => new(a.Row - b.Row, a.Col - b.Col);

    public int ManhattanDistance(Position target)
    {
        return Math.Abs(Row - target.Row)
            + Math.Abs(Col - target.Col);
    }

    public override string ToString()
    {
        return $"[{Row}, {Col}]";
    }
}
