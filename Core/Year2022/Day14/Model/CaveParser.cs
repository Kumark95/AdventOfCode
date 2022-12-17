namespace AdventOfCode.Core.Year2022.Day14.Model;

internal class CaveParser
{
    internal static Cave Parse(string[] rockPaths, bool hasFloor = false)
    {
        var rockPositions = new HashSet<Position>();
        foreach (var path in rockPaths)
        {
            var corners = path
                .Split(" -> ")
                .Select(c =>
                {
                    var coordinates = c.Split(',');
                    return new Position(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
                })
                .ToList();


            foreach (var (startCorner, endCorner) in corners.Zip(corners.Skip(1)))
            {
                rockPositions.Add(startCorner);

                var xDifference = endCorner.X - startCorner.X;
                var xIncrement = xDifference > 0 ? 1 : -1;
                var xCount = 1;
                while (xDifference != 0)
                {
                    var nextPosition = startCorner with { X = startCorner.X + xCount * xIncrement };
                    rockPositions.Add(nextPosition);

                    xDifference -= xIncrement;
                    xCount++;
                }

                var yDifference = endCorner.Y - startCorner.Y;
                var yIncrement = yDifference > 0 ? 1 : -1;
                var yCount = 1;
                while (yDifference != 0)
                {
                    var nextPosition = startCorner with { Y = startCorner.Y + yCount * yIncrement };
                    rockPositions.Add(nextPosition);

                    yDifference -= yIncrement;
                    yCount++;
                }

                rockPositions.Add(endCorner);
            }
        }

        return new Cave(rockPositions.ToList(), hasFloor);
    }
}
