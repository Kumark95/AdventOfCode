using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Solvers.Year2024.Day04.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day04;

[PuzzleName("Ceres Search")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 4;

    [PuzzleInput(filename: "example.txt", expectedResult: 18)]
    [PuzzleInput(filename: "input.txt", expectedResult: 2370)]
    public object SolvePartOne(string[] inputLines)
    {
        var map = InputParser.ParseInput(inputLines);

        char[] target = ['X', 'M', 'A', 'S'];
        Position[] directions = [
                new Position(-1, 0),
                new Position(0, 1),
                new Position(1, 0),
                new Position(0, -1),
                new Position(-1, -1),
                new Position(1, -1),
                new Position(-1, 1),
                new Position(1, 1),
            ];

        var result = 0;
        for (var row = 0; row < inputLines.Length; row++)
        {
            for (var col = 0; col < inputLines[row].Length; col++)
            {
                var currentChar = map[row, col];
                if (currentChar != target[0])
                {
                    continue;
                }

                foreach (var direction in directions)
                {
                    var searchPos = new Position(row, col);

                    var isValid = true;
                    for (var k = 0; k < target.Length; k++)
                    {
                        if (!map.IsValidPosition(searchPos)
                            || map[searchPos.Row, searchPos.Col] != target[k])
                        {
                            isValid = false;
                            break;
                        }

                        searchPos += direction;
                    }

                    if (isValid)
                    {
                        result++;
                    }
                }
            }
        }

        return result;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 9)]
    [PuzzleInput(filename: "input.txt", expectedResult: 1908)]
    public object SolvePartTwo(string[] inputLines)
    {
        var map = InputParser.ParseInput(inputLines);

        // Diagonals only
        Position[] directions = [
                new Position(-1, -1),
                new Position(1, 1),
                new Position(1, -1),
                new Position(-1, 1),
            ];

        var result = 0;
        for (var row = 0; row < inputLines.Length; row++)
        {
            for (var col = 0; col < inputLines[row].Length; col++)
            {
                var currentChar = map[row, col];
                if (currentChar != 'A')
                {
                    continue;
                }

                // Seach the diagonals, the possible values are limited
                var diagonalValues = new List<(Position Position, char Character)>();
                foreach (var direction in directions)
                {
                    var diagonalPosition = new Position(row + direction.Row, col + direction.Col);
                    if (!map.IsValidPosition(diagonalPosition))
                    {
                        break;
                    }

                    var diagonalCharacter = map[diagonalPosition.Row, diagonalPosition.Col];

                    // Save the direction instead of the actual position to ease the check
                    diagonalValues.Add((direction, diagonalCharacter));
                }

                // All solutions have 2 M's and 2 S's, but some combinations are invalid:
                // M S
                //  A
                // S M
                var mPositions = diagonalValues.Where(v => v.Character == 'M').Select(v => v.Position).ToList();
                var sPositions = diagonalValues.Where(v => v.Character == 'S').Select(v => v.Position).ToList();
                if (mPositions.Count != 2 || sPositions.Count != 2)
                {
                    continue;
                }

                // If both rows or columns are the same, its an invalid combination
                var mDifRows = mPositions.Select(p => p.Row).Distinct().ToList();
                var mDifCols = mPositions.Select(p => p.Col).Distinct().ToList();

                var sDifRows = sPositions.Select(p => p.Row).Distinct().ToList();
                var sDifCols = sPositions.Select(p => p.Col).Distinct().ToList();

                if (mDifRows.Count == 2
                    && mDifCols.Count == 2
                    && sDifRows.Count == 2
                    && sDifCols.Count == 2)
                {
                    continue;
                }

                result++;
            }
        }

        return result;
    }
}
