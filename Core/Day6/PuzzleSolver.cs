using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Day6;

[PuzzleName("Lanternfish")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 6;

    private static List<int> Input(string line) => line
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(i => int.Parse(i))
            .ToList();

    private static long CalculateSchoolSize(string[] inputLines, int days)
    {
        var school = new FishSchool(Input(inputLines[0]));
        return school.SimulateReproduction(days).Count();
    }

    public long SolvePartOne(string[] inputLines) => CalculateSchoolSize(inputLines, days: 80);

    public long? SolvePartTwo(string[] inputLines) => CalculateSchoolSize(inputLines, days: 256);
}
