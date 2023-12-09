using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day05.Model;

internal record Almanac(CategoryMap[] CategoryMaps)
{
    public long ConvertSeedToLocation(long seedNumber)
    {
        var convertedNumber = seedNumber;
        foreach (var map in CategoryMaps)
        {
            convertedNumber = map.ConvertCategory(convertedNumber);
        }

        return convertedNumber;
    }

    public long CalculateMinLocation(Range<long> range)
    {
        List<Range<long>> convertedRanges = [range];

        foreach (var map in CategoryMaps)
        {
            convertedRanges = convertedRanges
                .SelectMany(map.ConvertCategory)
                .ToList();
        }

        return convertedRanges
            .Select(r => r.Start)
            .Min();
    }
};
