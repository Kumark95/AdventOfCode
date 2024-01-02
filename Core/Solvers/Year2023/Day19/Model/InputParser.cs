using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2023.Day19.Model;

internal static partial class InputParser
{
    public static (Dictionary<string, ConditionExpression[]> Workflows, List<Rating> Ratings) ParseInput(string[] inputLines, bool parseRatings)
    {
        var dividerIdx = Array.IndexOf(inputLines, "");

        var workflowRegex = WorkflowRegex();
        var workflows = new Dictionary<string, ConditionExpression[]>();
        foreach (var line in inputLines[0..dividerIdx])
        {
            int bracketIdx = line.IndexOf('{');
            var workflowName = line[..bracketIdx];
            var expressions = new List<ConditionExpression>();

            // The last expression only contains the destination
            var conditionTexts = line[(bracketIdx + 1)..(line.Length - 1)].Split(',');
            foreach (var condText in conditionTexts.SkipLast(1))
            {
                var match = workflowRegex.Match(condText);
                if (!match.Success)
                {
                    throw new InvalidOperationException("Could not extract rating data");
                }

                var partCategory = char.Parse(match.Groups["PartCategory"].Value);
                var condition = match.Groups["Condition"].Value switch
                {
                    ">" => Condition.GreaterThan,
                    "<" => Condition.LessThan,
                    _ => throw new UnreachableException($"MATCHED {line} | G: {match.Groups}")
                };
                var value = int.Parse(match.Groups["Value"].Value);
                var destination = match.Groups["Destination"].Value;
                expressions.Add(new ConditionExpression(partCategory, condition, value, destination));
            }

            // Add the last expression
            expressions.Add(new ConditionExpression(null, null, null, conditionTexts.Last()));

            workflows.Add(workflowName, expressions.ToArray());
        }

        if (!parseRatings)
        {
            return (workflows, []);
        }

        // Parse the ratings now
        var ratingRegex = RatingRegex();
        var ratings = new List<Rating>();
        foreach (var line in inputLines[(dividerIdx + 1)..])
        {
            var match = ratingRegex.Match(line);
            if (!match.Success)
            {
                throw new InvalidOperationException("Could not extract rating data");
            }

            var x = int.Parse(match.Groups["X"].Value);
            var m = int.Parse(match.Groups["M"].Value);
            var a = int.Parse(match.Groups["A"].Value);
            var s = int.Parse(match.Groups["S"].Value);

            // Using a range of a single element
            var rating = new Rating(x: new(x, x), m: new(m, m), a: new(a, a), s: new(s, s));

            ratings.Add(rating);
        }

        return (workflows, ratings);
    }

    [GeneratedRegex(@"{x=(?<X>\d+),m=(?<M>\d+),a=(?<A>\d+),s=(?<S>\d+)}")]
    private static partial Regex RatingRegex();

    [GeneratedRegex(@"(?<PartCategory>x|m|a|s)?(?<Condition>>|<)?(?<Value>\d+)?:?(?<Destination>\w+)")]
    private static partial Regex WorkflowRegex();
}
