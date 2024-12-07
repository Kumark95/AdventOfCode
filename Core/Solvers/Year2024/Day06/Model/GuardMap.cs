using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day06.Model;

internal class GuardMap
{
    private readonly char[,] _map;
    private readonly Position _guardInitialPosition;

    public GuardMap(char[,] map, Position guardInitialPosition)
    {
        _guardInitialPosition = guardInitialPosition;
        _map = map;
    }

    public HashSet<Position> FindGuardRoutePositions()
    {
        var workMap = _map.DeepClone();

        var currentPosition = _guardInitialPosition;
        var visitedPositions = new HashSet<Position>();

        while (true)
        {
            visitedPositions.Add(currentPosition);

            var guardOrientationChar = workMap.GetValueAt(currentPosition);

            Position nextPosition = guardOrientationChar switch
            {
                '^' => workMap.Move(currentPosition, Direction.Up),
                '>' => workMap.Move(currentPosition, Direction.Right),
                'v' => workMap.Move(currentPosition, Direction.Down),
                '<' => workMap.Move(currentPosition, Direction.Left),
                _ => throw new NotImplementedException()
            };

            // Exit map
            if (!workMap.IsValidPosition(nextPosition))
            {
                break;
            }

            var nextCharacter = workMap.GetValueAt(nextPosition);
            if (nextCharacter == '#')
            {
                var nextOrientation = Rotate90Degrees(guardOrientationChar);
                workMap.SetValueAt(currentPosition, nextOrientation);
                continue;
            }
            else
            {
                // Move straight
                workMap.SetValueAt(nextPosition, guardOrientationChar);
                workMap.SetValueAt(currentPosition, '.');

                currentPosition = nextPosition;
            }
        }

        return visitedPositions;
    }

    public bool HasInfiniteLoop(Position newObstaclePosition)
    {
        var workMap = _map.DeepClone();
        workMap.SetValueAt(newObstaclePosition, '#');

        var currentPosition = _guardInitialPosition;

        HashSet<(Position, char)> obstacleVisits = new();

        while (true)
        {
            var guardOrientation = workMap.GetValueAt(currentPosition);

            Position nextPosition = guardOrientation switch
            {
                '^' => workMap.Move(currentPosition, Direction.Up),
                '>' => workMap.Move(currentPosition, Direction.Right),
                'v' => workMap.Move(currentPosition, Direction.Down),
                '<' => workMap.Move(currentPosition, Direction.Left),
                _ => throw new NotImplementedException()
            };

            // Exit map
            if (!workMap.IsValidPosition(nextPosition))
            {
                return false;
            }

            var nextCharacter = workMap.GetValueAt(nextPosition);
            if (nextCharacter == '#')
            {
                // An infinite loop happens when we arrive to the same obstacle from the same direction
                var posOrientation = (currentPosition, guardOrientation);
                if (obstacleVisits.Contains(posOrientation))
                {
                    return true;
                }
                obstacleVisits.Add(posOrientation);

                var nextOrientation = Rotate90Degrees(guardOrientation);
                workMap.SetValueAt(currentPosition, nextOrientation);
                continue;
            }
            else
            {
                // Move straight
                workMap.SetValueAt(nextPosition, guardOrientation);
                workMap.SetValueAt(currentPosition, '.');

                currentPosition = nextPosition;
            }
        }
    }

    private static char Rotate90Degrees(char orientation)
    {
        return orientation switch
        {
            '^' => '>',
            '>' => 'v',
            'v' => '<',
            '<' => '^',
            _ => throw new Exception("Invalid orientation")
        };
    }
}
