using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day05.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day05;

public record R(long SourceStart, long SourceEnd, long DestinationStart, long DestinationEnd);

[PuzzleName("If You Give A Seed A Fertilizer")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 5;

    public long SolvePartOne(string[] inputLines)
    {
        var seedNumbers = Almanac.ParseSeedNumbers(inputLines[0]);
        var converters = Almanac.ParseConverters(inputLines[2..]);


        var i = 0;
        foreach (var cv in converters)
        {
            Console.WriteLine($"Converter {i}");
            i++;

            //var rr = cv.Ranges
            //    .OrderBy(r => r.DestinationStart)
            //    .Select(r => new R(r.SourceStart, r.SourceStart + r.Length - 1, r.DestinationStart, r.DestinationStart + r.Length - 1))
            //    .ToList();

            //var prev = rr.First();
            //Console.WriteLine($"R [{prev.DestinationStart}, {prev.DestinationEnd}]");
            //foreach (var r in rr.Skip(1))
            //{
            //    Console.WriteLine($"R [{r.DestinationStart}, {r.DestinationEnd}]");

            //    if (r.DestinationStart != prev.DestinationEnd + 1)
            //    {
            //        Console.WriteLine($"\t -> Range not contiguous");
            //    }

            //    prev = r;
            //}

            var rr = cv.Ranges
                .OrderBy(r => r.SourceStart)
                .Select(r => new R(r.SourceStart, r.SourceStart + r.Length - 1, r.DestinationStart, r.DestinationStart + r.Length - 1))
                .ToList();

            var prev = rr.First();
            Console.WriteLine($"R [{prev.SourceStart}, {prev.SourceEnd}]");
            foreach (var r in rr.Skip(1))
            {
                Console.WriteLine($"R [{r.SourceStart}, {r.SourceEnd}]");

                if (r.SourceStart != prev.SourceEnd + 1)
                {
                    Console.WriteLine($"\t -> Range not contiguous");
                }

                prev = r;
            }

            //foreach (var r in rr)
            //{
            //    var sF = r.SourceStart;
            //    var sT = r.SourceStart + r.Length - 1;

            //    var dF = r.DestinationStart;
            //    var dT = r.DestinationStart + r.Length - 1;
            //    Console.WriteLine($"FROM [{sF},{sT}] TO [{dF},{dT}]");
            //}
        }


        var minConvertedValue = long.MaxValue;
        foreach (var number in seedNumbers)
        {
            minConvertedValue = Math.Min(minConvertedValue, Almanac.ConvertSeed(number, converters));
        }

        return minConvertedValue;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        return null;

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
