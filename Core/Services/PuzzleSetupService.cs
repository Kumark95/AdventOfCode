using Microsoft.Extensions.Logging;

namespace AdventOfCode.Core.Services;

internal sealed class PuzzleSetupService
{
    private readonly ILogger<PuzzleSetupService> _logger;
    private readonly AdventOfCodeService _adventOfCodeService;

    public PuzzleSetupService(
        ILogger<PuzzleSetupService> logger,
        AdventOfCodeService adventOfCodeService)
    {
        _logger = logger;
        _adventOfCodeService = adventOfCodeService;
    }

    public async Task SetupFiles(string solverDirectory, int year, int day)
    {
        var puzzleName = await _adventOfCodeService.GetPuzzleName(year, day);
        if (puzzleName is null)
        {
            return;
        }

        if (!Directory.Exists(solverDirectory))
        {
            Directory.CreateDirectory(solverDirectory);
        }

        _logger.LogDebug("Generating template files");
        File.WriteAllText(Path.Combine(solverDirectory, "example.txt"), string.Empty);
        File.WriteAllText(Path.Combine(solverDirectory, "input.txt"), await _adventOfCodeService.GetPuzzleInput(year, day));
        File.WriteAllText(Path.Combine(solverDirectory, "PuzzleSolver.cs"), GenerateSolver(year, day, puzzleName));

        Directory.CreateDirectory(Path.Combine(solverDirectory, "Model"));
        File.WriteAllText(Path.Combine(solverDirectory, "Model", "InputParser.cs"), GenerateParser(year, day));
    }

    private static string GenerateSolver(int year, int day, string puzzleName)
    {
        return $$"""
            using AdventOfCode.Common.Attributes;
            using AdventOfCode.Common.Interfaces;
            using AdventOfCode.Core.Solvers.Year{{year:D4}}.Day{{day:D2}}.Model;

            namespace AdventOfCode.Core.Solvers.Year{{year:D4}}.Day{{day:D2}};

            [PuzzleName("{{puzzleName}}")]
            public sealed class PuzzleSolver : IPuzzleSolver
            {
                public int Year => {{year}};
                public int Day => {{day}};

                [PuzzleInput(filename: "example.txt", expectedResult: 999_999_999_999)]
                [PuzzleInput(filename: "input.txt", expectedResult: 999_999_999_999)]
                public object SolvePartOne(string[] inputLines)
                {
                    return 0;
                }

                [PuzzleInput(filename: "example.txt", expectedResult: 999_999_999_999)]
                [PuzzleInput(filename: "input.txt", expectedResult: 999_999_999_999)]
                public object SolvePartTwo(string[] inputLines)
                {
                    return null;
                }
            }

            """;
    }

    private static string GenerateParser(int year, int day)
    {
        return $$"""
            namespace AdventOfCode.Core.Year{{year:D4}}.Day{{day:D2}}.Model;

            internal static class InputParser
            {
                public static char[,] ParseInput(string[] inputLines)
                {
                    var rowLength = inputLines.Length;
                    var colLength = inputLines[0].Length;
                    var map = new char[rowLength, colLength];

                    for (var row = 0; row < rowLength; row++)
                    {
                        for (var col = 0; col < colLength; col++)
                        {
                            map[row, col] = inputLines[row][col];
                        }
                    }

                    return map;
                }
            }
            """;
    }
}
