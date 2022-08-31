using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Year2021.Day24.Model;

namespace AdventOfCode.Core.Year2021.Day24;

[PuzzleName("Arithmetic Logic Unit")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 24;

    public long SolvePartOne(string[] inputLines)
    {
        return ArithmeticLogicUnit.FindModelNumber(inputLines, SearchMode.Maximum);
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        return ArithmeticLogicUnit.FindModelNumber(inputLines, SearchMode.Minimum);
    }
}
