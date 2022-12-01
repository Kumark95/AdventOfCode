﻿using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Year2022.Day01;
[PuzzleName("Calorie Counting")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 1;

    private IEnumerable<int> CalculateCalorieSum(string[] inputLines)
    {
        var list = new List<int>();
        var currentSum = 0;

        foreach (var line in inputLines)
        {
            if (line == string.Empty)
            {
                list.Add(currentSum);

                // Reset
                currentSum = 0;
                continue;
            }

            currentSum += int.Parse(line);
        }

        list.Add(currentSum);

        return list;
    }

    public long SolvePartOne(string[] inputLines)
    {
        var calorieSumList = CalculateCalorieSum(inputLines);

        return calorieSumList.Max();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var calorieSumList = CalculateCalorieSum(inputLines);

        return calorieSumList
            .OrderByDescending(i => i)
            .Take(3)
            .Sum();
    }
}
