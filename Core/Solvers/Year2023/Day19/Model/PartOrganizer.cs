using System.Diagnostics;
using Rating = System.Collections.Generic.Dictionary<char, int>;

namespace AdventOfCode.Core.Solvers.Year2023.Day19.Model;

internal class PartOrganizer
{
    private readonly Dictionary<string, ConditionExpression[]> _workflows;

    private enum Result
    {
        Accepted,
        Rejected
    }

    public PartOrganizer(Dictionary<string, ConditionExpression[]> workflows)
    {
        _workflows = workflows;
    }

    public long CalculateTotalRating(List<Rating> ratings)
    {
        var total = 0;
        foreach (var rating in ratings)
        {
            if (EvaluateRating(rating) == Result.Accepted)
            {
                total += rating.Values.Sum();
            }
        }

        return total;
    }

    private Result EvaluateRating(Rating rating)
    {
        // Always start with the "in" workflow
        var stack = new Stack<string>();
        stack.Push("in");

        while (stack.Count > 0)
        {
            var expressions = _workflows[stack.Pop()];

            // Evaluate the conditions
            foreach (var expression in expressions)
            {
                if (expression.SendDirectly)
                {
                    if (expression.Destination == "A")
                    {
                        return Result.Accepted;
                    }
                    else if (expression.Destination == "R")
                    {
                        return Result.Rejected;
                    }
                    else
                    {
                        stack.Push(expression.Destination);
                    }
                }
                else
                {
                    int partValue = rating[(char)expression.PartCategory!];
                    int expressionValue = (int)expression.Value!;

                    // Evaluate
                    var conditionMatched = expression.Condition switch
                    {
                        Condition.LessThan => partValue < expressionValue,
                        Condition.GreaterThan => partValue > expressionValue,
                        _ => throw new UnreachableException()
                    };

                    if (conditionMatched)
                    {
                        if (expression.Destination == "A")
                        {
                            return Result.Accepted;
                        }
                        else if (expression.Destination == "R")
                        {
                            return Result.Rejected;
                        }
                        else
                        {
                            // Need to stop the evaluation of the rest of expressions
                            stack.Push(expression.Destination);
                            break;
                        }
                    }
                }
            }
        }

        throw new UnreachableException();
    }
}
