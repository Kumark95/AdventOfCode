using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Day6;

[PuzzleName("Lanternfish")]
public class Day6Solver : IPuzzleSolver
{
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
