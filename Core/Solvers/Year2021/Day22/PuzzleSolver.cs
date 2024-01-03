using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2021.Day22.Model;

namespace AdventOfCode.Core.Solvers.Year2021.Day22;

[PuzzleName("Reactor Reboot")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 22;

    public object SolvePartOne(string[] inputLines)
    {
        return Reactor.RebootAndCountLightCubes(inputLines, onlyInitialyze: true);
    }

    public object SolvePartTwo(string[] inputLines)
    {
        return Reactor.RebootAndCountLightCubes(inputLines, onlyInitialyze: false);
    }
}
