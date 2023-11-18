namespace AdventOfCode.Core.Solvers.Year2021.Day22.Model;

internal record struct Cuboid(int XMin, int XMax, int YMin, int YMax, int ZMin, int ZMax)
{
    private long XLen => XMax - XMin + 1;
    private long YLen => YMax - YMin + 1;
    private long ZLen => ZMax - ZMin + 1;

    internal long Count()
    {
        return XLen * YLen * ZLen;
    }

    internal bool Intersects(Cuboid target)
    {
        return XMin <= target.XMax
            && XMax >= target.XMin
            && YMin <= target.YMax
            && YMax >= target.YMin
            && ZMin <= target.ZMax
            && ZMax >= target.ZMin;
    }

    internal Cuboid? Intersection(Cuboid target)
    {
        if (!Intersects(target))
        {
            return null;
        }

        return new Cuboid
        {
            XMin = Math.Max(XMin, target.XMin),
            XMax = Math.Min(XMax, target.XMax),
            YMin = Math.Max(YMin, target.YMin),
            YMax = Math.Min(YMax, target.YMax),
            ZMin = Math.Max(ZMin, target.ZMin),
            ZMax = Math.Min(ZMax, target.ZMax)
        };
    }
}
