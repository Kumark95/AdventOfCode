using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Solvers.Year2021.Day10;


[PuzzleName("Syntax Scoring")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 10;

    private readonly Dictionary<char, char> _expectedClosingTag = new()
    {
        { '(', ')' },
        { '[', ']' },
        { '{', '}' },
        { '<', '>' }
    };

    private readonly List<char> _closingTags = new() { ')', ']', '}', '>' };
    private readonly List<char> _openingTags = new() { '(', '[', '{', '<' };

    private readonly Dictionary<char, int> _ilegalTagScores = new()
    {
        { ')', 3 },
        { ']', 57 },
        { '}', 1197 },
        { '>', 25137 }
    };

    private readonly Dictionary<char, long> _completionTagScores = new()
    {
        { ')', 1 },
        { ']', 2 },
        { '}', 3 },
        { '>', 4 }
    };

    private (Stack<char> stack, char? corruptedTag) Analyze(string line)
    {
        var stack = new Stack<char>();

        foreach (var tag in line)
        {
            if (_openingTags.Contains(tag))
            {
                stack.Push(tag);
                continue;
            }

            if (_closingTags.Contains(tag))
            {
                var isCorrupted = _expectedClosingTag[stack.Peek()] != tag;
                if (isCorrupted)
                {
                    return (stack, tag);
                }

                // Continue
                stack.Pop();
            }
        }

        return (stack, null);
    }

    public long? SolvePartOne(string[] inputLines)
    {
        var result = 0;

        foreach (var line in inputLines)
        {
            (Stack<char> stack, char? corruptedTag) = Analyze(line);
            if (corruptedTag.HasValue)
            {
                result += _ilegalTagScores[corruptedTag.Value];
            }
        }

        return result;
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var completionStringScores = new List<long>();

        foreach (var line in inputLines)
        {
            (Stack<char> stack, char? corruptedTag) = Analyze(line);
            if (corruptedTag.HasValue)
            {
                continue;
            }

            // Try to repair
            long completionScore = 0;
            while (stack.Count > 0)
            {
                var topTag = stack.Pop();

                // Calculate the score
                var closingTag = _expectedClosingTag[topTag];
                completionScore = completionScore * 5 + _completionTagScores[closingTag];
            }

            completionStringScores.Add(completionScore);
        }

        return completionStringScores
            .OrderBy(s => s)
            .ToList()
            .Skip(completionStringScores.Count / 2)
            .Take(1)
            .First();
    }
}
