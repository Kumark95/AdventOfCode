using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day10.Model;

internal class PipeMaze
{
    private readonly char[,] _map;
    private readonly Position _startPosition;
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
    }

    public int CalculateLoopLength()
    {
        var currentPosition = _startPosition;
        var previousPosition = _startPosition;
        var loopLength = 0;
        while (true)
        {
            loopLength++;
            var nextPosition = NextPosition(currentPosition, previousPosition);
            if (nextPosition == _startPosition)
            {
                break;
            }

            previousPosition = currentPosition;
            currentPosition = nextPosition;
        }

        return loopLength;
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

    private Position NextPosition(Position currentPosition, Position previousPosition)
    {
        var character = _map[currentPosition.Row, currentPosition.Col];

        var directionIncrements = _directionIncrements[character];

        var previousIncrement = previousPosition - currentPosition;
        var selectedIncrement = directionIncrements.First(i => i != previousIncrement);

        return currentPosition + selectedIncrement;
    }
}
