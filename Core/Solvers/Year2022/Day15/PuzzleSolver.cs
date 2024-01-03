using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2022.Day15.Model;

namespace AdventOfCode.Core.Solvers.Year2022.Day15;

[PuzzleName("Beacon Exclusion Zone")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 15;

    public object SolvePartOne(string[] inputLines)
    {
        var map = SubterraneanZoneParser.Parse(inputLines);
        return map.GetBeaconExclusionPositionCount(row: 2000000);
    }

    public object SolvePartTwo(string[] inputLines)
    {
        return null;
    }
}
