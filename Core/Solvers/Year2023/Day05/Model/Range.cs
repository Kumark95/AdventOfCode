namespace AdventOfCode.Core.Solvers.Year2023.Day05.Model;

internal record Range(long Start, long End)
{
    public long Length => End - Start + 1;

    public bool Contains(long value)
    {
        return value >= Start && value <= End;
    }

    public bool Contains(Range target)
    {
        return target.Start >= Start && target.End <= End;
    }

    public bool Intersects(Range target)
    {
        return target.Start <= End && target.End >= Start;
    }

    public Range Intersection(Range target)
    {
        return new Range(Math.Max(target.Start, Start), Math.Min(target.End, End));
    }

    public Range[] Difference(Range target)
    {
        if (!Intersects(target))
        {
            return [this];
        }

        if (Start < target.Start && End > target.End)
        {
            return [new Range(Start, target.Start - 1), new Range(target.End + 1, End)];
        }

        if (Start < target.Start)
        {
            return [new Range(Start, target.Start - 1)];
        }

        if (End > target.End)
        {
            return [new Range(target.End + 1, End)];
        }

        return Array.Empty<Range>();
    }
}
