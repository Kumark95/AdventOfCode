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
        foreach (var sampleFile in _sampleFiles)
        {
            var sampleFilePath = Path.Combine(solverDirectory, sampleFile);
            _logger.LogInformation("Using {SampleFile}", sampleFile);
            if (!File.Exists(sampleFilePath))
            {
                _logger.LogWarning("Input file {SampleFilePath} does not exists. Skipping...", sampleFilePath);
                continue;
            }

            var inputContent = File.ReadAllLines(sampleFilePath);

            MeasurePerformance(() => solver.SolvePartOne(inputContent), "PartOne");
            MeasurePerformance(() => solver.SolvePartTwo(inputContent), "PartTwo");
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

    private void MeasurePerformance<T>(Func<T> func, string funcName)
    {
        var startMemory = 0L;
        using (Process proc = Process.GetCurrentProcess())
        {
            startMemory = Process.GetCurrentProcess().PrivateMemorySize64;
        }
        var startTimestamp = Stopwatch.GetTimestamp();
        var result = func();
        var elapsed = Stopwatch.GetElapsedTime(startTimestamp);
        var endMemory = 0L;
        using (Process proc = Process.GetCurrentProcess())
        {
            endMemory = Process.GetCurrentProcess().PrivateMemorySize64;
        }

        if (result is null)
        {
            _logger.LogInformation("{FuncName} not yet unlocked", funcName);
        }
        else
        {
            var elapsedMessage = elapsed.Minutes > 0
                ? $"{elapsed.Minutes}m {elapsed.Seconds}s {elapsed.Milliseconds}ms {elapsed.Microseconds}μs"
                : $"{elapsed.Seconds}s {elapsed.Milliseconds}ms {elapsed.Microseconds}μs";

            _logger.LogInformation("{FuncName} result: {Result} | Took: {Elapsed} | Memory: {MemoryUsed}",
                funcName, result, elapsedMessage, FormatBytes(endMemory - startMemory));
        }
    }

    private static string FormatBytes(long bytes)
    {
        const int scale = 1024;
        string[] orders = ["GB", "MB", "KB", "Bytes"];
        long max = (long)Math.Pow(scale, orders.Length - 1);

        foreach (string order in orders)
        {
            if (bytes > max)
            {
                return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);
            }

            max /= scale;
        }

        return "0 Bytes";
    }
}
