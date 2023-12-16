namespace AdventOfCode.Core.Solvers.Year2023.Day15.Model;

internal class Box
{
    public List<Lens> Lenses { get; } = [];

    public void AddLens(Lens lens)
    {
        var currentLensIndex = Lenses.FindIndex(l => l.Label == lens.Label);
        if (currentLensIndex >= 0)
        {
            // Replace with the new lens
            Lenses[currentLensIndex] = lens;
        }
        else
        {
            Lenses.Add(lens);
        }
    }

    public void RemoveLens(string label)
    {
        var currentLensIndex = Lenses.FindIndex(l => l.Label == label);
        if (currentLensIndex >= 0)
        {
            Lenses.RemoveAt(currentLensIndex);
        }
    }
}
