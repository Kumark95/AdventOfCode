namespace AdventOfCode.Core.Solvers.Year2022.Day03.Model;

internal record RucksackItem
{
    private char Value { get; init; }

    public RucksackItem(char item)
    {
        Value = item;
    }

    internal int Priority()
    {
        var decimalValue = (int)Value;
        return decimalValue switch
        {
            >= 97 and <= 122 => decimalValue - 96, // Lowercase from 1 to 26
            >= 65 and <= 90 => decimalValue - 38, // Uppercase from 27 to 52
            _ => throw new ArgumentException("Only lowercase or uppercase letters are valid")
        };
    }
}
