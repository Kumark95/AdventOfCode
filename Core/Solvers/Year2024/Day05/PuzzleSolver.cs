using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2024.Day05.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day05;

[PuzzleName("Print Queue")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 5;

    [PuzzleInput(filename: "example.txt", expectedResult: 143)]
    [PuzzleInput(filename: "input.txt", expectedResult: 4569)]
    public object SolvePartOne(string[] inputLines)
    {
        (List<List<int>> pages, Dictionary<int, HashSet<int>> instructions) = InputParser.ParseInput(inputLines);

        var result = 0;
        foreach (var page in pages)
        {
            // The page is invalid if it had to be fixed
            var wasFixed = FixPage(page, instructions, out var _);
            if (!wasFixed)
            {
                result += page[page.Count / 2];
            }
        }

        return result;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 123)]
    [PuzzleInput(filename: "input.txt", expectedResult: 6456)]
    public object SolvePartTwo(string[] inputLines)
    {
        (List<List<int>> pages, Dictionary<int, HashSet<int>> instructions) = InputParser.ParseInput(inputLines);

        var result = 0;
        foreach (var page in pages)
        {
            var wasFixed = FixPage(page, instructions, out var fixedPage);
            if (wasFixed)
            {
                result += fixedPage[fixedPage.Count / 2];
            }
        }

        return result;
    }

    private static bool FixPage(List<int> page, Dictionary<int, HashSet<int>> instructions, out List<int> fixedPage)
    {
        fixedPage = new List<int>(page);
        bool wasModified = false;

        bool shouldReevaluate;
        do
        {
            shouldReevaluate = false;
            for (int i = 0; i < fixedPage.Count; i++)
            {
                var currentNumber = fixedPage[i];
                var mustFollowNumbers = instructions.TryGetValue(currentNumber, out HashSet<int>? set)
                    ? set
                    : [];

                // Check all the other numbers in the page
                for (var j = 0; j < fixedPage.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var comparisonNumber = fixedPage[j];
                    if (mustFollowNumbers.Contains(comparisonNumber) && j < i)
                    {
                        // Swap numbers
                        fixedPage[j] = currentNumber;
                        fixedPage[i] = comparisonNumber;

                        shouldReevaluate = true;
                        wasModified = true;

                        break;
                    }
                }
            }

        } while (shouldReevaluate);

        return wasModified;
    }
}
