namespace AdventOfCode.Common.Interfaces;

public interface IPuzzleSolver
{
    public int Year { get; }
    public int Day { get; }

    long? SolvePartOne(string[] inputLines);
    long? SolvePartTwo(string[] inputLines);
}
