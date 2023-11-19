using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode.Core.Services;

internal sealed class PuzzleSolverService
{
    private readonly ILogger<PuzzleSolverService> _logger;

    private readonly string[] _sampleFiles = ["example.txt", "input.txt"];

    public PuzzleSolverService(ILogger<PuzzleSolverService> logger)
    {
        _logger = logger;
    }

    public void Solve(string solverDirectory, IPuzzleSolver solver)
    {
        var puzzleNameAttribute = solver.GetType().GetCustomAttribute<PuzzleName>();
        if (puzzleNameAttribute is not null)
        {
            _logger.LogInformation("Puzzle name: {PuzzleName}", puzzleNameAttribute.Name);
        }

        // Start
        var timer = new Stopwatch();

        foreach (var sampelFile in _sampleFiles)
        {
            var sampleFilePath = Path.Combine(solverDirectory, sampelFile);
            _logger.LogInformation("Using {SampleFile}", sampelFile);
            if (!File.Exists(sampleFilePath))
            {
                _logger.LogWarning("Input file {SampleFilePath} does not exists. Skipping...", sampleFilePath);
                continue;
            }

            var inputContent = File.ReadAllLines(sampleFilePath);

            timer.Restart();
            var resultPartOne = solver.SolvePartOne(inputContent);
            timer.Stop();
            _logger.LogInformation("Part one result: {Result} | Took: {ElapsedSeconds}s {ElapsedMilliseconds}ms {ElapsedMicroseconds}us",
                resultPartOne, timer.Elapsed.Seconds, timer.Elapsed.Milliseconds, timer.Elapsed.Microseconds);

            timer.Restart();
            var resultPartTwo = solver.SolvePartTwo(inputContent);
            timer.Stop();

            if (resultPartTwo is null)
            {
                _logger.LogInformation("Part two not yet unlocked");
            }
            else
            {
                _logger.LogInformation("Part two result: {Result} | Took: {ElapsedSeconds}s {ElapsedMilliseconds}ms {ElapsedMicroseconds}us",
                    resultPartTwo, timer.Elapsed.Seconds, timer.Elapsed.Milliseconds, timer.Elapsed.Microseconds);
            }
        }
    }

    public IPuzzleSolver? FindSolver(int year, int day)
    {
        _logger.LogDebug("Searching asembly for solvers");
        List<IPuzzleSolver> solvers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IPuzzleSolver).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => Activator.CreateInstance(t) as IPuzzleSolver)
            .OfType<IPuzzleSolver>()
            .ToList();

        // Select target solver
        return solvers.SingleOrDefault(s => s.Year == year && s.Day == day);
    }
}
