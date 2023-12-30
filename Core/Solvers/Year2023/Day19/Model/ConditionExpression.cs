namespace AdventOfCode.Core.Solvers.Year2023.Day19.Model;

internal readonly record struct ConditionExpression(char? PartCategory, Condition? Condition, int? Value, string Destination)
{
    internal readonly bool SendDirectly => PartCategory is null;
}
