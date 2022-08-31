using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Year2021.Day25.Model;

internal class SeaFloor
{
    private char[,] Map { get; set; }
    private int FloorRowLength { get; init; }
    private int FloorColLength { get; init; }

    private List<MatrixPosition> EastCucumberPositions { get; set; }
    private List<MatrixPosition> SouthCucumberPositions { get; set; }

    private const char EastCucumber = '>';
    private const char SouthCucumber = 'v';
    private const char Empty = '.';

    public SeaFloor(string[] inputLines)
    {
        FloorRowLength = inputLines.Length;
        FloorColLength = inputLines[0].Length;
        Map = new char[FloorRowLength, FloorColLength];

        EastCucumberPositions = new List<MatrixPosition>();
        SouthCucumberPositions = new List<MatrixPosition>();

        for (int row = 0; row < FloorRowLength; row++)
        {
            var line = inputLines[row];

            for (int col = 0; col < FloorColLength; col++)
            {
                var character = line[col];
                Map[row, col] = character;

                if (character == EastCucumber)
                {
                    EastCucumberPositions.Add(new MatrixPosition(row, col));
                }
                else if (character == SouthCucumber)
                {
                    SouthCucumberPositions.Add(new MatrixPosition(row, col));
                }
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

            var hasMovement = false;

            // Need a snapshot of the map as the movement check needs to be performed simultaneously
            var mapSnapshot = (char[,])Map.Clone();

            // The east cucumber herd have priority
            // TODO: Optimize
            var currentEastCucumbers = EastCucumberPositions
                .OrderByDescending(c => c.Row)
                    .ThenBy(c => c.Col)
                .ToList();

            foreach (var currentPosition in currentEastCucumbers)
            {
                var (canMove, newPosition) = TryToMove(currentPosition, mapSnapshot);
                if (!canMove)
                {
                    continue;
                }

                hasMovement = true;
                Map[currentPosition.Row, currentPosition.Col] = Empty;
                Map[newPosition.Value.Row, newPosition.Value.Col] = EastCucumber;

                EastCucumberPositions.Remove(currentPosition);
                EastCucumberPositions.Add(newPosition.Value);
            }

            // South cucumbers
            mapSnapshot = (char[,])Map.Clone();

            var currentSouthCucumbers = SouthCucumberPositions
                .OrderByDescending(c => c.Row)
                    .ThenBy(c => c.Col)
                .ToList();

            foreach (var currentPosition in currentSouthCucumbers)
            {
                var (canMove, newPosition) = TryToMove(currentPosition, mapSnapshot);
                if (!canMove)
                {
                    continue;
                }

                hasMovement = true;
                Map[currentPosition.Row, currentPosition.Col] = Empty;
                Map[newPosition.Value.Row, newPosition.Value.Col] = SouthCucumber;

                SouthCucumberPositions.Remove(currentPosition);
                SouthCucumberPositions.Add(newPosition.Value);
            }

            if (!hasMovement)
            {
                break;
            }
        }

        return steps;
    }

    /// <summary>
    /// Tries to move to the next position
    /// </summary>
    /// <param name="initialPosition"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    /// <remarks>
    /// The position depends on the type of cucumber
    /// </remarks>
    /// <exception cref="Exception"></exception>
    public (bool result, MatrixPosition? newPosition) TryToMove(MatrixPosition initialPosition, char[,] map)
    {
        var cucumber = map[initialPosition.Row, initialPosition.Col];

        var targetPosition = cucumber switch
        {
            EastCucumber => initialPosition with { Col = (initialPosition.Col + 1) % FloorColLength },
            SouthCucumber => initialPosition with { Row = (initialPosition.Row + 1) % FloorRowLength },
            _ => throw new Exception("Not a cucumber")
        };

        var canMove = map[targetPosition.Row, targetPosition.Col] == Empty;
        return (canMove, canMove ? targetPosition : null);
    }
}

