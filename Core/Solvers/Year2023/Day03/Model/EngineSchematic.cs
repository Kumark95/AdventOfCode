using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day03.Model;

internal sealed class EngineSchematic
{
    private readonly char[,] _map;

    private EngineSchematic(char[,] map)
    {
        _map = map;
    }

    public static EngineSchematic Parse(string[] inputLines)
    {
        var rows = inputLines.Length;
        var cols = inputLines[0].Length;

        var map = new char[rows, cols];

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < cols; c++)
            {
                map[r, c] = inputLines[r][c];
            }
        }

        return new EngineSchematic(map);
    }

    public int CalculateTotalPartNumber()
    {
        var result = 0;

        for (int r = 0; r < _map.GetLength(0); r++)
        {
            for (int c = 0; c < _map.GetLength(1); c++)
            {
                var character = _map[r, c];
                if (!IsSymbol(character))
                {
                    continue;
                }

                result += GetPartNumbers(r, c)
                        .Sum();
            }
        }

        return result;
    }

    public int CalculateTotalGearRatio()
    {
        var result = 0;

        for (int r = 0; r < _map.GetLength(0); r++)
        {
            for (int c = 0; c < _map.GetLength(1); c++)
            {
                var character = _map[r, c];
                if (!IsSymbol(character))
                {
                    continue;
                }

                var numbers = GetPartNumbers(r, c);
                if (numbers.Count != 2)
                {
                    continue;
                }

                result += numbers.Aggregate((a, b) => a * b);
            }
        }

        return result;
    }

    private static bool IsSymbol(char character)
    {
        return character != '.' && !char.IsDigit(character);
    }

    private List<int> GetPartNumbers(int row, int col)
    {
        List<int> numbers = [];
        List<Position> visitedDigitPositions = [];

        foreach (var (neighbourPosition, neightbourCharacter) in _map.GetAllNeighbours(row, col, MapConnectivity.EightConnected))
        {
            if (!char.IsDigit(neightbourCharacter) || visitedDigitPositions.Contains(neighbourPosition))
            {
                continue;
            }

            // Build the full number
            List<char> digits = [neightbourCharacter];
            visitedDigitPositions.Add(neighbourPosition);

            var dCol = neighbourPosition.Col + 1;
            while (dCol < _map.GetLength(1))
            {
                var dCharacter = _map[neighbourPosition.Row, dCol];
                if (!char.IsDigit(dCharacter))
                {
                    break;
                }

                visitedDigitPositions.Add(new Position(neighbourPosition.Row, dCol));
                digits.Add(dCharacter);

                dCol++;
            }

            dCol = neighbourPosition.Col - 1;
            while (dCol >= 0)
            {
                var dCharacter = _map[neighbourPosition.Row, dCol];
                if (!char.IsDigit(dCharacter))
                {
                    break;
                }

                visitedDigitPositions.Add(new Position(neighbourPosition.Row, dCol));
                digits.Insert(0, dCharacter);
                dCol--;
            }

            numbers.Add(int.Parse(new string(digits.ToArray())));
        }

        return numbers;
    }
}
