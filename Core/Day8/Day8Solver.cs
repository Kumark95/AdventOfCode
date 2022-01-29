using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Day8;

[PuzzleName("The Treachery of Whales")]
public class Day8Solver : IPuzzleSolver
{
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

    public long SolvePartOne(string[] inputLines)
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
            var mappings = new Dictionary<string, int>();

            // Order inputs
            var orderedPatterns = displayReading.Patterns
                .Select(x => new string(x.OrderBy(c => c).ToArray()))
                .OrderBy(p => p.Length)
                .ToList();

            // Select unique mappings
            foreach (var segmentLength in _uniqueSegmentMappings)
            {
                var pattern = orderedPatterns.Single(p => p.Length == segmentLength.Key);
                mappings[pattern] = segmentLength.Value;
            }

            // 6 segment digits
            var sixSegmentPatterns = orderedPatterns.Where(p => p.Length == 6).ToList();

            // (9) shares 3 segments with (4)
            var ninePattern = sixSegmentPatterns.Single(p => p.ContainsAllChars(mappings.First(m => m.Value == 4).Key));
            sixSegmentPatterns.Remove(ninePattern);
            mappings[ninePattern] = 9;

            // (0) shares 3 segments with (7)
            var zeroPattern = sixSegmentPatterns.Single(p => p.ContainsAllChars(mappings.First(m => m.Value == 7).Key));
            sixSegmentPatterns.Remove(zeroPattern);
            mappings[zeroPattern] = 0;

            // (6) is the remaining digit with 6 segments
            var sixPattern = sixSegmentPatterns.Single(p => p.Length == 6);
            mappings[sixPattern] = 6;


            // 5 digit segments
            var fiveSegmentPatterns = orderedPatterns.Where(p => p.Length == 5).ToList();

            // (3) shares 3 segments with (7)
            var threePattern = fiveSegmentPatterns.Single(p => p.ContainsAllChars(mappings.First(m => m.Value == 7).Key));
            fiveSegmentPatterns.Remove(threePattern);
            mappings[threePattern] = 3;

            // (5) shares 3 segments with (4)
            var fivePattern = fiveSegmentPatterns.Single(p => p.ContainsNChars(mappings.First(m => m.Value == 4).Key, 3));
            fiveSegmentPatterns.Remove(fivePattern);
            mappings[fivePattern] = 5;

            // (2) is the remaining digit with 5 segments
            var twoPattern = fiveSegmentPatterns.Single(p => p.Length == 5);
            mappings[twoPattern] = 2;


            // Decode the output values
            var orderedOutputValues = displayReading.OutputValues
                .Select(x => new string(x.OrderBy(c => c).ToArray()))
                .ToList();

            var lineResult = "";
            foreach (var outputValue in orderedOutputValues)
            {
                lineResult += Convert.ToString(mappings[outputValue]);
            }

            counter += int.Parse(lineResult);
        }

        return counter;
    }
}
