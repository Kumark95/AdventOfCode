﻿using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2021.Day24.Model;

namespace AdventOfCode.Core.Solvers.Year2021.Day24;

[PuzzleName("Arithmetic Logic Unit")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 24;

    public object SolvePartOne(string[] inputLines)
    {
        return ArithmeticLogicUnit.FindModelNumber(inputLines, SearchMode.Maximum);
    }

    public object SolvePartTwo(string[] inputLines)
    {
        return ArithmeticLogicUnit.FindModelNumber(inputLines, SearchMode.Minimum);
    }
}
