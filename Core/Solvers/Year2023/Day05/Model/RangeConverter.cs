using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day05.Model;

internal record RangeConverter(Range<long> SourceRange, Range<long> DestinationRange)
{
    public bool TryConvert(long inputNumber, out long outputNumber)
    {
        if (SourceRange.Contains(inputNumber))
        {
            var delta = inputNumber - SourceRange.Start;

            outputNumber = DestinationRange.Start + delta;
            return true;
        }

        outputNumber = default;
        return false;
    }

    public bool TryConvert(Range<long> input, out Range<long> output)
    {
        if (SourceRange.Contains(input))
        {
            var delta = input.Start - SourceRange.Start;
            var destinationRangeStart = DestinationRange.Start + delta;
            var destinarionRangeEnd = DestinationRange.Start + delta + input.Length - 1;

            output = new Range<long>(destinationRangeStart, destinarionRangeEnd);
            return true;
        }

        output = default!;
        return false;
    }
}
