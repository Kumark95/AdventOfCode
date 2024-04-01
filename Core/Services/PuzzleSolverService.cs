using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode.Core.Services;

internal sealed class PuzzleSolverService
{
    private readonly ILogger<PuzzleSolverService> _logger;

    public PuzzleSolverService(ILogger<PuzzleSolverService> logger)
    {
        _logger = logger;
    }

    public void Solve(string solverDirectory, IPuzzleSolver solver)
    {
        var solverType = solver.GetType();
        var puzzleNameAttribute = solverType.GetCustomAttribute<PuzzleName>();
        if (puzzleNameAttribute is not null)
        {
            _logger.LogInformation("Puzzle name: {PuzzleName}", puzzleNameAttribute.Name);
        }

        ConsoleDisplayManager.DisplayHeader();

        _logger.LogInformation("Solving Part One");
        var partOneInputs = solverType.GetMethod(nameof(IPuzzleSolver.SolvePartOne))!.GetCustomAttributes<PuzzleInput>();
        var partOneResults = FetchInputsAndSolve(solverDirectory, partOneInputs, solver.SolvePartOne);
        ConsoleDisplayManager.DisplayResults(partOneResults);

        _logger.LogInformation("Solving Part Two");
        var partTwoInputs = solverType.GetMethod(nameof(IPuzzleSolver.SolvePartTwo))!.GetCustomAttributes<PuzzleInput>();
        var partTwoResults = FetchInputsAndSolve(solverDirectory, partTwoInputs, solver.SolvePartTwo);
        ConsoleDisplayManager.DisplayResults(partTwoResults);
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

    private IEnumerable<ExecutionResult> FetchInputsAndSolve(string solverDirectory, IEnumerable<PuzzleInput> inputs, Func<string[], object> solverMethod)
    {
        if (!inputs.Any())
        {
            _logger.LogWarning("No input files provided");
        }

        foreach (var input in inputs)
        {
            _logger.LogInformation("Using {Filename}", input.Filename);
            var inputFilePath = Path.Combine(solverDirectory, input.Filename);
            if (!File.Exists(inputFilePath))
            {
                _logger.LogWarning("Input file {FilePath} does not exists. Skipping...", inputFilePath);
                continue;
            }

            var inputContent = File.ReadAllLines(inputFilePath);

            var (result, executionMetrics) = ExecuteAndMeasure(() => solverMethod(inputContent));

            yield return new ExecutionResult(input.Filename, input.ExpectedResult, result, executionMetrics);
        }
    }

    private static (ulong, ExecutionMetrics) ExecuteAndMeasure(Func<object> calculation)
    {
        var startMemory = 0L;
        using (Process proc = Process.GetCurrentProcess())
        {
            startMemory = Process.GetCurrentProcess().PrivateMemorySize64;
        }
        var startTimestamp = Stopwatch.GetTimestamp();
        var result = calculation();
        var elapsed = Stopwatch.GetElapsedTime(startTimestamp);
        var endMemory = 0L;
        using (Process proc = Process.GetCurrentProcess())
        {
            endMemory = Process.GetCurrentProcess().PrivateMemorySize64;
        }

        var memoryUsed = endMemory - startMemory;
        if (memoryUsed < 0)
        {
            memoryUsed = 0;
        }

        return (Convert.ToUInt64(result), new ExecutionMetrics(elapsed, memoryUsed));
    }
}
