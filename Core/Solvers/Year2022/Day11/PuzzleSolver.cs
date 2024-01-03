using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2022.Day11.Model;

namespace AdventOfCode.Core.Solvers.Year2022.Day11;

[PuzzleName("Monkey in the Middle")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 11;

    public object SolvePartOne(string[] inputLines)
    {
        var monkeyTroop = MonkeyTroop.Parse(inputLines);
        monkeyTroop.PlayRounds(rounds: 20, isWorryReliefActivated: true);

        return monkeyTroop.Troop
            .Select(m => m.InspectionCounter)
            .OrderByDescending(c => c)
            .Take(2)
            .Aggregate((a, b) => a * b);
    }

    public object SolvePartTwo(string[] inputLines)
    {
        var monkeyTroop = MonkeyTroop.Parse(inputLines);
        monkeyTroop.PlayRounds(rounds: 10_000, isWorryReliefActivated: false);

        return monkeyTroop.Troop
            .Select(m => m.InspectionCounter)
            .OrderByDescending(c => c)
            .Take(2)
            .Aggregate((a, b) => a * b);
    }
}
