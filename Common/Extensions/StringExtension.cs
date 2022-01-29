namespace AdventOfCode.Common.Extensions;

public static class StringExtension
{
    public static bool ContainsAllChars(this string first, string second)
    {
        return second.All(c => first.Contains(c));
    }

    public static bool IntersectsWithNChars(this string first, string second, int n)
    {
        return first.Intersect(second).Count() >= n;
    }
}
