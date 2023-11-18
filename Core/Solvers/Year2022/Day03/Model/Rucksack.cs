namespace AdventOfCode.Core.Solvers.Year2022.Day03.Model;

internal class Rucksack
{
    internal RucksackItem[] Items { get; init; }
    internal RucksackItem[] LeftCompartmentItems { get; init; }
    internal RucksackItem[] RightCompartmentItems { get; init; }

    internal Rucksack(char[] items)
    {
        Items = items
            .Select(i => new RucksackItem(i))
            .ToArray();

        var compartmentSize = Items.Length / 2;
        LeftCompartmentItems = Items.Take(compartmentSize).ToArray();
        RightCompartmentItems = Items.Skip(compartmentSize).ToArray();
    }

    internal RucksackItem CompartmentCommonItem()
    {
        return LeftCompartmentItems
            .Intersect(RightCompartmentItems)
            .First();
    }
}
