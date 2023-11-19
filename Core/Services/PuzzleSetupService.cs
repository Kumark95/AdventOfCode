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
        if (!Directory.Exists(solverDirectory))
        {
            Directory.CreateDirectory(solverDirectory);
        }

        var puzzleName = await _adventOfCodeService.GetPuzzleName(year, day);

        _logger.LogDebug("Generating template files");
        File.WriteAllText(Path.Combine(solverDirectory, "example.txt"), string.Empty);
        File.WriteAllText(Path.Combine(solverDirectory, "input.txt"), await _adventOfCodeService.GetPuzzleInput(year, day));
        File.WriteAllText(Path.Combine(solverDirectory, "PuzzleSolver.cs"), GenerateSolver(year, day, puzzleName));
    }

    private static string GenerateSolver(int year, int day, string puzzleName)
    {
        return $$"""
            using AdventOfCode.Common.Attributes;
            using AdventOfCode.Common.Interfaces;

            namespace AdventOfCode.Core.Year{{year:D4}}.Day{{day:D2}};

            [PuzzleName("{{puzzleName}}")]
            public class PuzzleSolver : IPuzzleSolver
            {
                public int Year => {{year}};
                public int Day => {{day}};

                public long SolvePartOne(string[] inputLines)
                {
                    return 0;
                }

                public long? SolvePartTwo(string[] inputLines)
                {
                    return null;
                }
            }

            """;
    }
}
