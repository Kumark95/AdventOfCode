using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Solvers.Year2024.Day15.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day15;

[PuzzleName("Warehouse Woes")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 15;

    [PuzzleInput(filename: "example.txt", expectedResult: 2028)]
    [PuzzleInput(filename: "example-3.txt", expectedResult: 10092)]
    [PuzzleInput(filename: "input.txt", expectedResult: 1441031)]
    public object SolvePartOne(string[] inputLines)
    {
        (char[,] map, Position robotInitialPosition, char[] instructions) = InputParser.ParseInput(inputLines);

        var colorDict = new Dictionary<char, ConsoleColor>()
        {
            { '@', ConsoleColor.Green },
            { 'O', ConsoleColor.Blue },
            { '#', ConsoleColor.Red },
        };
        const char ROBOT = '@';
        const char BOX = 'O';
        const char OBSTACLE = '#';
        const char EMPTY_SPACE = '.';

        var draw = false;

        if (draw)
        {
            map.Print(colorDict);
            Console.WriteLine();
            Console.WriteLine("------------------");
            Console.WriteLine();
        }

        var currentPosition = robotInitialPosition;
        foreach (var instruction in instructions)
        {
            if (draw)
            {
                Console.WriteLine("INSTRUCTION: " + instruction);
            }

            var direction = instruction switch
            {
                '^' => new Position(-1, 0),
                'v' => new Position(1, 0),
                '<' => new Position(0, -1),
                '>' => new Position(0, 1),
                _ => throw new InvalidOperationException()
            };

            var nextPosition = currentPosition + direction;
            if (map.GetValueAt(nextPosition) == OBSTACLE)
            {
                continue;
            }

            // Move to empty spaces
            if (map.GetValueAt(nextPosition) == EMPTY_SPACE)
            {
                map.SetValueAt(currentPosition, EMPTY_SPACE);
                map.SetValueAt(nextPosition, ROBOT);
            }
            else if (map.GetValueAt(nextPosition) == BOX)
            {
                var consecutiveBoxes = 0;
                var boxPosition = nextPosition;
                while (true)
                {
                    if (map.GetValueAt(boxPosition) == BOX)
                    {
                        consecutiveBoxes++;
                        boxPosition += direction;
                    }
                    else if (map.GetValueAt(boxPosition) == EMPTY_SPACE)
                    {
                        break;
                    }
                    else if (map.GetValueAt(boxPosition) == OBSTACLE)
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

                map.SetValueAt(currentPosition, EMPTY_SPACE);
                map.SetValueAt(nextPosition, ROBOT);
                map.SetValueAt(firstFreePosition, BOX);
            }

            currentPosition = nextPosition;

            if (draw)
            {
                map.Print(colorDict);
                Console.WriteLine();
            }
        }

        return CalculateSumGpsCoordinates(map, BOX);
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 1751)]
    [PuzzleInput(filename: "example-2.txt", expectedResult: 618)]
    [PuzzleInput(filename: "example-3.txt", expectedResult: 9021)]
    [PuzzleInput(filename: "input.txt", expectedResult: 1425169)]
    public object SolvePartTwo(string[] inputLines)
    {
        (char[,] map, Position robotInitialPosition, char[] instructions) = InputParser.ParseWideInput(inputLines);

        var colorDict = new Dictionary<char, ConsoleColor>()
        {
            { '@', ConsoleColor.Green },
            { 'O', ConsoleColor.Blue },
            { '#', ConsoleColor.Red },
        };
        const char ROBOT = '@';
        const char LEFT_BOX = '[';
        const char RIGHT_BOX = ']';

        const char OBSTACLE = '#';
        const char EMPTY_SPACE = '.';

        var draw = false;

        if (draw)
        {
            map.Print(colorDict);
            Console.WriteLine();
            Console.WriteLine("------------------");
            Console.WriteLine();
        }

        var currentPosition = robotInitialPosition;
        foreach (var instruction in instructions)
        {
            if (draw)
            {
                Console.WriteLine("INSTRUCTION: " + instruction);
            }

            var direction = instruction switch
            {
                '^' => new Position(-1, 0),
                'v' => new Position(1, 0),
                '<' => new Position(0, -1),
                '>' => new Position(0, 1),
                _ => throw new InvalidOperationException()
            };

            var nextPosition = currentPosition + direction;
            if (map.GetValueAt(nextPosition) == OBSTACLE)
            {
                continue;
            }

            // Move to empty spaces
            if (map.GetValueAt(nextPosition) == EMPTY_SPACE)
            {
                map.SetValueAt(currentPosition, EMPTY_SPACE);
                map.SetValueAt(nextPosition, ROBOT);
            }
            else if (map.GetValueAt(nextPosition) == LEFT_BOX || map.GetValueAt(nextPosition) == RIGHT_BOX)
            {
                if (instruction == '^' || instruction == 'v')
                {
                    var canMove = CanMoveVertically(map, currentPosition, direction, out var leftBoxPositions);
                    if (!canMove)
                    {
                        continue;
                    }

                    var rows = leftBoxPositions.Select(p => p.Row).Distinct().ToArray();

                    // Move boxes
                    var orderedRows = instruction == '^'
                        ? rows.OrderBy(r => r).ToArray()
                        : rows.OrderByDescending(r => r).ToArray();

                    foreach (var row in orderedRows)
                    {
                        var boxes = leftBoxPositions.Where(p => p.Row == row).ToArray();

                        // Swap
                        foreach (var boxLeft in boxes)
                        {
                            var nextLeft = boxLeft + direction;
                            var nextRight = boxLeft + new Position(0, 1) + direction; // Right side

                            var nextCharLeft = map.GetValueAt(nextLeft);
                            var nextCharRight = map.GetValueAt(nextRight);

                            map.SetValueAt(nextLeft, '[');
                            map.SetValueAt(nextRight, ']');

                            map.SetValueAt(boxLeft, nextCharLeft);
                            map.SetValueAt(boxLeft + new Position(0, 1), nextCharRight);
                        }
                    }
                }
                else
                {
                    var consecutiveBoxes = 0;
                    var boxPosition = nextPosition;
                    while (true)
                    {

                        if (map.GetValueAt(boxPosition) == LEFT_BOX || map.GetValueAt(boxPosition) == RIGHT_BOX)
                        {
                            consecutiveBoxes++;
                            boxPosition += direction * 2;
                        }
                        else if (map.GetValueAt(boxPosition) == EMPTY_SPACE)
                        {
                            break;
                        }
                        else if (map.GetValueAt(boxPosition) == OBSTACLE)
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

                    var firstFreePosition = nextPosition + (direction * 2 * consecutiveBoxes);

                    // Go back and swap
                    var p = firstFreePosition;
                    while (p != currentPosition)
                    {
                        var p1 = p - direction;

                        var curChar = map.GetValueAt(p);
                        var nextChar = map.GetValueAt(p1);

                        map.SetValueAt(p, nextChar);
                        map.SetValueAt(p1, curChar);

                        p -= direction;
                    }
                }

                map.SetValueAt(currentPosition, EMPTY_SPACE);
                map.SetValueAt(nextPosition, ROBOT);
            }

            currentPosition = nextPosition;

            if (draw)
            {
                map.Print(colorDict);
                Console.WriteLine();
            }
        }

        return CalculateSumGpsCoordinates(map, searchChar: '[');
    }

    private static bool CanMoveVertically(char[,] map, Position start, Position direction, out HashSet<Position> boxLeftPositions)
    {
        boxLeftPositions = new HashSet<Position>();

        var startChar = map.GetValueAt(start);

        var nextPosition = start + direction;
        var nextChar = map.GetValueAt(nextPosition);

        if (startChar == '[')
        {
            boxLeftPositions.Add(start);
        }

        if (nextChar == '.')
        {
            return true;
        }
        else if (nextChar == '#')
        {
            return false;
        }

        HashSet<Position> recursivePositions1, recursivePositions2;

        // Check recursively the other side of the boxes
        if (nextChar == '[')
        {
            // Check next and right side
            bool canMove1 = CanMoveVertically(map, nextPosition, direction, out recursivePositions1);
            bool canMove2 = CanMoveVertically(map, nextPosition + new Position(0, 1), direction, out recursivePositions2);

            boxLeftPositions.UnionWith(recursivePositions1);
            boxLeftPositions.UnionWith(recursivePositions2);

            return canMove1 && canMove2;
        }
        else if (nextChar == ']')
        {
            bool canMove1 = CanMoveVertically(map, nextPosition, direction, out recursivePositions1);
            bool canMove2 = CanMoveVertically(map, nextPosition + new Position(0, -1), direction, out recursivePositions2);

            boxLeftPositions.UnionWith(recursivePositions1);
            boxLeftPositions.UnionWith(recursivePositions2);

            return canMove1 && canMove2;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private static long CalculateSumGpsCoordinates(char[,] map, char searchChar)
    {
        var result = 0;
        for (var row = 0; row < map.RowLength(); row++)
        {
            for (var col = 0; col < map.ColLength(); col++)
            {
                if (map[row, col] == searchChar)
                {
                    result += 100 * row + col;
                }
            }
        }

        return result;
    }
}
