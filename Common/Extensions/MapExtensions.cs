using AdventOfCode.Common.Model;

namespace AdventOfCode.Common.Extensions;

public static class MapExtensions
{
    public static int RowLength<T>(this T[,] map) => map.GetLength(0);
    public static int ColLength<T>(this T[,] map) => map.GetLength(1);

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

    public static T GetValueAt<T>(this T[,] map, Position position)
    {
        return map[position.Row, position.Col];
    }

    public static void SetValueAt<T>(this T[,] map, Position position, T value)
    {
        map[position.Row, position.Col] = value;
    }

    public static Position Move<T>(this T[,] _, Position position, Direction direction)
    {
        return direction switch
        {
            Direction.Up => position with { Row = position.Row - 1 },
            Direction.Down => position with { Row = position.Row + 1 },
            Direction.Left => position with { Col = position.Col - 1 },
            Direction.Right => position with { Col = position.Col + 1 },
            _ => throw new InvalidOperationException()
        };
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

    public static IEnumerable<Position> GetAllEdgePositions<T>(this T[,] map)
    {
        var rowLength = map.RowLength();
        var colLength = map.ColLength();

        for (int col = 0; col < colLength; col++)
        {
            // Return top and bottom rows
            yield return new Position(0, col);
            yield return new Position(rowLength - 1, col);
        }

        // Skip the first and last rows
        for (int row = 1; row < rowLength - 1; row++)
        {
            yield return new Position(row, 0);
            yield return new Position(row, colLength - 1);
        }
    }

    public static T[,] DeepClone<T>(this T[,] map)
    {
        var clone = new T[map.GetLength(0), map.GetLength(1)];

        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                clone[row, col] = map[row, col];
            }
        }

        return clone;
    }

    public static void Print<T>(this T[,] map, Dictionary<T, ConsoleColor> colors) where T : notnull
    {
        for (int row = 0; row < map.RowLength(); row++)
        {
            for (int col = 0; col < map.ColLength(); col++)
            {
                var character = map[row, col];

                if (colors.TryGetValue(character, out ConsoleColor color))
                {
                    Console.ForegroundColor = color;
                    Console.Write(character);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(character);
                }
            }

            Console.WriteLine();
        }
    }

    public static void Print<T>(this T[,] map, HashSet<Position> coloredPositions)
    {
        for (int row = 0; row < map.RowLength(); row++)
        {
            for (int col = 0; col < map.ColLength(); col++)
            {
                var character = map[row, col];

                if (coloredPositions.Contains(new Position(row, col)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(character);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(character);
                }
            }

            Console.WriteLine();
        }
    }
}
