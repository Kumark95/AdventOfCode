using AdventOfCode.Common.Model;

namespace AdventOfCode.Common.Extensions;

public static class DirectionExtensions
{
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
}
