using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Year2021.Day15;

public class Dijkstra
{
    private int[,] Map { get; }
    private bool[,] Visited { get; }
    private Dictionary<Point, int> Cost { get; }

    public Dijkstra(int[,] map)
    {
        Map = map;
        Visited = new bool[map.GetLength(0), map.GetLength(1)];
        Cost = new Dictionary<Point, int>();
    }

    /// <summary>
    /// Checks if a given position is valid in the current map
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool IsValid(Point position)
    {
        if (position.X < 0 || position.X >= Map.GetLength(0) || position.Y < 0 || position.Y >= Map.GetLength(1))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Returns the adjacent nodes to a given position
    /// </summary>
    /// <remarks>
    /// It does not take into account diagonal positions
    /// </remarks>
    /// <param name="position"></param>
    /// <returns></returns>
    private List<Point> Adjacent(Point position)
    {
        var xDirection = new List<int> { 0, 1, 0, -1 };
        var yDirection = new List<int> { 1, 0, -1, 0 };

        var points = new List<Point>();
        for (int i = 0; i < xDirection.Count; i++)
        {
            var adjacent = new Point(position.X + xDirection[i], position.Y + yDirection[i]);
            if (IsValid(adjacent))
            {
                points.Add(adjacent);
            }
        }

        return points;
    }

    /// <summary>
    /// Calculates the minimum cost to reach the destination point
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public int MinimumCost(Point origin, Point destination)
    {
        // Init distance to infinity
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                var position = new Point(i, j);
                Cost[position] = int.MaxValue;  // Simulate infinity
            }
        }

        // Base condition
        Cost[origin] = 0;
        Visited[origin.X, origin.Y] = true;

        //
        var queue = new PriorityQueue<Point, int>();
        queue.Enqueue(origin, 0);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            Visited[current.X, current.Y] = true;

            var adjacentNodes = Adjacent(current);
            foreach (var node in adjacentNodes)
            {
                if (Visited[node.X, node.Y]) continue;

                if (Cost[node] > Cost[current] + Map[node.X, node.Y])
                {
                    Cost[node] = Cost[current] + Map[node.X, node.Y];

                    queue.Enqueue(node, Cost[node]);
                }
            }
        }

        return Cost[destination];
    }
}
