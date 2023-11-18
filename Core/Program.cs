// See https://aka.ms/new-console-template for more information
using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;


Console.WriteLine("**********************************************");
Console.WriteLine("*************** Advent of Code ***************");
Console.WriteLine("**********************************************");
Console.WriteLine();

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
        Console.WriteLine("Usage: dotnet run <Year> <Day>");
        Environment.Exit(1);
        return;
}

Console.WriteLine($"Solving day {targetDay} of year {targetYear}");

// Load classes implementing IPuzzleSolver
List<IPuzzleSolver> solvers = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => typeof(IPuzzleSolver).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
    .Select(t => Activator.CreateInstance(t) as IPuzzleSolver)
    .OfType<IPuzzleSolver>()
    .ToList();

// Select target solver
var solver = solvers.SingleOrDefault(s => s.Year == targetYear && s.Day == targetDay);
if (solver is null)
{
    Console.WriteLine("No solver available");
    Console.WriteLine();
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
    Console.WriteLine($"Part one result: {resultPartOne} | Took: {timer.Elapsed.Seconds}s {timer.Elapsed.Milliseconds}ms");

    timer.Restart();
    var resultPartTwo = solver.SolvePartTwo(inputContent);
    timer.Stop();

    if (resultPartTwo is not null)
    {
        Console.WriteLine($"Part two result: {resultPartTwo} | Took: {timer.Elapsed.Seconds}s {timer.Elapsed.Milliseconds}ms");
    }
    else
    {
        Console.WriteLine("Part two not yet unlocked");
    }

    Console.WriteLine();
}

static (int year, int day) GetMostRecentYearDay()
{
    var directory = Directory.GetDirectories(".", "Day*", SearchOption.AllDirectories)
        .Max() ?? throw new InvalidOperationException("No directories found");

    Console.WriteLine(directory);

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
