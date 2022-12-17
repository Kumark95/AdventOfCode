using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Year2022.Day15.Model;

namespace AdventOfCode.Core.Year2022.Day15;

[PuzzleName("Beacon Exclusion Zone")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 15;

    public long SolvePartOne(string[] inputLines)
    {
        var map = SubterraneanZoneParser.Parse(inputLines);
        map.AnalyzeMeasurements();

        return map.GetBeaconExclusionPositionCount(row: 10);
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        return null;
    }
}
