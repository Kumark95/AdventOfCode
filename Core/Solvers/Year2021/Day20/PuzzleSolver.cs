using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Solvers.Year2021.Day20;

[PuzzleName("Trench Map")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 20;

    private static TrenchMap Input(string[] inputLines)
    {
        return new TrenchMap(inputLines[0], inputLines[2..]);
    }

    public long SolvePartOne(string[] inputLines)
    {
        var map = Input(inputLines);

        return map
            .ApplyEnhancement(iterations: 2)
            .Image.CountLightPixels();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var map = Input(inputLines);

        return map
            .ApplyEnhancement(iterations: 50)
            .Image.CountLightPixels();
    }
}
