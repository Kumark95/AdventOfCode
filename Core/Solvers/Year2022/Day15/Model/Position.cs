namespace AdventOfCode.Core.Solvers.Year2022.Day15.Model;

internal record Position(int Row, int Col)
{
    public int ManhattanDistance(Position target)
    {
        return Math.Abs(Row - target.Row)
            + Math.Abs(Col - target.Col);
    }
}
