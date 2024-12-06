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
            if (IsValid(page, instructions))
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
            if (IsValid(page, instructions))
            {
                continue;
            }

            var fixedPage = FixPage(page, instructions);
            result += fixedPage[fixedPage.Count / 2];
        }

        return result;
    }

    private static bool IsValid(List<int> page, Dictionary<int, HashSet<int>> instructions)
    {
        for (int i = 0; i < page.Count; i++)
        {
            var number = page[i];
            var afterNumbers = instructions.TryGetValue(number, out HashSet<int>? set)
                ? set
                : [];

            // Check all the other numbers in the page
            for (var j = 0; j < page.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                var testNumber = page[j];
                if (afterNumbers.Contains(testNumber) && j < i)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static List<int> FixPage(List<int> page, Dictionary<int, HashSet<int>> instructions)
    {
        var fixedPage = new List<int>(page);

        bool shouldReevaluate;
        do
        {
            shouldReevaluate = false;
            for (int i = 0; i < fixedPage.Count; i++)
            {
                var number = fixedPage[i];
                var afterNumbers = instructions.TryGetValue(number, out HashSet<int>? set)
                    ? set
                    : [];

                // Check all the other numbers in the page
                for (var j = 0; j < fixedPage.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var testNumber = fixedPage[j];
                    if (afterNumbers.Contains(testNumber) && j < i)
                    {
                        // Swap numbers
                        fixedPage[j] = number;
                        fixedPage[i] = testNumber;

                        shouldReevaluate = true;

                        break;
                    }
                }
            }

        } while (shouldReevaluate);

        return fixedPage;
    }
}
