namespace AdventOfCode.Core.Solvers.Year2023.Day15.Model;

internal static class AsciiStringHelper
{
    public static int Hash(string input)
    {
        return input
            .ToCharArray()
            .Aggregate(seed: 0, (result, character) =>
            {
                var asciiValue = (int)character;
                return (result + asciiValue) * 17 % 256;
            });
    }
}
