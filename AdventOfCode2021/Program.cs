// See https://aka.ms/new-console-template for more information
using AdventOfCode2021;
using AdventOfCode2021.Day1;
using AdventOfCode2021.Day2;
using AdventOfCode2021.Day3;
using System.Diagnostics;

var targetDay = 3;

Console.WriteLine("*************** Advent of Code 2021 ***************");
Console.WriteLine($"\nSolving day: {targetDay}\n");

//
IPuzzleSolver solver;
switch (targetDay)
{
    case 1:
        solver = new Day1Solver();
        break;

    case 2:
        solver = new Day2Solver();
        break;

    case 3:
        solver = new Day3Solver();
        break;

    default:
        throw new Exception("Day not yet solved!");
}


// Read inputs
var inputFilenames = new List<string>
{
    "example.txt",
    "input.txt"
};

// Start
var timer = new Stopwatch();

var puzzleDir = Path.Combine(Directory.GetCurrentDirectory(), $"Day{targetDay}");
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
    Console.WriteLine($"Part one result: {resultPartTwo} | Took: {timer.Elapsed.TotalSeconds}");

    Console.WriteLine();
}
