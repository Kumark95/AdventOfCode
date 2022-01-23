namespace AdventOfCode.Core.Day6;

public class FishSchool
{
    public List<LanternFish> FishCollection { get; private set; } = new List<LanternFish>();

    public FishSchool(string initialState)
    {
        Parse(initialState);
    }

    private void Parse(string initialState)
    {
        FishCollection = initialState
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(i => new LanternFish(int.Parse(i)))
            .ToList();
    }

    public FishSchool Age(int nDays)
    {
        for (int day = 1; day <= nDays; day++)
        {
            var newbornFishes = new List<LanternFish>();
            foreach (var fish in FishCollection)
            {
                if (fish.CanReproduce())
                {
                    newbornFishes.Add(new LanternFish());
                }

                fish.Age();
            }

            FishCollection.AddRange(newbornFishes);
        }

        return this;
    }
}
