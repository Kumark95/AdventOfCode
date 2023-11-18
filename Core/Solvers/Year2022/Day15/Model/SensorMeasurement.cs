namespace AdventOfCode.Core.Solvers.Year2022.Day15.Model;

internal class SensorMeasurement
{
    public Position Sensor { get; init; }
    public Position ClosestBeacon { get; init; }
    public int Distance { get; init; }

    public SensorMeasurement(Position sensor, Position closestBeacon)
    {
        Sensor = sensor;
        ClosestBeacon = closestBeacon;

        Distance = sensor.ManhattanDistance(closestBeacon);
    }

    public List<Position> GetBeaconExclussionPositions(int row)
    {
        var excludedPositions = new List<Position>();

        var rowDiff = Math.Abs(Sensor.Row - row);
        var minCol = Sensor.Col - Distance + rowDiff;
        var maxCol = Sensor.Col + Distance - rowDiff;

        for (int c = minCol; c <= maxCol; c++)
        {
            var pos = new Position(row, c);
            if (pos == ClosestBeacon)
            {
                continue;
            }

            excludedPositions.Add(pos);
        }

        return excludedPositions;
    }
}
