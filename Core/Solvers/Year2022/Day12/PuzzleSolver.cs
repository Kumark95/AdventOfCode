using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2022.Day12.Model;

namespace AdventOfCode.Core.Solvers.Year2022.Day12;

[PuzzleName("Hill Climbing Algorithm")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 12;

    public object SolvePartOne(string[] inputLines)
    {
        var maze = Maze.Parse(inputLines);
        return maze.FindShortestDistance(maze.StartPosition, maze.EndPosition);
    }

    public object SolvePartTwo(string[] inputLines)
    {
        var maze = Maze.Parse(inputLines);

        var lowPositionDistances = new List<int>();
        foreach (var lowPosition in maze.LowPositions)
        {
            maze.Reset();

            var shortestDistance = maze.FindShortestDistance(lowPosition, maze.EndPosition);
            if (shortestDistance == -1)
            {
                continue;
            }

            lowPositionDistances.Add(shortestDistance);
        }

        return lowPositionDistances.Min();
    }
}

