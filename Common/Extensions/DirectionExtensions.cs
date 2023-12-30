using AdventOfCode.Common.Model;

namespace AdventOfCode.Common.Extensions;

public static class DirectionExtensions
{
    private static readonly Dictionary<Direction, Position> _dirIncrements = new()
    {
        { Direction.Up, new(-1, 0) },
        { Direction.Down, new(1, 0) },
        { Direction.Left, new(0, -1) },
        { Direction.Right, new(0, 1) },
    };

    public static bool IsOposite(this Direction baseDir, Direction targetDir)
    {
        return (baseDir, targetDir) switch
        {
            (Direction.Up, Direction.Down) => true,
            (Direction.Down, Direction.Up) => true,
            (Direction.Left, Direction.Right) => true,
            (Direction.Right, Direction.Left) => true,
            _ => false
        };
    }

    public static Position DirectionIncrement(this Direction direction)
    {
        return _dirIncrements[direction];
    }
}
