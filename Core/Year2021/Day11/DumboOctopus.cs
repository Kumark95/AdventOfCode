using AdventOfCode.Common.Model;

namespace AdventOfCode.Core.Year2021.Day11;

public class DumboOctopus
{
    public Point Location { get; private set; }
    public int Energy { get; private set; }

    public DumboOctopus(int row, int col, int initialEnergy)
    {
        Location = new Point(row, col);
        Energy = initialEnergy;
    }

    public void Charge() => Energy++;

    public void Reset() => Energy = 0;
    public bool IsFlashing() => Energy > 9;
}
