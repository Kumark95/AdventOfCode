namespace AdventOfCode.Core.Year2021.Day4;

public class BingoCard
{
    private BingoCardNumber[][] Grid { get; set; }

    public BingoCard(int[][] numbers)
    {
        Grid = numbers
            .Select(
                row => row
                    .Select(i => new BingoCardNumber(i))
                    .ToArray()
            )
            .ToArray();
    }

    public bool MarkNumber(int number)
    {
        for (int row = 0; row < Grid.Length; row++)
        {
            for (int col = 0; col < Grid[row].Length; col++)
            {
                var cardNumber = Grid[row][col];
                if (cardNumber.Number == number)
                {
                    cardNumber.Mark();

                    return Check();
                }
            }
        }

        return false;
    }

    public int SumNotMarked()
    {
        return Grid
            .SelectMany(l => l)
            .Where(i => !i.IsMarked)
            .Select(i => i.Number)
            .Sum();
    }

    private bool Check()
    {
        for (int row = 0; row < Grid.Length; row++)
        {
            var rowResult = true;
            var colResult = true;

            for (int col = 0; col < Grid[row].Length; col++)
            {
                rowResult = rowResult && Grid[row][col].IsMarked;
                colResult = colResult && Grid[col][row].IsMarked;
            }

            if (rowResult || colResult)
            {
                return true;
            }
        }

        return false;
    }
}
