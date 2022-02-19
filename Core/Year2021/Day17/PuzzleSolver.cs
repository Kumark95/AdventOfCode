using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Year2021.Day17.Model;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Year2021.Day17;

[PuzzleName("Trick Shot")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 17;


    /// <summary>
    /// Parse the TargetArea from the input
    /// </summary>
    /// <param name="inputLines"></param>
    /// <returns></returns>
    private static TargetArea Input(string[] inputLines)
    {
        var match = Regex.Match(inputLines[0], @"target area: x=(?<MinX>-?\d+)\.\.(?<MaxX>-?\d+), y=(?<MinY>-?\d+)\.\.(?<MaxY>-?\d+)");

        var minX = int.Parse(match.Groups["MinX"].Value);
        var minY = int.Parse(match.Groups["MinY"].Value);
        var maxX = int.Parse(match.Groups["MaxX"].Value);
        var maxY = int.Parse(match.Groups["MaxY"].Value);

        return new TargetArea(new Point(minX, minY), new Point(maxX, maxY));
    }

    /// <summary>
    /// Find the maximum Y Position reached during launch to the TargetArea.
    /// </summary>
    /// <remarks>
    /// If the initialVelocity does not cause the probe to reach the target, returns null
    /// </remarks>
    /// <param name="targetArea"></param>
    /// <param name="initialPosition"></param>
    /// <param name="initialVelocity"></param>
    /// <returns>The maximum Y position reached or null</returns>
    private static int? FindMaxYPositionReached(TargetArea targetArea, Point initialPosition, Velocity initialVelocity)
    {
        int maxYPosition = int.MinValue;

        var position = initialPosition;
        var velocity = initialVelocity;

        while (true)
        {
            // Position increases based on the velocity
            position = new Point(position.X + velocity.X, position.Y + velocity.Y);

            // X velocity changes by 1 towards 0
            var newXVelocity = velocity.X;
            if (velocity.X > 0) newXVelocity--;
            if (velocity.X < 0) newXVelocity++;

            // Y velocity just decreases by 1
            velocity = new Velocity(newXVelocity, velocity.Y - 1);

            // Update max Y position
            if (position.Y > maxYPosition) maxYPosition = position.Y;

            // Check the Target area
            if (targetArea.IsInArea(position))
            {
                return maxYPosition;
            }

            // Exit if the target area will never be reached
            if (position.X > targetArea.Max.X || position.Y < targetArea.Min.Y)
            {
                break;
            }
        }

        return null;
    }

    public long SolvePartOne(string[] inputLines)
    {
        var targetArea = Input(inputLines);
        var initialPosition = new Point(0, 0);
        var maxReachedY = int.MinValue;

        // The X velocity cannot be higher than the target area
        for (int velX = 1; velX <= targetArea.Max.X; velX++)
        {
            // As the puzzle asks for the highest Y position we can limit the Y velocity to positive values
            // The max Y velocity cannot exceed the min X position of the target area, otherwise it will never land
            for (int velY = 0; velY <= targetArea.Min.X; velY++)
            {
                var maxY = FindMaxYPositionReached(targetArea, initialPosition, new Velocity(velX, velY));
                if (maxY.HasValue && maxY.Value > maxReachedY)
                {
                    maxReachedY = maxY.Value;
                }
            }
        }

        return maxReachedY;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var targetArea = Input(inputLines);
        var initialPosition = new Point(0, 0);
        var counter = 0;

        // The X velocity cannot be higher than the target area
        for (int velX = 1; velX <= targetArea.Max.X; velX++)
        {
            // This time the puzzle ask for all possible solutions
            // The max Y velocity cannot exceed the min X position of the target area, otherwise it will never land
            for (int velY = targetArea.Min.Y; velY <= Math.Abs(targetArea.Min.Y); velY++)
            {
                var maxY = FindMaxYPositionReached(targetArea, initialPosition, new Velocity(velX, velY));
                if (maxY.HasValue)
                {
                    counter++;
                }
            }
        }

        return counter;
    }
}
