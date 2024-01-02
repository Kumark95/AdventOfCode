using AdventOfCode.Common.Model;
using System.Numerics;

namespace AdventOfCode.Core.Solvers.Year2023.Day19.Model;

internal class PartOrganizer
{
    private readonly Dictionary<string, ConditionExpression[]> _workflows;

    public PartOrganizer(Dictionary<string, ConditionExpression[]> workflows)
    {
        _workflows = workflows;
    }

    public long CalculateTotalRating(List<RatingRange> ratings)
    {
        var total = 0;
        foreach (var rating in ratings)
        {
            var acceptedCombinations = AcceptedCombinations(rating);
            if (acceptedCombinations == 1)
            {
                // Since the ranges consist of a single value we can access the value directly
                total += rating.GetRange('x').Start
                    + rating.GetRange('m').Start
                    + rating.GetRange('a').Start
                    + rating.GetRange('s').Start;
            }
        }

        return total;
    }

    public ulong AcceptedCombinations(RatingRange initialRating)
    {
        // Always start with the "in" workflow
        var stack = new Stack<(RatingRange, string, int)>();
        stack.Push((initialRating, "in", 0));

        ulong total = 0;

        while (stack.Count > 0)
        {
            var (rating, workflowName, expressionIdx) = stack.Pop();
            var expressions = _workflows[workflowName];
            if (expressionIdx >= expressions.Length)
            {
                continue;
            }

            var expression = expressions[expressionIdx];

            // Evaluate the conditions
            if (expression.SendDirectly)
            {
                if (expression.Destination == "A")
                {
                    total += rating.Combinations();
                }
                else if (expression.Destination == "R")
                {
                    continue;
                }
                else
                {
                    stack.Push((rating, expression.Destination, 0));
                }
            }
            else
            {
                var partCategory = (char)expression.PartCategory!;
                var partValueRange = rating.GetRange(partCategory);

                int expressionValue = (int)expression.Value!;

                // Split range
                foreach (var (splitRange, conditionMatched) in partValueRange.Split((Condition)expression.Condition!, expressionValue))
                {
                    var newRating = rating.With(partCategory, splitRange);

                    if (conditionMatched)
                    {
                        if (expression.Destination == "A")
                        {
                            total += newRating.Combinations();
                        }
                        else if (expression.Destination == "R")
                        {
                            continue;
                        }
                        else
                        {
                            stack.Push((newRating, expression.Destination, 0));
                        }
                    }
                    else
                    {
                        // Send to the same workflow but to evaluate the next expression
                        stack.Push((newRating, workflowName, expressionIdx + 1));
                    }
                }
            }
        }

        return total;
    }
}

internal static class RangeExtensions
{
    public static List<(Range<T>, bool)> Split<T>(this Range<T> range, Condition condition, T conditionValue) where T : INumber<T>
    {
        if (!range.Contains(conditionValue))
        {
            return [(range, false)];
        }

        if (condition == Condition.LessThan)
        {
            return [
                (new(range.Start, conditionValue - T.One), true),
                (new(conditionValue, range.End), false)
            ];
        }
        else if (condition == Condition.GreaterThan)
        {
            return [
                (new(range.Start, conditionValue), false),
                (new(conditionValue + T.One, range.End), true)
            ];
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
