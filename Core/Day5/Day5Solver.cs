using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Day5;

[PuzzleName("Hydrothermal Venture")]
public class Day5Solver : IPuzzleSolver
{
    private List<VentLine> Input(string[] inputLines, bool excludeDiagonal)
    {
        return inputLines
            .Select(line => new VentLine(line, excludeDiagonal))
            .Where(ventLine => ventLine.Points.Count > 0)
            .ToList();
    }

    private List<Point> OverlappingPoints(List<VentLine> ventLines) => ventLines
            .SelectMany(line => line.Points)
            .GroupBy(point => point)
            .Where(group => group.Count() >= 2)
            .Select(group => group.Key)
            .ToList();

    public long SolvePartOne(string[] inputLines)
    {
        var input = Input(inputLines, excludeDiagonal: true);
        return OverlappingPoints(input).Count();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var input = Input(inputLines, excludeDiagonal: false);
        return OverlappingPoints(input).Count();
    }
}
