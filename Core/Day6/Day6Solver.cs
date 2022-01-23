using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Day6;

[PuzzleName("Lanternfish")]
public class Day6Solver : IPuzzleSolver
{
    public long SolvePartOne(string[] inputLines)
    {
        var fishSchool = new FishSchool(inputLines[0]);
        return fishSchool
            .Age(80)
            .FishCollection.Count;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        return null;
    }
}
