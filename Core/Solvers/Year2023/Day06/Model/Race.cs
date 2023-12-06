namespace AdventOfCode.Core.Solvers.Year2023.Day06.Model;

internal record struct Race(long Time, long RecordDistance)
{
    public readonly long CalculateTotalRecordBeatingWays()
    {
        // Quadratic equation.
        // Note: 0.1 added to get a value just above the record distance
        var discriminant = Math.Pow(Time, 2) - 4 * (RecordDistance + 0.1);
        var maxChargeTime = (long)Math.Floor((Time + Math.Sqrt(discriminant)) / 2.0);
        var minChargeTime = (long)Math.Ceiling((Time - Math.Sqrt(discriminant)) / 2.0);

        return maxChargeTime - minChargeTime + 1;
    }
};
