namespace AdventOfCode.Core.Year2021.Day6;

public class FishSchool
{
    private Dictionary<int, long> FishFrequency { get; set; } = new();

    public FishSchool(IEnumerable<int> initialState)
    {
        FishFrequency = initialState
            .GroupBy(i => i)
            .ToDictionary(g => Convert.ToInt32(g.Key), l => Convert.ToInt64(l.Count()));

        // Fill missing entries to contain all possible values
        for (var i = 0; i <= LanternFish.NewbornDaysToReproduce; i++)
        {
            FishFrequency.TryAdd<int, long>(i, 0);
        }
    }

    public long Count() => FishFrequency.Select(g => g.Value).Sum();

    public FishSchool SimulateReproduction(int nDays)
    {
        for (int day = 1; day <= nDays; day++)
        {
            // Keep the frequency of each day
            var currentFrequency = new Dictionary<int, long>(FishFrequency);

            foreach (var (daysLeftToReproduce, counter) in FishFrequency)
            {
                // Use a fish to simulate the reproduction. This fish represent n = counter
                var fish = new LanternFish(daysLeftToReproduce);
                if (fish.CanReproduce())
                {
                    // Each fish generates a newborn
                    currentFrequency[LanternFish.NewbornDaysToReproduce] += counter;
                }

                // Update the frequency
                currentFrequency[daysLeftToReproduce] -= counter;

                fish.Age();

                currentFrequency[fish.DaysLeftToReproduce] += counter;
            }

            FishFrequency = new Dictionary<int, long>(currentFrequency);
        }

        return this;
    }
}
