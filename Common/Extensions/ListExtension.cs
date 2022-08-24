namespace AdventOfCode.Common.Extensions;

public static class ListExtension
{
    public static IEnumerable<IEnumerable<T>> CombinationsWithoutRepetition<T>(this IEnumerable<T> list, int sampleSize)
    {
        if (sampleSize == 1)
        {
            return list.Select(item => new List<T> { item });
        }

        return list.SelectMany((item, i) => list.Skip(i + 1)
                                                .CombinationsWithoutRepetition(sampleSize - 1)
                                                .Select(result => new List<T> { item }.Concat(result)));
    }
}
