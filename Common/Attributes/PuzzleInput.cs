namespace AdventOfCode.Common.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class PuzzleInput : Attribute
{
    public string Filename { get; private set; }
    public long? ExpectedResult { get; private set; }

    public PuzzleInput(string filename, long expectedResult)
    {
        Filename = filename;
        ExpectedResult = expectedResult;
    }
}
