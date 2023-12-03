using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2021.Day25.Model;

internal class SeaFloor
{
    private char[,] Map { get; set; }
    private int FloorRowLength { get; init; }
    private int FloorColLength { get; init; }

    private const char EastCucumber = '>';
    private const char SouthCucumber = 'v';
    private const char Empty = '.';

    public SeaFloor(string[] inputLines)
    {
        FloorRowLength = inputLines.Length;
        FloorColLength = inputLines[0].Length;
        Map = new char[FloorRowLength, FloorColLength];

        for (int row = 0; row < FloorRowLength; row++)
        {
            var line = inputLines[row];

            for (int col = 0; col < FloorColLength; col++)
            {
                Map[row, col] = line[col];
            }
        }
    }

    /// <summary>
    /// Return the number of steps needed until all the cucumbers stop moving
    /// </summary>
    /// <returns></returns>
    public long StepsUntilStop()
    {
        var steps = 0;

        while (true)
        {
            steps++;

            var movedCucumbers = MoveEastHerd() + MoveSouthHerd();
            if (movedCucumbers == 0)
            {
                break;
            }
        }

        return steps;
    }

    /// <summary>
    /// Move the east-facing herd of cucumbers
    /// </summary>
    /// <returns></returns>
    private int MoveEastHerd()
    {
        var movedCucumbers = 0;

        for (int row = 0; row < FloorRowLength; row++)
        {
            // Used to check the original value when a wrap around happens
            var wrappedOriginalValue = Map[row, 0];

            for (int col = 0; col < FloorColLength; col++)
            {
                var value = Map[row, col];
                if (value != EastCucumber)
                {
                    continue;
                }

                var targetPosition = CalculateTargetPosition(row, col, isFromEast: true);
                var targetValue = Map[targetPosition.Row, targetPosition.Col];
                if (targetValue == Empty)
                {
                    if (targetPosition.Col == 0 && wrappedOriginalValue != Empty)
                    {
                        continue;
                    }

                    Map[row, col] = Empty;
                    Map[targetPosition.Row, targetPosition.Col] = EastCucumber;

                    col++; // Skip the newly moved cucumber

                    movedCucumbers++;
                }
            }
        }

        return movedCucumbers;
    }

    /// <summary>
    /// Move the south-facing herd of cucumbers
    /// </summary>
    /// <returns></returns>
    private int MoveSouthHerd()
    {
        var movedCucumbers = 0;

        // Iterate with columns first (following the herd direction)
        for (int col = 0; col < FloorColLength; col++)
        {
            // Used to check the original value when a wrap around happens
            var wrappedOriginalValue = Map[0, col];

            for (int row = 0; row < FloorRowLength; row++)
            {
                var value = Map[row, col];
                if (value != SouthCucumber)
                {
                    continue;
                }

                var targetPosition = CalculateTargetPosition(row, col, isFromEast: false);
                var targetValue = Map[targetPosition.Row, targetPosition.Col];
                if (targetValue == Empty)
                {
                    if (targetPosition.Row == 0 && wrappedOriginalValue != Empty)
                    {
                        continue;
                    }

                    Map[row, col] = Empty;
                    Map[targetPosition.Row, targetPosition.Col] = SouthCucumber;

                    row++; // Skip the newly moved cucumber

                    movedCucumbers++;
                }
            }
        }

        return movedCucumbers;
    }

    /// <summary>
    /// Calculates the target position
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="isFromEast"></param>
    /// <remarks>
    /// Wraps around the bound of the map
    /// </remarks>
    /// <returns></returns>
    private Position CalculateTargetPosition(int row, int col, bool isFromEast)
    {
        return isFromEast
            ? new Position(row, (col + 1) % FloorColLength)
            : new Position((row + 1) % FloorRowLength, col);
    }
}

