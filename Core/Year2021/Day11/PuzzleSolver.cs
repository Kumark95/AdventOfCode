using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Year2021.Day11;


[PuzzleName("Dumbo Octopus")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 11;


    private static DumboOctopus[][] Input(string[] inputLines)
    {
        var input = new DumboOctopus[inputLines.Length][];

        for (int row = 0; row < inputLines.Length; row++)
        {
            var line = inputLines[row];

            // Init
            input[row] = new DumboOctopus[line.Length];
            for (int col = 0; col < line.Length; col++)
            {
                var energy = line[col] - '0';
                input[row][col] = new DumboOctopus(row, col, energy);
            }
        }

        return input;
    }

    private static List<DumboOctopus> Adjacent(DumboOctopus[][] grid, int row, int col)
    {
        var adjacentLocations = new List<DumboOctopus>();

        var rowDirection = new int[] { -1, 0, 1, 0, -1, 1, -1, 1 };
        var colDirection = new int[] { 0, 1, 0, -1, -1, -1, 1, 1 };

        for (int i = 0; i < rowDirection.Length; i++)
        {
            var adjX = row + rowDirection[i];
            var adjY = col + colDirection[i];

            if (adjX < 0 || adjY < 0 || adjX >= grid.Length || adjY >= grid[0].Length)
            {
                continue;
            }

            adjacentLocations.Add(grid[adjX][adjY]);
        }

        return adjacentLocations;
    }

    private static int ChargeField(DumboOctopus[][] grid)
    {
        var flashMap = new bool[grid.Length][];
        for (int i = 0; i < flashMap.Length; i++)
        {
            flashMap[i] = new bool[grid[0].Length];
        }

        var flashingDumbos = new Queue<DumboOctopus>();
        // Increase the energy and select the ones flashing
        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[0].Length; col++)
            {
                var dumbo = grid[row][col];
                dumbo.Charge();
                if (dumbo.IsFlashing())
                {
                    flashingDumbos.Enqueue(dumbo);
                    flashMap[row][col] = true;
                }
            }
        }

        var flashesRecorded = flashingDumbos.Count;

        // Increase energy of adjacent
        while (flashingDumbos.Count > 0)
        {
            var flashingDumbo = flashingDumbos.Dequeue();

            var adjacentDumbos = Adjacent(grid, flashingDumbo.Location.X, flashingDumbo.Location.Y);
            foreach (var adjacentDumbo in adjacentDumbos)
            {
                if (flashMap[adjacentDumbo.Location.X][adjacentDumbo.Location.Y]) continue;

                // Charge
                adjacentDumbo.Charge();
                if (adjacentDumbo.IsFlashing())
                {
                    flashingDumbos.Enqueue(adjacentDumbo);
                    flashMap[adjacentDumbo.Location.X][adjacentDumbo.Location.Y] = true;

                    flashesRecorded++;
                }
            }
        }

        // Reset
        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[0].Length; col++)
            {
                var dumbo = grid[row][col];
                if (dumbo.IsFlashing())
                {
                    dumbo.Reset();
                }
            }
        }

        return flashesRecorded;
    }

    public long SolvePartOne(string[] inputLines)
    {
        var result = 0;

        var input = Input(inputLines);

        var maxSteps = 100;
        for (int step = 1; step <= maxSteps; step++)
        {
            result += ChargeField(input);
        }

        return result;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var input = Input(inputLines);

        var step = 1;
        while (true)
        {
            var flashesRecorded = ChargeField(input);
            if (flashesRecorded == input.Length * input[0].Length)
            {
                return step;
            };

            step++;
        }
    }
}
