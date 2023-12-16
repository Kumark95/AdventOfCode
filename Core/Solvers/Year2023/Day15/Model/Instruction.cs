namespace AdventOfCode.Core.Solvers.Year2023.Day15.Model;

internal readonly record struct Instruction(string Label, char Operation, int? FocalLength)
{
    public override string ToString()
    {
        return Label + Operation + FocalLength.ToString();
    }
};
