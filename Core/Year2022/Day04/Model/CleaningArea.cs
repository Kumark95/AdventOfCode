namespace AdventOfCode.Core.Year2022.Day04.Model;

internal record struct CleaningArea(int Start, int End)
{
    internal bool FullyContains(CleaningArea target)
    {
        if (target.Start > Start
            || target.End < End)
        {
            return false;
        }

        return true;
    }

    internal bool PartiallyContains(CleaningArea target)
    {
        if (target.Start > End
            || target.End < Start)
        {
            return false;
        }

        return true;
    }
}
