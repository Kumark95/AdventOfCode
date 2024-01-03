using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2022.Day06.Model;

namespace AdventOfCode.Core.Solvers.Year2022.Day06;

[PuzzleName("Tuning Trouble")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 6;

    public object SolvePartOne(string[] inputLines)
    {
        return Subrutine.FindStartOfPacket(inputLines[0], markerLenght: 4);
    }

    public object SolvePartTwo(string[] inputLines)
    {
        return Subrutine.FindStartOfPacket(inputLines[0], markerLenght: 14);
    }
}
