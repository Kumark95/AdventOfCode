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

        _logger.LogInformation("Solving Part One");
        var partOneInputs = solverType.GetMethod(nameof(IPuzzleSolver.SolvePartOne))!.GetCustomAttributes<PuzzleInput>();
        FetchInputsAndSolve(solverDirectory, partOneInputs, solver.SolvePartOne);

        _logger.LogInformation("Solving Part Two");
        var partTwoInputs = solverType.GetMethod(nameof(IPuzzleSolver.SolvePartTwo))!.GetCustomAttributes<PuzzleInput>();
        FetchInputsAndSolve(solverDirectory, partTwoInputs, solver.SolvePartTwo);
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

    private void FetchInputsAndSolve(string solverDirectory, IEnumerable<PuzzleInput> inputs, Func<string[], object> solverMethod)
    {
        if (!inputs.Any())
        {
            _logger.LogWarning("No input files provided");
        }

        var table = new Table();
        table.AddColumn("File");
        table.AddColumn("Expected");
        table.AddColumn("Value");
        table.AddColumn("Result");
        table.AddColumn("Execution time");
        table.AddColumn("Memory used");

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

            var executionData = ExecuteAndMeasure(() => solverMethod(inputContent));
            if (executionData.Result is null)
            {
                _logger.LogInformation("");
                break;
            }

            // Converting both to the same type to ensure that the equality does not faile with ints/longs
            var resultMatch = Convert.ToUInt64(executionData.Result) == input.ExpectedResult
                ? "PASS"
                : "FAIL";

            _logger.LogInformation("Result: {Result} | Expected result: {ExpectedResult} ({ResultMatch}) | Execution time: {ExecutionTime} | Memory used: {MemoryUsed}",
                executionData.Result,
                input.ExpectedResult,
                resultMatch,
                FormatTimeSpan(executionData.ExecutionTime), FormatBytes(executionData.MemoryUsed));


            // Results
            var resultHighlight = resultMatch == "PASS"
                ? "[green]Ok[/]"
                : "[red]Fail[/]";
            table.AddRow(input.Filename, input.ExpectedResult.ToString(), executionData.Result.ToString()!, resultHighlight,
                FormatTimeSpan(executionData.ExecutionTime), FormatBytes(executionData.MemoryUsed));

            if (resultMatch == "FAIL")
            {
                _logger.LogWarning("Halting further tests as the previous results did not match");
                break;
            }
        }

        AnsiConsole.Write(table);
    }

    private static ExecutionData ExecuteAndMeasure(Func<object> calculation)
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

        return new ExecutionData(result, elapsed, memoryUsed);
    }

    private static string FormatTimeSpan(TimeSpan elapsed)
    {
        return elapsed.Minutes > 0
                ? $"{elapsed.Minutes}m {elapsed.Seconds}s {elapsed.Milliseconds}ms {elapsed.Microseconds}μs"
                : $"{elapsed.Seconds}s {elapsed.Milliseconds}ms {elapsed.Microseconds}μs";
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
