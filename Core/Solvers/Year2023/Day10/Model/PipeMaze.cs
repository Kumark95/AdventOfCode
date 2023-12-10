using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day10.Model;

internal class PipeMaze
{
    private readonly char[,] _map;
    private readonly Position _startPosition;
    private readonly Position[] _pipeLoop;
    private readonly Dictionary<char, Position[]> _directionIncrements = new()
    {
        { '|', [new Position(1, 0), new Position(-1, 0)] },
        { '-', [new Position(0, 1), new Position(0, -1)] },
        { 'L', [new Position(0, 1), new Position(-1, 0)] },
        { 'J', [new Position(0, -1), new Position(-1, 0)] },
        { '7', [new Position(1, 0), new Position(0, -1)] },
        { 'F', [new Position(1, 0), new Position(0, 1)] },
    };

    public PipeMaze(char[,] map, Position startPosition)
    {
        _map = map;
        _startPosition = startPosition;

        ReplaceStartCharacter();
        _pipeLoop = GetLoopPositions();
    }

    public void Print()
    {
        var polygonVertices = _pipeLoop
            .Where(pos =>
            {
                var val = _map[pos.Row, pos.Col];
                return val != '|' && val != '-';
            })
            .ToArray();

        for (var row = 0; row < _map.GetLength(0); row++)
        {
            for (var col = 0; col < _map.GetLength(1); col++)
            {
                var character = _map[row, col];
                var position = new Position(row, col);

                if (_pipeLoop.Contains(position))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (IsTileInsidePolygon(position, polygonVertices))
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.Write(character);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine();
        }
    }

    public int CalculateMaxLoopDistance()
    {
        return _pipeLoop.Length / 2;
    }

    public int CalculateTotalEnclosedTiles()
    {
        var total = 0;

        var polygonVertices = _pipeLoop
            .Where(pos =>
            {
                var val = _map[pos.Row, pos.Col];
                return val != '|' && val != '-';
            })
            .ToArray();

        for (var row = 0; row < _map.GetLength(0); row++)
        {
            for (var col = 0; col < _map.GetLength(1); col++)
            {
                var position = new Position(row, col);

                // Random pipes that are not connected to the main loop also count
                if (!_pipeLoop.Contains(position) && IsTileInsidePolygon(position, polygonVertices))
                {
                    total++;
                }
            }
        }

        return total;
    }

    private void ReplaceStartCharacter()
    {
        char? topCharacter = _map.IsValidPosition(_startPosition + new Position(-1, 0)) ? _map[_startPosition.Row - 1, _startPosition.Col] : null;
        char? bottomCharacter = _map.IsValidPosition(_startPosition + new Position(1, 0)) ? _map[_startPosition.Row + 1, _startPosition.Col] : null;
        char? leftCharacter = _map.IsValidPosition(_startPosition + new Position(0, -1)) ? _map[_startPosition.Row, _startPosition.Col - 1] : null;
        char? rightCharacter = _map.IsValidPosition(_startPosition + new Position(0, 1)) ? _map[_startPosition.Row, _startPosition.Col + 1] : null;

        char[] topOptions = ['|', 'F', '7'];
        char[] bottomOptions = ['|', 'J', 'L'];
        char[] leftOptions = ['-', 'L', 'F'];
        char[] rightOptions = ['-', '7', 'J'];

        var isTopConnected = topCharacter is not null && topOptions.Contains((char)topCharacter);
        var isBottomConnected = bottomCharacter is not null && bottomOptions.Contains((char)bottomCharacter);
        var isLeftConnected = leftCharacter is not null && leftOptions.Contains((char)leftCharacter);
        var isRightConnected = rightCharacter is not null && rightOptions.Contains((char)rightCharacter);

        if (isTopConnected && isBottomConnected)
        {
            _map[_startPosition.Row, _startPosition.Col] = '|';
        }
        else if (isLeftConnected && isRightConnected)
        {
            _map[_startPosition.Row, _startPosition.Col] = '-';
        }
        else if (isBottomConnected && isLeftConnected)
        {
            _map[_startPosition.Row, _startPosition.Col] = '7';
        }
        else if (isBottomConnected && isRightConnected)
        {
            _map[_startPosition.Row, _startPosition.Col] = 'F';
        }
        else if (isTopConnected && isRightConnected)
        {
            _map[_startPosition.Row, _startPosition.Col] = 'L';
        }
        else if (isTopConnected && isLeftConnected)
        {
            _map[_startPosition.Row, _startPosition.Col] = 'J';
        }
        else
        {
            throw new ArgumentException("Invalid pipe connection");
        }
    }

    private Position NextLoopPosition(Position currentPosition, Position previousPosition)
    {
        var character = _map[currentPosition.Row, currentPosition.Col];

        var directionIncrements = _directionIncrements[character];

        var previousIncrement = previousPosition - currentPosition;
        var selectedIncrement = directionIncrements.First(i => i != previousIncrement);

        return currentPosition + selectedIncrement;
    }

    private Position[] GetLoopPositions()
    {
        var currentPosition = _startPosition;
        var previousPosition = _startPosition;

        var loopPositions = new List<Position>();
        while (true)
        {
            var nextPosition = NextLoopPosition(currentPosition, previousPosition);
            loopPositions.Add(nextPosition);

            if (nextPosition == _startPosition)
            {
                break;
            }

            previousPosition = currentPosition;
            currentPosition = nextPosition;
        }

        return loopPositions.ToArray();
    }

    private static bool IsTileInsidePolygon(Position tilePosition, Position[] polygonVertices)
    {
        // Ray Casting Algorithm to determine if a point is inside a polygon (pipe loop)
        var isInside = false;

        // Iterates over adjacent positions of the polygon
        // For each pair, it checks if the horizontal line extending to the right intersects with the polygon edge
        var j = polygonVertices.Length - 1;
        for (int i = 0; i < polygonVertices.Length; i++)
        {
            var vertexA = polygonVertices[i];
            var vertexB = polygonVertices[j];

            if (vertexA.Row < tilePosition.Row && vertexB.Row >= tilePosition.Row
                || vertexB.Row < tilePosition.Row && vertexA.Row >= tilePosition.Row)
            {
                if (vertexA.Col
                        + (tilePosition.Row - vertexA.Row)
                        / (vertexB.Row - vertexA.Row)
                        * (vertexB.Col - vertexA.Col)
                    < tilePosition.Col)
                {
                    isInside = !isInside;
                }
            }

            j = i;
        }

        return isInside;
    }
}
