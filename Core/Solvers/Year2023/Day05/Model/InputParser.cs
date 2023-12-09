using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day05.Model;

internal static class InputParser
{
    public static long[] ParseSeedNumbers(string line)
    {
        return line
            .Replace("seeds: ", "")
            .Split(' ')
            .Select(x => long.Parse(x))
            .ToArray();
    }

    public static Range<long>[] ParseSeedRanges(string line)
    {
        var baseNumbers = ParseSeedNumbers(line);

        // The values represent pairs where the second value is the length
        return Enumerable.Range(0, baseNumbers.Length / 2)
            .Select(i =>
            {
                var start = baseNumbers[i * 2];
                var length = baseNumbers[i * 2 + 1];
                return new Range<long>(start, start + length - 1);
            })
            .ToArray();
    }

    public static Almanac ParseAlmanac(string[] inputLines)
    {
        var categoryMaps = new List<CategoryMap>();

        List<RangeConverter> rangeConverters = [];
        string? sourceCategory = null;
        string? destinationCategory = null;
        foreach (var line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                if (rangeConverters.Count > 0 && sourceCategory is not null && destinationCategory is not null)
                {
                    categoryMaps.Add(new CategoryMap(sourceCategory, destinationCategory, rangeConverters.ToArray()));
                }

                // Reset values
                rangeConverters = [];
                sourceCategory = null;
                destinationCategory = null;
            }
            else if (char.IsDigit(line[0]))
            {
                var lineParts = line.Split(' ');
                var destinationStart = long.Parse(lineParts[0]);
                var sourceStart = long.Parse(lineParts[1]);
                var rangeLength = long.Parse(lineParts[2]);

                var sourceRange = new Range<long>(sourceStart, sourceStart + rangeLength - 1);
                var destinationRange = new Range<long>(destinationStart, destinationStart + rangeLength - 1);

                rangeConverters.Add(new RangeConverter(sourceRange, destinationRange));
            }
            else
            {
                var lineParts = line.Replace(" map:", "").Split("-to-");
                sourceCategory = lineParts[0];
                destinationCategory = lineParts[1];
            }
        }

        if (rangeConverters.Count > 0 && sourceCategory is not null && destinationCategory is not null)
        {
            categoryMaps.Add(new CategoryMap(sourceCategory, destinationCategory, rangeConverters.OrderBy(c => c.SourceRange.Start).ToArray()));
        }

        return new Almanac(categoryMaps.ToArray());
    }
}
