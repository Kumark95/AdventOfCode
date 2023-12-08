using AdventOfCode.Common.Algorithms;

namespace AdventOfCode.Core.Solvers.Year2023.Day08.Model;

internal class NavigationMap
{
    private readonly char[] _instructionSet;
    private readonly Dictionary<string, Node> _nodes;

    public NavigationMap(char[] instructionSet, Dictionary<string, Node> nodes)
    {
        _instructionSet = instructionSet;
        _nodes = nodes;
    }

    private Node NextNode(Node node, char instruction)
    {
        return instruction == 'L'
            ? _nodes[node.LeftLabel]
            : _nodes[node.RightLabel];
    }

    public long CalculateStepsForHumans()
    {
        var startNode = _nodes["AAA"];

        return CalculateSteps(startNode, node => node.Label == "ZZZ");
    }

    private long CalculateSteps(Node startNode, Func<Node, bool> exitCondition)
    {
        var steps = 0L;
        var currentNode = startNode;

        bool endReached = false;
        do
        {
            foreach (var instruction in _instructionSet)
            {
                steps++;

                currentNode = NextNode(currentNode, instruction);
                if (exitCondition(currentNode))
                {
                    endReached = true;
                    break;
                }
            }
        } while (!endReached);

        return steps;
    }

    public long CalculateStepsForGhosts()
    {
        var startNodes = _nodes.Values
            .Where(n => n.Label.EndsWith('A'))
            .ToList();

        var startNodeSteps = startNodes
            .Select(n => CalculateSteps(n, exitCondition: node => node.Label.EndsWith('Z')))
            .ToList();

        // Each time a node reaches the exit condition, it will keep cycling until all the nodes reach the end
        return startNodeSteps
            .Aggregate((a, b) => Arithmetic.LowestCommonMultiplier(a, b));
    }
}
