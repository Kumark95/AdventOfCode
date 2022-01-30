using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Year2021.Day7;

[PuzzleName("The Treachery of Whales")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 7;

    private static int[] Input(string[] inputLines) => inputLines[0]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(i => int.Parse(i))
            .ToArray();

    private static int TriangularNumber(int n) => (n * (n + 1)) / 2;

    private static int CalculateLeastFuelUsed(int[] inputPositions, Func<int, int> incrementCalculation)
    {
        var fuelSpent = new int[inputPositions.Max()];
        for (int i = 0; i < fuelSpent.Length; i++)
        {
            foreach (var position in inputPositions)
            {
                var distance = Math.Abs(position - i);
                fuelSpent[i] += incrementCalculation(distance);
            }
        }

        return fuelSpent.Min();
    }

    public long SolvePartOne(string[] inputLines) => CalculateLeastFuelUsed(Input(inputLines), i => i);

    public long? SolvePartTwo(string[] inputLines) => CalculateLeastFuelUsed(Input(inputLines), i => TriangularNumber(i));
}
