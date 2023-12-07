namespace AdventOfCode.Core.Solvers.Year2023.Day07.Model;

internal readonly record struct CardHand
{
    public string Cards { get; }
    public int BidAmount { get; }
    public HandType HandType { get; }

    public CardHand(string cards, int bidAmount, bool jAsJokers)
    {
        Cards = cards;
        BidAmount = bidAmount;
        HandType = CalculateType(cards, jAsJokers);
    }

    private static HandType CalculateType(string cards, bool jAsJokers)
    {
        var cardsFrequency = cards
            .GroupBy(c => c)
            .Select(group => new CardFrequency(group.Key, group.Count()))
            .OrderByDescending(f => f.Frequency)
            .ToList();

        if (jAsJokers && cardsFrequency.Count > 1 && cards.Contains('J'))
        {
            var jokers = cardsFrequency.First(c => c.Card == 'J');
            cardsFrequency.Remove(jokers);

            // Add its value to the most frequent card
            cardsFrequency[0] = cardsFrequency[0] with { Frequency = cardsFrequency[0].Frequency + jokers.Frequency };
        }

        return (cardsFrequency.Count, cardsFrequency.First().Frequency) switch
        {
            (1, _) => HandType.FiveOfAKind,
            (2, 4) => HandType.FourOfAKind,
            (2, 3) => HandType.FullHouse,
            (3, 3) => HandType.ThreeOfAKind,
            (3, 2) => HandType.TwoPair,
            (4, _) => HandType.OnePair,
            _ => HandType.HighCard
        };
    }
}
