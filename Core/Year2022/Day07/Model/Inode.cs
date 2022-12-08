namespace AdventOfCode.Core.Year2022.Day07.Model;

internal class Inode
{
    public Inode? Parent { get; private set; }
    public HashSet<Inode> Children { get; private set; }
    public string Name { get; private set; }
    public int? Size { get; private set; }
    public bool IsDir => !Size.HasValue;

    /// <summary>
    /// Directory constructor
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    public Inode(Inode? parent, string name)
    {
        Parent = parent;
        Name = name;
        Children = new HashSet<Inode>();
    }

    /// <summary>
    /// File constructor
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <param name="size"></param>
    public Inode(Inode? parent, string name, int size)
    {
        Parent = parent;
        Name = name;
        Size = size;
        Children = new HashSet<Inode>();
    }

    internal int TotalSize()
    {
        var result = 0;
        foreach (var child in Children)
        {
            if (child.IsDir)
            {
                result += child.TotalSize();
            }
            else
            {
                result += child.Size!.Value;
            }
        }

        return result;
    }

    internal static List<int> ListDirectorySizes(Inode rootNode)
    {
        var directorySizes = new List<int>();

        var stack = new Stack<Inode>();
        stack.Push(rootNode);

        while (stack.Count > 0)
        {
            var currentNode = stack.Pop();
            if (currentNode.IsDir)
            {
                foreach (var child in currentNode.Children)
                {
                    stack.Push(child);
                }

                directorySizes.Add(currentNode.TotalSize());
            }

        }

        return directorySizes;
    }
}
