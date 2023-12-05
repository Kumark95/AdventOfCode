using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day05.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day05;

[PuzzleName("If You Give A Seed A Fertilizer")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 5;

    public long SolvePartOne(string[] inputLines)
    {
        var seedNumbers = Almanac.ParseSeedNumbers(inputLines[0]);
        var converters = Almanac.ParseConverters(inputLines[2..]);

        var minConvertedValue = long.MaxValue;
        foreach (var number in seedNumbers)
        {
            minConvertedValue = Math.Min(minConvertedValue, Almanac.ConvertSeed(number, converters));
        }

        return minConvertedValue;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var seedRanges = Almanac.ParseSeedNumbers(inputLines[0]);
        var converters = Almanac.ParseConverters(inputLines[2..]);

        var minConvertedValue = long.MaxValue;

        // Seed input now corresponds to a range instead of discrete values
        for (var i = 0; i < seedRanges.Length; i += 2)
        {
            var startingSeed = seedRanges[i];
            var endSeed = startingSeed + seedRanges[i + 1] - 1;

            for (var seed = startingSeed; seed <= endSeed; seed++)
            {
                minConvertedValue = Math.Min(minConvertedValue, Almanac.ConvertSeed(seed, converters));
            }
        }

        return minConvertedValue;
    }
}
