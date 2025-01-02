using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day15.Model;

internal class Warehouse
{
    private readonly char[,] _map;
    private readonly Position _robotInitialPosition;
    private readonly char[] _instructions;
    private readonly int _wideFactor;

    private static class Items
    {
        internal const char Robot = '@';
        internal const char Obstacle = '#';
        internal const char EmptySpace = '.';
        internal const char SmallBox = 'O';
        internal const char WideBoxLeftSide = '[';
        internal const char WideBoxRightSide = ']';
    }

    private static class Instructions
    {
        internal const char MoveUp = '^';
        internal const char MoveDown = 'v';
        internal const char MoveLeft = '<';
        internal const char MoveRight = '>';

        internal static Position GetDirection(char instruction)
        {
            return instruction switch
            {
                MoveUp => new Position(-1, 0),
                MoveDown => new Position(1, 0),
                MoveLeft => new Position(0, -1),
                MoveRight => new Position(0, 1),
                _ => throw new InvalidOperationException()
            };
        }
    }

#if DEBUG
    private readonly Dictionary<char, ConsoleColor> _colorDict = new()
    {
        { Items.Robot, ConsoleColor.Green },
        { Items.Obstacle, ConsoleColor.Red },
        { Items.SmallBox, ConsoleColor.Blue },
        { Items.WideBoxLeftSide, ConsoleColor.Blue },
        { Items.WideBoxRightSide, ConsoleColor.Blue }
    };
#endif

    private Warehouse(char[,] map, Position robotInitialPosition, char[] instructions, int wideFactor)
    {
        _map = map;
        _robotInitialPosition = robotInitialPosition;
        _instructions = instructions;
        _wideFactor = wideFactor;
    }

    public static Warehouse BuildFromInput(string[] inputLines, bool isWideMap)
    {
        Position? robotPosition = null;

        // In the second part the items are twice as wide
        var wideFactor = isWideMap ? 2 : 1;

        var rowLength = inputLines.Where(l => l.StartsWith(Items.Obstacle)).Count();
        var inputLength = inputLines[0].Length;
        var colLength = inputLength * wideFactor;

        var map = new char[rowLength, colLength];

        for (var row = 0; row < rowLength; row++)
        {
            for (var col = 0; col < inputLength; col++)
            {
                var item = inputLines[row][col];

                if (item == Items.Robot)
                {
                    robotPosition = new Position(row, col * wideFactor);
                }

                map[row, col * wideFactor] = item;
                if (isWideMap)
                {
                    if (item == Items.Robot)
                    {
                        // @ => @.
                        map[row, col * wideFactor + 1] = Items.EmptySpace;
                    }
                    else if (item == Items.SmallBox)
                    {
                        // O => []
                        map[row, col * wideFactor] = Items.WideBoxLeftSide;
                        map[row, col * wideFactor + 1] = Items.WideBoxRightSide;
                    }
                    else
                    {
                        // Duplicate
                        map[row, col * wideFactor + 1] = item;
                    }
                }
            }
        }

        if (robotPosition is null)
        {
            throw new InvalidOperationException("Could not find robot initial position");
        }

        var instructions = inputLines.Skip(rowLength + 1).SelectMany(l => l.ToCharArray()).ToArray();

        return new Warehouse(map, robotPosition.Value, instructions, wideFactor);
    }

    public void ExecuteInstructions()
    {
        if (_wideFactor == 1)
        {
            ExecuteSimpleInstructions();
        }
        else
        {
            ExecuteWideInstructions();
        }
    }

