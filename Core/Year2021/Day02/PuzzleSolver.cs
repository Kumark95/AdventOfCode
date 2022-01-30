using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Year2021.Day2;


[PuzzleName("Dive!")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 2;

    private readonly Regex _regexExp;

    public PuzzleSolver()
    {
        _regexExp = new Regex(@"^(?<Instruction>\w+) (?<Number>\d+)$");
    }

    private List<string> Input(string[] inputLines) => inputLines.ToList();

    public long SolvePartOne(string[] inputLines)
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

    public long? SolvePartTwo(string[] inputLines)
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
