namespace AdventOfCode.Core.Year2021.Day12;

public class Path
{
    public Cave Start { get; }
    public Cave End { get; }

    public Path(Cave start, Cave end)
    {
        Start = start;
        End = end;
    }
}
