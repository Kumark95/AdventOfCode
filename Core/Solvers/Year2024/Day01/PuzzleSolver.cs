using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2024.Day01.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day01;

[PuzzleName("Historian Hysteria")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 1;

    [PuzzleInput(filename: "example.txt", expectedResult: 11)]
    [PuzzleInput(filename: "input.txt", expectedResult: 2580760)]
    public object SolvePartOne(string[] inputLines)
    {
        (int[] firstList, int[] secondList) = InputParser.ParseLists(inputLines);

        Array.Sort(firstList);
        Array.Sort(secondList);

        var totalDistance = 0;
        for (int i = 0; i < inputLines.Length; i++)
        {
            totalDistance += Math.Abs(firstList[i] - secondList[i]);
        }

        return totalDistance;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 31)]
    [PuzzleInput(filename: "input.txt", expectedResult: 25358365)]
    public object SolvePartTwo(string[] inputLines)
    {
        (int[] firstList, int[] secondList) = InputParser.ParseLists(inputLines);

        var secondListItemFrequency = secondList
            .GroupBy(item => item)
            .ToDictionary(group => group.Key, group => group.Count());

        var singularityScore = firstList
            .Where(secondListItemFrequency.ContainsKey)
            .Sum(item => item * secondListItemFrequency[item]);

        return singularityScore;
    }
}
