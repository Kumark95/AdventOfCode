using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Solvers.Year2021.Day08;


[PuzzleName("The Treachery of Whales")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 8;

    /*
     * Map the unique segment lengths to the digit value.
     * Key: segment count. Value: digit
     */
    private readonly Dictionary<int, int> _uniqueSegmentMappings = new()
    {
        { 2, 1 },
        { 3, 7 },
        { 4, 4 },
        { 7, 8 }
    };

    private static List<DisplayReading> Input(string[] inputLines)
    {
        var displayReadings = new List<DisplayReading>();

        foreach (var entry in inputLines)
        {
            var entryParts = entry.Split(" | ");

            var patterns = entryParts[0].Split(' ').ToList();
            var outputValues = entryParts[1].Split(' ').ToList();

            displayReadings.Add(new DisplayReading(patterns, outputValues));
        }

        return displayReadings;
    }

    public long? SolvePartOne(string[] inputLines)
    {
        var counter = 0;

        var displayReadings = Input(inputLines);
        foreach (var displayReading in displayReadings)
        {
            foreach (var outputValue in displayReading.OutputValues)
            {
                if (_uniqueSegmentMappings.ContainsKey(outputValue.Length)) counter++;
            }
        }

        return counter;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var counter = 0;

        var displayReadings = Input(inputLines);
        foreach (var displayReading in displayReadings)
        {
            // Build mappings for each character to its correct pattern
            var mappings = new Dictionary<int, string>();

            // Order inputs
            var orderedPatterns = displayReading.Patterns
                .Select(x => new string(x.OrderBy(c => c).ToArray()))
                .OrderBy(p => p.Length)
                .ToList();

            // Select unique mappings
            foreach (var segmentLength in _uniqueSegmentMappings)
            {
                var pattern = orderedPatterns.Single(p => p.Length == segmentLength.Key);
                mappings[segmentLength.Value] = pattern;
            }

            // 6 segment patterns
            mappings[9] = orderedPatterns.Single(p => p.Length == 6 && p.ContainsAllChars(mappings[4]));
            mappings[0] = orderedPatterns.Single(p => p.Length == 6 && p.ContainsAllChars(mappings[7]) && p != mappings[9]);
            mappings[6] = orderedPatterns.Single(p => p.Length == 6 && p != mappings[9] && p != mappings[0]);

            // 5 segment patterns
            mappings[3] = orderedPatterns.Single(p => p.Length == 5 && p.ContainsAllChars(mappings[7]));
            mappings[5] = orderedPatterns.Single(p => p.Length == 5 && p.IntersectsWithNChars(mappings[4], 3) && p != mappings[3]);
            mappings[2] = orderedPatterns.Single(p => p.Length == 5 && p != mappings[5] && p != mappings[3]);

            // Decode the output values
            var orderedOutputValues = displayReading.OutputValues
                .Select(x => new string(x.OrderBy(c => c).ToArray()))
                .ToList();

            var lineResult = "";
            foreach (var outputValue in orderedOutputValues)
            {
                lineResult += Convert.ToString(
                        mappings.Single(m => m.Value == outputValue).Key
                    );
            }

            counter += int.Parse(lineResult);
        }

        return counter;
    }
}
