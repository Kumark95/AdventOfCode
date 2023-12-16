namespace AdventOfCode.Core.Solvers.Year2023.Day15.Model;

internal readonly record struct Lens(string Label, int FocalLength)
{
    public override string ToString()
    {
        return $"[{Label} {FocalLength}]";
    }
};
