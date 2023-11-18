namespace AdventOfCode.Core.Solvers.Year2021.Day18;

internal class TreeNode
{
    public TreeNode? Parent { get; private set; }
    public TreeNode? Left { get; private set; }
    public TreeNode? Right { get; private set; }
    public int? ContentValue { get; private set; }
    public bool IsLeaf => ContentValue.HasValue;
    public int Depth => Parent != null ? Parent.Depth + 1 : 0;


    /// <summary>
    /// Constructor for pair nodes
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public TreeNode(TreeNode? parent, TreeNode? left, TreeNode? right)
    {
        Parent = parent;
        Left = left;
        Right = right;
    }

    /// <summary>
    /// Constructor for leaf nodes
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="value"></param>
    public TreeNode(TreeNode? parent, int value)
    {
        Parent = parent;
        ContentValue = value;
    }

    /// <summary>
    /// Adding two nodes generate a new pair
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static TreeNode Add(TreeNode left, TreeNode right)
    {
        // Need to generate new objects to not modify the original objects
        var leftClone = Parse(left.ToString(), null);
        var rightClone = Parse(right.ToString(), null);

        var root = new TreeNode(null, leftClone, rightClone);
        leftClone.Parent = root;
        rightClone.Parent = root;

        root.Reduce();
        return root;
    }

    public int Magnitude()
    {
        if (IsLeaf)
        {
            return ContentValue!.Value;
        }

        return 3 * Left!.Magnitude() + 2 * Right!.Magnitude();
    }

    private void Reduce()
    {
        // Only one action can be performed each iteration
        var shouldContinue = true;
        do
        {
            var nodeToExplode = FindNodeToExplode(this);
            var nodeToSplit = FindNodeToSplit(this);
            if (nodeToExplode != null)
            {
                ExplodeNode(nodeToExplode);
            }
            else if (nodeToSplit != null)
            {
                SplitNode(nodeToSplit);
            }
            else
            {
                shouldContinue = false;
            }
        } while (shouldContinue);
    }

