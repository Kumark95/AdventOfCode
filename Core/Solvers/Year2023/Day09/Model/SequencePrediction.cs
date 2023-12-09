namespace AdventOfCode.Core.Solvers.Year2023.Day09.Model;

internal static class SequencePrediction
{
    public static long PredictNextValue(long[] sequence)
    {
        return BuildIncrementLists(sequence)
            .Select(seq => seq.Last())
            .Sum();
    }

    public static long PredictPreviousValue(long[] sequence)
    {
        // Reverse is needed to calculate from the bottom
        return BuildIncrementLists(sequence)
            .Select(seq => seq.First())
            .Reverse()
            .Aggregate(0L, (prev, current) => current - prev);
    }

    private static List<long[]> BuildIncrementLists(long[] sequence)
    {
        List<long[]> incrementLists = [sequence];

        while (true)
        {
            var currentList = incrementLists.Last();

            var diff = currentList
                .Zip(currentList[1..])
                .Select(tup => tup.Second - tup.First)
                .ToArray();

            if (diff.All(elem => elem == 0))
            {
                break;
            }

            incrementLists.Add(diff);
        }

        return incrementLists;
    }
}
