namespace AdventOfCode.Core.Year2022.Day15.Model;

internal class SensorMeasurement
{
    public Position Sensor { get; init; }
    public Position ClosestBeacon { get; init; }

    public int Distance { get; init; }
    public Position ExcusionZoneNorth { get; init; }
    public Position ExcusionZoneSouth { get; init; }
    public Position ExcusionZoneWest { get; init; }
    public Position ExcusionZoneEast { get; init; }

    public SensorMeasurement(Position sensor, Position closestBeacon)
    {
        Sensor = sensor;
        ClosestBeacon = closestBeacon;

        Distance = sensor.ManhattanDistance(closestBeacon);
        ExcusionZoneNorth = new Position(sensor.Row - Distance, sensor.Col);
        ExcusionZoneSouth = new Position(sensor.Row + Distance, sensor.Col);
        ExcusionZoneWest = new Position(sensor.Row, sensor.Col - Distance);
        ExcusionZoneEast = new Position(sensor.Row, sensor.Col + Distance);
    }

    public bool IsInsideExclusionZone(Position position)
    {
        var distnaceToTarget = Sensor.ManhattanDistance(position);
        return distnaceToTarget <= Distance;
    }

    public List<Position> GetExclussionPositions(int row)
    {
        var positions = new List<Position>();
        return positions;
    }
}