    /// <summary>
    /// Find a node that matches the exploding conditions
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private static TreeNode? FindNodeToExplode(TreeNode node)
    {
        if (!node.IsLeaf && node.Depth >= 4)
        {
            return node;
        }
        else
        {
            if (node.Left != null)
            {
                var leftNodeToExplode = FindNodeToExplode(node.Left);
                if (leftNodeToExplode != null)
                {
                    return leftNodeToExplode;
                }
            }
            if (node.Right != null)
            {
                var rightNodeToExplode = FindNodeToExplode(node.Right);
                if (rightNodeToExplode != null)
                {
                    return rightNodeToExplode;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Explodes a node.
    /// </summary>
    /// <remarks>
    /// Adds the pair's left value to the first regular number to the left (if any), and the pair's right value to the first 
    /// regular number to the right of the exploding pair (if any).
    /// The entire exploding pair is replaced with the regular number 0.
    /// </remarks>
    /// <param name="node"></param>
    private static void ExplodeNode(TreeNode node)
    {
        // Used to prevent visiting the wrong side of the parent nodes
        var rootPath = new List<TreeNode>();
        var current = node;
        while (current != null)
        {
            rootPath.Add(current);
            current = current.Parent;
        }

        var previousLeaf = FindPreviousLeaf(node, rootPath);
        if (previousLeaf != null)
        {
            previousLeaf.ContentValue += node.Left!.ContentValue;
        }

        var nextLeaf = FindNextLeaf(node, rootPath);
        if (nextLeaf != null)
        {
            nextLeaf.ContentValue += node.Right!.ContentValue;
        }

        // The exploding pair is replaced with the regular number 0
        node.ContentValue = 0;
        node.Left = null;
        node.Right = null;
    }

    /// <summary>
    /// Finds the rightmost leaf to a given node
    /// </summary>
    /// <param name="node"></param>
    /// <param name="rootPath"></param>
    /// <returns></returns>
    private static TreeNode? FindPreviousLeaf(TreeNode node, IEnumerable<TreeNode> rootPath)
    {
        if (node.Parent == null)
        {
            return null;
        }

        // Rightmost leaf has more priority
        const int LEFT_PRIORITY = 2;
        const int RIGHT_PRIORITY = 1;
        const int PARENT_PRIORITY = 3;

        var visitedNodes = new List<TreeNode>
        {
            node
        };

        var queue = new PriorityQueue<TreeNode, int>();
        queue.Enqueue(node.Parent, PARENT_PRIORITY);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (visitedNodes.Contains(current))
            {
                continue;
            }
            visitedNodes.Add(current);

            if (current.IsLeaf)
            {
                return current;
            }

            if (current.Left != null && !visitedNodes.Contains(current.Left))
            {
                queue.Enqueue(current.Left, LEFT_PRIORITY);
            }
            if (current.Right != null && !rootPath.Contains(current) && !visitedNodes.Contains(current.Right))
            {
                queue.Enqueue(current.Right, RIGHT_PRIORITY);
            }
            if (current.Parent != null && !visitedNodes.Contains(current.Parent))
            {
                queue.Enqueue(current.Parent, PARENT_PRIORITY);
            }
        }


        return null;
    }

    /// <summary>
    /// Find the leftmost leaf to a given node
    /// </summary>
    /// <param name="node"></param>
    /// <param name="rootPath"></param>
    /// <returns></returns>
    private static TreeNode? FindNextLeaf(TreeNode node, IEnumerable<TreeNode> rootPath)
    {

        if (node.Parent == null)
        {
            return null;
        }

        // Leftmost leaf has more priority
        const int LEFT_PRIORITY = 1;
        const int RIGHT_PRIORITY = 2;
        const int PARENT_PRIORITY = 3;

        var visitedNodes = new List<TreeNode>
        {
            node
        };

        var queue = new PriorityQueue<TreeNode, int>();
        queue.Enqueue(node.Parent, PARENT_PRIORITY);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (visitedNodes.Contains(current))
            {
                continue;
            }
            visitedNodes.Add(current);

            if (current.IsLeaf)
            {
                return current;
            }

            if (current.Left != null && !rootPath.Contains(current) && !visitedNodes.Contains(current.Left))
            {
                queue.Enqueue(current.Left, LEFT_PRIORITY);
            }
            if (current.Right != null && !visitedNodes.Contains(current.Right))
            {
                queue.Enqueue(current.Right, RIGHT_PRIORITY);
            }
            if (current.Parent != null && !visitedNodes.Contains(current.Parent))
            {
                queue.Enqueue(current.Parent, PARENT_PRIORITY);
            }
        }


        return null;
    }

    /// <summary>
    /// Find a node that matches the splitting conditions
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private static TreeNode? FindNodeToSplit(TreeNode node)
    {
        if (node.IsLeaf && node.ContentValue >= 10)
        {
            return node;
        }
        else
        {
            if (node.Left != null)
            {
                var leftNodeToSplit = FindNodeToSplit(node.Left);
                if (leftNodeToSplit != null)
                {
                    return leftNodeToSplit;
                }
            }
            if (node.Right != null)
            {
                var rightNodeToSplit = FindNodeToSplit(node.Right);
                if (rightNodeToSplit != null)
                {
                    return rightNodeToSplit;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Splits a node.
    /// </summary>
    /// <remarks>
    /// Replaces the number with a pair. The left element should be the regular number divided by two and rounded down, 
    /// while the right element should be the regular number divided by two and rounded up.
    /// </remarks>
    /// <param name="node"></param>
    /// <exception cref="Exception">If the node is not a regular number</exception>
    private static void SplitNode(TreeNode node)
    {
        if (!node.ContentValue.HasValue)
        {
            throw new Exception("Trying to split a pair directly");
        }

        node.Left = new TreeNode(node, (int)Math.Truncate(node.ContentValue.Value / 2.0));
        node.Right = new TreeNode(node, (int)Math.Ceiling(node.ContentValue.Value / 2.0));
        node.ContentValue = null;
    }

    #region PARSING
    public static TreeNode Parse(string content, TreeNode? parentNode)
    {
        var root = new TreeNode(parentNode, left: null, right: null);
        if (content.Contains(','))
        {
            // Remove the brackets before dissolving
            content = content[1..^1];

            var (leftSide, rightSide) = DisolvePair(content);

            root.Left = Parse(leftSide, root);
            root.Right = Parse(rightSide, root);
        }
        else
        {
            var nodeValue = int.Parse(content);
            return new TreeNode(parent: parentNode, nodeValue);
        }

        return root;
    }

    private static (string leftSide, string rigthSide) DisolvePair(string pairRepresentation)
    {
        //var minDepth = 0;
        var currentDepth = 0;
        for (var idx = 0; idx < pairRepresentation.Length; idx++)
        {
            var character = pairRepresentation[idx];
            if (character == '[')
            {
                currentDepth++;
            }
            else if (character == ']')
            {
                currentDepth--;
            }
            else
            {
                if (character == ',' && currentDepth == 0)
                {
                    return (pairRepresentation[..idx], pairRepresentation[(idx + 1)..]);
                }
            }
        }

        throw new Exception("The pair representation is not valid");
    }

    #endregion

    #region REPRESENTATION
    public override string ToString()
    {
        if (ContentValue.HasValue)
        {
            return ContentValue.Value.ToString();
        }
        else
        {
            return $"[{Left},{Right}]";
        }
    }

    /// <summary>
    /// Graphical representation of the node and its children
    /// </summary>
    public void Print()
    {
        PrintIndented("", true);
    }

    private void PrintIndented(string indent, bool isLastPrinted)
    {
        Console.Write(indent);
        if (isLastPrinted)
        {
            Console.Write("└─");
            indent += "  ";
        }
        else
        {
            Console.Write("├─");
            indent += "| ";
        }

        Console.WriteLine(ContentValue != null ? $"{ContentValue}({Depth})" : $"*({Depth})");
        Left?.PrintIndented(indent, isLastPrinted: false);
        Right?.PrintIndented(indent, isLastPrinted: true);
    }
    #endregion
}
