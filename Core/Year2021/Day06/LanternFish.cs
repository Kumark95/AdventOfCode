namespace AdventOfCode.Core.Year2021.Day6;

public class LanternFish
{
    public const int AdultDaysToReproduce = 6;
    public const int DaysToMaturity = 2;
    public const int NewbornDaysToReproduce = AdultDaysToReproduce + DaysToMaturity;

    public int DaysLeftToReproduce { get; private set; }

    public LanternFish()
    {
        DaysLeftToReproduce = NewbornDaysToReproduce;
    }

    public LanternFish(int currentDaysLeft)
    {
        DaysLeftToReproduce = currentDaysLeft;
    }

    public bool CanReproduce() => DaysLeftToReproduce == 0;

    public void Age()
    {
        DaysLeftToReproduce--;
        if (DaysLeftToReproduce < 0)
        {
            DaysLeftToReproduce = AdultDaysToReproduce;
        }
    }
}
