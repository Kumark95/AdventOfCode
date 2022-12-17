namespace AdventOfCode.Core.Year2022.Day15.Model;

internal class SubterraneanZone
{
    public List<SensorMeasurement> SensorMeasurements { get; init; }

    public SubterraneanZone(List<SensorMeasurement> sensorMeasurements)
    {
        SensorMeasurements = sensorMeasurements;
    }

    public int GetBeaconExclusionPositionCount(int row)
    {
        // TO CHECK:
        // * Elements inside the Exclusion zone of each sensor at a given row
        // * To HashSet?

        var minCol = 0;
        var maxCol = 0;

        foreach (var measurement in SensorMeasurements)
        {
            var sensorCol = measurement.Sensor.Col;
            var sensorColExpandedWest = sensorCol - measurement.Distance;
            var sensorColExpandedEast = sensorCol + measurement.Distance;

            var beaconCol = measurement.ClosestBeacon.Col;

            minCol = Math.Min(minCol, Math.Min(sensorColExpandedWest, beaconCol));
            maxCol = Math.Max(minCol, Math.Max(sensorColExpandedEast, beaconCol));
        }

        var count = 0;
        for (int i = minCol; i <= maxCol; i++)
        {
            var position = new Position(row, i);
            if (IsInsideAnyExclusionZone(position))
            {
                count++;
            }
        }

        return count;
    }

    public bool IsInsideAnyExclusionZone(Position position)
    {
        foreach (var measurement in SensorMeasurements)
        {
            if (position == measurement.Sensor || position == measurement.ClosestBeacon)
            {
                // Positions occupied by sensors or beacons do not count
                return false;
            }

            if (measurement.IsInsideExclusionZone(position))
            {
                return true;
            }
        }

        return false;
    }
}
