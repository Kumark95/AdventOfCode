// See https://aka.ms/new-console-template for more information
using AdventOfCode2021;
using AdventOfCode2021.Day1;
using AdventOfCode2021.Day2;
using AdventOfCode2021.Day3;

var currentDay = 3;

Console.WriteLine("*************** Advent of Code 2021 ***************");
Console.WriteLine($"\nSolving day: {currentDay}\n");

//
IPuzzleSolver solver;
switch (currentDay)
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

solver.Solve();
