using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2021.Day22.Model;

internal class Reactor
{
    /// <summary>
    /// Reboot the reactor and return the number of light cubes
    /// </summary>
    /// <param name="instructions"></param>
    /// <param name="onlyInitialyze"></param>
    /// <returns></returns>
    public static long RebootAndCountLightCubes(string[] instructions, bool onlyInitialyze)
    {
        var rebootSteps = ParseInstructions(instructions);

        if (onlyInitialyze)
        {
            rebootSteps = rebootSteps
                .Where(s => Math.Abs(s.Cuboid.XMin) <= 50
                    || Math.Abs(s.Cuboid.XMax) <= 50
                    || Math.Abs(s.Cuboid.YMin) <= 50
                    || Math.Abs(s.Cuboid.YMax) <= 50
                    || Math.Abs(s.Cuboid.ZMin) <= 50
                    || Math.Abs(s.Cuboid.ZMax) <= 50)
                .ToList();
        }

        // The inclusion-exclusion principle is used to count the number of cubes (coordinates) in each cuboid 
        // |A union B| = |A| + |B| - |A intersection B|
        var cardinalityOperations = new List<(Operation, Cuboid)>();

        foreach (var step in rebootSteps)
        {
            var baseCuboid = step.Cuboid;

            // Only account for cubes with On switches
            var newOperations = new List<(Operation, Cuboid)>();
            if (step.SwitchMode == SwitchMode.On)
            {
                // From the previous operation |B| can be skipped if the SwitchMode is off
                newOperations.Add((Operation.Add, baseCuboid));
            }

            foreach (var (targetOperation, targetCuboid) in cardinalityOperations)
            {
                var cuboidIntersection = baseCuboid.Intersection(targetCuboid);
                if (cuboidIntersection.HasValue)
                {
                    // Invert the operation for the intersection
                    var intersectionOperation = targetOperation == Operation.Add ? Operation.Subtract : Operation.Add;
                    newOperations.Add((intersectionOperation, cuboidIntersection.Value));
                }
            }

            cardinalityOperations.AddRange(newOperations);
        }

        // Parse the resulting operations
        long totalLightCubes = 0L;
        foreach (var (operation, cuboid) in cardinalityOperations)
        {
            if (operation == Operation.Add)
            {
                totalLightCubes += cuboid.Count();
            }
            else
            {
                totalLightCubes -= cuboid.Count();
            }
        }

        return totalLightCubes;
    }

    private static List<RebootStep> ParseInstructions(string[] instructions)
    {
        var regex = new Regex(@"(?<SwitchMode>on|off) x=(?<XMin>-?\d+)..(?<XMax>-?\d+),y=(?<YMin>-?\d+)..(?<YMax>-?\d+),z=(?<ZMin>-?\d+)..(?<ZMax>-?\d+)");

        var steps = new List<RebootStep>();
        foreach (var instruction in instructions)
        {
            var matches = regex.Match(instruction);

            var cuboid = new Cuboid
            {
                XMin = int.Parse(matches.Groups["XMin"].Value),
                XMax = int.Parse(matches.Groups["XMax"].Value),
                YMin = int.Parse(matches.Groups["YMin"].Value),
                YMax = int.Parse(matches.Groups["YMax"].Value),
                ZMin = int.Parse(matches.Groups["ZMin"].Value),
                ZMax = int.Parse(matches.Groups["ZMax"].Value),
            };

            var res = Enum.TryParse(matches.Groups["SwitchMode"].Value, ignoreCase: true, out SwitchMode switchMode);
            if (!res)
            {
                throw new Exception("Could not translate mode value");
            }

            steps.Add(new RebootStep(switchMode, cuboid));
        }

        return steps;
    }

}

internal record RebootStep(SwitchMode SwitchMode, Cuboid Cuboid);

internal enum SwitchMode
{
    On,
    Off
}

internal enum Operation
{
    Add,
    Subtract,
}