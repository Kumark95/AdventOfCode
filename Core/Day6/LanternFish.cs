namespace AdventOfCode.Core.Day6;

public class LanternFish
{
    private const int DAYS_TO_REPRODUCE = 6;
    private const int DAYS_TO_MATURITY = 2;

    public int DaysLeftToReproduce { get; private set; }

    public LanternFish()
    {
        DaysLeftToReproduce = DAYS_TO_REPRODUCE + DAYS_TO_MATURITY;
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
            DaysLeftToReproduce = DAYS_TO_REPRODUCE;
        }
    }
    public LanternFish? TryToReproduce()
    {
        if (CanReproduce())
        {

            return new LanternFish();
        }

        return null;
    }
}
