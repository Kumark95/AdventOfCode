namespace AdventOfCode.Core.Year2022.Day15.Model;

internal class SubterraneanZone
{
    public const char SensorSymbol = 'S';
    public const char BeaconSymbol = 'B';
    public const char BeaconExclusionPositionSymbol = '#';

    public List<SensorMeasurement> SensorMeasurements { get; init; }
    private Dictionary<Position, char> MarkedPositions { get; init; }

    public SubterraneanZone(List<SensorMeasurement> sensorMeasurements)
    {
        SensorMeasurements = sensorMeasurements;
        MarkedPositions = InitMarkedPositions(sensorMeasurements);
    }

    public void AnalyzeMeasurements()
    {
        foreach (var measurement in SensorMeasurements)
        {
            var sensor = measurement.Sensor;
            var beaconDistance = sensor.ManhattanDistance(measurement.ClosestBeacon);

            var queue = new Queue<Position>();
            queue.Enqueue(sensor);

            // Analyze its surroundings
            while (queue.Count > 0)
            {
                var position = queue.Dequeue();

                foreach (var neightbour in position.Neighbours())
                {
                    if (IsMarked(neightbour))
                    {
                        continue;

                    }

                    var neighbourDistance = sensor.ManhattanDistance(neightbour);
                    if (neighbourDistance > beaconDistance)
                    {
                        continue;
                    }

                    MarkExclusionPosition(neightbour);
                    queue.Enqueue(neightbour);
                }
            }
        }
    }

    public int GetBeaconExclusionPositionCount(int row)
    {
        var positions = MarkedPositions.Keys.ToList();

        return MarkedPositions
            .Where(p => p.Key.Row == row && p.Value == BeaconExclusionPositionSymbol)
            .Count();
    }

    private static Dictionary<Position, char> InitMarkedPositions(List<SensorMeasurement> sensorMeasurements)
    {
        var occupiedPositions = new Dictionary<Position, char>();
        foreach (var measurement in sensorMeasurements)
        {
            occupiedPositions.TryAdd(measurement.Sensor, SensorSymbol);
            occupiedPositions.TryAdd(measurement.ClosestBeacon, BeaconSymbol);
        }
        return occupiedPositions;
    }

    private bool IsMarked(Position position)
    {
        return MarkedPositions.ContainsKey(position);
    }

    private void MarkExclusionPosition(Position position)
    {
        MarkedPositions.Add(position, BeaconExclusionPositionSymbol);
    }
}