    private void ExecuteSimpleInstructions()
    {
        var currentPosition = _robotInitialPosition;
        foreach (var instruction in _instructions)
        {
            //Console.WriteLine("Instruction: " + instruction);

            var direction = Instructions.GetDirection(instruction);

            var nextPosition = currentPosition + direction;
            if (_map.GetValueAt(nextPosition) == Items.Obstacle)
            {
                continue;
            }
            var nextItem = _map.GetValueAt(nextPosition);

            // Move to empty spaces
            if (nextItem == Items.EmptySpace)
            {
                _map.SetValueAt(currentPosition, Items.EmptySpace);
                _map.SetValueAt(nextPosition, Items.Robot);
            }
            else if (nextItem == Items.SmallBox)
            {
                var consecutiveBoxes = 0;
                var boxPosition = nextPosition;
                while (true)
                {
                    if (_map.GetValueAt(boxPosition) == Items.SmallBox)
                    {
                        consecutiveBoxes++;
                        boxPosition += direction;
                    }
                    else if (_map.GetValueAt(boxPosition) == Items.EmptySpace)
                    {
                        break;
                    }
                    else if (_map.GetValueAt(boxPosition) == Items.Obstacle)
                    {
                        // The boxes cannot be moved
                        consecutiveBoxes = -1;
                        break;
                    }
                }

                if (consecutiveBoxes == -1)
                {
                    continue;
                }

                var firstFreePosition = nextPosition + direction * consecutiveBoxes;

                _map.SetValueAt(currentPosition, Items.EmptySpace);
                _map.SetValueAt(nextPosition, Items.Robot);
                _map.SetValueAt(firstFreePosition, Items.SmallBox);
            }

            currentPosition = nextPosition;
        }
    }


    public void ExecuteWideInstructions()
    {
        var currentPosition = _robotInitialPosition;
        foreach (var instruction in _instructions)
        {
            //Console.WriteLine("Instruction: " + instruction);

            var direction = Instructions.GetDirection(instruction);

            var nextPosition = currentPosition + direction;
            if (_map.GetValueAt(nextPosition) == Items.Obstacle)
            {
                continue;
            }
            var nextItem = _map.GetValueAt(nextPosition);

            // Move to empty spaces
            if (nextItem == Items.EmptySpace)
            {
                _map.SetValueAt(currentPosition, Items.EmptySpace);
                _map.SetValueAt(nextPosition, Items.Robot);
            }
            else if (nextItem == Items.WideBoxLeftSide || nextItem == Items.WideBoxRightSide)
            {
                if (instruction == Instructions.MoveUp || instruction == Instructions.MoveDown)
                {
                    var canMove = CanMoveVertically(currentPosition, direction, out var leftSidePositions);
                    if (!canMove)
                    {
                        continue;
                    }

                    // Move the boxes at each row based on the direction
                    var rows = leftSidePositions.Select(p => p.Row).Distinct().ToArray();

                    var orderedRows = instruction == Instructions.MoveUp
                        ? rows.OrderBy(r => r).ToArray()
                        : rows.OrderByDescending(r => r).ToArray();

                    var rightSideDirection = Instructions.GetDirection(Instructions.MoveRight);

                    foreach (var row in orderedRows)
                    {
                        var boxes = leftSidePositions.Where(p => p.Row == row).ToArray();

                        foreach (var leftSide in boxes)
                        {
                            var rightSide = leftSide + rightSideDirection;

                            var nextLeft = leftSide + direction;
                            var nextRight = rightSide + direction;

                            var nextCharLeft = _map.GetValueAt(nextLeft);
                            var nextCharRight = _map.GetValueAt(nextRight);

                            // Swap
                            _map.SetValueAt(nextLeft, Items.WideBoxLeftSide);
                            _map.SetValueAt(nextRight, Items.WideBoxRightSide);

                            _map.SetValueAt(leftSide, nextCharLeft);
                            _map.SetValueAt(rightSide, nextCharRight);
                        }
                    }
                }
                else
                {
                    var canMove = CanMoveHorizontally(currentPosition, direction, out var leftSidePositions);
                    if (!canMove)
                    {
                        continue;
                    }

                    // Order the boxes based on the direction. Horizontally only one box can be at each column
                    var orderedBoxes = instruction == Instructions.MoveLeft
                        ? leftSidePositions.OrderBy(p => p.Col).ToArray()
                        : leftSidePositions.OrderByDescending(p => p.Col).ToArray();

                    var rightSideDirection = Instructions.GetDirection(Instructions.MoveRight);
                    foreach (var leftSide in orderedBoxes)
                    {
                        var rightSide = leftSide + rightSideDirection;

                        var nextLeft = leftSide + direction;
                        var nextRight = rightSide + direction;

                        // Swap
                        _map.SetValueAt(nextLeft, Items.WideBoxLeftSide);
                        _map.SetValueAt(nextRight, Items.WideBoxRightSide);

                        if (instruction == Instructions.MoveLeft)
                        {
                            _map.SetValueAt(rightSide, Items.EmptySpace);
                        }
                        else
                        {
                            _map.SetValueAt(leftSide, Items.EmptySpace);
                        }
                    }
                }

                _map.SetValueAt(currentPosition, Items.EmptySpace);
                _map.SetValueAt(nextPosition, Items.Robot);
            }

            currentPosition = nextPosition;
        }

#if DEBUG
        _map.Print(_colorDict);
        Console.WriteLine();
#endif
    }

