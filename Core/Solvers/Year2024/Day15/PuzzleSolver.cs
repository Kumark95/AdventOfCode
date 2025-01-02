using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2024.Day15.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day15;

[PuzzleName("Warehouse Woes")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 15;

    [PuzzleInput(filename: "example.txt", expectedResult: 2028)]
    [PuzzleInput(filename: "example-3.txt", expectedResult: 10092)]
    [PuzzleInput(filename: "input.txt", expectedResult: 1441031)]
    public object SolvePartOne(string[] inputLines)
    {
        var warehouse = Warehouse.BuildFromInput(inputLines, isWideMap: false);

        warehouse.ExecuteInstructions();

        return warehouse.CalculateSumGpsCoordinates();
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 1751)]
    [PuzzleInput(filename: "example-2.txt", expectedResult: 618)]
    [PuzzleInput(filename: "example-3.txt", expectedResult: 9021)]
    [PuzzleInput(filename: "input.txt", expectedResult: 1425169)]
    public object SolvePartTwo(string[] inputLines)
    {
        var warehouse = Warehouse.BuildFromInput(inputLines, isWideMap: true);

        warehouse.ExecuteInstructions();

        return warehouse.CalculateSumGpsCoordinates();
    }
}
