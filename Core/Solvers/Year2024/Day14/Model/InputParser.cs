using AdventOfCode.Common.Model;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2024.Day14.Model;

internal static partial class InputParser
{
    public static List<(Position, Position)> ParseInput(string[] inputLines)
    {
        var l = new List<(Position, Position)>();

        foreach (var line in inputLines)
        {
            var match = InputRegex().Match(line);
            if (!match.Success)
            {
                throw new InvalidOperationException("Error parsing input data");
            }

            var px = int.Parse(match.Groups["PX"].Value);
            var py = int.Parse(match.Groups["PY"].Value);

            var vx = int.Parse(match.Groups["VX"].Value);
            var vy = int.Parse(match.Groups["VY"].Value);

            // Inverting the positions to get the row/col
            l.Add((new Position(py, px), new Position(vy, vx)));
        }

        return l;
    }

    [GeneratedRegex("p=(?<PX>-?\\d+),(?<PY>-?\\d+) v=(?<VX>-?\\d+),(?<VY>-?\\d+)")]
    private static partial Regex InputRegex();
}
