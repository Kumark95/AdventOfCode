namespace AdventOfCode.Core.Year2021.Day19;

internal class Scanner
{
    public int Id { get; init; }
    public List<Coordinate3d> BeaconCoordinates { get; init; }

    private const int N_ORIENTATIONS = 24;
    private const int MIN_MATCHED_BEACONS = 12;

    public Scanner(int id)
    {
        Id = id;
        BeaconCoordinates = new List<Coordinate3d>();
    }

    public Scanner(int id, List<Coordinate3d> beacons)
    {
        Id = id;
        BeaconCoordinates = beacons;
    }

    /// <summary>
    /// Reorient the scanner using another as reference
    /// </summary>
    /// <param name="reference"></param>
    /// <remarks>
    /// To reorient both scanners need to share a subset of beacons
    /// </remarks>
    /// <returns></returns>
    public Scanner? ReorientScanner(Scanner reference)
    {
        for (int rotationOffset = 0; rotationOffset < N_ORIENTATIONS; rotationOffset++)
        {
            var rotatedBeacons = BeaconCoordinates
                .Select(b => b.ChangeOrientation(rotationOffset))
                .ToList();

            foreach (var sourceBeacon in reference.BeaconCoordinates)
            {
                foreach (var rotatedBeacon in rotatedBeacons)
                {
                    // Offset to the reference coordinate system
                    var posOffset = sourceBeacon - rotatedBeacon;

                    // Set the beacons in the same coordinate system
                    var movedBeacons = rotatedBeacons
                        .Select(rb => rb + posOffset)
                        .ToList();

                    var reorientedScanner = new Scanner(Id, movedBeacons);
                    if (reorientedScanner.HasEnoughCommonBeacons(reference))
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
