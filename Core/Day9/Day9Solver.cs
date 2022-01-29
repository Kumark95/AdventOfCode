using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Day9;


[PuzzleName("Smoke Basin")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 9;

    private static int[][] Input(string[] inputLines) => inputLines
            .Select(line => line.Select(c => Convert.ToInt32(c)).ToArray())
            .ToArray();

    public long SolvePartOne(string[] inputLines)
    {
        var input = Input(inputLines);

        var result = 0;
        //for (int row = 0; row < input.Length; row++)
        //{
        //    for (int col = 0; col < input[0].Length; col++)
        //    {
        //        int? top = input input[row - 1][col] ?? null;
        //        int? bottom = input[row + 1][col] ?? null;
        //    }
        //}

        return result;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        return null;
    }
}