    public long CalculateSumGpsCoordinates()
    {
        var result = 0;
        for (var row = 0; row < _map.RowLength(); row++)
        {
            for (var col = 0; col < _map.ColLength(); col++)
            {
                // For the wide boxes only the left side is used as its closer to the top-left corner
                if (_map[row, col] == Items.SmallBox || _map[row, col] == Items.WideBoxLeftSide)
                {
                    result += 100 * row + col;
                }
            }
        }

        return result;
    }

    private bool CanMoveVertically(Position start, Position direction, out HashSet<Position> boxLeftSidePositions)
    {
        boxLeftSidePositions = new HashSet<Position>();

        var startChar = _map.GetValueAt(start);

        var nextPosition = start + direction;
        var nextChar = _map.GetValueAt(nextPosition);

        if (startChar == Items.WideBoxLeftSide)
        {
            boxLeftSidePositions.Add(start);
        }

        if (nextChar == Items.EmptySpace)
        {
            return true;
        }
        else if (nextChar == Items.Obstacle)
        {
            return false;
        }

        HashSet<Position> positions1, positions2;

        // Check recursively the other side of the boxes
        if (nextChar == Items.WideBoxLeftSide)
        {
            // Check next and right side
            bool canMove1 = CanMoveVertically(nextPosition, direction, out positions1);
            bool canMove2 = CanMoveVertically(nextPosition + new Position(0, 1), direction, out positions2);

            boxLeftSidePositions.UnionWith(positions1);
            boxLeftSidePositions.UnionWith(positions2);

            return canMove1 && canMove2;
        }
        else if (nextChar == Items.WideBoxRightSide)
        {
            bool canMove1 = CanMoveVertically(nextPosition, direction, out positions1);
            bool canMove2 = CanMoveVertically(nextPosition + new Position(0, -1), direction, out positions2);

            boxLeftSidePositions.UnionWith(positions1);
            boxLeftSidePositions.UnionWith(positions2);

            return canMove1 && canMove2;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private bool CanMoveHorizontally(Position start, Position direction, out HashSet<Position> boxLeftSidePositions)
    {
        boxLeftSidePositions = new HashSet<Position>();

        var nextPosition = start + direction;

        var isMovingRight = direction == new Position(0, 1);

        // Find the first empty space or obstacle
        var consecutiveBoxes = 0;
        var boxPosition = nextPosition;
        while (true)
        {
            var item = _map.GetValueAt(boxPosition);

            if (item == Items.WideBoxLeftSide || item == Items.WideBoxRightSide)
            {
                if (isMovingRight && item == Items.WideBoxLeftSide)
                {
                    boxLeftSidePositions.Add(boxPosition);
                }
                else if (!isMovingRight && item == Items.WideBoxRightSide)
                {
                    boxLeftSidePositions.Add(boxPosition + direction);
                }

                consecutiveBoxes++;
                boxPosition += direction * _wideFactor;
            }
            else if (item == Items.EmptySpace)
            {
                return true;
            }
            else if (item == Items.Obstacle)
            {
                return false;
            }
        }
    }
}
