using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Common.Model;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Solvers.Year2021.Day13;

[PuzzleName("Transparent Origami")]
internal class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 13;

    private static (HashSet<Point> markedPositions, List<(string axis, int value)> foldInstructions) Input(string[] inputLines)
    {
        var markedPositions = new HashSet<Point>();
        var foldInstructions = new List<(string axis, int value)>();

        var foldRegex = new Regex(@"fold along (?<axis>\w)=(?<value>\d+)");

        foreach (var line in inputLines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var coordinates = line.Split(',');
            if (coordinates.Length == 2)
            {
                markedPositions.Add(new Point(int.Parse(coordinates[0]), int.Parse(coordinates[1])));
            }
            else
            {
                var matches = foldRegex.Match(line);

                var axis = matches.Groups["axis"].Value;
                var value = matches.Groups["value"].Value;
                foldInstructions.Add((axis, int.Parse(value)));
            }
        }

        return (markedPositions, foldInstructions);
    }

    private static HashSet<Point> Fold(HashSet<Point> markedPositions, string axis, int value)
    {
        var newMarkedPositions = new HashSet<Point>();

        foreach (var position in markedPositions)
        {
            if (axis == "x" && position.X > value)
            {
                var xDistance = position.X - value;
                newMarkedPositions.Add(position with { X = position.X - 2 * xDistance });
            }
            else if (axis == "y" && position.Y > value)
            {
                var yDistance = position.Y - value;
                newMarkedPositions.Add(position with { Y = position.Y - 2 * yDistance });
            }
            else
            {
                newMarkedPositions.Add(position);
            }
        }

        return newMarkedPositions;
    }

    private static void Draw(HashSet<Point> markedPositions)
    {
        var rotatedPositions = new HashSet<Point>();
        foreach (var position in markedPositions)
        {
            // Rotate 90º
            rotatedPositions.Add(position with { X = position.Y, Y = position.X });
        }

        var maxX = rotatedPositions.Select(p => p.X).Max();
        var maxY = rotatedPositions.Select(p => p.Y).Max();

        for (int i = 0; i <= maxX; i++)
        {
            for (int j = 0; j <= maxY; j++)
            {
                var elem = rotatedPositions.FirstOrDefault(p => p.X == i && p.Y == j);
                if (elem != null)
                {
                    Console.Write("█");
                }
                else
                {
                    Console.Write(" ");
                }
            }

            Console.WriteLine();
        }
    }

    public long? SolvePartOne(string[] inputLines)
    {
        var (markedPositions, foldInstructions) = Input(inputLines);

        // Fold only once
        var (axis, value) = foldInstructions.First();
        var newMarkedPositions = Fold(markedPositions, axis, value);

        return newMarkedPositions.Count;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var (markedPositions, foldInstructions) = Input(inputLines);

        foreach (var (axis, value) in foldInstructions)
        {
            markedPositions = Fold(markedPositions, axis, value);
        }

        // Draw
        Draw(markedPositions);

        return markedPositions.Count;
    }
}
