using System.Diagnostics;

namespace AdventOfCode.Core.Solvers.Year2022.Day11.Model;

internal class MonkeyTroop
{
    public List<Monkey> Troop { get; init; }

    public MonkeyTroop(List<Monkey> troop)
    {
        Troop = troop;
    }

    public static MonkeyTroop Parse(string[] propertyLines)
    {
        var troop = new List<Monkey>();

        var index = 0;
        var divisorList = new List<int>();
        while (index < propertyLines.Length)
        {
            var monkeyProperties = propertyLines.Skip(index).Take(6).ToArray();
            var monkey = Monkey.Parse(monkeyProperties);
            troop.Add(monkey);
            divisorList.Add(monkey.GetTestDivisor());

            index += 7;
        }

        var commonDivisor = divisorList.Aggregate((a, b) => a * b);
        foreach (var monkey in troop)
        {
            monkey.SetTroopCommonDivisor(commonDivisor);
        }

        return new MonkeyTroop(troop);
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
                (var newItem, var nextMonkeyPosition) = monkey.Play(isWorryReliefActivated);
                var nextMonkey = Troop[nextMonkeyPosition];
                nextMonkey.AddItem(newItem);
            }
        }
    }
}
