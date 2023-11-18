using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2022.Day04.Model;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2022.Day04;

[PuzzleName("Camp Cleanup")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 4;

    private static List<(CleaningArea first, CleaningArea second)> Input(string[] inputLines)
    {
        var groups = new List<(CleaningArea first, CleaningArea second)>();

        var regex = new Regex(@"(?<StartFirstArea>\d+)-(?<EndFirstArea>\d+),(?<StartSecondArea>\d+)-(?<EndSecondArea>\d+)|");
        foreach (var line in inputLines)
        {
            var matchGroups = regex.Match(line).Groups;

            var firstCleaningArea = new CleaningArea(
                    int.Parse(matchGroups["StartFirstArea"].Value),
                    int.Parse(matchGroups["EndFirstArea"].Value)
                );

            var secondCleaningArea = new CleaningArea(
                    int.Parse(matchGroups["StartSecondArea"].Value),
                    int.Parse(matchGroups["EndSecondArea"].Value)
                );

            groups.Add((firstCleaningArea, secondCleaningArea));
        }

        return groups;
    }

    public long SolvePartOne(string[] inputLines)
    {
        var result = 0;

        foreach (var (firstArea, secondArea) in Input(inputLines))
        {
            if (firstArea.FullyContains(secondArea)
                || secondArea.FullyContains(firstArea))
            {
                result++;
            }
        }

        return result;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var result = 0;

        foreach (var (firstArea, secondArea) in Input(inputLines))
        {
            if (firstArea.PartiallyContains(secondArea))
            {
                result++;
            }
        }

        return result;
    }
}
