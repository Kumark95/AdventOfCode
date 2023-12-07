namespace AdventOfCode.Core.Solvers.Year2023.Day07.Model;

internal static class InputParser
{
    public static CardHand[] ParseHands(string[] inputLines, bool jAsJokers)
    {
        return inputLines
            .Select(l =>
            {
                var parts = l.Split(' ');
                return new CardHand(parts[0], int.Parse(parts[1]), jAsJokers);
            })
            .ToArray();
    }
}
