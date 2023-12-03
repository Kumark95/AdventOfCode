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

                result += GetAdjacentNumbers(r, c)
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

                var numbers = GetAdjacentNumbers(r, c);
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

    private List<int> GetAdjacentNumbers(int row, int col)
    {
        int[] rowDirection = [-1, 0, 1, 0, -1, 1, -1, 1];
        int[] colDirection = [0, 1, 0, -1, -1, -1, 1, 1];

        List<int> numbers = [];
        List<Position> visited = [];

        for (int i = 0; i < rowDirection.Length; i++)
        {
            var adjRow = row + rowDirection[i];
            var adjCol = col + colDirection[i];
            if (adjRow < 0 || adjCol < 0 || adjRow >= _map.GetLength(0) || adjCol >= _map.GetLength(0))
            {
                continue;
            }

            var adjCharacter = _map[adjRow, adjCol];
            if (!char.IsDigit(adjCharacter))
            {
                continue;
            }

            var adjPosition = new Position(adjRow, adjCol);
            if (visited.Contains(adjPosition))
            {
                continue;
            }

            // Build the number
            List<char> digits = [adjCharacter];
            visited.Add(adjPosition);

            var dCol = adjCol + 1;
            while (dCol < _map.GetLength(1))
            {
                var dCharacter = _map[adjRow, dCol];
                if (!char.IsDigit(dCharacter))
                {
                    break;
                }

                visited.Add(new Position(adjRow, dCol));
                digits.Add(dCharacter);

                dCol++;
            }

            dCol = adjCol - 1;
            while (dCol >= 0)
            {
                var dCharacter = _map[adjRow, dCol];
                if (!char.IsDigit(dCharacter))
                {
                    break;
                }

                visited.Add(new Position(adjRow, dCol));
                digits.Insert(0, dCharacter);
                dCol--;
            }

            numbers.Add(int.Parse(new string(digits.ToArray())));
        }

        return numbers;
    }
}
