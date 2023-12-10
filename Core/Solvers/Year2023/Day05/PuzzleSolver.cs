using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day05.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day05;

[PuzzleName("If You Give A Seed A Fertilizer")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 5;

    public long? SolvePartOne(string[] inputLines)
    {
        var seedNumbers = InputParser.ParseSeedNumbers(inputLines[0]);
        var almanac = InputParser.ParseAlmanac(inputLines[2..]);

        return seedNumbers
            .Select(almanac.ConvertSeedToLocation)
            .Min();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var seedRanges = InputParser.ParseSeedRanges(inputLines[0]);
        var almanac = InputParser.ParseAlmanac(inputLines[2..]);

        return seedRanges
            .Select(almanac.CalculateMinLocation)
            .Min();
    }
}
