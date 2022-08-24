using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Year2021.Day19;

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

    public long SolvePartOne(string[] inputLines)
    {
        var scanners = Input(inputLines);

        // As all the scanners return relative locations, we can use the first scanner as a reference
        var firstScanner = scanners.First();
        var identifiedScanners = new HashSet<Scanner> { scanners.First() };
        var remainingScanners = scanners.Skip(1).ToList();

        var identifiedBeacons = new HashSet<Point3d>();
        foreach (var beacon in firstScanner.BeaconCoordinates)
        {
            identifiedBeacons.Add(beacon);
        }

        // The queue contains the scanners used as reference to match other scanners
        var queue = new Queue<Scanner>();
        queue.Enqueue(firstScanner);

        while (queue.Count > 0)
        {
            var referenceScanner = queue.Dequeue();

            // Compare
            foreach (var targetScanner in remainingScanners)
            {
                if (identifiedScanners.Contains(targetScanner))
                {
                    continue;
                }

                // The reoriented scanner is the same as the target scanner but in the same
                // coordinate system as the reference scanner
                var reorientedScanner = targetScanner.ReorientScanner(referenceScanner);
                if (reorientedScanner != null)
                {
                    identifiedScanners.Add(targetScanner);

                    foreach (var reorientedBeacon in reorientedScanner.BeaconCoordinates)
                    {
                        identifiedBeacons.Add(reorientedBeacon);
                    }

                    // The reoriented scanner can now be used as a reference scanner
                    queue.Enqueue(reorientedScanner);
                }
            }
        }

        return identifiedBeacons.Count;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var input = Input(inputLines);

        return null;
    }
}
