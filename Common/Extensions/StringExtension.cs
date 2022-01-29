namespace AdventOfCode.Common.Extensions;

public static class StringExtension
{
    public static bool ContainsAllChars(this string first, string second)
    {
        return second.All(c => first.Contains(c));
    }

    public static bool ContainsNChars(this string first, string second, int n)
    {
        var counter = 0;
        foreach (var character in second)
        {
            if (first.Contains(character)) counter++;

            if (counter == n) return true;
        }

        return false;
    }
}
