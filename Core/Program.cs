// See https://aka.ms/new-console-template for more information
using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using System.Diagnostics;
using System.Reflection;

var targetYear = 2021;
var targetDay = 8;

Console.WriteLine($"*************** Advent of Code {targetYear} ***************");
Console.WriteLine($"\nSolving day: {targetDay}");


// Load classes implementing IPuzzleSolver
List<IPuzzleSolver> solvers = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => typeof(IPuzzleSolver).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
    .Select(t => Activator.CreateInstance(t) as IPuzzleSolver)
    .OfType<IPuzzleSolver>()
    .ToList();

// Select target solver
var solver = solvers.FirstOrDefault(s => s.Year == targetYear && s.Day == targetDay);
if (solver == null)
{
    throw new Exception($"No solver available for day {targetDay} of year {targetYear}");
}


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

var puzzleDir = Path.Combine(Directory.GetCurrentDirectory(), $"Year{targetYear}", $"Day{targetDay}");
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
    Console.WriteLine($"Part one result: {resultPartOne} | Took: {timer.Elapsed.TotalSeconds}");

    timer.Restart();
    var resultPartTwo = solver.SolvePartTwo(inputContent);
    timer.Stop();

    if (resultPartTwo.HasValue)
    {
        Console.WriteLine($"Part two result: {resultPartTwo} | Took: {timer.Elapsed.TotalSeconds}");
    }
    else
    {
        Console.WriteLine("Part two not yet unlocked");
    }

    Console.WriteLine();
}
