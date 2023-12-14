namespace AdventOfCode.Core.Solvers.Year2023.Day12.Model;

internal readonly record struct HotSpringsRecords(string Records, int[] DamagedSprings)
{
    private readonly Dictionary<string, long> _cache = new();

    public long CalculatePossibleArrangements()
    {
        return CalculateMatches(recordIndex: 0, damagedSpringsIndex: 0, currentDamagedCount: 0, replacementCharacter: null);
    }

    public long CalculateMatches(int recordIndex, int damagedSpringsIndex, int currentDamagedCount, char? replacementCharacter)
    {
        // Memoization
        var cacheKey = string.Join('_', recordIndex, damagedSpringsIndex, currentDamagedCount, replacementCharacter);
        if (_cache.TryGetValue(cacheKey, out var cachedResult))
        {
            return cachedResult;
        }

        if (recordIndex > Records.Length - 1)
        {
            return 0;
        }

        long result;

        var recordStatus = Records[recordIndex];
        var expectedDamagedSprings = damagedSpringsIndex < DamagedSprings.Length ? DamagedSprings[damagedSpringsIndex] : 0;

        if (recordStatus == '#' || replacementCharacter == '#')
        {
            if (currentDamagedCount + 1 > expectedDamagedSprings)
            {
                result = 0;
            }
            else if (currentDamagedCount + 1 == expectedDamagedSprings && recordIndex == Records.Length - 1 && damagedSpringsIndex == DamagedSprings.Length - 1)
            {
                // Reached the end and the count matches
                result = 1;
            }
            else
            {
                result = CalculateMatches(recordIndex + 1, damagedSpringsIndex, currentDamagedCount + 1, null);
            }
        }
        else if (recordStatus == '.' || replacementCharacter == '.')
        {
            if (expectedDamagedSprings > 0)
            {
                if (currentDamagedCount == 0)
                {
                    // Move to the next damaged springs group
                    result = CalculateMatches(recordIndex + 1, damagedSpringsIndex, 0, null);
                }

                else if (currentDamagedCount == expectedDamagedSprings)
                {
                    if (recordIndex == Records.Length - 1 && damagedSpringsIndex == DamagedSprings.Length - 1)
                    {
                        // Reached the end
                        result = 1;
                    }
                    else
                    {
                        // Move to the next damaged springs group
                        result = CalculateMatches(recordIndex + 1, damagedSpringsIndex + 1, 0, null);
                    }
                }
                else
                {
                    result = 0;
                }
            }
            else
            {
                if (recordIndex == Records.Length - 1 && expectedDamagedSprings == 0)
                {
                    result = 1;
                }
                else
                {
                    result = CalculateMatches(recordIndex + 1, damagedSpringsIndex, 0, null);
                }
            }
        }
        else
        {
            result = CalculateMatches(recordIndex, damagedSpringsIndex, currentDamagedCount, '#')
                + CalculateMatches(recordIndex, damagedSpringsIndex, currentDamagedCount, '.');
        }

        _cache[cacheKey] = result;
        return result;
    }
};
