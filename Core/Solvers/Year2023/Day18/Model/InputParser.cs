using AdventOfCode.Common.Model;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2023.Day18.Model;

internal static partial class InputParser
{
    public static DigInstruction[] ParseInput(string[] inputLines)
    {
        var regex = DigInstructionRegex();

        return Enumerable.Range(0, inputLines.Length)
            .Select(i =>
            {
                var line = inputLines[i];
                var matches = regex.Match(line);
                if (!matches.Success)
                {
                    throw new InvalidOperationException("Could not extract DigInstruction data");
                }

                var direction = MatchDirection(matches.Groups["Direction"].Value);
                var steps = int.Parse(matches.Groups["Steps"].Value);
                var color = matches.Groups["Color"].Value;

                return new DigInstruction(direction, steps, color);
            })
            .ToArray();
    }

    [GeneratedRegex(@"(?<Direction>U|D|L|R) (?<Steps>\d{1,2}) \((?<Color>#[\w\d]+)\)")]
    private static partial Regex DigInstructionRegex();

    private static Direction MatchDirection(string dir)
    {
        return dir switch
        {
            "U" => Direction.Up,
            "D" => Direction.Down,
            "L" => Direction.Left,
            "R" => Direction.Right,
            _ => throw new UnreachableException()
        };
    }
}
