using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2023.Day08.Model;

internal static partial class InputParser
{
    [GeneratedRegex("(?<Label>\\w{3}) = \\((?<LeftNode>\\w{3}), (?<RightNode>\\w{3})\\)")]
    private static partial Regex InputRegex();

    public static NavigationMap ParseInput(string[] inputLines)
    {
        var instructionSet = inputLines[0].ToCharArray();

        Dictionary<string, Node> nodesDict = new();

        var regex = InputRegex();
        foreach (var line in inputLines[2..])
        {
            var matches = regex.Match(line);
            if (!matches.Success)
            {
                throw new ArgumentException("Could not match input node description");
            }

            var label = matches.Groups["Label"].Value;
            var leftNodeLabel = matches.Groups["LeftNode"].Value;
            var rightNodeLabel = matches.Groups["RightNode"].Value;

            nodesDict[label] = new Node(label, leftNodeLabel, rightNodeLabel);
        }

        return new NavigationMap(instructionSet, nodesDict);
    }
}
