using AdventOfCode.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

// Configure DI and services
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
        services.AddTransient<PuzzleSolverService>();
    })
    .UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    })
    .Build();


// Read parameters from console args
int targetYear, targetDay;
switch (args.Length)
{
    case 0:
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

// Setup
var outputDirectory = AppContext.BaseDirectory;
var projectDirectory = Directory.GetParent(outputDirectory)?.Parent?.Parent?.Parent?.FullName
    ?? throw new InvalidOperationException("The project directory could not be determined");
var solverDirectory = Path.Join(projectDirectory, "Solvers", $"Year{targetYear:D4}", $"Day{targetDay:D2}");

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var puzzleSetupService = host.Services.GetRequiredService<PuzzleSetupService>();
var puzzleSolverService = host.Services.GetRequiredService<PuzzleSolverService>();

// Solve the puzzle
logger.LogInformation("Solving day {Day} of year {Year}", targetDay, targetYear);
var solver = puzzleSolverService.FindSolver(targetYear, targetDay);
if (solver is not null)
{
    puzzleSolverService.Solve(solverDirectory, solver);
}
else
{
    Console.WriteLine("No solver available. Setup directories? (y/n)");
    var answer = Console.ReadLine();
    if (answer == "y")
    {
        await puzzleSetupService.SetupFiles(solverDirectory, targetYear, targetDay);
    }

    Environment.Exit(1);
}

[DoesNotReturn]
static void ShowUsageAndExit()
{
    Console.WriteLine("Usage: dotnet run <Year> <Day> [PuzzleName]");
    Environment.Exit(1);
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
