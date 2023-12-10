using AdventOfCode.Common.Model;

namespace AdventOfCode.Common.Extensions;

public static class MapExtensions
{
    public static bool IsValidPosition<T>(this T[,] map, int row, int col)
    {
        return row >= 0
            && row < map.GetLength(0)
            && col >= 0
            && col < map.GetLength(1);
    }

    public static bool IsValidPosition<T>(this T[,] map, Position position)
    {
        return position.Row >= 0
            && position.Row < map.GetLength(0)
            && position.Col >= 0
            && position.Col < map.GetLength(1);
    }

    public static IEnumerable<(Position Position, T Value)> GetAllNeighbours<T>(this T[,] map, int row, int col, MapConnectivity connectivity)
    {
        int[] rowDirection;
        int[] colDirection;

        if (connectivity == MapConnectivity.EightConnected)
        {
            rowDirection = [-1, 0, 1, 0, -1, 1, -1, 1];
            colDirection = [0, 1, 0, -1, -1, -1, 1, 1];
        }
        else
        {
            rowDirection = [-1, 0, 1, 0];
            colDirection = [0, 1, 0, -1];
        }

        for (int i = 0; i < rowDirection.Length; i++)
        {
            var adjRow = row + rowDirection[i];
            var adjCol = col + colDirection[i];
            if (!map.IsValidPosition(adjRow, adjCol))
            {
                continue;
            }

            var adjCharacter = map[adjRow, adjCol];
            yield return (new Position(adjRow, adjCol), adjCharacter);
        }
    }

    public static IEnumerable<(Position Position, T Value)> GetAllNeighbours<T>(this T[,] map, Position position, MapConnectivity connectivity)
    {
        int[] rowDirection;
        int[] colDirection;

        if (connectivity == MapConnectivity.EightConnected)
        {
            rowDirection = [-1, 0, 1, 0, -1, 1, -1, 1];
            colDirection = [0, 1, 0, -1, -1, -1, 1, 1];
        }
        else
        {
            rowDirection = [-1, 0, 1, 0];
            colDirection = [0, 1, 0, -1];
        }

        for (int i = 0; i < rowDirection.Length; i++)
        {
            var adjPosition = new Position(position.Row + rowDirection[i], position.Col + colDirection[i]);
            if (!map.IsValidPosition(adjPosition))
            {
                continue;
            }

            var adjCharacter = map[adjPosition.Row, adjPosition.Col];
            yield return (adjPosition, adjCharacter);
        }
    }
}
