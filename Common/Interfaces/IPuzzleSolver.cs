namespace AdventOfCode.Common.Interfaces;

public interface IPuzzleSolver
{
    long SolvePartOne(string[] inputLines);
    long? SolvePartTwo(string[] inputLines);
}
