using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day17.Model;

internal class CrucibleMap
{
    private readonly int[,] _map;
    private readonly Dictionary<(Position, Direction), bool> _visited;
    private readonly Dictionary<Position, int> _cost;

    public CrucibleMap(int[,] map)
    {
        _map = map;
        _visited = new Dictionary<(Position, Direction), bool>();
        _cost = new Dictionary<Position, int>();
    }

    /// <summary>
    /// Part one calculation
    /// </summary>
    public int CalculateMinimumHeatLoss()
    {
        var origin = new Position(0, 0);
        var destination = new Position(_map.RowLength() - 1, _map.ColLength() - 1);

        return MinimumCost(origin, destination);
    }

    public int MinimumCost(Position origin, Position destination)
    {
        // Init the cost to "infinity"
        for (int row = 0; row < _map.RowLength(); row++)
        {
            for (int col = 0; col < _map.ColLength(); col++)
            {
                var position = new Position(row, col);
                _cost[position] = int.MaxValue;
            }
        }

        // Base condition
        _cost[origin] = 0;

        var maxStraightMoves = 3;

        // Store the position, the current direction and the steps from the last turn as the key
        var queue = new PriorityQueue<(Position Position, Direction CurrentDir, int SameDirSteps), int>();
        queue.Enqueue((origin, Direction.Right, 0), priority: 0);
        queue.Enqueue((origin, Direction.Down, 0), priority: 0);

        while (queue.Count > 0)
        {
            var (currentPos, currentDir, sameDirSteps) = queue.Dequeue();
            _visited[(currentPos, currentDir)] = true;

            foreach (var (adjDir, adjPos) in GetNeighbours(currentPos))
            {
                if (_visited.ContainsKey((adjPos, adjDir)))
                {
                    continue;
                }

                var adjCost = _map[adjPos.Row, adjPos.Col];
                var adjSameDirSteps = adjDir == currentDir
                    ? sameDirSteps + 1
                    : 0;

                if (adjSameDirSteps > maxStraightMoves)
                {
                    continue;
                }

                if (_cost[adjPos] > _cost[currentPos] + adjCost)
                {
                    _cost[adjPos] = _cost[currentPos] + adjCost;

                    queue.Enqueue((adjPos, adjDir, adjSameDirSteps), _cost[adjPos]);
                }
            }
        }

        return _cost[destination];
    }

    // TODO: Update generic method
    private IEnumerable<(Direction, Position)> GetNeighbours(Position position)
    {
        var dirIncrements = new Dictionary<Direction, (int RowInc, int ColInc)>()
        {
            { Direction.Up, (-1, 0) },
            { Direction.Down, (1, 0) },
            { Direction.Left, (0, -1) },
            { Direction.Right, (0, 1) },
        };

        foreach (var (direction, (rowInc, colInc)) in dirIncrements)
        {
            var nextPosition = new Position(position.Row + rowInc, position.Col + colInc);
            if (!_map.IsValidPosition(nextPosition))
            {
                continue;
            }

            yield return (direction, nextPosition);
        }
    }
}
