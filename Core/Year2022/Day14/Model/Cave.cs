namespace AdventOfCode.Core.Year2022.Day14.Model;

internal class Cave
{
    public Dictionary<Position, bool> Obstacles { get; init; }
    public Position SandOrigin { get; init; }
    public int SandUnitsAtRest { get; private set; }

    private int BedrockWestLimit { get; init; }
    private int BedrockEastLimit { get; init; }
    private int? FloorDepth { get; init; }

    public Cave(List<Position> obstacles, bool hasFloor)
    {
        SandOrigin = new Position(500, 0);
        SandUnitsAtRest = 0;

        // Calculate the limits of the obstacles
        var xValues = obstacles.Select(p => p.X).ToList();
        var yValues = obstacles.Select(p => p.Y).ToList();

        if (hasFloor)
        {
            BedrockWestLimit = int.MinValue;
            BedrockEastLimit = int.MaxValue;

            var obstacleDepth = obstacles.Select(p => p.Y).Max();
            FloorDepth = obstacleDepth + 2;

            // Add a few more obstacles in the cave floor
            // The floor expansion needs to account for the new sand units that can be at rest
            var floorExpansion = obstacleDepth - 1;
            for (int i = xValues.Min() - floorExpansion; i <= xValues.Max() + floorExpansion; i++)
            {
                var rockFloorPosition = new Position(i, FloorDepth.Value);
                obstacles.Add(rockFloorPosition);
            }
        }
        else
        {
            BedrockWestLimit = xValues.Min();
            BedrockEastLimit = xValues.Max();
            FloorDepth = null;
        }

        Obstacles = obstacles.ToDictionary(keySelector: o => o, elementSelector: o => true);
    }

    public void FallingSand()
    {
        while (true)
        {
            var sandPosition = SandOrigin;

        keepFalling:
            foreach (var nextPosition in sandPosition.PossibleMoves())
            {
                if (IsSandOverflowing(nextPosition))
                {
                    return;
                }

                if (CanMove(nextPosition))
                {
                    sandPosition = nextPosition;
                    goto keepFalling;
                }
            }

            // Cannot move
            AddObstacle(sandPosition);

            if (sandPosition == SandOrigin)
            {
                return;
            }
        }
    }

    private bool IsSandOverflowing(Position sandPosition)
    {
        return !FloorDepth.HasValue && (sandPosition.X < BedrockWestLimit || sandPosition.X > BedrockEastLimit);
    }

    private bool CanMove(Position position)
    {
        if (FloorDepth.HasValue)
        {
            return position.Y < FloorDepth.Value && !Obstacles.ContainsKey(position);
        }
        else
        {
            return !Obstacles.ContainsKey(position);
        }
    }

    private void AddObstacle(Position position)
    {
        Obstacles.Add(position, true);
        SandUnitsAtRest++;
    }
}
