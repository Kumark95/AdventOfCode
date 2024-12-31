using AdventOfCode.Common.Model;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2024.Day13.Model;

internal static partial class InputParser
{
    public static List<(Position APosInc, Position BPosInc, Position PrizePos)> ParseInput(string[] inputLines)
    {
        var l = new List<(Position, Position, Position)>();

        Position? aPosInc = null;
        Position? bPosInc = null;

        foreach (var line in inputLines)
        {
            if (line.StartsWith("Button A:"))
            {
                aPosInc = ExtractPosition(line);
            }
            else if (line.StartsWith("Button B:"))
            {
                bPosInc = ExtractPosition(line);
            }
            else if (line.StartsWith("Prize"))
            {
                var prizePos = ExtractPosition(line);

                if (aPosInc is null || bPosInc is null)
                {
                    throw new InvalidOperationException("Error during parsing");
                }

                l.Add((aPosInc.Value, bPosInc.Value, prizePos));

                aPosInc = null;
                bPosInc = null;
            }
        }

        return l;
    }

    private static Position ExtractPosition(string line)
    {
        var match = PositionRegex().Match(line);
        if (!match.Success)
        {
            throw new InvalidOperationException("Could not extract button data");
        }

        var x = int.Parse(match.Groups["X"].Value);
        var y = int.Parse(match.Groups["Y"].Value);

        return new Position(x, y);
    }

    [GeneratedRegex("X[+|=](?<X>\\d+), Y[+|=](?<Y>\\d+)")]
    private static partial Regex PositionRegex();
}
