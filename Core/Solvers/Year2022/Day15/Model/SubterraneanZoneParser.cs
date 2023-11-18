using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2022.Day15.Model;

internal static class SubterraneanZoneParser
{
    public static SubterraneanZone Parse(string[] inputLines)
    {
        var sensorMeasurements = new List<SensorMeasurement>();
        var regex = new Regex(@"Sensor at x=(?<SensorCol>-?\d+), y=(?<SensorRow>-?\d+): closest beacon is at x=(?<BeaconCol>-?\d+), y=(?<BeaconRow>-?\d+)", RegexOptions.Compiled);
        foreach (var line in inputLines)
        {
            var match = regex.Match(line);
            if (!match.Success)
            {
                throw new Exception("Could not extract the sensor and beacon coordinates");
            }

            var sensorRow = int.Parse(match.Groups["SensorRow"].Value);
            var sensorCol = int.Parse(match.Groups["SensorCol"].Value);
            var sensorPosition = new Position(sensorRow, sensorCol);

            var beaconRow = int.Parse(match.Groups["BeaconRow"].Value);
            var beaconCol = int.Parse(match.Groups["BeaconCol"].Value);
            var beaconPosition = new Position(beaconRow, beaconCol);

            sensorMeasurements.Add(new SensorMeasurement(sensorPosition, beaconPosition));
        }

        return new SubterraneanZone(sensorMeasurements);
    }
}
