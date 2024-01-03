using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Interfaces;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2021.Day19;

[PuzzleName("Beacon Scanner")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 19;

    private static IEnumerable<Scanner> Input(string[] inputLines)
    {
        var scanners = new List<Scanner>();

        Scanner? currentScanner = null;
        foreach (var line in inputLines)
        {
            if (line.StartsWith("--- scanner "))
            {
                var matches = Regex.Match(line, @"--- scanner (?<ScannerId>\d+) ---");

                currentScanner = new Scanner(int.Parse(matches.Groups["ScannerId"].Value));
                scanners.Add(currentScanner);
            }
            else if (line.Contains(',') && currentScanner != null)
            {
                var parts = line.Split(',');
                var beaconCoordinate = new Coordinate3d(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                currentScanner.BeaconCoordinates.Add(beaconCoordinate);
            }
        }

        return scanners;
    }

    public object SolvePartOne(string[] inputLines)
    {
        var scanners = Input(inputLines);
        var reorientedScanners = Scanner.Reorient(scanners);

        return reorientedScanners
            .SelectMany(s => s.BeaconCoordinates)
            .Distinct()
            .Count();
    }

    public object SolvePartTwo(string[] inputLines)
    {
        var scanners = Input(inputLines);
        var reorientedScanners = Scanner.Reorient(scanners);

        return reorientedScanners
            .CombinationsWithoutRepetition(sampleSize: 2)
            .Select(pair => pair.First().ManhattanDistance(pair.Last()))
            .Max();
    }
}
