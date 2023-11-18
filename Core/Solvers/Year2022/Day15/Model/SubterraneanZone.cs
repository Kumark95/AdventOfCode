namespace AdventOfCode.Core.Solvers.Year2022.Day15.Model;

internal class SubterraneanZone
{
    public List<SensorMeasurement> SensorMeasurements { get; init; }

    public SubterraneanZone(List<SensorMeasurement> sensorMeasurements)
    {
        SensorMeasurements = sensorMeasurements;
    }

    public int GetBeaconExclusionPositionCount(int row)
    {
        var exclusionPositions = new HashSet<Position>();
        foreach (var measurement in SensorMeasurements)
        {
            var sensorExcPositions = measurement.GetBeaconExclussionPositions(row);

            foreach (var pos in sensorExcPositions)
            {
                exclusionPositions.Add(pos);
            }
        }

        return exclusionPositions.Count;
    }
}
