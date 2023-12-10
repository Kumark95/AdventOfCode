using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2021.Day25.Model;

namespace AdventOfCode.Core.Solvers.Year2021.Day25;

[PuzzleName("Sea Cucumber")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 25;

    public long? SolvePartOne(string[] inputLines)
    {
        var floor = new SeaFloor(inputLines);
        return floor.StepsUntilStop();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        return null;
    }
}
