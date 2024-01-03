using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2022.Day02.Model;

namespace AdventOfCode.Core.Solvers.Year2022.Day02;

[PuzzleName("Rock Paper Scissors")]
public partial class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 2;

    public object SolvePartOne(string[] inputLines)
    {
        var game = new HandGame(inputLines, DecryptionStrategy.UserSelection);
        return game.CalculateTotalScore();
    }

    public object SolvePartTwo(string[] inputLines)
    {
        var game = new HandGame(inputLines, DecryptionStrategy.RoundResult);
        return game.CalculateTotalScore();
    }
}
