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

        for (int row = -2; row < 22; row++)
        {
            var count = map.GetBeaconExclusionPositionCount(row);
            Console.WriteLine($"Row: {row} | Count: {count}");
        }

        return 1;

        return map.GetBeaconExclusionPositionCount(row: 2000000);
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        return null;
    }
}