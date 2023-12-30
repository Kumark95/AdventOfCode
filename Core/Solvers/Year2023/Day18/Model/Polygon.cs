using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Solvers.Year2023.Day18.Model;

internal class Polygon
{
    private readonly Position[] _positions;

    public Polygon(IEnumerable<Position> positions)
    {
        if (positions.Last() != positions.First())
        {
            throw new ArgumentException("The polygon needs to be closed");
        }

        _positions = positions.ToArray();
    }

    /// <summary>
    /// Uses the Shoelace formula
    /// </summary>
    public double CalculateArea()
    {
        var area = 0.0;

        for (int i = 0; i < _positions.Length - 1; i++)
        {
            var startEdge = _positions[i];
            var endEdge = _positions[i + 1];

            area += (startEdge.Row + endEdge.Row) * (startEdge.Col - endEdge.Col);
        }

        return Math.Abs(area / 2.0);
    }
}
