using System.Diagnostics;

namespace AdventOfCode2021.Day1;

/// <summary>
/// Puzzle for Day 1.
/// Src: https://adventofcode.com/2021/day/1
/// </summary>
public class Day1Solver : IPuzzleSolver
{
    public int CountPreviousLargerItem(int[] input)
    {
        var counter = 0;

        for (var i = 0; i < input.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }

            if (input[i] > input[i - 1])
            {
                counter++;
            }
        }

        return counter;
    }

    public int[] SlidingWindow(int[] input, int window)
    {
        int[] result = new int[input.Length - window + 1];
        int stopIndex = input.Length - window;
        int? currentSum = null;
        for (int i = 0; i < input.Length; i++)
        {
            // Exit if the window sum cannot be computed
            if (i > stopIndex)
            {
                break;
            }

            if (currentSum.HasValue)
            {
                currentSum += input[i + window - 1]; // Add next
                currentSum -= input[i - 1]; // Remove previous
            }
            else
            {
                currentSum = input[i] + input[i + 1] + input[i + 2];
            }

            result[i] = currentSum.Value;
        }

        return result;
    }

    public void Solve()
    {
        var timer = new Stopwatch();

        // Parse input
        var inputFile = Path.Combine(Directory.GetCurrentDirectory(), "Day1", "input.txt");
        var content = File.ReadAllLines(inputFile);
        var inputData = content.Select(l => int.Parse(l)).ToArray();

        // Part 1
        timer.Start();

        Console.WriteLine("Part 1: How many measurements are larger than the previous measurement?");
        var resultPart1 = CountPreviousLargerItem(inputData);

        timer.Stop();
        Console.WriteLine($"Answer: {resultPart1} | Took {timer.Elapsed.TotalSeconds} seconds\n");

        //
        timer.Restart();

        Console.WriteLine("Part 2: Consider sums of a three-measurement sliding window. How many sums are larger than the previous sum?");
        var slidingWindowInput = SlidingWindow(inputData, window: 3);
        var resultPart2 = CountPreviousLargerItem(slidingWindowInput);

        timer.Stop();
        Console.WriteLine($"Answer: {resultPart2} | Took {timer.Elapsed.TotalSeconds} seconds\n");
    }
}
