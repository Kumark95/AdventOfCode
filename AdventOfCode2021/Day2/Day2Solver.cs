using System.Text.RegularExpressions;

namespace AdventOfCode2021.Day2;

/// <summary>
/// Puzzle for Day 2.
/// Src: https://adventofcode.com/2021/day/2
/// </summary>
public class Day2Solver : IPuzzleSolver
{
    private readonly Regex _regexExp;

    public Day2Solver()
    {
        _regexExp = new Regex(@"^(?<Instruction>\w+) (?<Number>\d+)$");
    }

    private List<string> Input(string[] inputLines) => inputLines.ToList();

    public int SolvePartOne(string[] inputLines)
    {
        var position = 0;
        var depth = 0;
        foreach (var line in Input(inputLines))
        {
            var matches = _regexExp.Match(line);
            if (!matches.Success)
            {
                throw new Exception("Could not extract instructions from input line");
            }

            var instruction = matches.Groups["Instruction"].Value;
            var number = int.Parse(matches.Groups["Number"].Value);
            switch (instruction)
            {
                case ShipCommands.Forward:
                    position += number;
                    break;

                case ShipCommands.Up:
                    depth -= number;
                    break;

                case ShipCommands.Down:
                    depth += number;
                    break;

                default:
                    throw new Exception($"Instruction not recognized {instruction}");
            }
        }

        return position * depth;
    }

    public int SolvePartTwo(string[] inputLines)
    {
        var aim = 0;
        var position = 0;
        var depth = 0;

        foreach (var line in Input(inputLines))
        {
            var matches = _regexExp.Match(line);
            if (!matches.Success)
            {
                throw new Exception("Could not extract instructions from input line");
            }

            var instruction = matches.Groups["Instruction"].Value;
            var number = int.Parse(matches.Groups["Number"].Value);
            switch (instruction)
            {
                case ShipCommands.Forward:
                    position += number;
                    depth += (aim * number);
                    break;

                case ShipCommands.Up:
                    aim -= number;
                    break;

                case ShipCommands.Down:
                    aim += number;
                    break;

                default:
                    throw new Exception($"Instruction not recognized {instruction}");
            }
        }

        return position * depth;
    }
}
