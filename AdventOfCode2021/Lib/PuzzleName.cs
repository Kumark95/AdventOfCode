namespace AdventOfCode2021.Lib;

[AttributeUsage(AttributeTargets.Class)]
public class PuzzleName : Attribute
{
    public string Name { get; private set; }

    public PuzzleName(string name)
    {
        Name = name;
    }
}
