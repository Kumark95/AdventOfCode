namespace AdventOfCode.Core.Solvers.Year2023.Day05.Model;

internal static class Almanac
{
    public static long[] ParseSeedNumbers(string line)
    {
        return line
            .Replace("seeds: ", "")
            .Split(' ')
            .Select(x => long.Parse(x))
            .ToArray();
    }

    public static List<Converter> ParseConverters(string[] lines)
    {
        var converters = new List<Converter>();

        List<Range> ranges = [];
        string? source = null;
        string? target = null;
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                if (ranges.Count > 0 && source is not null && target is not null)
                {
                    converters.Add(new Converter(source, target, ranges));
                }

                // Reset values
                ranges = [];
                source = null;
                target = null;

                continue;
            }
            else if (char.IsDigit(line[0]))
            {
                var lineParts = line.Split(' ');
                var destinationStart = long.Parse(lineParts[0]);
                var sourceStart = long.Parse(lineParts[1]);
                var rangeLength = long.Parse(lineParts[2]);

                ranges.Add(new Range(sourceStart, destinationStart, rangeLength));
            }
            else
            {
                var lineParts = line.Replace(" map:", "").Split("-to-");
                source = lineParts[0];
                target = lineParts[1];
            }
        }

        if (ranges.Count > 0 && source is not null && target is not null)
        {
            converters.Add(new Converter(source, target, ranges));
        }

        return converters;
    }

    public static long ConvertSeed(long initialSeed, List<Converter> converters)
    {
        var convertedSeed = initialSeed;

        foreach (var converter in converters)
        {
            convertedSeed = converter.Convert(convertedSeed);
        }

        return convertedSeed;
    }
}

internal record Range(long SourceStart, long DestinationStart, long Length)
{
    public bool TryConvert(long number, out long convertedNumber)
    {
        var inRange = number >= SourceStart && number < SourceStart + Length;
        if (!inRange)
        {
            convertedNumber = default;
            return false;
        }

        convertedNumber = number + (DestinationStart - SourceStart);
        return true;
    }
};

internal record Converter(string Source, string Destination, List<Range> Ranges)
{
    public long Convert(long number)
    {
        foreach (var range in Ranges)
        {
            if (range.TryConvert(number, out long convertedValue))
            {
                return convertedValue;
            }
        }

        return number;
    }
};
