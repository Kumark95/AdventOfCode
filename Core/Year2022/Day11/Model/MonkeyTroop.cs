using System.Diagnostics;

namespace AdventOfCode.Core.Year2022.Day11.Model;

internal class MonkeyTroop
{
    public List<Monkey> Troop { get; init; }

    public MonkeyTroop(List<Monkey> troop)
    {
        Troop = troop;
    }

    public static MonkeyTroop Parse(string[] propertyLines)
    {
        var monkeys = new List<Monkey>();

        var index = 0;
        while (index < propertyLines.Length)
        {
            var monkeyProperties = propertyLines.Skip(index).Take(6).ToArray();
            monkeys.Add(Monkey.Parse(monkeyProperties));

            index += 7;
        }

        return new MonkeyTroop(monkeys);
    }

    public void PlayRounds(int rounds, bool isWorryReliefActivated)
    {
        for (int r = 1; r <= rounds; r++)
        {
            PlayRound(isWorryReliefActivated);

            if (r == 1 || r == 20 || r % 1000 == 0)
            {
                Debug.WriteLine($"---- Results for round {r} ----");
                var i = 0;
                foreach (var monkey in Troop)
                {
                    Debug.WriteLine($"Monkey {i++}: {monkey.InspectionCounter}");
                }
                Debug.WriteLine("");
            }
        }
    }

    public void PlayRound(bool isWorryReliefActivated)
    {
        foreach (var monkey in Troop)
        {
            while (monkey.HasItems())
            {
                (long newItem, int nextMonkeyPosition) = monkey.Play(isWorryReliefActivated);
                var nextMonkey = Troop[nextMonkeyPosition];
                nextMonkey.AddItem(newItem);
            }
        }
    }
}
