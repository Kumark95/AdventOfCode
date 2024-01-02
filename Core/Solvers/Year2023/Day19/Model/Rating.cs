using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day19.Model;

internal class Rating
{
    private readonly Dictionary<char, Range<int>> _values = new();

    public Rating(Range<int> x, Range<int> m, Range<int> a, Range<int> s)
    {
        _values['x'] = x;
        _values['m'] = m;
        _values['a'] = a;
        _values['s'] = s;
    }

    public Rating With(char part, Range<int> range)
    {
        return new Rating(
            part == 'x' ? range : _values['x'],
            part == 'm' ? range : _values['m'],
            part == 'a' ? range : _values['a'],
            part == 's' ? range : _values['s']
        );
    }

    public Range<int> GetRange(char part)
    {
        return _values[part];
    }

    public ulong Combinations()
    {
        return (ulong)_values['x'].Length
            * (ulong)_values['m'].Length
            * (ulong)_values['a'].Length
            * (ulong)_values['s'].Length;
    }


    public override string ToString()
    {
        return $$"""{ x: {{_values['x']}}, m: {{_values['m']}}, a: {{_values['a']}}, s: {{_values['s']}} }""";
    }
}
