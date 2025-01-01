using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Solvers.Year2024.Day14.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day14;

[PuzzleName("Restroom Redoubt")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 14;

    [PuzzleInput(filename: "example.txt", expectedResult: 21)]
    [PuzzleInput(filename: "input.txt", expectedResult: 225810288)]
    public object SolvePartOne(string[] inputLines)
    {
        var input = InputParser.ParseInput(inputLines);

        const int N_SECONDS = 100;
        const int MAX_ROW = 103;
        const int MAX_COL = 101;

        var finalPositions = MoveRobots(input, N_SECONDS, MAX_ROW, MAX_COL);

        // Robots in the middle row and column don't count
        var firstQuadrant = finalPositions.Where(p => p.Row < MAX_ROW / 2 && p.Col < MAX_COL / 2).Count();
        var secondQuadrant = finalPositions.Where(p => p.Row < MAX_ROW / 2 && p.Col > MAX_COL / 2).Count();
        var thridQuadrant = finalPositions.Where(p => p.Row > MAX_ROW / 2 && p.Col < MAX_COL / 2).Count();
        var fourthQuadrant = finalPositions.Where(p => p.Row > MAX_ROW / 2 && p.Col > MAX_COL / 2).Count();

        return firstQuadrant * secondQuadrant * thridQuadrant * fourthQuadrant;
    }

    // NOTE: Not using the example as the area is too small for the tree to appear
    [PuzzleInput(filename: "input.txt", expectedResult: 6752)]
    public object SolvePartTwo(string[] inputLines)
    {
        var input = InputParser.ParseInput(inputLines);

        const int MAX_SECONDS = 10000;
        const int MAX_ROW = 103;
        const int MAX_COL = 101;

        // Only used to draw the map
        var draw = false;

        for (int i = 1; i < MAX_SECONDS; i++)
        {
            var finalPositions = MoveRobots(input, i, MAX_ROW, MAX_COL);

            var distinctFinalPositions = finalPositions.ToHashSet();

            if (distinctFinalPositions.Count == input.Count)
            {
                if (draw)
                {
                    var map = new int[MAX_ROW, MAX_COL];
                    map.Print(distinctFinalPositions);
                }

                return i;
            }
        }

        // Should not happen
        return -1;
    }

    private static List<Position> MoveRobots(List<(Position, Position)> robots, int seconds, int maxRows, int maxCols)
    {
        var finalPositions = new List<Position>();

        foreach ((Position robotPos, Position velocity) in robots)
        {
            var newPosition = robotPos + velocity * seconds;

            // Both row and column must be positive
            var positionInsideBounds = new Position((newPosition.Row % maxRows + maxRows) % maxRows, (newPosition.Col % maxCols + maxCols) % maxCols);
            finalPositions.Add(positionInsideBounds);
        }

        return finalPositions;
    }
}
