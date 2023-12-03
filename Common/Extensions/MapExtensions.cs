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

    public static IEnumerable<(Position pos, T value)> GetAllNeighbours<T>(this T[,] map, int row, int col, MapConnectivity connectivity)
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
}
