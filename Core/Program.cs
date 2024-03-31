using AdventOfCode.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics.CodeAnalysis;

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
if (args.Length != 2)
{
    ShowUsageAndExit();
}

int targetYear = int.Parse(args[0]);
int targetDay = int.Parse(args[1]);

// Setup
var outputDirectory = AppContext.BaseDirectory;
var projectDirectory = Directory.GetParent(outputDirectory)?.Parent?.Parent?.Parent?.FullName
    ?? throw new InvalidOperationException("The project directory could not be determined");

var solverDirectory = Path.Join(projectDirectory, "Solvers", $"Year{targetYear:D4}", $"Day{targetDay:D2}");

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var puzzleSolverService = host.Services.GetRequiredService<PuzzleSolverService>();

// Validate
if (!AdventOfCodeService.IsValidDay(targetYear, targetDay))
{
    logger.LogWarning("Invalid day/year");
    return;
}

// Solve the puzzle
logger.LogInformation("Solving day {Day} of year {Year}", targetDay, targetYear);
var solver = puzzleSolverService.FindSolver(targetYear, targetDay);
if (solver is not null)
{
    puzzleSolverService.Solve(solverDirectory, solver);
}
else
{
    logger.LogInformation("No solver available. Setting up directories...");
    var puzzleSetupService = host.Services.GetRequiredService<PuzzleSetupService>();
    await puzzleSetupService.SetupFiles(solverDirectory, targetYear, targetDay);

    Environment.Exit(1);
}

[DoesNotReturn]
static void ShowUsageAndExit()
{
    Console.WriteLine("Usage: dotnet run <Year> <Day> [PuzzleName]");
    Environment.Exit(1);
}
