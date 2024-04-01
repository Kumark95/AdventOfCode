using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Solvers.Year2023.Day21.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day21;

[PuzzleName("Step Counter")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 21;

    [PuzzleInput(filename: "example.txt", expectedResult: 42)]
    [PuzzleInput(filename: "input.txt", expectedResult: 3_697)]
    public object SolvePartOne(string[] inputLines)
    {
        var (map, initialPosition, rockPositions) = InputParser.ParseInput(inputLines);

        var mDistanceRocks = new Dictionary<Position, long>();
        foreach (var rockPos in rockPositions)
        {
            mDistanceRocks.Add(rockPos, rockPos.ManhattanDistance(initialPosition));
        }

        var stepTarget = 3;

        var baseRes = Math.Pow(stepTarget + 1, 2);
        Console.WriteLine(baseRes);

        var rocksInPath = mDistanceRocks.Values.Where(d => d <= stepTarget).Count();

        return baseRes - rocksInPath;

        // Identify the positions of the 


        return 1;

        HashSet<Position> reached = [initialPosition];
        for (var step = 0; step < stepTarget; step++)
        {
            HashSet<Position> reachedNext = [];
            foreach (var position in reached)
            {
                foreach (var (adjPosition, adjCharacter) in map.GetAllNeighbours(position, MapConnectivity.FourConnected))
                {
                    if (adjCharacter == '#')
                    {
                        continue;
                    }

                    reachedNext.Add(adjPosition);
                }
            }

            reached = reachedNext;
        }

        return reached.Count;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 16733044)]
    //[PuzzleInput(filename: "input.txt", expectedResult: 999_999_999_999)]
    public object SolvePartTwo(string[] inputLines)
    {
        return 1;

        var (map, initialPosition, _) = InputParser.ParseInput(inputLines);

        var stepTarget = 5000;

        HashSet<Position> reached = [initialPosition];
        for (var step = 0; step < stepTarget; step++)
        {
            HashSet<Position> reachedNext = [];
            foreach (var position in reached)
            {
                foreach (var (adjPosition, adjCharacter) in Neighbours(map, position))
                {
                    if (adjCharacter == '#')
                    {
                        continue;
                    }

                    reachedNext.Add(adjPosition);
                }
            }

            reached = reachedNext;
        }

        return reached.Count;
    }

    public static IEnumerable<(Position Position, char Value)> Neighbours(char[,] map, Position position)
    {
        int[] rowDirection = [-1, 0, 1, 0];
        int[] colDirection = [0, 1, 0, -1];

        var rowLength = map.RowLength();
        var colLength = map.ColLength();

        for (int i = 0; i < rowDirection.Length; i++)
        {
            var adjPosition = new Position(position.Row + rowDirection[i], position.Col + colDirection[i]);
            if (map.IsValidPosition(adjPosition))
            {
                var adjCharacter = map[adjPosition.Row, adjPosition.Col];
                yield return (adjPosition, adjCharacter);
            }
            else
            {
                // Map to a position in original domain
                var rowDelta = adjPosition.Row < 0
                    ? (adjPosition.Row % rowLength + rowLength) % rowLength
                    : adjPosition.Row % rowLength;

                var colDelta = adjPosition.Col < 0
                    ? (adjPosition.Col % colLength + colLength) % colLength
                    : adjPosition.Col % colLength;

                //Console.WriteLine(adjPosition + " R: " + rowDelta + " C: " + colDelta);
                var adjCharacter = map[rowDelta, colDelta];
                // Using the position with the invalid values
                yield return (adjPosition, adjCharacter);
            }
        }
    }

    public void Print(char[,] map, HashSet<Position> positions)
    {
        for (int row = 0; row < map.RowLength(); row++)
        {
            for (int col = 0; col < map.ColLength(); col++)
            {
                if (positions.Contains(new Position(row, col)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write('O');
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(map[row, col]);
                }
            }

            Console.WriteLine();
        }
    }
}
