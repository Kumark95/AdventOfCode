using AdventOfCode.Common.Algorithms;
using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day20.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day20;

[PuzzleName("Pulse Propagation")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 20;

    [PuzzleInput(filename: "example.txt", expectedResult: 32_000_000)]
    [PuzzleInput(filename: "example-2.txt", expectedResult: 11_687_500)]
    [PuzzleInput(filename: "input.txt", expectedResult: 949_764_474)]
    public object SolvePartOne(string[] inputLines)
    {
        var modules = InputParser.ParseInput(inputLines);
        var machine = new Machine(modules);

        var targetButtonPresses = 1000;
        for (int i = 0; i < targetButtonPresses; i++)
        {
            machine.PushButton();
        }

        return machine.TotalLowPulses * machine.TotalHighPulses;
    }

    [PuzzleInput(filename: "input.txt", expectedResult: 243_221_023_462_303)]
    public object SolvePartTwo(string[] inputLines)
    {
        var modules = InputParser.ParseInput(inputLines);
        var machine = new Machine(modules);

        // The "rx" module only has one input and it is always a conjunction
        var targetModule = (ConjunctionModule)machine.GetModule("rx").Inputs[0];

        while (true)
        {
            machine.PushButton();

            if (targetModule.MinButtonPressesForHighPulse.Count == targetModule.Inputs.Count)
            {
                break;
            }
        }

        return targetModule.MinButtonPressesForHighPulse.Values.Aggregate(Arithmetic.LowestCommonMultiplier);
    }
}
