using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;
using AdventOfCode.Core.Solvers.Year2022.Day07.Model;

namespace AdventOfCode.Core.Solvers.Year2022.Day07;

[PuzzleName("No Space Left On Device")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2022;
    public int Day => 7;

    public object SolvePartOne(string[] inputLines)
    {
        var rootNode = InstructionParser.ReadTerminalOutput(inputLines);

        return Inode.ListDirectorySizes(rootNode)
            .Where(size => size <= 100_000)
            .Sum();
    }

    public object SolvePartTwo(string[] inputLines)
    {
        var rootNode = InstructionParser.ReadTerminalOutput(inputLines);

        var currentUnusedSpace = FileSystem.TotalDiskSpace - rootNode.TotalSize();
        var missingRequiredSpace = SystemUpdate.RequiredSpace - currentUnusedSpace;

        return Inode.ListDirectorySizes(rootNode)
            .Where(size => size >= missingRequiredSpace)
            .OrderBy(size => size)
            .First();
    }
}
