namespace AdventOfCode.Common.Model;

public readonly record struct Position(long Row, long Col)
{
    public static Position operator +(Position a, Position b) => new(a.Row + b.Row, a.Col + b.Col);
    public static Position operator +(Position a, long b) => new(a.Row + b, a.Col + b);
    public static Position operator -(Position a, Position b) => new(a.Row - b.Row, a.Col - b.Col);
    public static Position operator -(Position a, long b) => new(a.Row - b, a.Col - b);
    public static Position operator *(Position a, long b) => new(a.Row * b, a.Col * b);

    public long ManhattanDistance(Position target)
    {
        return Math.Abs(Row - target.Row)
            + Math.Abs(Col - target.Col);
    }

    public override string ToString()
    {
        return $"[{Row}, {Col}]";
    }
}
