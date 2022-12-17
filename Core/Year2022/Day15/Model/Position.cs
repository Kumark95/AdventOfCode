namespace AdventOfCode.Core.Year2022.Day15.Model;

internal record Position(int Row, int Col)
{
    //public Position North() => new(Row - 1, Col);
    //public Position South() => new(Row + 1, Col);
    //public Position West() => new(Row, Col - 1);
    //public Position East() => new(Row, Col + 1);

    //public IEnumerable<Position> Neighbours()
    //{
    //    yield return North();
    //    yield return South();
    //    yield return West();
    //    yield return East();
    //}

    public int ManhattanDistance(Position target)
    {
        return Math.Abs(Row - target.Row)
            + Math.Abs(Col - target.Col);
    }
}
