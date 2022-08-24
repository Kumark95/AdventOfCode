namespace AdventOfCode.Common.Model;

public record Point3d
{
    public int X { get; init; }
    public int Y { get; init; }
    public int Z { get; init; }

    public Point3d(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString()
    {
        return $"[{X}, {Y}, {Z}]";
    }
}
