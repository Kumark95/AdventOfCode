using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using AdventOfCode.Core.Solvers.Year2024.Day08.Model;

namespace AdventOfCode.Core.Solvers.Year2024.Day08;

[PuzzleName("Resonant Collinearity")]
public sealed class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2024;
    public int Day => 8;

    [PuzzleInput(filename: "example.txt", expectedResult: 14)]
    [PuzzleInput(filename: "input.txt", expectedResult: 313)]
    public object SolvePartOne(string[] inputLines)
    {
        (char[,] map, Dictionary<char, List<Position>> antennaPositions) = InputParser.ParseInput(inputLines);

        var antiNodePositions = new HashSet<Position>();

        foreach ((char antenna, List<Position> positions) in antennaPositions)
        {
            if (positions.Count == 1)
            {
                continue;
            }

            foreach (var pair in positions.CombinationsWithoutRepetition(sampleSize: 2))
            {
                var firstAntennaPos = pair.First();
                var secondAntennaPos = pair.Last();

                var distance = firstAntennaPos - secondAntennaPos;

                var antiNodeA = firstAntennaPos + distance;
                if (map.IsValidPosition(antiNodeA))
                {
                    antiNodePositions.Add(antiNodeA);
                }

                var antiNodeB = secondAntennaPos - distance;
                if (map.IsValidPosition(antiNodeB))
                {
                    antiNodePositions.Add(antiNodeB);
                }
            }
        }

        return antiNodePositions.Count;
    }

    [PuzzleInput(filename: "example.txt", expectedResult: 34)]
    [PuzzleInput(filename: "input.txt", expectedResult: 1064)]
    public object SolvePartTwo(string[] inputLines)
    {
        (char[,] map, Dictionary<char, List<Position>> antennaPositions) = InputParser.ParseInput(inputLines);

        var antiNodePositions = new HashSet<Position>();

        foreach ((char antenna, List<Position> positions) in antennaPositions)
        {
            if (positions.Count == 1)
            {
                continue;
            }

            foreach (var pair in positions.CombinationsWithoutRepetition(sampleSize: 2))
            {
                var firstAntennaPos = pair.First();
                var secondAntennaPos = pair.Last();

                var distance = firstAntennaPos - secondAntennaPos;

                // The multiplier starts at 0 as the antennas are now valid positions
                var multiplier = 0;
                while (true)
                {
                    var antiNodeA = firstAntennaPos + (distance * multiplier);
                    if (!map.IsValidPosition(antiNodeA))
                    {
                        break;
                    }

                    antiNodePositions.Add(antiNodeA);
                    multiplier++;
                }

                multiplier = 0;
                while (true)
                {
                    var antiNodeB = secondAntennaPos - (distance * multiplier);
                    if (!map.IsValidPosition(antiNodeB))
                    {
                        break;
                    }

                    antiNodePositions.Add(antiNodeB);
                    multiplier++;
                }
            }
        }

        return antiNodePositions.Count;
    }
}
