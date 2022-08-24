namespace AdventOfCode.Core.Year2021.Day19;

internal class Scanner
{
    public int Id { get; init; }
    public List<Coordinate3d> BeaconCoordinates { get; init; }
    public Coordinate3d Position { get; init; }

    private const int N_ORIENTATIONS = 24;
    private const int MIN_MATCHED_BEACONS = 12;

    public Scanner(int id)
    {
        Id = id;
        BeaconCoordinates = new List<Coordinate3d>();
        Position = new Coordinate3d(0, 0, 0);
    }

    public Scanner(int id, List<Coordinate3d> beacons, Coordinate3d position)
    {
        Id = id;
        BeaconCoordinates = beacons;
        Position = position;
    }

    /// <summary>
    /// Calculate the Manhattan distance to another scanner
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public int ManhattanDistance(Scanner target)
    {
        return Math.Abs(Position.X - target.Position.X)
            + Math.Abs(Position.Y - target.Position.Y)
            + Math.Abs(Position.Z - target.Position.Z);
    }

    /// <summary>
    /// Reorient a group of scanners
    /// </summary>
    /// <param name="scanners"></param>
    /// <remarks>
    /// The first scanner is used as reference
    /// </remarks>
    /// <returns></returns>
    public static IEnumerable<Scanner> Reorient(IEnumerable<Scanner> scanners)
    {
        // As all the scanners return relative locations, we can use the first scanner as a reference
        var firstScanner = scanners.First();
        var remainingScanners = scanners.Skip(1).ToList();

        var reorientedScanners = new HashSet<Scanner> { firstScanner };
        var visitedScanners = new HashSet<Scanner> { firstScanner };

        // The queue contains the scanners used as reference to match other scanners
        var queue = new Queue<Scanner>();
        queue.Enqueue(firstScanner);

        while (queue.Count > 0)
        {
            var referenceScanner = queue.Dequeue();

            // Compare
            foreach (var relativeScanner in remainingScanners)
            {
                if (visitedScanners.Contains(relativeScanner))
                {
                    continue;
                }

                // The reoriented scanner is the same as the target scanner but in the same
                // coordinate system as the reference scanner
                var reorientedRelativeScanner = relativeScanner.ReorientWithReference(referenceScanner);
                if (reorientedRelativeScanner != null)
                {
                    visitedScanners.Add(relativeScanner);

                    // The reoriented scanner can now be used as a reference scanner
                    reorientedScanners.Add(reorientedRelativeScanner);
                    queue.Enqueue(reorientedRelativeScanner);
                }
            }
        }

        return reorientedScanners;
    }

    /// <summary>
    /// Reorient the scanner using another as reference
    /// </summary>
    /// <param name="referenceScanner"></param>
    /// <remarks>
    /// To reorient both scanners need to share a subset of beacons
    /// </remarks>
    /// <returns></returns>
    public Scanner? ReorientWithReference(Scanner referenceScanner)
    {
        for (int rotationOffset = 0; rotationOffset < N_ORIENTATIONS; rotationOffset++)
        {
            var rotatedBeacons = BeaconCoordinates
                .Select(b => b.ChangeOrientation(rotationOffset))
                .ToList();

            foreach (var referenceBeacon in referenceScanner.BeaconCoordinates)
            {
                foreach (var rotatedBeacon in rotatedBeacons)
                {
                    // Offset to the reference coordinate system
                    var posOffset = referenceBeacon - rotatedBeacon;

                    // Set the beacons in the same coordinate system
                    var movedBeacons = rotatedBeacons
                        .Select(rb => rb + posOffset)
                        .ToList();

                    var reorientedScanner = new Scanner(Id, movedBeacons, posOffset);
                    if (reorientedScanner.HasEnoughCommonBeacons(referenceScanner))
                    {
                        return reorientedScanner;
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Checks the number of common beacons between two scanners
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool HasEnoughCommonBeacons(Scanner target)
    {
        var matchedBeacons = BeaconCoordinates.Intersect(target.BeaconCoordinates);
        if (matchedBeacons.Count() >= MIN_MATCHED_BEACONS)
        {
            return true;
        }

        return false;
    }
}
