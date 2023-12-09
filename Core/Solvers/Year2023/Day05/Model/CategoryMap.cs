using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day05.Model;

internal record CategoryMap(string SourceCategory, string DestinationCategory, RangeConverter[] Converters)
{
    public long ConvertCategory(long sourceNumber)
    {
        foreach (var converter in Converters)
        {
            if (converter.TryConvert(sourceNumber, out long destinationNumber))
            {
                return destinationNumber;
            }
        }

        return sourceNumber;
    }

    public List<Range<long>> ConvertCategory(Range<long> range)
    {
        var thresholdRanges = Converters
            .Select(c => c.SourceRange)
            .ToList();

        var destinationRanges = new List<Range<long>>();

        foreach (var splitRange in SplitRange(range, thresholdRanges))
        {
            var shouldAddSelf = true;
            foreach (var converter in Converters)
            {
                if (converter.TryConvert(splitRange, out Range<long> destinationRange))
                {
                    destinationRanges.Add(destinationRange);

                    shouldAddSelf = false;
                    break;
                }
            }

            // When the split range does not match any of the converters
            if (shouldAddSelf)
            {
                destinationRanges.Add(splitRange);
            }
        }

        return destinationRanges;
    }

    /// <summary>
    /// Split a range preserving the threshold ranges limits
    /// </summary>
    private List<Range<long>> SplitRange(Range<long> range, List<Range<long>> thresholdRanges)
    {
        List<Range<long>> result = [];
        Range<long> currentRange = range;

        foreach (var threshold in thresholdRanges.OrderBy(t => t.Start))
        {
            if (threshold.Contains(currentRange))
            {
                result.Add(currentRange);
            }
            else if (threshold.Intersects(currentRange))
            {
                var intersection = threshold.Intersection(currentRange);
                result.Add(intersection);

                // Only the first part is added as the next sections will be re-evaluated by other thresholds
                var firstDifferenceRange = currentRange.Difference(intersection)[0];
                if (firstDifferenceRange.Start < threshold.Start)
                {
                    result.Add(firstDifferenceRange);
                }

                currentRange = currentRange with { Start = intersection.End + 1 };
            }
        }

        if (result.Count == 0)
        {
            return [range];
        }

        return result;
    }
}
