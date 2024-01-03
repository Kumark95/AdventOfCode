using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2022.Day03.Model;

namespace AdventOfCode.Core.Solvers.Year2022.Day03;

[PuzzleName("Rucksack Reorganization")]
public partial class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 3;

    public object SolvePartOne(string[] inputLines)
    {
        var result = 0;
        foreach (var line in inputLines)
        {
            var rucksack = new Rucksack(items: line.ToCharArray());
            var compartmentCommonItem = rucksack.CompartmentCommonItem();

            result += compartmentCommonItem.Priority();
        }

        return result;
    }

    public object SolvePartTwo(string[] inputLines)
    {
        var result = 0;
        for (var i = 0; i < inputLines.Length; i += 3)
        {
            var firstElfRucksack = new Rucksack(inputLines[i].ToCharArray());
            var secondElfRucksack = new Rucksack(inputLines[i + 1].ToCharArray());
            var thirdElfRucksack = new Rucksack(inputLines[i + 2].ToCharArray());

            var badgeItem = firstElfRucksack.Items
                .Intersect(secondElfRucksack.Items)
                .Intersect(thirdElfRucksack.Items)
                .First();

            result += badgeItem.Priority();
        }

        return result;
    }
}
