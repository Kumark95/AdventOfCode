// See https://aka.ms/new-console-template for more information
using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using System.Diagnostics;
using System.Reflection;


Console.WriteLine("**********************************************");
Console.WriteLine("*************** Advent of Code ***************");
Console.WriteLine("**********************************************");
Console.WriteLine();

if (args.Length != 2)
{
    Console.WriteLine("Usage: dotnet run <Year> <Day>");

    // TODO: Activate
    //Environment.Exit(1);
}

// TODO: Activate
//int targetYear = int.Parse(args[0]);
//int targetDay = int.Parse(args[1]);

int targetYear = 2021;
int targetDay = 12;


Console.WriteLine($"Solving day {targetDay} of year {targetYear}");

// Load classes implementing IPuzzleSolver
List<IPuzzleSolver> solvers = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => typeof(IPuzzleSolver).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
    .Select(t => Activator.CreateInstance(t) as IPuzzleSolver)
    .OfType<IPuzzleSolver>()
    .ToList();

// Select target solver
var solver = solvers.SingleOrDefault(s => s.Year == targetYear && s.Day == targetDay);
if (solver == null)
{
    Console.WriteLine("No solver available");
    Console.WriteLine();
    Environment.Exit(1);
}

//
var puzzleNameAttribute = solver.GetType().GetCustomAttribute<PuzzleName>();
if (puzzleNameAttribute != null)
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
    Console.WriteLine($"Part one result: {resultPartOne} | Took: {timer.Elapsed.TotalMilliseconds}");

    timer.Restart();
    var resultPartTwo = solver.SolvePartTwo(inputContent);
    timer.Stop();

    if (resultPartTwo.HasValue)
    {
        Console.WriteLine($"Part two result: {resultPartTwo} | Took: {timer.Elapsed.TotalMilliseconds}");
    }
    else
    {
        Console.WriteLine("Part two not yet unlocked");
    }

    Console.WriteLine();
}
