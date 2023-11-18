using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

IConfiguration configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
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
        Console.WriteLine("Please provide the puzzle name");
        var newPuzzleName = Console.ReadLine();

        // TODO: Integrate with DI
        var sessionCookie = configuration["SessionCookie"];

        await SetupSolver(targetYear, targetDay, newPuzzleName ?? "", sessionCookie);
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

var puzzleDir = Path.Combine(Directory.GetCurrentDirectory(), $"Year{targetYear}", $"Day{targetDay:D2}");
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

static async Task SetupSolver(int year, int day, string puzzleName, string? sessionCookie)
{
    string directory = Path.Combine(Directory.GetCurrentDirectory(), $"Year{year:D4}", $"Day{day:D2}");
    Directory.CreateDirectory(directory);

    File.WriteAllText(Path.Combine(directory, "example.txt"), string.Empty);

    if (sessionCookie is not null)
    {
        var puzzleInput = await FetchPuzzleInput(year, day, sessionCookie);
        File.WriteAllText(Path.Combine(directory, "input.txt"), puzzleInput);
    }
    else
    {
        File.WriteAllText(Path.Combine(directory, "input.txt"), string.Empty);
    }

    var puzzleSolverTemplate = $$"""
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
    File.WriteAllText(Path.Combine(directory, "PuzzleSolver.cs"), puzzleSolverTemplate);

    var readmeTemplate = $"""
        # Day {day}: {puzzleName}

        ## Part 1


        ## Part 2

        """;
    File.WriteAllText(Path.Combine(directory, "README.md"), readmeTemplate);
}

static async Task<string?> FetchPuzzleInput(int year, int day, string sessionCookie)
{
    var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Add("Cookie", sessionCookie);

    try
    {
        Console.WriteLine("Requesting puzzle input");
        return await httpClient.GetStringAsync($"https://adventofcode.com/{year}/day/{day}/input");
    }
    catch (HttpRequestException exception)
    {
        Console.WriteLine("Error: " + exception.Message);
        return null;
    }
}

static (int year, int day) GetMostRecentYearDay()
{
    var directory = Directory.GetDirectories(".", "Day*", SearchOption.AllDirectories)
        .Max() ?? throw new InvalidOperationException("No directories found");

    var regex = new Regex(@"Year(\d{4})[\/\\]Day(\d{2})$");
    var match = regex.Match(directory);

    if (!match.Success)
    {
        throw new InvalidOperationException("Could not extract year and day from the directory name");
    }

    var year = int.Parse(match.Groups[1].Value);
    var day = int.Parse(match.Groups[2].Value);

    return (year, day);
}
