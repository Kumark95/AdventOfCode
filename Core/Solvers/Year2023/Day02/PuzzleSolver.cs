using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day02.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day02;

[PuzzleName("Cube Conundrum")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 2;

    [PuzzleInput(filename: "example.txt", expectedResult: 8)]
    [PuzzleInput(filename: "input.txt", expectedResult: 2879)]
    public long? SolvePartOne(string[] inputLines)
    {
        var result = 0;

        for (var gameNumber = 1; gameNumber <= inputLines.Length; gameNumber++)
        {
            var line = inputLines[gameNumber - 1];

            var validationResults = new HashSet<bool>();
            foreach (var round in GameRound.ParseGame(line))
            {
                validationResults.Add(round.IsValid(availableRedCubes: 12, availableGreenCubes: 13, availableBlueCubes: 14));
            }

            if (validationResults.Count == 1 && validationResults.First())
            {
                result += gameNumber;
            }
        }

        return result;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 2286)]
    [PuzzleInput(filename: "input.txt", expectedResult: 65122)]
    public long? SolvePartTwo(string[] inputLines)
    {
        var result = 0;

        for (var gameNumber = 1; gameNumber <= inputLines.Length; gameNumber++)
        {
            var line = inputLines[gameNumber - 1];

            var maxRedCubes = 0;
            var maxGreenCubes = 0;
            var maxBlueCubes = 0;
            foreach (var round in GameRound.ParseGame(line))
            {
                maxRedCubes = Math.Max(maxRedCubes, round.RedCubes);
                maxGreenCubes = Math.Max(maxGreenCubes, round.GreenCubes);
                maxBlueCubes = Math.Max(maxBlueCubes, round.BlueCubes);
            }

            var powerValue = maxRedCubes * maxGreenCubes * maxBlueCubes;
            result += powerValue;
        }

        return result;
    }
}
