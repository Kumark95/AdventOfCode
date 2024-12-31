using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Solvers.Year2024.Day12.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day12;

[PuzzleName("Garden Groups")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 12;

    [PuzzleInput(filename: "example.txt", expectedResult: 1930)]
    [PuzzleInput(filename: "example-2.txt", expectedResult: 1202)]
    [PuzzleInput(filename: "input.txt", expectedResult: 1473408)]
    public object SolvePartOne(string[] inputLines)
    {
        var input = InputParser.ParseInput(inputLines);

        var regions = FindRegions(input);

        var result = 0;
        foreach (var region in regions)
        {
            var perimeter = CalculatePerimeter(region);

            result += perimeter * region.Count;
        }

        return result;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 1206)]
    [PuzzleInput(filename: "example-2.txt", expectedResult: 452)]
    [PuzzleInput(filename: "input.txt", expectedResult: 886364)]
    public object SolvePartTwo(string[] inputLines)
    {
        var input = InputParser.ParseInput(inputLines);

        var regions = FindRegions(input);

        var result = 0;
        foreach (var region in regions)
        {
            // Finding the corners is the same as finding the sides of the polygon
            var corners = CountCorners(region);

            result += corners * region.Count;
        }

        return result;
    }

    private static List<HashSet<Position>> FindRegions(char[,] grid)
    {
        var regions = new List<HashSet<Position>>();

        var visited = new bool[grid.RowLength(), grid.RowLength()];

        for (var row = 0; row < grid.RowLength(); row++)
        {
            for (var col = 0; col < grid.ColLength(); col++)
            {
                if (visited[row, col])
                {
                    continue;
                }

                // Create a new region and find all its members
                var region = DFS(new Position(row, col));

                regions.Add(region);
            }
        }

        HashSet<Position> DFS(Position start)
        {
            var region = new HashSet<Position>();

            var stack = new Stack<Position>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                var currentPos = stack.Pop();

                var currentCode = grid.GetValueAt(currentPos);

                region.Add(currentPos);
                visited.SetValueAt(currentPos, true);

                foreach ((Position neighbourPos, char neighbourCode) in grid.GetAllNeighbours(currentPos, MapConnectivity.FourConnected))
                {
                    if (visited[neighbourPos.Row, neighbourPos.Col] || neighbourCode != currentCode)
                    {
                        continue;
                    }

                    stack.Push(neighbourPos);
                }
            }

            return region;
        }

        return regions;
    }

    private static int CalculatePerimeter(HashSet<Position> region)
    {
        (int[] rowDirection, int[] colDirection) = MapExtensions.GetDirections(MapConnectivity.FourConnected);

        var perimeter = 0;
        foreach (var pos in region)
        {
            var sharedSides = 0;
            for (int i = 0; i < rowDirection.Length; i++)
            {
                var adjPos = new Position(pos.Row + rowDirection[i], pos.Col + colDirection[i]);

                if (region.Contains(adjPos))
                {
                    sharedSides++;
                }
            }

            perimeter += 4 - sharedSides;
        }

        return perimeter;
    }

    private static int CountCorners(HashSet<Position> region)
    {
        (int[] rowDirection, int[] colDirection) = MapExtensions.GetDirections(MapConnectivity.Diagonals);

        var corners = 0;
        foreach (var pos in region)
        {
            // Get all neighbours, even invalid ones
            for (int i = 0; i < rowDirection.Length; i++)
            {
                // For figures like:
                //   +-+
                //   |A|
                // +-+ +-+
                // |A A A|
                // +-+ +-+ 
                //   |A|
                //   +-+
                //
                // When two sides meet we can have:
                // - Exterior corners, where the angle "points outwards". The neighbours forming an L are outside the region
                // - Interior corners, where the angle "points inward". The neighbours forming an L are inside the region, but the diagonal is not
                var adjRowPos = pos with { Row = pos.Row + rowDirection[i] };
                var adjColPos = pos with { Col = pos.Col + colDirection[i] };
                var diagonalPos = new Position(pos.Row + rowDirection[i], pos.Col + colDirection[i]);

                // Exterior 
                if (!region.Contains(adjRowPos) && !region.Contains(adjColPos))
                {
                    corners++;
                }

                // Interior
                if (region.Contains(adjRowPos) && region.Contains(adjColPos) && !region.Contains(diagonalPos))
                {
                    corners++;
                }
            }
        }

        return corners;
    }
}
