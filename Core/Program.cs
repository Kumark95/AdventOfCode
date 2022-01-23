// See https://aka.ms/new-console-template for more information
using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Day1;
using AdventOfCode.Core.Day2;
using AdventOfCode.Core.Day3;
using AdventOfCode.Core.Day4;
using AdventOfCode.Core.Day5;
using AdventOfCode.Core.Day6;
using System.Diagnostics;
using System.Reflection;

var targetDay = 6;

Console.WriteLine("*************** Advent of Code 2021 ***************");
Console.WriteLine($"\nSolving day: {targetDay}");

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

    case 4:
        solver = new Day4Solver();
        break;

    case 5:
        solver = new Day5Solver();
        break;

    case 6:
        solver = new Day6Solver();
        break;

    default:
        throw new Exception("Day not yet solved!");
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
