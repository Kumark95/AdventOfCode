using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Year2022.Day02.Model;

namespace AdventOfCode.Core.Year2022.Day02;

[PuzzleName("Rock Paper Scissors")]
public partial class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 2;

    public long SolvePartOne(string[] inputLines)
    {
        var game = new HandGame(inputLines, DecryptionStrategy.UserSelection);
        return game.CalculateTotalScore();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var game = new HandGame(inputLines, DecryptionStrategy.RoundResult);
        return game.CalculateTotalScore();
    }
}
