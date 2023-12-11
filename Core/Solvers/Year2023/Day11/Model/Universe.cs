using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day11.Model;

internal class Universe
{
    private readonly char[,] _map;

    public Universe(char[,] map)
    {
        _map = map;
    }

    public long CalculateTotalShortestPathBetweenGalaxies(int expansionConstant)
    {
        var galaxies = FindGalaxies();
        var emptyRows = FindEmptyRows();
        var emptyCols = FindEmptyCols();

        var result = 0L;
        foreach (var pair in galaxies.CombinationsWithoutRepetition(sampleSize: 2))
        {
            var startGalaxy = pair.First();
            var endGalaxy = pair.Last();

            var manhattanDistance = startGalaxy.ManhattanDistance(endGalaxy);
            var expansionOffset = CalculateExpansionOffset(startGalaxy, endGalaxy, emptyRows, emptyCols, expansionConstant);

            result += manhattanDistance + expansionOffset;
        }

        return result;
    }

    private List<Position> FindGalaxies()
    {
        var galaxies = new List<Position>();
        for (var row = 0; row < _map.GetLength(0); row++)
        {
            for (var col = 0; col < _map.GetLength(1); col++)
            {
                if (_map[row, col] == '#')
                {
                    galaxies.Add(new Position(row, col));
                }
            }
        }

        return galaxies;
    }

    private List<int> FindEmptyRows()
    {
        var emptyRows = new List<int>();
        for (var row = 0; row < _map.GetLength(0); row++)
        {
            var isEmptyRow = true;
            for (var col = 0; col < _map.GetLength(1); col++)
            {
                if (_map[row, col] == '#')
                {
                    isEmptyRow = false;
                    break;
                }
            }

            if (isEmptyRow)
            {
                emptyRows.Add(row);
            }
        }

        return emptyRows;
    }

    private List<int> FindEmptyCols()
    {
        var emptyCols = new List<int>();
        for (var col = 0; col < _map.GetLength(1); col++)
        {
            var isEmptyCol = true;
            for (var row = 0; row < _map.GetLength(0); row++)
            {
                if (_map[row, col] == '#')
                {
                    isEmptyCol = false;
                    break;
                }
            }

            if (isEmptyCol)
            {
                emptyCols.Add(col);
            }
        }

        return emptyCols;
    }

    private static long CalculateExpansionOffset(Position start, Position end, List<int> emptyRows, List<int> emptyCols, int expansionConstant)
    {
        var offset = 0L;

        var minRow = Math.Min(start.Row, end.Row);
        var maxRow = Math.Max(start.Row, end.Row);
        foreach (var emptyRow in emptyRows)
        {
            if (emptyRow >= minRow && emptyRow <= maxRow)
            {
                offset += expansionConstant;
            }
        }

        var minCol = Math.Min(start.Col, end.Col);
        var maxCol = Math.Max(start.Col, end.Col);
        foreach (var emptyCol in emptyCols)
        {
            if (emptyCol >= minCol && emptyCol <= maxCol)
            {
                offset += expansionConstant;
            }
        }

        return offset;
    }


#pragma warning disable IDE0051 // Supress "Remove unused private member" warning
    private static void Print(char[,] map)
    {
        for (var row = 0; row < map.GetLength(0); row++)
        {
            for (var col = 0; col < map.GetLength(1); col++)
            {
                var character = map[row, col];
                if (character == '#')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.Write(character);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
        }
    }
#pragma warning restore IDE0051
}
