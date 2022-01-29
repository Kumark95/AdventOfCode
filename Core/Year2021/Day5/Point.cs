namespace AdventOfCode.Core.Year2021.Day5;

public record Point
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}
