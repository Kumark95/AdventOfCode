using System.Diagnostics;
using System.Text.RegularExpressions;
using Rating = System.Collections.Generic.Dictionary<char, int>;

namespace AdventOfCode.Core.Solvers.Year2023.Day19.Model;

internal static partial class InputParser
{
    public static (Dictionary<string, ConditionExpression[]> Workflows, List<Rating> Ratings) ParseInput(string[] inputLines)
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

            var rating = new Rating
            {
                { 'x', int.Parse(match.Groups["X"].Value) },
                { 'm', int.Parse(match.Groups["M"].Value) },
                { 'a', int.Parse(match.Groups["A"].Value) },
                { 's', int.Parse(match.Groups["S"].Value) }
            };

            ratings.Add(rating);
        }

        return (workflows, ratings);
    }

    [GeneratedRegex(@"{x=(?<X>\d+),m=(?<M>\d+),a=(?<A>\d+),s=(?<S>\d+)}")]
    private static partial Regex RatingRegex();

    [GeneratedRegex(@"(?<PartCategory>x|m|a|s)?(?<Condition>>|<)?(?<Value>\d+)?:?(?<Destination>\w+)")]
    private static partial Regex WorkflowRegex();
}
