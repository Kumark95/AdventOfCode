using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Year2021.Day17.Model;
public class TargetArea
{
    public Point Min { get; }
    public Point Max { get; }

    public TargetArea(Point min, Point max)
    {
        Min = min;
        Max = max;
    }

    public bool IsInArea(Point location)
    {
        return location.X >= Min.X
            && location.X <= Max.X
            && location.Y >= Min.Y
            && location.Y <= Max.Y;
    }
}