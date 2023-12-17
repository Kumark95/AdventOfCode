using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Model;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Core.Solvers.Year2023.Day14.Model;

public class ReflectorDish
{
    private const char EMPTY_SPACE = '.';
    private const char ROUNDED_ROCK = 'O';
    private const char CUBE_ROCK = '#';

    private readonly char[,] _map;
    private readonly int _colLength;
    private readonly int _rowLength;

    public ReflectorDish(char[,] map)
    {
        _map = map;
        _rowLength = map.GetLength(0);
        _colLength = map.GetLength(1);
    }

    public void Tilt(Direction direction)
    {
        if (direction == Direction.Up || direction == Direction.Left)
        {
            for (var row = 0; row < _rowLength; row++)
            {
                for (var col = 0; col < _colLength; col++)
                {
                    var val = _map[row, col];
                    if (val == ROUNDED_ROCK)
                    {
                        MoveRoundedRock(row, col, direction);
                    }
                }
            }
        }
        else if (direction == Direction.Down || direction == Direction.Right)
        {
            for (var row = _rowLength - 1; row >= 0; row--)
            {
                for (var col = _colLength - 1; col >= 0; col--)
                {
                    var val = _map[row, col];
                    if (val == ROUNDED_ROCK)
                    {
                        MoveRoundedRock(row, col, direction);
                    }
                }
            }
        }
        else
        {
            throw new InvalidOperationException("Invalid direction");
        }
    }

    public void TiltCycle()
    {
        Tilt(Direction.Up);
        Tilt(Direction.Left);
        Tilt(Direction.Down);
        Tilt(Direction.Right);
    }

    public int CalculateTotalLoad()
    {
        var total = 0;
        for (var row = 0; row < _rowLength; row++)
        {
            for (var col = 0; col < _colLength; col++)
            {
                var item = _map[row, col];
                if (item == ROUNDED_ROCK)
                {
                    total += _rowLength - row;
                }
            }
        }

        return total;
    }

    public (int StartIndex, int LoopLength) CalculateLoopCycle()
    {
        var lastFound = new Dictionary<string, int>();

        var currentCycle = 1;

        while (true)
        {
            TiltCycle();

            var mapHash = CalculateMapFingerprint();
            if (lastFound.TryGetValue(mapHash, out var previousFoundCycle))
            {
                return (previousFoundCycle, currentCycle - previousFoundCycle);
            }

            lastFound.Add(mapHash, currentCycle);

            currentCycle++;
        }
    }

    private string CalculateMapFingerprint()
    {
        var stringBuilder = new StringBuilder();
        for (int row = 0; row < _rowLength; row++)
        {
            for (int col = 0; col < _colLength; col++)
            {
                stringBuilder.Append(_map[row, col]);
            }
        }

        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    private void MoveRoundedRock(int row, int col, Direction direction)
    {
        (int nextRow, int nextCol) = direction switch
        {
            Direction.Up => (row - 1, col),
            Direction.Down => (row + 1, col),
            Direction.Left => (row, col - 1),
            Direction.Right => (row, col + 1),
            _ => throw new InvalidOperationException("Invalid direction")
        };

        if (!_map.IsValidPosition(nextRow, nextCol))
        {
            return;
        }

        var nextValue = _map[nextRow, nextCol];
        if (nextValue == EMPTY_SPACE)
        {
            var currentValue = _map[row, col];

            // Swap
            _map[row, col] = EMPTY_SPACE;
            _map[nextRow, nextCol] = currentValue;

            // Continue moving
            MoveRoundedRock(nextRow, nextCol, direction);
        }
    }

#pragma warning disable IDE0051 // Supress "Remove unused private member" warning
    private void Print()
    {
        for (var row = 0; row < _rowLength; row++)
        {
            for (var col = 0; col < _colLength; col++)
            {
                var item = _map[row, col];
                if (item == CUBE_ROCK)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (item == ROUNDED_ROCK)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }

                Console.Write(item);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
        }
    }
#pragma warning restore IDE0051
}
