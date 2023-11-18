namespace AdventOfCode.Core.Solvers.Year2022.Day12.Model;

internal class Maze
{
    private static readonly int[] _rowDirection = new int[] { 1, 0, -1, 0 };
    private static readonly int[] _colDirection = new int[] { 0, 1, 0, -1 };

    public char[,] Grid { get; init; }
    private bool[,] Visited { get; set; }

    public GridPosition StartPosition { get; init; }
    public GridPosition EndPosition { get; init; }
    public List<GridPosition> LowPositions { get; set; }

    private Maze(char[,] grid, GridPosition start, GridPosition end, List<GridPosition> lowPositions)
    {
        Grid = grid;
        StartPosition = start;
        EndPosition = end;
        Visited = new bool[grid.GetLength(0), grid.GetLength(1)];
        LowPositions = lowPositions;
    }

    public static Maze Parse(string[] inputLines)
    {
        var rowDimension = inputLines.Length;
        var colDimension = inputLines[0].Length;

        var grid = new char[rowDimension, colDimension];
        GridPosition? start = null;
        GridPosition? end = null;
        var lowPositions = new List<GridPosition>();

        for (int row = 0; row < rowDimension; row++)
        {
            for (int col = 0; col < colDimension; col++)
            {
                var character = inputLines[row][col];
                var position = new GridPosition(row, col);

                // Replace the start ('S') and end ('E') characters to easily check for the height difference
                if (character == 'S')
                {
                    start = position;
                    character = 'a';
                }
                if (character == 'E')
                {
                    end = position;
                    character = 'z';
                }
                if (character == 'a')
                {
                    lowPositions.Add(position);
                }

                grid[row, col] = character;
            }
        }

        if (!start.HasValue || !end.HasValue)
        {
            throw new Exception("Start and end characters are not present");
        }

        return new Maze(grid, start.Value, end.Value, lowPositions);
    }

    public void Reset()
    {
        Visited = new bool[Grid.GetLength(0), Grid.GetLength(1)];
    }

    private bool IsValid(GridPosition position)
    {
        if (position.Row < 0 || position.Col < 0 || position.Row >= Grid.GetLength(0) || position.Col >= Grid.GetLength(1))
        {
            return false;
        }

        return true;
    }

    private List<GridPosition> Neighbours(GridPosition position)
    {
        var list = new List<GridPosition>();
        for (int i = 0; i < 4; i++)
        {
            var neighbourPosition = new GridPosition(position.Row + _rowDirection[i], position.Col + _colDirection[i]);
            if (IsValid(neighbourPosition))
            {
                list.Add(neighbourPosition);
            }
        }

        return list;
    }

    private bool CanClimb(GridPosition current, GridPosition target)
    {
        var currentElevation = Grid[current.Row, current.Col];
        var targetElevation = Grid[target.Row, target.Col];

        // Target can be at most one position above
        return targetElevation <= currentElevation + 1;
    }

    public int FindShortestDistance(GridPosition start, GridPosition end)
    {
        var queue = new Queue<(GridPosition position, int distance)>();
        queue.Enqueue((start, 0));
        Visited[start.Row, start.Col] = true;

        while (queue.Count != 0)
        {
            var (currentPosition, currentDistance) = queue.Dequeue();
            if (currentPosition == end)
            {
                return currentDistance;
            }

            // Traverse neightbours
            foreach (var neighbourPosition in Neighbours(currentPosition))
            {
                if (!Visited[neighbourPosition.Row, neighbourPosition.Col]
                    && CanClimb(currentPosition, neighbourPosition))
                {
                    queue.Enqueue((neighbourPosition, currentDistance + 1));
                    Visited[neighbourPosition.Row, neighbourPosition.Col] = true;
                }
            }
        }

        return -1;
    }
}

