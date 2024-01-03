using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day05.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day05;

[PuzzleName("If You Give A Seed A Fertilizer")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 5;

    [PuzzleInput(filename: "example.txt", expectedResult: 35)]
    [PuzzleInput(filename: "input.txt", expectedResult: 525792406)]
    public object SolvePartOne(string[] inputLines)
    {
        var seedNumbers = InputParser.ParseSeedNumbers(inputLines[0]);
        var almanac = InputParser.ParseAlmanac(inputLines[2..]);

        return seedNumbers
            .Select(almanac.ConvertSeedToLocation)
            .Min();
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 46)]
    [PuzzleInput(filename: "input.txt", expectedResult: 79004094)]
    public object SolvePartTwo(string[] inputLines)
    {
        var seedRanges = InputParser.ParseSeedRanges(inputLines[0]);
        var almanac = InputParser.ParseAlmanac(inputLines[2..]);

        return seedRanges
            .Select(almanac.CalculateMinLocation)
            .Min();
    }
}
