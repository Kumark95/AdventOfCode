using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Day3;


[PuzzleName("Binary Diagnostic")]
public class Day3Solver : IPuzzleSolver
{
    private List<string> Input(string[] inputLines) => inputLines.ToList();

    public int SolvePartOne(string[] inputLines)
    {
        var inputData = Input(inputLines);

        var itemLength = inputData[0].Length;
        int[] counter = new int[itemLength];
        foreach (var item in inputData)
        {
            for (int i = 0; i < itemLength; i++)
            {
                counter[i] += int.Parse(item[i].ToString());
            }
        }

        var commonBits = counter
            .Select(i => (i < inputData.Count / 2) ? "0" : "1")
            .ToList();

        var gammaBinary = string.Join("", commonBits);
        var gamma = Convert.ToInt32(gammaBinary, 2);
        var mask = Convert.ToInt32(Math.Pow(2, itemLength) - 1);
        var epsilon = gamma ^ mask; // XOR

        return gamma * epsilon;
    }

    public int? SolvePartTwo(string[] inputLines)
    {
        var inputData = Input(inputLines);

        var itemLength = inputData[0].Length;
        var oxigenElements = inputData;
        for (int i = 0; i < itemLength; i++)
        {
            var groups = oxigenElements.GroupBy(elem => elem[i]).ToList();

            var maxLength = groups.Select(l => l.Count()).Max();

            oxigenElements = groups
                .Where(l => l.Count() == maxLength)
                .OrderByDescending(k => k.Key)  // DESC
                .First()
                .Select(l => l)
                .ToList();

            if (oxigenElements.Count == 1)
            {
                break;
            }

        }
        var oxigenRating = Convert.ToInt32(oxigenElements[0], 2);

        //
        var coElements = inputData;
        for (int i = 0; i < itemLength; i++)
        {
            var groups = coElements.GroupBy(elem => elem[i]).ToList();

            var maxLength = groups.Select(l => l.Count()).Min();

            coElements = groups
                .Where(l => l.Count() == maxLength)
                .OrderBy(k => k.Key)    // ASC
                .First()
                .Select(l => l)
                .ToList();

            if (coElements.Count == 1)
            {
                break;
            }

        }
        var coRating = Convert.ToInt32(coElements[0], 2);

        return oxigenRating * coRating;
    }
}
