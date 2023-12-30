using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day18.Model;

internal class DigPlan
{
    private readonly DigInstruction[] _instructions;

    public DigPlan(DigInstruction[] instructions)
    {
        _instructions = instructions;
    }

    public long CalculateLagoonCapacity()
    {
        var trenchLength = 0L;
        var prevPosition = new LongPosition(0, 0);
        var positions = new List<LongPosition> { prevPosition };

        foreach (var instruction in _instructions)
        {
            var newPosition = prevPosition + (LongPosition)instruction.Direction.DirectionIncrement() * instruction.Steps;
            positions.Add(newPosition);

            prevPosition = newPosition;
            trenchLength += instruction.Steps;
        }

        var polygon = new Polygon(positions);

        // We need to add the border of the trench
        return (long)polygon.CalculateArea() + trenchLength / 2 + 1;
    }
}
