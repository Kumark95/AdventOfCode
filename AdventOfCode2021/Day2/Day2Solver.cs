using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Day2;

/// <summary>
/// Puzzle for Day 1.
/// Src: https://adventofcode.com/2021/day/1
/// </summary>
public class Day2Solver : IPuzzleSolver
{
    private const string Forward = "forward";
    private const string Up = "up";
    private const string Down = "down";

    public void Solve()
    {
        var timer = new Stopwatch();

        // Parse input
        var inputFile = Path.Combine(Directory.GetCurrentDirectory(), "Day2", "input.txt");
        var inputData = File.ReadAllLines(inputFile).ToList();

        // Part 1
        timer.Start();

        Console.WriteLine("Part 1: What do you get if you multiply your final horizontal position by your final depth?");
        var regexExp = new Regex(@"^(?<Instruction>\w+) (?<Number>\d+)$");

        var position = 0;
        var depth = 0;
        foreach (var line in inputData)
        {
            var matches = regexExp.Match(line);
            if (!matches.Success)
            {
                throw new Exception("Could not extract instructions from input line");
            }

            var instruction = matches.Groups["Instruction"].Value;
            var number = int.Parse(matches.Groups["Number"].Value);
            switch (instruction)
            {
                case Forward:
                    position += number;
                    break;

                case Up:
                    depth -= number;
                    break;

                case Down:
                    depth += number;
                    break;

                default:
                    throw new Exception($"Instruction not recognized {instruction}");
            }
        }

        var resultPart1 = position * depth;

        timer.Stop();
        Console.WriteLine($"Answer: {resultPart1} | Took {timer.Elapsed.TotalSeconds} seconds\n");

        //
        timer.Restart();

        Console.WriteLine("Part 2: What do you get if you multiply your final horizontal position by your final depth?");
        // Restart
        var aim = 0;
        position = depth = 0;


        //inputData = new List<string>()
        //{
        //    "forward 5",
        //    "down 5",
        //    "forward 8",
        //    "up 3",
        //    "down 8",
        //    "forward 2"
        //};

        foreach (var line in inputData)
        {
            var matches = regexExp.Match(line);
            if (!matches.Success)
            {
                throw new Exception("Could not extract instructions from input line");
            }

            var instruction = matches.Groups["Instruction"].Value;
            var number = int.Parse(matches.Groups["Number"].Value);
            switch (instruction)
            {
                case Forward:
                    position += number;
                    depth += (aim * number);
                    break;

                case Up:
                    aim -= number;
                    break;

                case Down:
                    aim += number;
                    break;

                default:
                    throw new Exception($"Instruction not recognized {instruction}");
            }
        }

        var resultPart2 = position * depth;

        timer.Stop();
        Console.WriteLine($"Answer: {resultPart2} | Took {timer.Elapsed.TotalSeconds} seconds\n");
    }
}
