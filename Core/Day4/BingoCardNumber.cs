namespace AdventOfCode.Core.Day4;

public class BingoCardNumber
{
    public bool IsMarked { get; private set; }
    public int Number { get; private set; }

    public BingoCardNumber(int number)
    {
        IsMarked = false;
        Number = number;
    }

    public void Mark()
    {
        IsMarked = true;
    }
}
