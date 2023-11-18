namespace AdventOfCode.Core.Solvers.Year2022.Day09.Model;

public readonly record struct Position
{
    public int X { get; init; }
    public int Y { get; init; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public double EuclideanDistance(Position target)
    {
        return Math.Sqrt(Math.Pow(X - target.X, 2) + Math.Pow(Y - target.Y, 2));
    }

    public static Position operator +(Position left, Position right)
    {
        return new Position(left.X + right.X, left.Y + right.Y);
    }

    public static Position operator -(Position left, Position right)
    {
        return new Position(left.X - right.X, left.Y - right.Y);
    }
}
