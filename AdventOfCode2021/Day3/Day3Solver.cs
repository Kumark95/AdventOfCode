using System.Diagnostics;

namespace AdventOfCode2021.Day3;

public class Day3Solver : IPuzzleSolver
{
    private List<string> _testInputData = new()
    {
        "00100",
        "11110",
        "10110",
        "10111",
        "10101",
        "01111",
        "00111",
        "11100",
        "10000",
        "11001",
        "00010",
        "01010",
    };

    public void Solve()
    {
        var timer = new Stopwatch();

        // Parse input
        var inputFile = Path.Combine(Directory.GetCurrentDirectory(), "Day3", "input.txt");
        var inputData = File.ReadAllLines(inputFile).ToList();

        // Part 1
        timer.Start();

        Console.WriteLine("Part 1: What is the power consumption of the submarine?");
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

        var resultPart1 = gamma * epsilon;

        timer.Stop();
        Console.WriteLine($"Answer: {resultPart1} | Took {timer.Elapsed.TotalSeconds} seconds\n");

        //
        Console.WriteLine("Part 2: What is the life support rating of the submarine?");
        // Restart
        timer.Restart();

        // Search for oxygen generator rating
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

        var resultPart2 = oxigenRating * coRating;

        timer.Stop();
        Console.WriteLine($"Answer: {resultPart2} | Took {timer.Elapsed.TotalSeconds} seconds\n");
    }
}
