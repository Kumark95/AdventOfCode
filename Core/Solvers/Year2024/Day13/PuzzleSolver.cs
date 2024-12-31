using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Solvers.Year2024.Day13.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day13;

[PuzzleName("Claw Contraption")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 13;

    [PuzzleInput(filename: "example.txt", expectedResult: 480)]
    [PuzzleInput(filename: "input.txt", expectedResult: 26299)]
    public object SolvePartOne(string[] inputLines)
    {
        var machines = InputParser.ParseInput(inputLines);

        const int MAX_PRESSES = 100;
        const int A_PRESS_COST = 3;

        long result = 0;
        foreach ((Position aPosInc, Position bPosInc, Position prizePos) in machines)
        {
            (decimal aPresses, decimal bPresses) = CalculateMinimumButtonPresses(aPosInc, bPosInc, prizePos);
            if (aPresses > MAX_PRESSES || bPresses > MAX_PRESSES || HasDecimals(aPresses) || HasDecimals(bPresses))
            {
                continue;
            }

            var tokensCost = aPresses * A_PRESS_COST + bPresses;
            result += (long)tokensCost;
        }

        return result;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 875318608908)]
    [PuzzleInput(filename: "input.txt", expectedResult: 107824497933339)]
    public object SolvePartTwo(string[] inputLines)
    {
        var machines = InputParser.ParseInput(inputLines);

        const long PRIZE_POSITION_OFFSET = 10000000000000;
        const int A_PRESS_COST = 3;

        long result = 0;
        foreach ((Position aPosInc, Position bPosInc, Position prizePos) in machines)
        {
            (decimal aPresses, decimal bPresses) = CalculateMinimumButtonPresses(aPosInc, bPosInc, prizePos + PRIZE_POSITION_OFFSET);

            // In this part the button presses can now exceed 100!
            if (HasDecimals(aPresses) || HasDecimals(bPresses))
            {
                continue;
            }

            var tokensCost = aPresses * A_PRESS_COST + bPresses;
            result += (long)tokensCost;
        }

        return result;
    }

    private static bool HasDecimals(decimal value) => value != Math.Floor(value);

    private static (decimal, decimal) CalculateMinimumButtonPresses(Position aPosInc, Position bPosInc, Position prizePos)
    {
        // Applying alebra we have two equaations:
        // A*aXIncrement + B*bXIncrement = prizeXPos
        // A*aYIncrement + B*bYIncrement = prizeYPos

        long aXIncrement = aPosInc.Row;
        long aYIncrement = aPosInc.Col;

        long bXIncrement = bPosInc.Row;
        long bYIncrement = bPosInc.Col;

        long prizeXPos = prizePos.Row;
        long prizeYPos = prizePos.Col;

        // Pen and paper...
        // Using the values from the first example we have:
        decimal p1 = prizeYPos * aXIncrement;
        decimal p2 = aYIncrement * prizeXPos;
        decimal p3 = (-1) * (aYIncrement * bXIncrement) + (bYIncrement * aXIncrement);

        decimal bPresses = (p1 - p2) / p3;

        decimal aPresses = (prizePos.Row - bPresses * bXIncrement) / aXIncrement;

        return (aPresses, bPresses);
    }
}
