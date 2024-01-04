using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day20.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day20;

[PuzzleName("Pulse Propagation")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 20;

    [PuzzleInput(filename: "example.txt", expectedResult: 999_999_999_999)]
    [PuzzleInput(filename: "input.txt", expectedResult: 999_999_999_999)]
    public object SolvePartOne(string[] inputLines)
    {
        var modules = InputParser.ParseInput(inputLines);

        var machine = new Machine(modules);
        var (lowPulses, highPulses) = machine.PushButton();

        return lowPulses * highPulses;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 999_999_999_999)]
    [PuzzleInput(filename: "input.txt", expectedResult: 999_999_999_999)]
    public object SolvePartTwo(string[] inputLines)
    {
        return null;
    }
}
