using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2022.Day08.Model;

namespace AdventOfCode.Core.Solvers.Year2022.Day08;

[PuzzleName("Treetop Tree House")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 8;

    public long SolvePartOne(string[] inputLines)
    {
        var forestMap = new Forest(inputLines);
        return forestMap.CountVisibleTrees();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var forestMap = new Forest(inputLines);
        return forestMap.HeighestScenicScore();
    }
}
