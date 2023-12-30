using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Model;
using System.Diagnostics;

namespace AdventOfCode.Core.Solvers.Year2023.Day17.Model;

internal class CrucibleMap
{
    private readonly int[,] _map;
    private readonly Dictionary<MapState, int> _cost;

    private readonly record struct MapState(Position Pos, Direction Dir, int SameDirSteps);

    public CrucibleMap(int[,] map)
    {
        _map = map;
        _cost = new Dictionary<MapState, int>();
    }

    public int CalculateMinimumHeatLoss(int minStraightMoves, int maxStraightMoves)
    {
        var origin = new Position(0, 0);
        var destination = new Position(_map.RowLength() - 1, _map.ColLength() - 1);

        return MinimumCost(origin, destination, minStraightMoves, maxStraightMoves);
    }

    public int MinimumCost(Position origin, Position destination, int minStraightMoves, int maxStraightMoves)
    {
        // Base condition
        var startRightState = new MapState(origin, Direction.Right, 0);
        _cost[startRightState] = 0;

        // Store the position, the current direction and the steps from the last turn as the key
        var queue = new PriorityQueue<MapState, int>();
        queue.Enqueue(startRightState, priority: 0);

        while (queue.Count > 0)
        {
            var currentState = queue.Dequeue();
            if (currentState.Pos == destination)
            {
                return _cost[currentState];
            }

            foreach (var adjState in GetAdjacentStates(currentState, minStraightMoves, maxStraightMoves))
            {
                var adjCost = _map[adjState.Pos.Row, adjState.Pos.Col];
                if (!_cost.ContainsKey(adjState) || _cost[adjState] > _cost[currentState] + adjCost)
                {
                    _cost[adjState] = _cost[currentState] + adjCost;

                    queue.Enqueue(adjState, _cost[adjState]);
                }
            }
        }

        throw new UnreachableException();
    }

    private IEnumerable<MapState> GetAdjacentStates(MapState state, int minStraightMoves, int maxStraightMoves)
    {
        var dirIncrements = new Dictionary<Direction, (int RowInc, int ColInc)>()
        {
            { Direction.Up, (-1, 0) },
            { Direction.Down, (1, 0) },
            { Direction.Left, (0, -1) },
            { Direction.Right, (0, 1) },
        };

        var inverseDirections = new Dictionary<Direction, Direction>()
        {
            { Direction.Up, Direction.Down },
            { Direction.Down, Direction.Up },
            { Direction.Left, Direction.Right},
            { Direction.Right, Direction.Left },
        };

        // TODO: Refactor
        foreach (var (adjDirection, (rowInc, colInc)) in dirIncrements)
        {
            if (adjDirection == inverseDirections[state.Dir])
            {
                continue;
            }

            var adjSameDirSteps = state.Dir == adjDirection
                    ? state.SameDirSteps + 1
                    : 1;

            if (state.Dir == adjDirection)
            {
                // Go straight
                if (adjSameDirSteps > maxStraightMoves)
                {
                    continue;
                }
            }
            else
            {
                // Go to the sides
                if (state.SameDirSteps < minStraightMoves)
                {
                    continue;
                }
            }

            var adjPosition = new Position(state.Pos.Row + rowInc, state.Pos.Col + colInc);
            if (!_map.IsValidPosition(adjPosition))
            {
                continue;
            }

            yield return new MapState(adjPosition, adjDirection, adjSameDirSteps);
        }
    }
}
