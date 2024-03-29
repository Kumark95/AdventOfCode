using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day06.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day06;

[PuzzleName("Wait For It")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 6;

    [PuzzleInput(filename: "example.txt", expectedResult: 288)]
    [PuzzleInput(filename: "input.txt", expectedResult: 771628)]
    public object SolvePartOne(string[] inputLines)
    {
        var timeMatches = InputParser.ExtractNumbers(inputLines[0]);
        var distanceMatches = InputParser.ExtractNumbers(inputLines[1]);

        var races = timeMatches
            .Zip(distanceMatches)
            .Select(t => new Race(t.First, t.Second))
            .ToArray();

        return races
            .Select(r => r.CalculateTotalRecordBeatingWays())
            .Aggregate((a, b) => a * b);
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 71503)]
    [PuzzleInput(filename: "input.txt", expectedResult: 27363861)]
    public object SolvePartTwo(string[] inputLines)
    {
        var time = InputParser.ExtractSingleNumber(inputLines[0]);
        var distance = InputParser.ExtractSingleNumber(inputLines[1]);
        var race = new Race(time, distance);

        return race.CalculateTotalRecordBeatingWays();
    }
}
