namespace AdventOfCode.Core.Solvers.Year2023.Day13.Model;

internal class Valley
{
    private readonly char[,] _map;
    private readonly int _colLength;
    private readonly int _rowLength;

    public Valley(char[,] map)
    {
        _map = map;
        _rowLength = map.GetLength(0);
        _colLength = map.GetLength(1);
    }

    /// <summary>
    /// Part one calculation
    /// </summary>
    public int CalculateScore()
    {
        var verticalIndex = FindVerticalReflectionIndex();
        if (verticalIndex is not null)
        {
            return (int)verticalIndex;
        }

        var horizontalIndex = FindHorizontalReflectionIndex();
        if (horizontalIndex is not null)
        {
            return (int)horizontalIndex * 100;
        }

        throw new Exception("Invalid input");
    }

    /// <summary>
    /// Part two calculation
    /// </summary>
    public int FixSmudgeAndCalculateScore()
    {
        // Used to skip the reflexion index found without fixing the smudge
        var initialVerticalIndex = FindVerticalReflectionIndex();
        var initialHorizontalIndex = initialVerticalIndex is null
            ? FindHorizontalReflectionIndex() : null;

        for (var row = 0; row < _rowLength; row++)
        {
            for (var col = 0; col < _colLength; col++)
            {
                // Test each position to find the smudge
                FlipValue(row, col);

                var verticalIndex = FindVerticalReflectionIndex(skipIndex: initialVerticalIndex);
                if (verticalIndex is not null)
                {
                    return (int)verticalIndex;
                }

                var horizontalIndex = FindHorizontalReflectionIndex(skipIndex: initialHorizontalIndex);
                if (horizontalIndex is not null)
                {
                    return (int)horizontalIndex * 100;
                }

                // Restore
                FlipValue(row, col);
            }
        }

        throw new Exception("Invalid input");
    }

    private void FlipValue(int row, int col)
    {
        var currentValue = _map[row, col];
        if (currentValue == '.')
        {
            _map[row, col] = '#';
        }
        else
        {
            _map[row, col] = '.';
        }
    }

    private int? FindVerticalReflectionIndex(int? skipIndex = null)
    {
        var col = 0;
        while (col < _colLength - 1)
        {
            if (skipIndex != col + 1 && IsPerfectVerticalReflection(col + 1))
            {
                return col + 1;
            }

            col++;
        }

        return null;
    }

    private int? FindHorizontalReflectionIndex(int? skipIndex = null)
    {
        var row = 0;

        while (row < _rowLength - 1)
        {
            if (skipIndex != row + 1 && IsPerfectHorizontalReflection(row + 1))
            {
                return row + 1;
            }

            row++;
        }

        return null;
    }

    private bool IsPerfectVerticalReflection(int reflectionIndex)
    {
        var leftCol = reflectionIndex - 1;
        var rightCol = reflectionIndex;
        while (leftCol >= 0 && rightCol < _colLength)
        {
            if (!IsReflectedColumn(leftCol, rightCol))
            {
                return false;
            }

            leftCol--;
            rightCol++;
        }

        return true;
    }

    private bool IsPerfectHorizontalReflection(int reflectionIndex)
    {
        var topRow = reflectionIndex - 1;
        var bottomRow = reflectionIndex;
        while (topRow >= 0 && bottomRow < _rowLength)
        {
            if (!IsReflectedRow(topRow, bottomRow))
            {
                return false;
            }

            topRow--;
            bottomRow++;
        }

        return true;
    }

    private bool IsReflectedColumn(int colA, int colB)
    {
        for (var row = 0; row < _rowLength; row++)
        {
            if (_map[row, colA] != _map[row, colB])
            {
                return false;
            }
        }

        return true;
    }

    private bool IsReflectedRow(int rowA, int rowB)
    {
        for (var col = 0; col < _colLength; col++)
        {
            if (_map[rowA, col] != _map[rowB, col])
            {
                return false;
            }
        }

        return true;
    }
};
