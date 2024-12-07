using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Solvers.Year2024.Day06.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day06;

[PuzzleName("Guard Gallivant")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 6;

    [PuzzleInput(filename: "example.txt", expectedResult: 41)]
    [PuzzleInput(filename: "input.txt", expectedResult: 5131)]
    public object SolvePartOne(string[] inputLines)
    {
        (char[,] map, Position initialPosition) = InputParser.ParseInput(inputLines);

        var guardMap = new GuardMap(map, initialPosition);
        var routePositions = guardMap.FindGuardRoutePositions();

        return routePositions.Count;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 6)]
    [PuzzleInput(filename: "input.txt", expectedResult: 1784)]
    public object SolvePartTwo(string[] inputLines)
    {
        (char[,] map, Position initialPosition) = InputParser.ParseInput(inputLines);

        var guardMap = new GuardMap(map, initialPosition);

        // The new obstacles can only be placed in the base route
        var baseRoutePositions = guardMap.FindGuardRoutePositions();
        baseRoutePositions = baseRoutePositions.Where(p => p != initialPosition).ToHashSet();

        var infiniteLoops = 0;
        foreach (Position basePosition in baseRoutePositions)
        {
            if (guardMap.HasInfiniteLoop(newObstaclePosition: basePosition))
            {
                infiniteLoops++;
            }
        }

        return infiniteLoops;
    }
}
