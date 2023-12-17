using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day16.Model;

internal class LightContraption
{
    private readonly char[,] _map;
    private Dictionary<(Position Position, Direction Direction), bool> _visited;

    public LightContraption(char[,] map)
    {
        _map = map;
        _visited = new();
    }

    private void ResetVisitedPositions()
    {
        _visited = new();
    }

    /// <summary>
    /// Part one calculation
    /// </summary>
    public int CalculateEnergizedPositions(Position initialPosition, Direction initialDirection)
    {
        ResetVisitedPositions();

        var initialValue = _map[initialPosition.Row, initialPosition.Col];

        var correctedInitialDirection = (initialValue, initialDirection) switch
        {
            ('/', Direction.Right) => Direction.Up,
            ('/', Direction.Left) => Direction.Down,
            ('/', Direction.Down) => Direction.Left,
            ('/', Direction.Up) => Direction.Right,

            ('\\', Direction.Right) => Direction.Down,
            ('\\', Direction.Left) => Direction.Up,
            ('\\', Direction.Down) => Direction.Right,
            ('\\', Direction.Up) => Direction.Left,

            _ => initialDirection,
        };

        FollowLight(initialPosition, correctedInitialDirection);

        return _visited.Keys
            .Select(k => k.Position)
            .Distinct()
            .Count();
    }

    /// <summary>
    /// Part two calculation
    /// </summary>
    public int CalculateMaxEnergizedPositions()
    {
        var maxExergizedPositions = 0;

        foreach (var initialPosition in _map.GetAllEdgePositions())
        {
            if (initialPosition.Row == 0)
            {
                maxExergizedPositions = Math.Max(maxExergizedPositions, CalculateEnergizedPositions(initialPosition, Direction.Down));
            }
            else if (initialPosition.Row == _map.RowLength() - 1)
            {
                maxExergizedPositions = Math.Max(maxExergizedPositions, CalculateEnergizedPositions(initialPosition, Direction.Up));
            }

            if (initialPosition.Col == 0)
            {
                maxExergizedPositions = Math.Max(maxExergizedPositions, CalculateEnergizedPositions(initialPosition, Direction.Right));
            }
            else if (initialPosition.Col == _map.ColLength() - 1)
            {
                maxExergizedPositions = Math.Max(maxExergizedPositions, CalculateEnergizedPositions(initialPosition, Direction.Left));
            }
        }

        return maxExergizedPositions;
    }

    public void FollowLight(Position position, Direction direction)
    {
        if (!_map.IsValidPosition(position) || _visited.ContainsKey((position, direction)))
        {
            return;
        }
        _visited[(position, direction)] = true;

        // Move
        Position nextPosition = _map.Move(position, direction);
        if (!_map.IsValidPosition(nextPosition))
        {
            return;
        }

        var nextItem = _map[nextPosition.Row, nextPosition.Col];
        if (nextItem == '|' && (direction == Direction.Left || direction == Direction.Right))
        {
            FollowLight(nextPosition, Direction.Up);
            FollowLight(nextPosition, Direction.Down);
        }
        else if (nextItem == '-' && (direction == Direction.Up || direction == Direction.Down))
        {
            FollowLight(nextPosition, Direction.Left);
            FollowLight(nextPosition, Direction.Right);
        }
        else if (nextItem == '/')
        {
            if (direction == Direction.Up)
            {
                FollowLight(nextPosition, Direction.Right);
            }
            else if (direction == Direction.Down)
            {
                FollowLight(nextPosition, Direction.Left);
            }
            else if (direction == Direction.Left)
            {
                FollowLight(nextPosition, Direction.Down);
            }
            else if (direction == Direction.Right)
            {
                FollowLight(nextPosition, Direction.Up);
            }
        }
        else if (nextItem == '\\')
        {
            if (direction == Direction.Up)
            {
                FollowLight(nextPosition, Direction.Left);
            }
            else if (direction == Direction.Down)
            {
                FollowLight(nextPosition, Direction.Right);
            }
            else if (direction == Direction.Left)
            {
                FollowLight(nextPosition, Direction.Up);
            }
            else if (direction == Direction.Right)
            {
                FollowLight(nextPosition, Direction.Down);
            }
        }
        else
        {
            // Continue moving in the same direction
            FollowLight(nextPosition, direction);
        }
    }
}
