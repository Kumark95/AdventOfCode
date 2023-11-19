using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configBuilder =>
    {
        configBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configBuilder.AddJsonFile("appsettings.json", optional: false);
        configBuilder.AddUserSecrets<Program>();
    })
    .ConfigureServices(services =>
    {
        services.AddHttpClient<AdventOfCodeService>();
        services.AddTransient<PuzzleSetupService>();
    })
    .UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    })
    .Build();


Console.WriteLine("**********************************************");
Console.WriteLine("*************** Advent of Code ***************");
Console.WriteLine("**********************************************");
Console.WriteLine();

int targetYear, targetDay;

switch (args.Length)
{
    case 0:
        Console.WriteLine("Running the most recent solver");
        (targetYear, targetDay) = GetMostRecentYearDay();
        break;
    case 2:
        targetYear = int.Parse(args[0]);
        targetDay = int.Parse(args[1]);
        break;
    default:
        ShowUsageAndExit();
        return;
}

Console.WriteLine($"Solving day {targetDay} of year {targetYear}");
var solver = FindSolver(targetYear, targetDay);
if (solver is null)
{
    Console.WriteLine("No solver available. Setup directories? (y/n)");
    var answer = Console.ReadLine();
    if (answer == "y")
    {
        var puzzleSetupService = host.Services.GetRequiredService<PuzzleSetupService>();

        var outputDirectory = AppContext.BaseDirectory;
        var projectDirectory = Directory.GetParent(outputDirectory)?.Parent?.Parent?.Parent?.FullName
            ?? throw new InvalidOperationException("The project directory could not be determined");

        var solverDirectory = Path.Join(projectDirectory, "Solvers");

        await puzzleSetupService.SetupFiles(solverDirectory, targetYear, targetDay);
    }

    Environment.Exit(1);
}

//
var puzzleNameAttribute = solver.GetType().GetCustomAttribute<PuzzleName>();
if (puzzleNameAttribute is not null)
{
    Console.WriteLine($"Puzzle name: {puzzleNameAttribute.Name}");
}
Console.WriteLine();

// Read inputs
var inputFilenames = new List<string>
{
    "example.txt",
    "input.txt"
};

// Start
var timer = new Stopwatch();

var puzzleDir = GetSolverDirectory(targetYear, targetDay);
foreach (var inputFilename in inputFilenames)
{
    var inputFile = Path.Combine(puzzleDir, inputFilename);
    Console.WriteLine($"Using {inputFilename}");
    if (!File.Exists(inputFile))
    {
        Console.WriteLine($"Input file {inputFile} does not exists. Skipping...");
        continue;
    }

    var inputContent = File.ReadAllLines(inputFile);

    timer.Restart();
    var resultPartOne = solver.SolvePartOne(inputContent);
    timer.Stop();
    Console.WriteLine($"Part one result: {resultPartOne} | Took: {timer.Elapsed.Seconds}s {timer.Elapsed.Milliseconds}ms {timer.Elapsed.Microseconds}us");

    timer.Restart();
    var resultPartTwo = solver.SolvePartTwo(inputContent);
    timer.Stop();

    if (resultPartTwo is not null)
    {
        Console.WriteLine($"Part two result: {resultPartTwo} | Took: {timer.Elapsed.Seconds}s {timer.Elapsed.Milliseconds}ms {timer.Elapsed.Microseconds}us");
    }
    else
    {
        Console.WriteLine("Part two not yet unlocked");
    }
}


[DoesNotReturn]
static void ShowUsageAndExit()
{
    Console.WriteLine("Usage: dotnet run <Year> <Day> [PuzzleName]");
    Environment.Exit(1);
}

static string GetSolverDirectory(int year, int day)
{
    return Path.Combine(Directory.GetCurrentDirectory(), "Solvers", $"Year{year:D4}", $"Day{day:D2}");
}

static IPuzzleSolver? FindSolver(int year, int day)
{
    List<IPuzzleSolver> solvers = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => typeof(IPuzzleSolver).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
        .Select(t => Activator.CreateInstance(t) as IPuzzleSolver)
        .OfType<IPuzzleSolver>()
        .ToList();

    // Select target solver
    return solvers.SingleOrDefault(s => s.Year == year && s.Day == day);
}

static (int year, int day) GetMostRecentYearDay()
{
    var directory = Directory.GetDirectories(".", "Day*", SearchOption.AllDirectories)
        .Max() ?? throw new InvalidOperationException("No directories found");

    var regex = new Regex(@"Year(?<Year>\d{4})[\/\\]Day(?<Day>\d{2})$");
    var match = regex.Match(directory);
    if (!match.Success)
    {
        throw new InvalidOperationException("Could not extract year and day from the directory name");
    }

    var year = int.Parse(match.Groups["Year"].Value);
    var day = int.Parse(match.Groups["Day"].Value);

    return (year, day);
}
