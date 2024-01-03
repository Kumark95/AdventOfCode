using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2022.Day14.Model;

namespace AdventOfCode.Core.Solvers.Year2022.Day14;

[PuzzleName("Regolith Reservoir")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 14;

    public object SolvePartOne(string[] inputLines)
    {
        var cave = CaveParser.Parse(inputLines, hasFloor: false);
        cave.FallingSand();

        return cave.SandUnitsAtRest;
    }

    public object SolvePartTwo(string[] inputLines)
    {
        var cave = CaveParser.Parse(inputLines, hasFloor: true);
        cave.FallingSand();

        return cave.SandUnitsAtRest;
    }
}
