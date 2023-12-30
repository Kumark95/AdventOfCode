using AdventOfCode.Common.Model;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2023.Day18.Model;

internal static partial class InputParser
{
    public static DigInstruction[] ParseInput(string[] inputLines, bool fixInstructions = false)
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

                Direction direction;
                long steps;
                if (fixInstructions)
                {
                    var color = matches.Groups["Color"].Value;

                    // First 5 characters represent the steps
                    steps = long.Parse(color[0..5], NumberStyles.HexNumber);

                    // Direction represented by the last character
                    direction = color[5] switch
                    {
                        '0' => Direction.Right,
                        '1' => Direction.Down,
                        '2' => Direction.Left,
                        '3' => Direction.Up,
                        _ => throw new UnreachableException()
                    };
                }
                else
                {
                    direction = MatchDirection(char.Parse(matches.Groups["Direction"].Value));
                    steps = long.Parse(matches.Groups["Steps"].Value);
                }

                return new DigInstruction(direction, steps);
            })
            .ToArray();
    }

    [GeneratedRegex(@"(?<Direction>U|D|L|R) (?<Steps>\d{1,2}) \(#(?<Color>[\w\d]+)\)")]
    private static partial Regex DigInstructionRegex();

    private static Direction MatchDirection(char dir)
    {
        return dir switch
        {
            'U' => Direction.Up,
            'D' => Direction.Down,
            'L' => Direction.Left,
            'R' => Direction.Right,
            _ => throw new UnreachableException()
        };
    }
}
