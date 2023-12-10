using AdventOfCode.Common.Attributes;
using AdventOfCode.Common.Interfaces;

namespace AdventOfCode.Core.Solvers.Year2021.Day18;

[PuzzleName("Snailfish")]
public class PuzzleSolver : IPuzzleSolver
{
    public int Year => 2021;
    public int Day => 18;


    private static List<TreeNode> Input(string[] inputLines)
    {
        var trees = new List<TreeNode>();

        foreach (var line in inputLines)
        {
            trees.Add(TreeNode.Parse(line, null));
        }

        return trees;
    }

    public long? SolvePartOne(string[] inputLines)
    {
        var resultingTree = Input(inputLines)
            .Aggregate((a, b) => TreeNode.Add(a, b));

        return resultingTree.Magnitude();
    }

    public long? SolvePartTwo(string[] inputLines)
    {
        var maxMagnitude = 0;
        var availableTrees = Input(inputLines);

        for (int i = 0; i < availableTrees.Count; i++)
        {
            for (int j = 0; j < availableTrees.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                var leftTree = availableTrees[i];
                var rightTree = availableTrees[j];

                var joinedTree = TreeNode.Add(leftTree, rightTree);
                maxMagnitude = Math.Max(maxMagnitude, joinedTree.Magnitude());
            }
        }

        return maxMagnitude;
    }
}
