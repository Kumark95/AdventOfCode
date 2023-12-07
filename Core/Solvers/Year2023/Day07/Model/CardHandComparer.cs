namespace AdventOfCode.Core.Solvers.Year2023.Day07.Model;

internal class CardHandComparer : IComparer<CardHand>
{
    private readonly char[] _cardOrder;

    public CardHandComparer(bool jAsJokers)
    {
        if (jAsJokers)
        {
            _cardOrder = ['J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A'];
        }
        else
        {
            _cardOrder = ['2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'];
        }
    }

    public int Compare(CardHand x, CardHand y)
    {
        int handTypeComparison = x.HandType.CompareTo(y.HandType);
        if (handTypeComparison != 0)
        {
            return handTypeComparison;
        }

        char[] xChars = x.Cards.ToCharArray();
        char[] yChars = y.Cards.ToCharArray();

        for (int i = 0; i < xChars.Length; i++)
        {
            int xIndex = Array.IndexOf(_cardOrder, xChars[i]);
            int yIndex = Array.IndexOf(_cardOrder, yChars[i]);

            int charComparison = xIndex.CompareTo(yIndex);
            if (charComparison != 0)
            {
                return charComparison;
            }
        }

        // Both are exactly the same
        return 1;
    }
}
