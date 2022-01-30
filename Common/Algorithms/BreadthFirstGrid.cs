using AdventOfCode.Common.Model;

namespace AdventOfCode.Common.Algorithms;

public class BreadthFirstGrid
{
    private static readonly int[] rowDirection = new int[] { -1, 0, 1, 0 };
    private static readonly int[] colDirection = new int[] { 0, 1, 0, -1 };

    private int[][] Grid { get; init; }
    private bool[][] Visited { get; init; }


    public BreadthFirstGrid(int[][] grid)
    {
        Grid = grid;
        Visited = new bool[Grid.Length][];

        // Init visited
        for (int i = 0; i < Grid.Length; i++)
        {
            Visited[i] = new bool[Grid[0].Length];
        }
    }

    bool IsValid(int row, int col)
    {
        if (row < 0 || col < 0 || row >= Grid.Length || col >= Grid[0].Length)
        {
            return false;
        }

        if (Visited[row][col])
        {
            return false;
        }

        return true;
    }

    public List<int> Traverse(int row, int col, Func<int, bool> exitCondition)
    {
        var result = new List<int>();
        var queue = new Queue<Point>();

        // Mark the starting cell as visited and push it into the queue
        queue.Enqueue(new Point(row, col));
        Visited[row][col] = true;

        while (queue.Count != 0)
        {
            Point position = queue.Dequeue();

            // Traverse adjacent
            for (int i = 0; i < 4; i++)
            {
                int adjx = position.X + rowDirection[i];
                int adjy = position.Y + colDirection[i];

                if (IsValid(adjx, adjy) && !exitCondition(Grid[adjx][adjy]))
                {
                    if (!exitCondition(Grid[adjx][adjy]))
                    {
                        queue.Enqueue(new Point(adjx, adjy));
                        result.Add(Grid[adjx][adjy]);
                    }

                    Visited[adjx][adjy] = true;
                }
            }
        }

        return result;
    }
}
