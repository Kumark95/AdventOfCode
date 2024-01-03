namespace AdventOfCode.Common.Interfaces;

public interface IPuzzleSolver
{
    public int Year { get; }
    public int Day { get; }

    object SolvePartOne(string[] inputLines);
    object SolvePartTwo(string[] inputLines);
}
