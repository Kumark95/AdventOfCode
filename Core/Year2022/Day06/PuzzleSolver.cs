using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Year2022.Day06.Model;

namespace AdventOfCode.Core.Year2022.Day06;

[PuzzleName("Tuning Trouble")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 6;

    public long SolvePartOne(string[] inputLines)
    {
        return Subrutine.FindStartOfPacket(inputLines[0], markerLenght: 4);
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        return Subrutine.FindStartOfPacket(inputLines[0], markerLenght: 14);
    }
}
