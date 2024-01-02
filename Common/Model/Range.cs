using System.Numerics;

namespace AdventOfCode.Common.Model;

public readonly record struct Range<T>(T Start, T End) where T : INumber<T>
{
    public T Length => End - Start + T.One;

    public bool Contains(T value)
    {
        return value >= Start && value <= End;
    }

    public bool Contains(Range<T> target)
    {
        return target.Start >= Start && target.End <= End;
    }

    public bool Intersects(Range<T> target)
    {
        return target.Start <= End && target.End >= Start;
    }

    public Range<T> Intersection(Range<T> target)
    {
        return new Range<T>(T.Max(target.Start, Start), T.Min(target.End, End));
    }

    public Range<T>[] Difference(Range<T> target)
    {
        if (!Intersects(target))
        {
            return [this];
        }

        if (Start < target.Start && End > target.End)
        {
            return [new Range<T>(Start, target.Start - T.One), new Range<T>(target.End + T.One, End)];
        }

        if (Start < target.Start)
        {
            return [new Range<T>(Start, target.Start - T.One)];
        }

        if (End > target.End)
        {
            return [new Range<T>(target.End + T.One, End)];
        }

        return Array.Empty<Range<T>>();
    }

    public override string ToString()
    {
        return $"[{Start}, {End}]";
    }
}
