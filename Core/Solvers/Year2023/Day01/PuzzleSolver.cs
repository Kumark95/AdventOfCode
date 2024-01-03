using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Solvers.Year2023.Day01;

[PuzzleName("Trebuchet?!")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 1;

    [PuzzleInput(filename: "example-1.txt", expectedResult: 142)]
    [PuzzleInput(filename: "input.txt", expectedResult: 55538)]
    public object SolvePartOne(string[] inputLines)
    {
        return inputLines
            .Select(ExtractSimpleCalibrationValue)
            .Sum();
    }

    [PuzzleInput(filename: "example-2.txt", expectedResult: 281)]
    [PuzzleInput(filename: "input.txt", expectedResult: 54875)]
    public object SolvePartTwo(string[] inputLines)
    {
        return inputLines
            .Select(ExtractComplexCalibrationValue)
            .Sum();
    }

    private int ExtractSimpleCalibrationValue(string line)
    {
        var numbers = new List<char>();
        for (var i = 0; i < line.Length; i++)
        {
            var character = line[i];

            if (character >= '0' && character <= '9')
            {
                numbers.Add(character);
            }
        }

        return int.Parse(new string([numbers.First(), numbers.Last()]));
    }

    private int ExtractComplexCalibrationValue(string line)
    {
        var numbers = new List<char>();
        for (var i = 0; i < line.Length; i++)
        {
            var character = line[i];

            if (character >= '0' && character <= '9')
            {
                numbers.Add(character);
            }
            else if (character == 'o')
            {
                if ((i + 2) < line.Length && line.Substring(i, 3) == "one")
                {
                    numbers.Add('1');
                }
            }
            else if (character == 't')
            {
                if ((i + 2) < line.Length && line.Substring(i, 3) == "two")
                {
                    numbers.Add('2');
                }
                else if ((i + 4) < line.Length && line.Substring(i, 5) == "three")
                {
                    numbers.Add('3');
                }
            }
            else if (character == 'f')
            {
                if ((i + 3) < line.Length && line.Substring(i, 4) == "four")
                {
                    numbers.Add('4');
                }
                else if ((i + 3) < line.Length && line.Substring(i, 4) == "five")
                {
                    numbers.Add('5');
                }
            }
            else if (character == 's')
            {
                if ((i + 2) < line.Length && line.Substring(i, 3) == "six")
                {
                    numbers.Add('6');
                }
                else if ((i + 4) < line.Length && line.Substring(i, 5) == "seven")
                {
                    numbers.Add('7');
                }
            }
            else if (character == 'e')
            {
                if ((i + 4) < line.Length && line.Substring(i, 5) == "eight")
                {
                    numbers.Add('8');
                }
            }
            else if (character == 'n')
            {
                if ((i + 3) < line.Length && line.Substring(i, 4) == "nine")
                {
                    numbers.Add('9');
                }
            }
        }

        return int.Parse(new string([numbers.First(), numbers.Last()]));
    }
}
