using System.Text.RegularExpressions;

namespace AdventOfCode.Core.Year2021.Day5;

public class VentLine
{
    public List<Point> Points { get; private set; } = new List<Point>();

    public VentLine(string lineContent, bool excludeDiagonal)
    {
        (Point start, Point end) = Parse(lineContent);

        if (excludeDiagonal && start.X != end.X && start.Y != end.Y)
        {
            return;
        }

        FillPoints(start, end);
    }

    private static (Point start, Point end) Parse(string lineContent)
    {
        var regexExp = new Regex(@"(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)");

        var matches = regexExp.Match(lineContent);
        if (!matches.Success)
        {
            throw new Exception("Could not extract point information from input line");
        }

        var start = new Point(int.Parse(matches.Groups["x1"].Value), int.Parse(matches.Groups["y1"].Value));
        var end = new Point(int.Parse(matches.Groups["x2"].Value), int.Parse(matches.Groups["y2"].Value));

        return (start, end);
    }

    private void FillPoints(Point start, Point end)
    {
        var displacement = new Point(end.X - start.X, end.Y - start.Y);

        var xIncrement = Math.Sign(displacement.X);
        var yIncrement = Math.Sign(displacement.Y);

        var maxDistance = Math.Max(Math.Abs(displacement.X), Math.Abs(displacement.Y));
        for (int i = 0; i <= maxDistance; i++)
        {
            Points.Add(new Point(start.X + i * xIncrement, start.Y + i * yIncrement));
        }
    }
}
