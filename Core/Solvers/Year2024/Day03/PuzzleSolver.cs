using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2024.Day03;

[PuzzleName("Mull It Over")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 3;

    [PuzzleInput(filename: "example.txt", expectedResult: 161)]
    [PuzzleInput(filename: "input.txt", expectedResult: 156388521)]
    public object SolvePartOne(string[] inputLines)
    {
        var result = 0;

        // language=regex
        var pattern = @"mul\((?<Left>\d{1,3}),(?<Right>\d{1,3})\)";
        foreach (var line in inputLines)
        {
            foreach (Match m in Regex.Matches(line, pattern))
            {
                result += int.Parse(m.Groups["Left"].Value) * int.Parse(m.Groups["Right"].Value);
            }
        }

        return result;
    }

    [PuzzleInput(filename: "example-2.txt", expectedResult: 48)]
    [PuzzleInput(filename: "input.txt", expectedResult: 75920122)]
    public object SolvePartTwo(string[] inputLines)
    {
        var result = 0;

        // language=regex
        var pattern = @"mul\((?<Left>\d{1,3}),(?<Right>\d{1,3})\)|do\(\)|don't\(\)";

        // Use a single line as some "don't" operations can affect the next line
        var joinedInputLine = string.Join("", inputLines);

        var isValid = true;
        foreach (Match m in Regex.Matches(joinedInputLine, pattern))
        {
            var matchResult = m.Value;

            if (matchResult == "don't()")
            {
                isValid = false;
            }
            else if (matchResult == "do()")
            {
                isValid = true;
            }
            else if (isValid)
            {
                result += int.Parse(m.Groups["Left"].Value) * int.Parse(m.Groups["Right"].Value);
            }
        }

        return result;
    }
}
