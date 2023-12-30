namespace AdventOfCode.Common.Model;

public readonly record struct LongPosition(long Row, long Col)
{
    public static LongPosition operator +(LongPosition a, LongPosition b) => new(a.Row + b.Row, a.Col + b.Col);
    public static LongPosition operator -(LongPosition a, LongPosition b) => new(a.Row - b.Row, a.Col - b.Col);
    public static LongPosition operator *(LongPosition a, long b) => new(a.Row * b, a.Col * b);

    public long ManhattanDistance(LongPosition target)
    {
        return Math.Abs(Row - target.Row)
            + Math.Abs(Col - target.Col);
    }

    public override string ToString()
    {
        return $"[{Row}, {Col}]";
    }

    //public static implicit operator LongPosition(Position position) => new LongPosition(position.Row, position.Col);
    public static explicit operator LongPosition(Position position) => new LongPosition(position.Row, position.Col);
}
