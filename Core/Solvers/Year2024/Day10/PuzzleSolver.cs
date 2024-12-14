using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Solvers.Year2024.Day10.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day10;

[PuzzleName("Hoof It")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 10;

    [PuzzleInput(filename: "example.txt", expectedResult: 36)]
    [PuzzleInput(filename: "input.txt", expectedResult: 794)]
    public object SolvePartOne(string[] inputLines)
    {
        (int[,] map, List<Position> trailheadPositions) = InputParser.ParseInput(inputLines);

        var result = 0;

        foreach (var trailheadPos in trailheadPositions)
        {
            var positionsToExplore = new Queue<Position>();
            positionsToExplore.Enqueue(trailheadPos);

            var visitedPositions = new HashSet<Position>();
            var reachedEndPositions = new HashSet<Position>();

            while (positionsToExplore.Count > 0)
            {
                var currentPosition = positionsToExplore.Dequeue();
                if (visitedPositions.Contains(currentPosition))
                {
                    continue;
                }

                var currentValue = map.GetValueAt(currentPosition);

                visitedPositions.Add(currentPosition);

                foreach ((Position neighbourPos, int neightbourValue) in map.GetAllNeighbours(currentPosition, MapConnectivity.FourConnected))
                {
                    // Stricly increasing values
                    if (neightbourValue - currentValue != 1)
                    {
                        continue;
                    }

                    if (neightbourValue == 9)
                    {
                        reachedEndPositions.Add(neighbourPos);
                    }
                    else
                    {
                        // Continue searching
                        positionsToExplore.Enqueue(neighbourPos);
                    }
                }
            }

            var trailScore = reachedEndPositions.Count;
            result += trailScore;
        }

        return result;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 227)]
    [PuzzleInput(filename: "input.txt", expectedResult: 1706)]
    public object SolvePartTwo(string[] inputLines)
    {
        (int[,] map, List<Position> trailheadPositions) = InputParser.ParseInput(inputLines);

        var result = 0;

        foreach (var trailheadPos in trailheadPositions)
        {
            // This time the whole path is needed
            var positionsToExplore = new Queue<(Position Current, List<Position> Prev)>();
            positionsToExplore.Enqueue((trailheadPos, []));

            var visitedPositions = new HashSet<(Position Current, List<Position> Prev)>();
            var distinctTrails = 0;

            while (positionsToExplore.Count > 0)
            {
                (Position currentPosition, List<Position> prevPositions) = positionsToExplore.Dequeue();

                if (prevPositions.Count > 0 && visitedPositions.Contains((currentPosition, prevPositions)))
                {
                    continue;
                }

                var currentValue = map.GetValueAt(currentPosition);

                visitedPositions.Add((currentPosition, prevPositions));

                foreach ((Position neighbourPos, int neightbourValue) in map.GetAllNeighbours(currentPosition, MapConnectivity.FourConnected))
                {
                    // Stricly increasing values
                    if (neightbourValue - currentValue != 1)
                    {
                        continue;
                    }

                    if (neightbourValue == 9)
                    {
                        distinctTrails++;
                    }
                    else
                    {
                        // Continue searching
                        positionsToExplore.Enqueue((neighbourPos, [.. prevPositions, currentPosition]));
                    }
                }
            }

            result += distinctTrails;
        }

        return result;
    }
}
