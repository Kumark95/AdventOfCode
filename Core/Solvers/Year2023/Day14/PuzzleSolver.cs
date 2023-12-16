using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2023.Day14.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day14;

[PuzzleName("Parabolic Reflector Dish")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2023;
    public int Day => 14;

    [PuzzleInput(filename: "example.txt", expectedResult: 136)]
    [PuzzleInput(filename: "input.txt", expectedResult: 105003)]
    public long? SolvePartOne(string[] inputLines)
    {
        var reflectorDish = InputParser.ParseInput(inputLines);

        reflectorDish.Tilt(Direction.Up);

        return reflectorDish.CalculateTotalLoad();
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 64)]
    [PuzzleInput(filename: "input.txt", expectedResult: 93742)]
    public long? SolvePartTwo(string[] inputLines)
    {
        var reflectorDish = InputParser.ParseInput(inputLines);

        // The map state repeats after a full loop
        //var cycleLoop = reflectorDish.CalculateLoopCycle();
        var (cycleStartIndex, cycleLength) = reflectorDish.CalculateLoopCycle();

        // Reset the map state
        reflectorDish = InputParser.ParseInput(inputLines);

        // Only loop the needed cycles
        var targetCycles = 1_000_000_000;
        var actualNeededCycles = (targetCycles - cycleStartIndex) % cycleLength + cycleStartIndex;

        for (var cycle = 1; cycle <= actualNeededCycles; cycle++)
        {
            reflectorDish.TiltCycle();
        }

        return reflectorDish.CalculateTotalLoad();
    }
}
